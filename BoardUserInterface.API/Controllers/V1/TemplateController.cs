using BoardUserInterface.API.BoardUserInterface.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BoardUserInterface.API.Controllers.V1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/template")]
    [ApiVersion("1.0")] // Specify the API version for this controller
    public class TemplateController : ControllerBase
    {
        private readonly FileUploadService _fileUploadService;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TemplateController> _logger;

        public TemplateController(ILogger<TemplateController> logger, FileUploadService fileUploadService)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
        }

        [HttpGet()]
        public IEnumerable<WeatherForecast> Get()
        {
            // SampleController.cs (inside one of the actions)
            //throw new Exception("This is a test exception.");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                var filePath = await _fileUploadService.UploadFileAsync(file);
                return Ok(new { filePath });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
