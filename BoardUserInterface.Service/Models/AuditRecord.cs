namespace BoardUserInterface.Service.Models;

public class AuditRecord
{
    public string Files_Audit_Id { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Action { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string RecordId { get; set; } = string.Empty;
    public string NewValues { get; set; } = string.Empty;
    public string OldValues { get; set; } = string.Empty;
}
