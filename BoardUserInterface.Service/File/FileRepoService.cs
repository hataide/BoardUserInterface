using BoardUserInterface.FileService.Service;
using BoardUserInterface.Repositories;
using BoardUserInterfaces.DataAccess.Models;

namespace BoardUserInterface.Service.DataAccess;

public class FileRepoService : IFileRepoService
{
    private readonly IRepository<Files> _fileRepository;

    public FileRepoService(IRepository<Files> fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<Files> GetFileAsync(int id)
    {
        return await _fileRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Files>> GetAllFilesAsync()
    {
        return await _fileRepository.GetAllAsync();
    }

    public async Task<Files> CreateFileAsync(Files file)
    {
        await _fileRepository.AddAsync(file);
        return file;
    }

    public async Task<Files> UpdateFileAsync(Files file)
    {
        await _fileRepository.UpdateAsync(file);
        return file;
    }

    public async Task DeleteFileAsync(int id)
    {
        var file = await _fileRepository.GetByIdAsync(id);
        if (file != null)
        {
            await _fileRepository.DeleteAsync(file);
        }
    }
}
