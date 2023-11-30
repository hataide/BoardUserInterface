namespace BoardUserInterface.API.UploadFiles;

public interface IVersionComparer
{
    bool CompareVersions(string uploadedVersion, string lastVersion);
}

public class VersionComparer : IVersionComparer
{
    public bool CompareVersions(string uploadedVersion, string lastVersion)
    {
        var _uploadedVersion = uploadedVersion.Split('.').Select(int.Parse).ToArray();
        var _repositoryVersion = lastVersion.Split('.').Select(int.Parse).ToArray();
        
        if (_uploadedVersion[0] < _repositoryVersion[0])
        {
            return false; // lastVersion is higher
        }
        
        if (_uploadedVersion[0] == _repositoryVersion[0] && _uploadedVersion[1] <= _repositoryVersion[1])
        {
            return false; // uploadedVersion is higher
        }

        return true; // uploadedVersion is equal or lower than lastVersion

        
    }
}
