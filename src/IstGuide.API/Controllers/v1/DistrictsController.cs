using IstGuide.Application.Features.Districts.Queries.GetAllDistricts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IstGuide.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class DistrictsController : ControllerBase
{
    private readonly IMediator _mediator;
    public DistrictsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Tüm ilçeleri getir</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllDistrictsQuery(), ct);
        return Ok(result);
    }

    /// <summary>Popüler ilçeler</summary>
    [HttpGet("popular")]
    public async Task<IActionResult> GetPopular(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllDistrictsQuery(true), ct);
        return Ok(result);
    }
}
