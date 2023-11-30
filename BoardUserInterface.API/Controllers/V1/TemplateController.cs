using BoardUserInterface.API.Exceptions;
using BoardUserInterface.API.FileStorageManagement;
using BoardUserInterface.API.FileStorageManagement.Models;
using BoardUserInterface.API.Services;
using BoardUserInterface.API.Services.Template;
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
        private readonly IVersionComparer _versionComparer;
        private readonly ILogger<TemplateController> _logger;
        private readonly IUploadTemplateService _uploadTemplateService;

        public TemplateController(ILogger<TemplateController> logger, IFileUploadService fileUploadService, IExcelMetadataService excelMetadataService, IFileStorage fileStorage, IVersionValidator versionValidator, IVersionComparer versionComparer, IUploadTemplateService uploadTemplateService)
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
            _excelMetadataService = excelMetadataService;
            _fileStorage = fileStorage;
            _versionValidator = versionValidator;
            _versionComparer = versionComparer;
            _uploadTemplateService = uploadTemplateService;

        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {

            var uploadedFileVersion = await _uploadTemplateService.Upload(file);
            return Ok(  $"File uploaded successfully: {file.FileName} with version: {uploadedFileVersion}" );
        
        }
    }
}
