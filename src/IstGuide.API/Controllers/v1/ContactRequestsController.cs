using IstGuide.Application.Features.ContactRequests.Commands.CreateContactRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IstGuide.API.Controllers.v1;

[ApiController]
[Route("api/v1/contact-requests")]
public class ContactRequestsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ContactRequestsController(IMediator mediator) => _mediator = mediator;

    /// <summary>İletişim talebi oluştur (herkese açık)</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactRequestCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }
}
