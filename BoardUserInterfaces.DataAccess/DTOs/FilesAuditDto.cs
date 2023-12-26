public class FilesAuditDto
{
    public DateTime Timestamp { get; set; }
    public string Action { get; set; }
    public int RecordId { get; set; }
    public byte[]? Content_old { get; set; }
    public byte[]? Content_new { get; set; }
    public string Version_old { get; set; }
    public string Version_new { get; set; }
    public string Name_old { get; set; }
    public string Name_new { get; set; }
    public string Extension_old { get; set; }
    public string Extension_new { get; set; }
}