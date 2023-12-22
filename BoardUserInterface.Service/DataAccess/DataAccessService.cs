using BoardUserInterface.Repositories;
using BoardUserInterfaces.DataAccess.Models;

namespace BoardUserInterface.Service.DataAccess;

public class DataAccessService : IDataAccessService
{
    private readonly IRepository<FilesAudit> _filesAuditRepository;

    public DataAccessService(IRepository<FilesAudit> filesAuditRepository)
    {
        _filesAuditRepository = filesAuditRepository;
    }

    public async Task<IEnumerable<FilesAudit>> GetAllFilesAuditAsync()
    {
        return await _filesAuditRepository.GetAllAsync();
    }
}
