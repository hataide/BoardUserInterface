using BoardUserInterfaces.DataAccess.Models;

namespace BoardUserInterface.Service.DataAccess;

public interface IFilesAuditRepoService
{
    Task<FilesAudit> GetFilesAuditAsync(int id);
    Task<IEnumerable<FilesAudit>> GetAllFilesAuditsAsync();
    Task<FilesAudit> CreateFilesAuditAsync(FilesAudit filesAudit);
    Task<FilesAudit> UpdateFilesAuditAsync(FilesAudit filesAudit);
    Task DeleteFilesAuditAsync(int id);
}
