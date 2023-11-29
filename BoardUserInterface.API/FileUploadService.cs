using System.Net.Http.Headers;

namespace BoardUserInterface.API.Services
{
    public class FileUploadService
    {
        private readonly ILogger<FileUploadService> _logger;

        public FileUploadService(ILogger<FileUploadService> logger)
        {
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                //_logger.LogError("Error during file upload. No file was uploaded or the file is empty.");
                throw new ArgumentException("File is empty or null.");
            }

            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            if (fileExtension != ".xlsx" && fileExtension != ".csv")
            {
                //_logger.LogError($"Error during file upload. Invalid file extension: {fileExtension}. Only .xlsx and .csv files are allowed.");
                throw new ArgumentException("Invalid file extension. Only .xlsx and .csv files are allowed.");
            }

            var folderName = Path.Combine("Resources", "Uploads");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var fullPath = Path.Combine(filePath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation($"File uploaded successfully: {fileName}");
            return fullPath;
        }
    }
}
