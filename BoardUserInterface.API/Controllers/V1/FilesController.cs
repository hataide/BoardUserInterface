using BoardUserInterface.Service.DataAccess;
using BoardUserInterfaces.DataAccess.DTOs;
using BoardUserInterfaces.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace BoardUserInterface.API.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileRepoService _fileService;

    public FilesController(IFileRepoService fileService)
    {
        _fileService = fileService;
    }

    // POST api/test/files
    [HttpPost("files")]
    public async Task<IActionResult> CreateFile([FromBody] FilesDto filesDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdFile = await _fileService.CreateFileAsync(filesDto);
        return CreatedAtAction(nameof(GetFile), new { id = createdFile.FileId }, createdFile);
    }

    // GET api/test/files
    [HttpGet("files")]
    public async Task<IActionResult> GetAllAudits()
    {
        var audits = await _fileService.GetAllFilesAsync();
        return Ok(audits);
    }

    [HttpGet("files/{id}")]
    public async Task<IActionResult> GetFile(int id)
    {
        var file = await _fileService.GetFileAsync(id);
        if (file == null)
        {
            return NotFound();
        }
        return Ok(file);
    }

    // DELETE api/test/files/{id}
    [HttpDelete("files/{id}")]
    public async Task<IActionResult> DeleteAudit(int id)
    {
        var file = await _fileService.GetFileAsync(id);
        if (file == null)
        {
            return NotFound();
        }

        await _fileService.DeleteFileAsync(id);
        return NoContent();
    }

    // PUT api/test/file/{id}
    [HttpPut("file/{id}")]
    public async Task<IActionResult> UpdateAudit(int id, [FromBody] Files file)
    {
        if (id != file.FileId)
        {
            return BadRequest();
        }

        var existingAudit = await _fileService.GetFileAsync(id);
        if (existingAudit == null)
        {
            return NotFound();
        }

        await _fileService.UpdateFileAsync(file);
        return NoContent();
    }

    // Add other CRUD operations (GET all, PUT, DELETE) here as needed.
}
