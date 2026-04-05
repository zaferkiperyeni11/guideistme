using IstGuide.Application.Features.Reviews.Commands.SubmitReview;
using IstGuide.Application.Features.Reviews.Queries.GetGuideReviews;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IstGuide.API.Controllers.v1;

[ApiController]
[Route("api/v1/guides/{guideId}/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Rehberin yorumlarını getir</summary>
    [HttpGet]
    public async Task<IActionResult> GetByGuideId(Guid guideId, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetGuideReviewsQuery(guideId), ct);
        return Ok(result);
    }

    /// <summary>Rehbere Yorum Gönder</summary>
    [HttpPost]
    public async Task<IActionResult> Submit(Guid guideId, [FromBody] SubmitReviewCommand command, CancellationToken ct = default)
    {
        if (guideId != command.GuideId)
            return BadRequest(new { Message = "URL ID ile Payload ID eşleşmiyor." });

        var result = await _mediator.Send(command, ct);
        
        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }
}
