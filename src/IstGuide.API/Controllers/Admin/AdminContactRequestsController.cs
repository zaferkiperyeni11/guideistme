using IstGuide.Application.Features.ContactRequests.Queries.GetContactRequestStats;
using IstGuide.Application.Features.ContactRequests.Queries.GetContactRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IstGuide.API.Controllers.Admin;

[ApiController]
[Route("api/admin/contact-requests")]
[Authorize(Roles = "Admin")]
public class AdminContactRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminContactRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Tüm iletişim taleplerini listele</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetContactRequestsQuery(), ct);
        return Ok(result);
    }

    /// <summary>İletişim talebi istatistikleri</summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetContactRequestStatsQuery(), ct);
        return Ok(result);
    }
}
