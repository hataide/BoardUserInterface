using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using BoardUserInterface.FileService.Service;
using BoardUserInterface.Common.Helpers;
using BoardUserInterface.Common.Exceptions;
using Microsoft.AspNetCore.StaticFiles;
using BoardUserInterface.Repository;
using BoardUserInterface.Repository.Models;
using BoardUserInterface.FileService.Helpers.VersionValidator;
using BoardUserInterface.FileService.Helpers.ExcelMetadata;
using BoardUserInterface.FileService.Helpers.VersionComparer.VersionComparer;
using BoardUserInterface.Service.Logging;
using System;
using BoardUserInterface.Service.Template;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Mime;
using BoardUserInterface.Service.Http.Response;
using System.Text.Json;
using DocumentFormat.OpenXml.Spreadsheet;
namespace BoardUserInterface.Service.Http;


public class GenericService : IGenericService
{
    private readonly IGenericHttpClient _genericHttpClient;
    //private readonly ITemplateService _templateService;


    public GenericService(IGenericHttpClient genericHttpClient)//, ITemplateService templateService)
    {
        _genericHttpClient = genericHttpClient;
        //_templateService = templateService;
    }
    
    public async Task<DownloadResponse> TestGet()
    {
        // Replace with the actual URL of your HttpClientTestApi endpoint
        var testApiUrl = ServiceEndpoints.TestGet;
        var headers = new Dictionary<string, string>
        {
            { "Custom-Header", "CustomValue" }
            // Add other headers as needed
        };

        var content = await _genericHttpClient.GetAsync(testApiUrl, headers);
    
        return content;
        
    }

    public async Task<String> TestPost(String sContent)
    {
        // Replace with the actual URL of your HttpClientTestApi endpoint
        var testApiUrl = ServiceEndpoints.TestPost;
        var headers = new Dictionary<string, string>
        {
            { "Custom-Header", "CustomValue" }
            // Add other headers as needed
        };

        var content = await _genericHttpClient.PostAsync(testApiUrl, sContent, headers);

        return content;

    }
    
    public async Task<String> TestDelete()
    {
        // Replace with the actual URL of your HttpClientTestApi endpoint
        var testApiUrl = ServiceEndpoints.TestDelete;
        var headers = new Dictionary<string, string>
        {
            { "Custom-Header", "CustomValue" }
            // Add other headers as needed
        };

        var result = await _genericHttpClient.DeleteAsync(testApiUrl, headers);
        return result.ToString();
    }

    /*
    public async Task<String> TestPost(String content)//IFormFile file)   //body
    {
        // Replace with the actual URL of your HttpClientTestApi endpoint
        //var testApiUrl = ServiceEndpoints.TestPost;

        var url = ServiceEndpoints.TestPost; // Replace with actual URL
        //using var content = new MultipartFormDataContent();
        //using var fileStream = file.OpenReadStream();
        //content.Add(new StreamContent(fileStream), "file", file.FileName);

        //var emptyContent = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");
        /*
        var emptyContent = new MultipartFormDataContent();

        var headers = new Dictionary<string, string>
        {
            { "Content-Type", "text/plain" }
            // Add other headers as needed
        };

        //var response = await _genericHttpClient.PostAsync<MultipartFormDataContent, string>(url, content);
        var response = await _genericHttpClient.PostAsync<MultipartFormDataContent, HttpResponseMessage>(url, emptyContent);
        //return response;
        return response.ToString();

        var stringContent = new StringContent(content, System.Text.Encoding.UTF8, "text/plain");

        // Send the POST request
        //var response = await _genericHttpClient.PostAsync<string, HttpResponseMessage>(url, stringContent.ToString(), headers);
        var response = await _genericHttpClient.PostAsync<StringContent, HttpResponseMessage>(url, stringContent);
        response.EnsureSuccessStatusCode(); // Ensure we got a successful response

        // Read and return the response content
        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }*/

