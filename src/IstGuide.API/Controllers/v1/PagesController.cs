using IstGuide.Application.Features.Pages.Queries.GetPageContent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IstGuide.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class PagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Statik Sayfa İçeriğini Getir (Hakkımızda, Gizlilik vs.)
    /// </summary>
    [HttpGet("{key}")]
    public async Task<IActionResult> Get(string key, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetPageContentQuery(key), ct);
        
        if (!result.Succeeded)
            return NotFound(result);

        return Ok(result);
    }
}
