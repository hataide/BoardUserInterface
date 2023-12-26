using BoardUserInterfaces.DataAccess.Models;

namespace BoardUserInterface.Service.DataAccess;

public interface IFileRepoService
{
    Task<Files> GetFileAsync(int id);
    Task<IEnumerable<Files>> GetAllFilesAsync();
    Task<Files> CreateFileAsync(FilesDto file);
    Task<Files> UpdateFileAsync(Files file);
    Task DeleteFileAsync(int id);
}
