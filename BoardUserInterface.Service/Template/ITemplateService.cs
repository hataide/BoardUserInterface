﻿using Microsoft.AspNetCore.Http;

namespace BoardUserInterface.Service.Template;

public interface ITemplateService
{
    (string fileName, string version) RemoveLastVersion();
    List<(string filename, string version)> RemoveAllVersionsAsync();
    Task<string> Upload(IFormFile file);
    (string fileContentBase64, string contentType, string fileName) DownloadLatestFile();
}