using FileUpload.Extensions;
using FileUpload.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileUpload.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly IUploadFileService _uploadFileService;
    private readonly IDownloadFileService _downloadFileService;

    public FileController(IUploadFileService fileService, IDownloadFileService downloadFileService)
    {
        _uploadFileService = fileService;
        _downloadFileService = downloadFileService;
    }

    [HttpPost()]
    public async Task<IActionResult> Post([FromForm]FileUploadRequest request)
    {
        if (request.File == null || request.File.Length == 0)
        {
            return BadRequest("File was not provided");
        }

        var fileName = await _uploadFileService.UploadFile(
            Path.GetExtension(request.File.FileName),
            request.File.OpenReadStream());

        return Created($"/file/{fileName}", null);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var binaryResult = await _downloadFileService.DownloadFile(id);
        return File(binaryResult, "image/jpeg");
    }
}

public class FileUploadRequest
{
    public IFormFile File { get; set; }
}