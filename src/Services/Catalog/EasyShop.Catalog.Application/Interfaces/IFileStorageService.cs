namespace EasyShop.Catalog.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(byte[] image, string fileName, string contentType, CancellationToken cancellationToken);
    Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken);

    string GetSecureUrl(string fileName, CancellationToken cancellationToken);
}