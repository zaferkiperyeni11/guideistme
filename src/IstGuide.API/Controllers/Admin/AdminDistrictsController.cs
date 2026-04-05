using IstGuide.Application.Features.Districts.Commands.CreateDistrict;
using IstGuide.Application.Features.Districts.Commands.UpdateDistrict;
using IstGuide.Application.Features.Districts.Queries.GetAllDistricts;
using IstGuide.Application.Features.Districts.Queries.GetPopularDistricts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IstGuide.API.Controllers.Admin;

[ApiController]
[Route("api/admin/districts")]
[Authorize(Roles = "Admin")]
public class AdminDistrictsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminDistrictsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Tüm ilçeleri listele</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllDistrictsQuery(), ct);
        return Ok(result);
    }

    /// <summary>Popüler ilçeleri getir</summary>
    [HttpGet("popular")]
    public async Task<IActionResult> GetPopular(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetPopularDistrictsQuery(), ct);
        return Ok(result);
    }

    /// <summary>İlçe oluştur</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDistrictCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        if (!result.Succeeded) return BadRequest(result);
        return CreatedAtAction(nameof(GetAll), result);
    }

    /// <summary>İlçe güncelle</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDistrictCommand command, CancellationToken ct = default)
    {
        command = command with { Id = id };
        var result = await _mediator.Send(command, ct);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }
}
