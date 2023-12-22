using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BoardUserInterfaces.DataAccess.Models;

public class FilesAudit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FileAuditId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Action { get; set; } = string.Empty;
    public int RecordId { get; set; }
    public byte[]? Content_old { get; set; }
    public byte[]? Content_new { get; set; }
    public string Version_old { get; set; } = string.Empty;
    public string Version_new { get; set; } = string.Empty;
    public string Name_old { get; set; } = string.Empty;
    public string Name_new { get; set; } = string.Empty;
    public string Extension_old { get; set; } = string.Empty;
    public string Extension_new { get; set; } = string.Empty;

}
