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
using BoardUserInterface.Service.Http.Exceptions;
using DocumentFormat.OpenXml.InkML;
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
        try
        {
            var testApiUrl = ServiceEndpoints.TestGet;
            var headers = new Dictionary<string, string>
        {
            { "Custom-Header", "CustomValue" }
            // Add other headers as needed
        };

            var content = await _genericHttpClient.GetAsync(testApiUrl, headers);

            return content;
        }
        catch (HttpRequestException ex)
        {
            throw new ServiceEndpointNotFoundException("The service endpoint could not be reached.");
        }

    }

    public async Task<String> TestPost(String sContent)
    {
        try
        {
            var testApiUrl = ServiceEndpoints.TestPost;
            var headers = new Dictionary<string, string>
        {
            { "Custom-Header", "CustomValue" }
            // Add other headers as needed
        };

            var content = await _genericHttpClient.PostAsync(testApiUrl, sContent, headers);

            return content;
        }
        catch (HttpRequestException ex)
        {
            throw new ServiceEndpointNotFoundException("The service endpoint could not be reached.");
        }

    }


    public async Task<string> UpdateData<TRequest>(TRequest data)
    {
        try
        {
            var testApiUrl = ServiceEndpoints.TestPut;
            return await _genericHttpClient.PutAsync(testApiUrl, data);
        }
        catch (HttpRequestException ex)
        {
            throw new ServiceEndpointNotFoundException("The service endpoint could not be reached.");
        }
    }


    public async Task<String> TestDelete()
    {
        try
        {
            var testApiUrl = ServiceEndpoints.TestDelete;
            var headers = new Dictionary<string, string>
        {
            { "Custom-Header", "CustomValue" }
            // Add other headers as needed
        };

            var result = await _genericHttpClient.DeleteAsync(testApiUrl, headers);
            return result.ToString();
        }
        catch (HttpRequestException ex)
        {
            throw new ServiceEndpointNotFoundException("The service endpoint could not be reached.");
        }
    }
}

