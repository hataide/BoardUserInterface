using BoardUserInterfaces.DataAccess.Models;

namespace BoardUserInterface.Service.DataAccess;

public interface IDataAccessService
{
    Task<IEnumerable<FilesAudit>> GetAllFilesAuditAsync();
}
