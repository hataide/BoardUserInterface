using BoardUserInterface.Service.Http;
using BoardUserInterface.Service.Http.Response;
using BoardUserInterface.Service.Logging;
using Microsoft.AspNetCore.Mvc;

namespace BoardUserInterface.API.Controllers.V1;


[ApiController]
[Route("api/v{version:apiVersion}/Http")]
[ApiVersion("1.0")] // Specify the API version for this controller
public class GenericController : ControllerBase
{
    private readonly IGenericService _genericService;
    private readonly ILogService _logService;

    public GenericController(IGenericService genericService, ILogService logService)
    {
        _genericService = genericService;
        _logService = logService;
    }
    
    [HttpGet("get-Test")]
    public async Task<IActionResult> DownloadExcel()
    {
        try
        {
            var downloadResponse = await _genericService.TestGet();

            if (downloadResponse == null)
            {
                // Handle the case where the response was not JSON or deserialization failed
                return BadRequest("The response from the service was not in the expected format.");
            }

            var fileBytes = Convert.FromBase64String(downloadResponse.FileContentBase64);
            //var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(fileBytes, downloadResponse.ContentType, downloadResponse.FileName);
        }
        catch (Exception ex)
        {
            // Log the exception and return an error response
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error downloading the file." });
        }
    }




    [HttpPost("post-test")]
    public async Task<IActionResult> TestPost(String s)//IFormFile file) 
    {
        var result = await _genericService.TestPost(s);
        return Ok(result);
    }

    [HttpDelete("delete-test")]
    public async Task<IActionResult> TestDelete()
    {
        var result = await _genericService.TestDelete();
        return Ok(result);

    }
    /*
    [HttpGet("call-download")]
    public async Task<IActionResult> CallDownload()
    {
        var (fileContentBase64, contentType, fileName) = await _genericService.CallDownloadEndpoint();

        return Ok(new { FileContent = fileContentBase64, ContentType = contentType, FileName = fileName });
    }


    
    [HttpPost("genericUpload")]
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
    }*/
}