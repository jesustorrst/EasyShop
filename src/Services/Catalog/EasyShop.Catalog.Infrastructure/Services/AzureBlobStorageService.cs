using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EasyShop.Catalog.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EasyShop.Catalog.Infrastructure.Services;

public class AzureBlobStorageService : IFileStorageService
{
    public AzureBlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }
    private readonly BlobServiceClient _blobServiceClient;

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
    {
        if (fileStream == null || string.IsNullOrEmpty(fileName))
            return "https://tuaccount.blob.core.windows.net/products/placeholder.jpg";



        var containerClient = _blobServiceClient.GetBlobContainerClient("products");
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

        // var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
        var blobClient = containerClient.GetBlobClient(fileName);

        var blobHttpHeader = new BlobHttpHeaders { ContentType = "image/jpeg" };

        await blobClient.UploadAsync(
            fileStream,
            new BlobUploadOptions { HttpHeaders = blobHttpHeader },
            cancellationToken
        );

        return blobClient.Uri.ToString();
    }

    public async Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return false;


        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("products");
            var blobClient = containerClient.GetBlobClient(fileName);

            var response = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
            return response.Value;
        }
        catch
        {
            return false;
        }


    }
}