    /*
    public async Task<string> CallUploadEndpoint(IFormFile file)
    {
        var url = ServiceEndpoints.Upload; // Replace with actual URL
        using var content = new MultipartFormDataContent();
        using var fileStream = file.OpenReadStream();
        content.Add(new StreamContent(fileStream), "file", file.FileName);

        var response = await _genericHttpClient.PostAsync<MultipartFormDataContent, string>(url, content);
        return response;
        
        var url = ServiceEndpoints.Upload; // Replace with actual URL

        var uploadedFileVersion = await _templateService.Upload(file);
        var folderName = Path.Combine("Resources", "Template");
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        var fileContent = File.ReadAllBytes(filePath);

        var response = await _genericHttpClient.PostAsync<MultipartFormDataContent, string>(url, fileContent);
        return response;
}
    */
    /*
        public async Task<(string FileContentBase64, string ContentType, string FileName)> CallDownloadEndpoint()
        {
            var url = ServiceEndpoints.Download; // The URL of the download endpoint

            // Send a GET request to the download endpoint
            var response = await _genericHttpClient.GetAsync<(string FileContentBase64, string ContentType, string FileName)>(url);

            // The response should contain the file content, content type, and file name
            return response;
        }*/


    /*
    public async Task<string> Upload(IFormFile file)
        {
            //var url = $"{TemplateServiceEndpoints.BaseUrl}{TemplateServiceEndpoints.Upload}";
            var url = $"{ServiceEndpoints.Upload}";

            // 'content' is an instance of MultipartFormDataContent, which is used
            // to build multipart/form-data content that can be sent in an HTTP request.
            // This type of content is typically used for file uploads as it supports binary data.
            using var content = new MultipartFormDataContent();

            // 'fileStream' represents a stream that reads from the uploaded file.
            // OpenReadStream() provides a stream to read the file's content.
            using var fileStream = file.OpenReadStream();

            // The next line adds the file content to the multipart/form-data content.
            // The 'file' parameter is the name expected by the server for the file content.
            // 'file.FileName' is the name of the uploaded file, which is included in the
            // content disposition header of the multipart content.
            content.Add(new StreamContent(fileStream), "file", file.FileName);

            // 'response' is the result of making an asynchronous HTTP POST request to the specified URL with the constructed content.
            // The PostAsync method on _genericHttpClient sends the request and awaits the response from the external service.
            // It is a generic method where MultipartFormDataContent is the type of content being sent,
            // and string is the expected type of the response body.
            var response = await _genericHttpClient.PostAsync<MultipartFormDataContent, string>(url, content);

            // The method returns the response body as a string. This assumes that the external service sends back a string as a response,
            // which could be a confirmation message, a result identifier, or any other information relevant to the upload operation.
            return response;

        }*/

    /*
    public (string fileContentBase64, string contentType, string fileName) DownloadLatestFile()
    //public async Task<(string fileContentBase64, string contentType, string fileName)> DownloadLatestFile()
    {
        var url = $"{TemplateServiceEndpoints.BaseUrl}{TemplateServiceEndpoints.Download}";
        // Rest of the download logic...
    }

    public (string fileName, string version) RemoveLastVersion()
    //public async Task<bool> RemoveLastVersion()
    {
        var url = $"{TemplateServiceEndpoints.BaseUrl}{TemplateServiceEndpoints.RemoveVersion}";
        // Rest of the remove version logic...
    }



    public List<(string filename, string version)> RemoveAllVersionsAsync()
    {
        try
        {
            var files = _repositoryStorage.Read();
            // Remove all files from repo
            _repositoryStorage.RemoveAllFiles();
            // Remove all files from system
            _fileService.RemoveAllFiles();

            _logService.LogMessage("Backend", "Successful", "All versions were removed successfully", "Information");
            //_logger.LogInformation($"All versions were removed successfully");

            return files.Select(p => (p.FileName, p.VersionNumber)).ToList();
        }
        catch
        {
            throw new NoVersionException("Failed to remove the last version of the Excel file.");
        }
    }*/
}

