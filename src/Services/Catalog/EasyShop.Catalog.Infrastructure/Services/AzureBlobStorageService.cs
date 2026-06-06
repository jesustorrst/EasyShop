using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EasyShop.Catalog.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Sas;
namespace EasyShop.Catalog.Infrastructure.Services;

public class AzureBlobStorageService : IFileStorageService
{
    public AzureBlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }
    private readonly BlobServiceClient _blobServiceClient;

    public async Task<string> UploadFileAsync(byte[] image, string fileName, string contentType, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(fileName))
            return null!; ;

        var containerClient = _blobServiceClient.GetBlobContainerClient("products");

        var blobClient = containerClient.GetBlobClient(fileName);

        var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };

        await using var stream = new MemoryStream(image);


        await blobClient.UploadAsync(
            stream,
            new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } },
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

    public string GetSecureUrl(string fileName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return null!;

        if (fileName.Contains("https://") || fileName.Contains("http://"))
        {
            fileName = fileName.Split('/').Last();

            if (fileName.Contains("?"))
            {
                fileName = fileName.Split('?').First();
            }
        }

        var containerClient = _blobServiceClient.GetBlobContainerClient("products");
        var blobClient = containerClient.GetBlobClient(fileName);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = "products",
            BlobName = fileName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow.AddMinutes(-10),
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30)

        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        Uri sasUri = blobClient.GenerateSasUri(sasBuilder);


        return sasUri.ToString();
    }
}