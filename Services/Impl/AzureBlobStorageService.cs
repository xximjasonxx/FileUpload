using Azure.Identity;
using Azure.Storage.Blobs;

namespace FileUpload.Services.Impl;

public class AzureBlobStorageService : IUploadFileService, IDownloadFileService
{
    private readonly IConfiguration _configuration;

    public AzureBlobStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    BlobContainerClient GetContainerClient()
    {
        var credential = string.IsNullOrEmpty(_configuration["ManagedIdentityClientId"])
            ? new DefaultAzureCredential()
            : new DefaultAzureCredential(new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = _configuration["ManagedIdentityClientId"]
                });

        var uriHost = $"https://{_configuration["ACCOUNT_NAME"]}.blob.core.windows.net/{_configuration["CONTAINER_NAME"]}";
        return new BlobContainerClient(new Uri(uriHost), credential);
    }

    public async Task<byte[]> DownloadFile(string filename)
    {
        var containerClient = GetContainerClient();
        var blob = containerClient.GetBlobClient(filename);

        using var memStream = new MemoryStream();
        await blob.DownloadToAsync(memStream);
        return memStream.ToArray();
    }

    public async Task<string> UploadFile(string fileExtension, Stream fileStream)
    {
        string filename = $"{Guid.NewGuid()}{fileExtension}";
        var containerClient = GetContainerClient();
        var blob = containerClient.GetBlobClient(filename);

        await blob.UploadAsync(fileStream);
        return filename;
    }
}