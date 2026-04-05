using IstGuide.Application.Features.Pages.Commands.UpdatePageContent;
using IstGuide.Application.Features.Pages.Queries.GetPageContent;
using IstGuide.Application.Features.Specialties.Commands.CreateSpecialty;
using IstGuide.Application.Features.Specialties.Commands.UpdateSpecialty;
using IstGuide.Application.Features.Specialties.Queries.GetAllSpecialties;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IstGuide.API.Controllers.Admin;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminSettingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminSettingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ─── Specialties ───

    /// <summary>Tüm uzmanlık alanlarını listele</summary>
    [HttpGet("specialties")]
    public async Task<IActionResult> GetAllSpecialties(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllSpecialtiesQuery(), ct);
        return Ok(result);
    }

    /// <summary>Uzmanlık alanı oluştur</summary>
    [HttpPost("specialties")]
    public async Task<IActionResult> CreateSpecialty([FromBody] CreateSpecialtyCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        if (!result.Succeeded) return BadRequest(result);
        return CreatedAtAction(nameof(GetAllSpecialties), result);
    }

    /// <summary>Uzmanlık alanı güncelle</summary>
    [HttpPut("specialties/{id:guid}")]
    public async Task<IActionResult> UpdateSpecialty(Guid id, [FromBody] UpdateSpecialtyCommand command, CancellationToken ct = default)
    {
        command = command with { Id = id };
        var result = await _mediator.Send(command, ct);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    // ─── Pages (CMS) ───

    /// <summary>Sayfa içeriğini getir</summary>
    [HttpGet("pages/{key}")]
    public async Task<IActionResult> GetPage(string key, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetPageContentQuery(key), ct);
        if (!result.Succeeded) return NotFound(result);
        return Ok(result);
    }

    /// <summary>Sayfa içeriğini güncelle</summary>
    [HttpPut("pages/{key}")]
    public async Task<IActionResult> UpdatePage(string key, [FromBody] UpdatePageContentCommand command, CancellationToken ct = default)
    {
        command = command with { Key = key };
        var result = await _mediator.Send(command, ct);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }
}
