using BoardUserInterface.Service.Models;

namespace BoardUserInterface.Service.Auditing;

public interface IAuditService
{
    void RecordAuditAsync(AuditRecord auditRecord);
}
