using Microsoft.AspNetCore.Mvc;
using BoardUserInterface.Service.DataAccess;
using BoardUserInterfaces.DataAccess.Models;

namespace BoardUserInterface.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IAuditRepoService _auditService;
    private readonly IFileRepoService _fileService;
    private readonly IFilesAuditRepoService _filesAuditService;
    private readonly ILogsRepoService _logsService;

    public TestController(
        IAuditRepoService auditService,
        IFileRepoService fileService,
        IFilesAuditRepoService filesAuditService,
        ILogsRepoService logsService)
    {
        _auditService = auditService;
        _fileService = fileService;
        _filesAuditService = filesAuditService;
        _logsService = logsService;
    }


    // GET api/test/audit/{id}
    [HttpGet("audit/{id}")]
    public async Task<IActionResult> GetAudit(int id)
    {
        var audit = await _auditService.GetAuditAsync(id);
        if (audit == null)
        {
            return NotFound();
        }
        return Ok(audit);
    }

    // GET api/test/audits
    [HttpGet("audits")]
    public async Task<IActionResult> GetAllAudits()
    {
        var audits = await _auditService.GetAllAuditsAsync();
        return Ok(audits);
    }

    [HttpPost("audit")]
    public async Task<IActionResult> CreateAudit([FromBody] AuditDto auditDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var audit = new Audits
        {
            // Map properties from DTO to entity, except for AuditId which should be auto-generated.
            FileAuditId = auditDto.FileAuditId,
            // ... map other properties as needed ...
        };

        var createdAudit = await _auditService.CreateAuditAsync(audit);
        return CreatedAtAction(nameof(GetAudit), new { id = createdAudit.AuditId }, createdAudit);
    }


    // PUT api/test/audit/{id}
    [HttpPut("audit/{id}")]
    public async Task<IActionResult> UpdateAudit(int id, [FromBody] Audits audit)
    {
        if (id != audit.AuditId)
        {
            return BadRequest();
        }

        var existingAudit = await _auditService.GetAuditAsync(id);
        if (existingAudit == null)
        {
            return NotFound();
        }

        await _auditService.UpdateAuditAsync(audit);
        return NoContent();
    }

    // DELETE api/test/audit/{id}
    [HttpDelete("audit/{id}")]
    public async Task<IActionResult> DeleteAudit(int id)
    {
        var audit = await _auditService.GetAuditAsync(id);
        if (audit == null)
        {
            return NotFound();
        }

        await _auditService.DeleteAuditAsync(id);
        return NoContent();
    }

}
