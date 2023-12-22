using System.Net.Http;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using BoardUserInterface.Service.Http.Response;
using BoardUserInterface.FileService.Service;
using Microsoft.Extensions.Logging;
using Serilog.Core;


namespace BoardUserInterface.Service.Http;

public class GenericHttpClient : IGenericHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<IFileService> _logger;

    public GenericHttpClient(IHttpClientFactory httpClientFactory, ILogger<IFileService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<DownloadResponse> GetAsync(string requestUri, Dictionary<string, string> headers = null)
    {
        var client = _httpClientFactory.CreateClient();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        var response = await client.GetAsync(requestUri);
        if (!response.IsSuccessStatusCode)
        {
            // Log error details here
            throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();

        // Log the raw JSON content for inspection
        _logger.LogInformation("GenericHttpClient", "GetAsync", $"Raw JSON content: {content}", "Information");

        try
        {
            // Configure JsonSerializerOptions if necessary
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // This will handle case differences between JSON keys and class properties
            };

            var downloadResponse = JsonSerializer.Deserialize<DownloadResponse>(content, options);
            if (downloadResponse == null)
            {
                // Handle or log the error if deserialization returns null
                throw new InvalidOperationException("Deserialization of the response content failed.");
            }

            return downloadResponse;
        }
        catch (JsonException jsonEx)
        {
            // Handle JSON deserialization errors
            throw new InvalidOperationException("Error deserializing the response content.", jsonEx);
        }
    }


    public async Task<HttpResponseMessage> DeleteAsync(string requestUri, Dictionary<string, string> headers = null)
    {
        var client = _httpClientFactory.CreateClient();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
        var response = await client.DeleteAsync(requestUri);
        if (!response.IsSuccessStatusCode)
        {
            // Log error details here
            throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
        }
        return response;
    }

    public async Task<string> PostAsync<TRequest>(string requestUri, TRequest content, Dictionary<string, string> headers = null)
    {
        // Create an HttpClient instance using the IHttpClientFactory.
        var client = _httpClientFactory.CreateClient();

        // Serialize the request content to JSON.
        var jsonContent = new StringContent(JsonSerializer.Serialize(content), System.Text.Encoding.UTF8, "application/json");

        // Add any custom headers to the request.
        if (headers != null)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        // Send the POST request.
        var response = await client.PostAsync(requestUri, jsonContent);

        // Ensure we got a successful response.
        if (!response.IsSuccessStatusCode)
        {
            // If not successful, log and throw an exception with the status code.
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Error posting data. Status code: {response.StatusCode}, Content: {errorContent}");
            throw new HttpRequestException($"Error posting data. Status code: {response.StatusCode}, Content: {errorContent}");
        }

        // Read and return the response content as a JSON string.
        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }

    public async Task<string> PutAsync<TRequest>(string requestUri, TRequest content, Dictionary<string, string> headers = null)
    {
        var client = _httpClientFactory.CreateClient();
        // Serialize the request content to JSON
        var jsonContent = new StringContent(JsonSerializer.Serialize(content), System.Text.Encoding.UTF8, "application/json");
        // Add any custom headers to the request
        if (headers != null)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
        // Send the PUT request
        var response = await client.PutAsync(requestUri, jsonContent);
        // Ensure we got a successful response
        if (!response.IsSuccessStatusCode)
        {
            // If not successful, log and throw an exception with the status code
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Error putting data. Status code: {response.StatusCode}, Content: {errorContent}");
            throw new HttpRequestException($"Error putting data. Status code: {response.StatusCode}, Content: {errorContent}");
        }
        // Read and return the response content as a JSON string
        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }


}
