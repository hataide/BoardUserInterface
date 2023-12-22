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

    [HttpPut("put-test")]
    public async Task<IActionResult> PutTest(String data)
    {
        try
        {
            var result = await _genericService.UpdateData(data);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Log and handle exceptions appropriately
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
        }
    }
}