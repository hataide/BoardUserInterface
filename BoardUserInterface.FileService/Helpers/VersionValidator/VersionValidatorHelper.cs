using System.Text.RegularExpressions;

namespace BoardUserInterface.FileService.Helpers.VersionValidator;

public class VersionValidatorHelper : IVersionValidatorHelper
{
    public bool IsValidVersion(string version)
    {
        var regex = new Regex(@"^\d+\.\d+$");
        return regex.IsMatch(version);
    }
}

