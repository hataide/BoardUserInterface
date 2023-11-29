using Microsoft.AspNetCore.Mvc;

namespace BoardUserInterface.API.Controllers.V1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/template")]
    [ApiVersion("1.0")] // Specify the API version for this controller
    public class TemplateController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TemplateController> _logger;

        public TemplateController(ILogger<TemplateController> logger)
        {
            _logger = logger;
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
    }
}
