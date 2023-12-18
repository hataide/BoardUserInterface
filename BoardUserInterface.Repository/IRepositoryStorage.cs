using BoardUserInterface.Repository.Models;

namespace BoardUserInterface.Repository;
public interface IRepositoryStorage
{
    List<RepositoryTemplateInformation> Read();
    void Save(RepositoryTemplateInformation fileData);

    string GetLatestVersionNumber();
    void RemoveFile(string fileName);

    void RemoveAllFiles();
    string GetLastFileName();

    RepositoryTemplateInformation GetLatestFile();
}

