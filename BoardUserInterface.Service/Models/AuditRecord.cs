namespace BoardUserInterface.Service.Models;

public class AuditRecord
{
    public string UserId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Action { get; set; }
    public string TableName { get; set; }
    public string RecordId { get; set; }
    public string NewValues { get; set; }
    public string OldValues { get; set; }
}
