using IstGuide.Application.Features.Specialties.Queries.GetAllSpecialties;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IstGuide.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class SpecialtiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpecialtiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Tüm uzmanlık alanlarını getir</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllSpecialtiesQuery(), ct);
        return Ok(result);
    }
}
