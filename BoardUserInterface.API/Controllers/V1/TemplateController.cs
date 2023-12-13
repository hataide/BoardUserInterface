using BoardUserInterface.Service.Template;
using Microsoft.AspNetCore.Mvc;

namespace BoardUserInterface.API.Controllers.V1;


[ApiController]
[Route("api/v{version:apiVersion}/template")]
[ApiVersion("1.0")] // Specify the API version for this controller
public class TemplateController : ControllerBase
{
    private readonly ITemplateService _templateService;

    public TemplateController(ITemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {

        var uploadedFileVersion = await _templateService.Upload(file);
        return Ok($"File uploaded successfully: {file.FileName} with version: {uploadedFileVersion}");

    }

    [HttpDelete("remove-version")]
    public async Task<IActionResult> RemoveVersion()
    {
        var (fileName, version) = _templateService.RemoveLastVersion();
        return Ok($"Version {version} of {fileName} was removed successfully.");

    }

    [HttpDelete("remove-all-versions")]
    public async Task<IActionResult> RemoveAllVersions()
    {
        var files = _templateService.RemoveAllVersionsAsync();
        return Ok("All template versions have been removed successfully.");
    }

    [HttpGet("download")]
    public IActionResult Download()
    {
        try
        {
            // Use the download service to get the latest file
            var (fileContentBase64, contentType, fileName) = _templateService.DownloadLatestFile();

            // Return a JSON object with the Base64-encoded file content and file name
            return Ok(new { FileContent = fileContentBase64, ContentType = contentType, FileName = fileName });
        }
        catch (FileNotFoundException ex)
        {
            // If there's an error, such as no file found, you can return a NotFound result with a custom message.
            return NotFound(new { Message = ex.Message });
        }
    }
}