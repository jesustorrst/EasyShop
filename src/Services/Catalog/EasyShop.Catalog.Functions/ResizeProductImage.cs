using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace EasyShop.Catalog.Functions.BlobTriggers;

public class ResizeProductImage
{
    private readonly ILogger<ResizeProductImage> _logger;

    public ResizeProductImage(ILogger<ResizeProductImage> logger)
    {
        _logger = logger;
    }

    [Function("ResizeProductImage")]
    [BlobOutput("products-thumbs/{name}", Connection = "AzureWebJobsStorage")]
    // 🎯 CAMBIO CLAVE: Cambiamos a byte[] para que .NET Isolated v2.x pase el binario directo sin intentar parsearlo como JSON
    public async Task<byte[]> Run(
        [BlobTrigger("products/{name}", Connection = "AzureWebJobsStorage")] byte[] blobContent,
        string name)
    {
        _logger.LogInformation($"🔥 Procesando archivo detectado en Azure: {name} | Tamaño: {blobContent.Length} bytes");

        try
        {
            // 1. Cargar la imagen desde el array de bytes binarios
            using var image = Image.Load(blobContent);

            int maxWidth = 600;
            int maxHeight = 600;

            // 2. Redimensionar conservando la relación de aspecto
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(maxWidth, maxHeight),
                Mode = ResizeMode.Max
            }));

            // 3. Guardar el resultado en memoria
            using var ms = new MemoryStream();

            if (name.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                await image.SaveAsPngAsync(ms);
            }
            else if (name.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
            {
                await image.SaveAsWebpAsync(ms);
            }
            else
            {
                await image.SaveAsJpegAsync(ms);
            }

            _logger.LogInformation($"✅ La imagen {name} fue optimizada con éxito. Enviando a products-thumbs...");

            return ms.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ Error crítico al procesar los pixeles de {name}: {ex.Message}");
            throw;
        }
    }
}