namespace BoardUserInterface.Service.Http;
public static class ServiceEndpoints
{
    public const string TestGet = "http://localhost:7165/control/Tget"; // The base URL of the external service
    public const string TestPost = "http://localhost:7165/control/Tpost";
    public const string TestPut = "http://localhost:7165/control/Tput";
    public const string TestDelete = "http://localhost:7165/control/Tdelete";
    public const string Upload = "http://localhost:5257/api/v1/template/upload";
    public const string Download = "http://localhost:5257/api/v1/template/download\"";
    public const string RemoveVersion = "/api/v1/template/remove-version";
    // Add other endpoint constants as needed
}

