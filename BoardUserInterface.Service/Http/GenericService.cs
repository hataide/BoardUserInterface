using BoardUserInterface.Common.Exceptions;
using BoardUserInterface.Service.Http.Response;
namespace BoardUserInterface.Service.Http;


public class GenericService : IGenericService
{
    private readonly IGenericHttpClient _genericHttpClient;

    public GenericService(IGenericHttpClient genericHttpClient)
    {
        _genericHttpClient = genericHttpClient;
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

            //var content = await _genericHttpClient.GetAsync(testApiUrl, headers);
            var content = await _genericHttpClient.GetAsync<DownloadResponse>(testApiUrl, headers);

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

            //var content = await _genericHttpClient.PostAsync(testApiUrl, sContent, headers);
            var content = await _genericHttpClient.PostAsync<String, String>(testApiUrl, sContent, headers);
            //var content = await _genericHttpClient.PostAsync<String, String>(testApiUrl, sContent);//PostAsync<MyRequestType, MyResponseType>(testApiUrl, sContent);

            return content.ToString();
        }
        catch (HttpRequestException ex)
        {
            throw new ServiceEndpointNotFoundException("The service endpoint could not be reached.");
        }
    }


    public async Task<string> PutTest(String data)
    {
        try
        {
            var testApiUrl = ServiceEndpoints.TestPut;
            //return await _genericHttpClient.PutAsync(testApiUrl, data);
            return await _genericHttpClient.PutAsync<String, String>(testApiUrl, data);
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

            //var result = await _genericHttpClient.DeleteAsync(testApiUrl, headers);
            var result = await _genericHttpClient.DeleteAsync<HttpResponseMessage>(testApiUrl, headers);
            return result.ToString();
        }
        catch (HttpRequestException ex)
        {
            throw new ServiceEndpointNotFoundException("The service endpoint could not be reached.");
        }
    }
}

