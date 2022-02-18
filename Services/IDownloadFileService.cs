namespace FileUpload.Services;

public interface IDownloadFileService
{
    Task<byte[]> DownloadFile(string filename);
}