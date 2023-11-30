namespace BoardUserInterface.API.UploadFiles;
public class VersionComparer
{
    public bool CompareVersions(string uploadedVersion, string lastVersion)
    {
        var version1Parts = uploadedVersion.Split('.').Select(int.Parse).ToArray();
        var version2Parts = lastVersion.Split('.').Select(int.Parse).ToArray();

        // Compare major version parts
        if (version1Parts[0] > version2Parts[0])
        {
            return true; // uploadedVersion is higher
        }
        else if (version1Parts[0] < version2Parts[0])
        {
            return false; // lastVersion is higher
        }
        else
        {
            if (version1Parts[1] > version2Parts[1])
            {
                return true; // uploadedVersion is higher
            }
            else
            {
                return false; // uploadedVersion is equal or lower than lastVersion
            }
        }
    }
}
