using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Features.Guides.Commands.ApproveGuide;
using IstGuide.Application.Features.Guides.Commands.DeleteGuide;
using IstGuide.Application.Features.Guides.Commands.RejectGuide;
using IstGuide.Application.Features.Guides.Commands.ToggleFeaturedGuide;
using IstGuide.Application.Features.Guides.Queries.GetAllGuidesAdmin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IstGuide.API.Controllers.Admin;

[ApiController]
[Route("api/admin/guides")]
[Authorize(Roles = "Admin")]
public class AdminGuidesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IApplicationDbContext _context;

    public AdminGuidesController(IMediator mediator, IApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    /// <summary>Tüm rehberleri listele (admin)</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Domain.Enums.GuideStatus? status = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllGuidesAdminQuery { Status = status, SearchTerm = searchTerm }, ct);
        return Ok(result);
    }

    /// <summary>Rehber detayı (admin)</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllGuidesAdminQuery(), ct);
        var guide = result.FirstOrDefault(g => g.Id == id);
        if (guide == null) return NotFound();
        return Ok(guide);
    }

    /// <summary>Rehberi onayla</summary>
    [HttpPut("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ApproveGuideCommand(id), ct);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>Rehberi reddet</summary>
    [HttpPut("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id, [FromBody] RejectGuideCommand command, CancellationToken ct = default)
    {
        command = command with { GuideId = id };
        var result = await _mediator.Send(command, ct);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>Rehberi askıya al</summary>
    [HttpPut("{id:guid}/suspend")]
    public async Task<IActionResult> Suspend(Guid id, CancellationToken ct = default)
    {
        var guide = await _context.Guides.FindAsync(new object[] { id }, ct);
        if (guide == null) return NotFound();
        guide.Status = Domain.Enums.GuideStatus.Suspended;
        guide.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
        return Ok(new { Message = "Rehber askıya alındı." });
    }

    /// <summary>Rehberi öne çıkar/geri al</summary>
    [HttpPut("{id:guid}/toggle-featured")]
    public async Task<IActionResult> ToggleFeatured(Guid id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ToggleFeaturedGuideCommand(id), ct);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>Rehberi sil (Admin - Soft Delete)</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGuide(Guid id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new DeleteGuideCommand(id), ct);
        if (!result.Succeeded) return BadRequest(result);
        return NoContent();
    }
}
