using BoardUserInterface.Repositories;
using BoardUserInterfaces.DataAccess.Models;

namespace BoardUserInterface.Service.DataAccess;

public class AuditRepoService : IAuditRepoService
{
    private readonly IRepository<Audits> _auditRepository;

    public AuditRepoService(IRepository<Audits> auditRepository)
    {
        _auditRepository = auditRepository;
    }

    public async Task<Audits> GetAuditAsync(int id)
    {
        return await _auditRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Audits>> GetAllAuditsAsync()
    {
        return await _auditRepository.GetAllAsync();
    }

    public async Task<Audits> CreateAuditAsync(Audits audit)
    {
        await _auditRepository.AddAsync(audit);
        return audit; // Assuming the AddAsync method sets the ID on the entity
    }

    public async Task<Audits> UpdateAuditAsync(Audits audit)
    {
        await _auditRepository.UpdateAsync(audit);
        return audit;
    }

    public async Task DeleteAuditAsync(int id)
    {
        var audit = await _auditRepository.GetByIdAsync(id);
        if (audit != null)
        {
            await _auditRepository.DeleteAsync(audit);
        }
    }
}