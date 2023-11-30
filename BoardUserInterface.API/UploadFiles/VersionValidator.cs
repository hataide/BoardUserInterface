using System.Text.RegularExpressions;

namespace BoardUserInterface.API.UploadFiles;

public interface IVersionValidator
{
    bool IsValidVersion(string version);
}

public class VersionValidator : IVersionValidator
{
    public bool IsValidVersion(string version)
    {
        var regex = new Regex(@"^\d+\.\d+$");
        return regex.IsMatch(version);
    }
}

