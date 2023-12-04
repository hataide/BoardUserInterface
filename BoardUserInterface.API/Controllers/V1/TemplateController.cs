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
        //private readonly IFileUploadService _fileUploadService;
        //private readonly IFileStorage _fileStorage;
        //private readonly IExcelMetadataService _excelMetadataService;
        //private readonly IVersionValidator _versionValidator;
        //private readonly IVersionComparer _versionComparer;
        //private readonly ILogger<TemplateController> _logger;
        private readonly IUploadTemplateService _uploadTemplateService;
        private readonly IRemoveTemplateService _removeTemplateService;

        public TemplateController(IUploadTemplateService uploadTemplateService, IRemoveTemplateService removeTemplateService)
        {
            //_logger = logger;
            //_fileUploadService = fileUploadService;
            //_excelMetadataService = excelMetadataService;
            //_fileStorage = fileStorage;
            //_versionValidator = versionValidator;
            //_versionComparer = versionComparer;
            _uploadTemplateService = uploadTemplateService;
            _removeTemplateService = removeTemplateService;

        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {

            var uploadedFileVersion = await _uploadTemplateService.Upload(file);
            return Ok(  $"File uploaded successfully: {file.FileName} with version: {uploadedFileVersion}" );
        
        }

        [HttpDelete("remove-version")]
        public async Task<IActionResult> RemoveVersion()
        {
            var fileName = await _removeTemplateService.RemoveLastVersion();
            return Ok( $"Last version of {fileName} was removed successfully." );
       
        }

        [HttpDelete("remove-all-versions")]
        public async Task<IActionResult> RemoveAllVersions()
        {
            await _removeTemplateService.RemoveAllVersionsAsync();
            return Ok("All template versions have been removed successfully.");
        }
    }
}
