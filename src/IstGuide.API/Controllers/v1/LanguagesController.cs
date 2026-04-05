using IstGuide.Application.Features.Languages.Queries.GetAllLanguages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IstGuide.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class LanguagesController : ControllerBase
{
    private readonly IMediator _mediator;
    public LanguagesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllLanguagesQuery(), ct);
        return Ok(result);
    }
}
