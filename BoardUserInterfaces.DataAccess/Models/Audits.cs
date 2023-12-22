using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BoardUserInterfaces.DataAccess.Models;

public class Audits
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AuditId { get; set; }
    public int FileAuditId { get; set; }

}
