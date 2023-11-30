using BoardUserInterface.API.Exceptions;
using BoardUserInterface.API.FileStorageManagement;
using BoardUserInterface.API.FileStorageManagement.Models;
using BoardUserInterface.API.Services;
using BoardUserInterface.API.UploadFiles;
using Microsoft.AspNetCore.Mvc;

namespace BoardUserInterface.API.Controllers.V1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/template")]
    [ApiVersion("1.0")] // Specify the API version for this controller
    public class TemplateController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IFileStorage _fileStorage;
        private readonly IExcelMetadataService _excelMetadataService;
        private readonly IVersionValidator _versionValidator;


        //private readonly ITemplateVersionRepository _templateVersionRepository;


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TemplateController> _logger;

        public TemplateController(ILogger<TemplateController> logger, IFileUploadService fileUploadService, IExcelMetadataService excelMetadataService, IFileStorage fileStorage, IVersionValidator versionValidator)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
            _excelMetadataService = excelMetadataService;
            _fileStorage = fileStorage;
            _versionValidator = versionValidator;

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
            // Check if the file is an Excel file based on its content type
            if (!file.ContentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidFileContentTypeException($"The uploaded file has an invalid content type.");
            }

            // Extract version number from the Excel file
            string uploadedFileVersion = _excelMetadataService.GetVersionNumberFromExcel(file.OpenReadStream());

            if (!_versionValidator.IsValidVersion(uploadedFileVersion))
            {
                throw new InvalidVersionException($"The version [{uploadedFileVersion}] of the uploaded file is not valid.");
            }

            if (string.IsNullOrEmpty(uploadedFileVersion))
            {
                throw new NoVersionException($"The uploaded Excel file does not contain a version number in its metadata.");
            }

            // Retrieve the latest version number from repository
            string latestVersion = _fileStorage.GetLatestVersionNumber();

            var comparer = new VersionComparer();

            // Compare versions
            if (!comparer.CompareVersions(uploadedFileVersion, latestVersion))
            {
                throw new OldVersionException($"Already has a template with that version or a newer one.");
            }

            // If the version is newer, store the file and update the version number in the repository
            var filePath = await _fileUploadService.UploadFileAsync(file);
            _fileStorage.Save( new FileTemplateInformation() { FileName = file.FileName , VersionNumber = uploadedFileVersion } );

            return Ok(  $"File uploaded successfully: {file.FileName} with version: {uploadedFileVersion}" );
        
        }
    }
}
