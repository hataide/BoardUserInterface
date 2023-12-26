using BoardUserInterface.Service.DataAccess;
using BoardUserInterfaces.DataAccess.DTOs;
using BoardUserInterfaces.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace BoardUserInterface.API.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class LogsController : ControllerBase
{
    private readonly ILogsRepoService _logsService;

    public LogsController(ILogsRepoService logsService)
    {
        _logsService = logsService;
    }

    [HttpGet("controllerRepo/{id}")]
    public async Task<IActionResult> GetLog(int id)
    {
        var log = await _logsService.GetLogAsync(id);
        if (log == null)
        {
            return NotFound();
        }
        return Ok(log);
    }

    [HttpGet("controllerRepo")]
    public async Task<IActionResult> GetAllLogs()
    {
        var logs = await _logsService.GetAllLogsAsync();
        return Ok(logs);
    }

    [HttpPost("controllerRepo")]
    public async Task<IActionResult> CreateLog([FromBody] LogsDto logsDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var log = new Logs
        {
            // Map properties from DTO to entity
            Context = logsDto.Context,
            Message = logsDto.Message,
            Type = logsDto.Type,
            Source = logsDto.Source
        };

        var createdLog = await _logsService.CreateLogAsync(log);
        return CreatedAtAction(nameof(GetLog), new { id = createdLog.LogId }, createdLog);
    }

    [HttpPut("controllerRepo/{id}")]
    public async Task<IActionResult> UpdateLog(int id, [FromBody] Logs log)
    {
        if (id != log.LogId)
        {
            return BadRequest();
        }

        var existingLog = await _logsService.GetLogAsync(id);
        if (existingLog == null)
        {
            return NotFound();
        }

        await _logsService.UpdateLogAsync(log);
        return NoContent();
    }

    [HttpDelete("controllerRepo/{id}")]
    public async Task<IActionResult> DeleteLog(int id)
    {
        var log = await _logsService.GetLogAsync(id);
        if (log == null)
        {
            return NotFound();
        }

        await _logsService.DeleteLogAsync(id);
        return NoContent();
    }
}
