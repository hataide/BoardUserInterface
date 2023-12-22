using BoardUserInterfaces.DataAccess.Models;

namespace BoardUserInterface.Service.DataAccess;

public interface IAuditRepoService
{
    Task<Audits> GetAuditAsync(int id);
    Task<IEnumerable<Audits>> GetAllAuditsAsync();
    Task<Audits> CreateAuditAsync(Audits audit);
    Task<Audits> UpdateAuditAsync(Audits audit);
    Task DeleteAuditAsync(int id);
}
