using BoardUserInterface.Repositories;
using BoardUserInterfaces.DataAccess.Models;

namespace BoardUserInterface.Service.DataAccess;

public class FilesAuditRepoService : IFilesAuditRepoService
{
    private readonly IRepository<FilesAudit> _filesAuditRepository;

    public FilesAuditRepoService(IRepository<FilesAudit> filesAuditRepository)
    {
        _filesAuditRepository = filesAuditRepository;
    }

    public async Task<FilesAudit> GetFilesAuditAsync(int id)
    {
        return await _filesAuditRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<FilesAudit>> GetAllFilesAuditsAsync()
    {
        return await _filesAuditRepository.GetAllAsync();
    }

    public async Task<FilesAudit> CreateFilesAuditAsync(FilesAudit filesAudit)
    {
        await _filesAuditRepository.AddAsync(filesAudit);
        return filesAudit;
    }

    public async Task<FilesAudit> UpdateFilesAuditAsync(FilesAudit filesAudit)
    {
        await _filesAuditRepository.UpdateAsync(filesAudit);
        return filesAudit;
    }

    public async Task DeleteFilesAuditAsync(int id)
    {
        var filesAudit = await _filesAuditRepository.GetByIdAsync(id);
        if (filesAudit != null)
        {
            await _filesAuditRepository.DeleteAsync(filesAudit);
        }
    }
}
