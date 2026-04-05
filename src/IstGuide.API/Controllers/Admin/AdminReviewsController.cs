using IstGuide.Application.Features.Reviews.Commands.ApproveReview;
using IstGuide.Application.Features.Reviews.Commands.RejectReview;
using IstGuide.Application.Features.Reviews.Commands.ReplyToReview;
using IstGuide.Application.Features.Reviews.Queries.GetAllReviews;
using IstGuide.Application.Features.Reviews.Queries.GetPendingReviews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IstGuide.API.Controllers.Admin;

[ApiController]
[Route("api/admin/reviews")]
[Authorize(Roles = "Admin")]
public class AdminReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Tüm yorumları listele</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Domain.Enums.ReviewStatus? status = null,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllReviewsQuery { Status = status }, ct);
        return Ok(result);
    }

    /// <summary>Bekleyen yorumları listele</summary>
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetPendingReviewsQuery(), ct);
        return Ok(result);
    }

    /// <summary>Yorumu onayla</summary>
    [HttpPut("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ApproveReviewCommand(id), ct);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>Yorumu reddet</summary>
    [HttpPut("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new RejectReviewCommand(id), ct);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>Yoruma admin cevabı ver</summary>
    [HttpPut("{id:guid}/reply")]
    public async Task<IActionResult> Reply(Guid id, [FromBody] ReplyToReviewCommand command, CancellationToken ct = default)
    {
        command = command with { ReviewId = id };
        var result = await _mediator.Send(command, ct);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }
}
