namespace FileUpload.Services;

public interface IUploadFileService
{
    Task<string> UploadFile(string fileExtension, Stream fileStream);
}