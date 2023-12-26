using Microsoft.AspNetCore.Mvc;
using BoardUserInterface.Service.DataAccess;
using BoardUserInterfaces.DataAccess.Models;
using BoardUserInterfaces.DataAccess.DTOs;

namespace BoardUserInterface.API.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class FilesAuditController : ControllerBase
{
    private readonly IFilesAuditRepoService _filesAuditService;

    public FilesAuditController(IFilesAuditRepoService filesAuditService)
    {
        _filesAuditService = filesAuditService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFilesAudit(int id)
    {
        var filesAudit = await _filesAuditService.GetFilesAuditAsync(id);
        if (filesAudit == null)
        {
            return NotFound();
        }
        return Ok(filesAudit);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFilesAudits()
    {
        var filesAudits = await _filesAuditService.GetAllFilesAuditsAsync();
        return Ok(filesAudits);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFilesAudit([FromBody] FilesAuditDto filesAuditDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var filesAudit = new FilesAudit
        {
            // Map properties from DTO to entity
            Timestamp = filesAuditDto.Timestamp,
            Action = filesAuditDto.Action,
            RecordId = filesAuditDto.RecordId,
            Content_old = filesAuditDto.Content_old,
            Content_new = filesAuditDto.Content_new,
            Version_old = filesAuditDto.Version_old,
            Version_new = filesAuditDto.Version_new,
            Name_old = filesAuditDto.Name_old,
            Name_new = filesAuditDto.Name_new,
            Extension_old = filesAuditDto.Extension_old,
            Extension_new = filesAuditDto.Extension_new
        };

        var createdFilesAudit = await _filesAuditService.CreateFilesAuditAsync(filesAudit);
        return CreatedAtAction(nameof(GetFilesAudit), new { id = createdFilesAudit.FileAuditId }, createdFilesAudit);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFilesAudit(int id, [FromBody] FilesAudit filesAudit)
    {
        if (id != filesAudit.FileAuditId)
        {
            return BadRequest();
        }

        var existingFilesAudit = await _filesAuditService.GetFilesAuditAsync(id);
        if (existingFilesAudit == null)
        {
            return NotFound();
        }

        await _filesAuditService.UpdateFilesAuditAsync(filesAudit);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilesAudit(int id)
    {
        var filesAudit = await _filesAuditService.GetFilesAuditAsync(id);
        if (filesAudit == null)
        {
            return NotFound();
        }

        await _filesAuditService.DeleteFilesAuditAsync(id);
        return NoContent();
    }
}
