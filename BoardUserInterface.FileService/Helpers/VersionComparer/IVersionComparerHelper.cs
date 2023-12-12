namespace BoardUserInterface.FileService.Helpers.VersionComparer.VersionComparer;

public interface IVersionComparerHelper
{
    bool CompareVersions(string uploadedVersion, string lastVersion);
}
