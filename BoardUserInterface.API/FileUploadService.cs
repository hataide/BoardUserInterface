using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BoardUserInterface.API
{

    namespace BoardUserInterface.API.Services
    {
        public class FileUploadService
        {
            public async Task<string> UploadFileAsync(IFormFile file)
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is empty or null.");
                }

                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

                if (fileExtension != ".xlsx" && fileExtension != ".csv")
                {
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

                return fullPath;
            }
        }
    }

}
