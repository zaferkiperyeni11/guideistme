using IstGuide.Application.Features.Guides.Commands.DeleteGuide;
using IstGuide.Application.Features.Guides.Commands.RegisterGuide;
using IstGuide.Application.Features.Guides.Commands.UpdateGuideProfile;
using IstGuide.Application.Features.Guides.Queries.GetApprovedGuides;
using IstGuide.Application.Features.Guides.Queries.GetFeaturedGuides;
using IstGuide.Application.Features.Guides.Queries.GetGuideBySlug;
using IstGuide.Application.Features.Guides.Queries.SearchGuides;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IstGuide.API.Controllers.v1;

[ApiController]
[Route("api/v1/guides")]
public class GuidesController : ControllerBase
{
    private readonly IMediator _mediator;

    public GuidesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Rehber ara ve filtrele (herkese açık)</summary>
    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string? searchTerm,
        [FromQuery] Guid? districtId,
        [FromQuery] Guid? specialtyId,
        [FromQuery] Guid? languageId,
        [FromQuery] double? minRating,
        [FromQuery] string? sortBy,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new SearchGuidesQuery
        {
            Criteria = new IstGuide.Domain.Repositories.GuideSearchCriteria
            {
                SearchTerm = searchTerm,
                DistrictId = districtId,
                SpecialtyId = specialtyId,
                LanguageId = languageId,
                MinRating = minRating,
                SortBy = sortBy,
                Page = page,
                PageSize = pageSize
            }
        }, ct);

        return Ok(result);
    }

    /// <summary>Öne çıkan rehberler (herkese açık)</summary>
    [HttpGet("featured")]
    public async Task<IActionResult> GetFeatured(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetFeaturedGuidesQuery(), ct);
        return Ok(result);
    }

    /// <summary>Rehber profili slug ile getir (herkese açık)</summary>
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetGuideBySlugQuery(slug), ct);
        if (result == null) return NotFound();
        return Ok(result);
    }

    /// <summary>Yeni rehber kaydı (herkese açık)</summary>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterGuideCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        if (!result.Succeeded) return BadRequest(result);
        return CreatedAtAction(nameof(GetBySlug), new { slug = result.Value }, result);
    }

    /// <summary>Rehber profilini güncelle (Kimlik doğrulama gerekli)</summary>
    [HttpPut("{id:guid}/profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateGuideProfileCommand command, CancellationToken ct = default)
    {
        command = command with { GuideId = id };
        var result = await _mediator.Send(command, ct);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>Rehberi sil (Kimlik doğrulama gerekli - Soft Delete)</summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteGuide(Guid id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new DeleteGuideCommand(id), ct);
        if (!result.Succeeded) return BadRequest(result);
        return NoContent();
    }

    /// <summary>Rehbere Fotoğraf Yükle</summary>
    [HttpPost("{id:guid}/photos")]
    [Authorize]
    public async Task<IActionResult> UploadPhoto(Guid id, IFormFile file, [FromQuery] bool isPrimary = false, CancellationToken ct = default)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { Message = "Lütfen geçerli bir dosya seçin." });

        using var stream = file.OpenReadStream();
        var command = new IstGuide.Application.Features.Guides.Commands.UploadGuidePhoto.UploadGuidePhotoCommand(id, stream, file.FileName, isPrimary);
        var result = await _mediator.Send(command, ct);

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>Rehber Fotoğrafı Sil</summary>
    [HttpDelete("{id:guid}/photos/{photoId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeletePhoto(Guid id, Guid photoId, CancellationToken ct = default)
    {
        var command = new IstGuide.Application.Features.Guides.Commands.DeleteGuidePhoto.DeleteGuidePhotoCommand(id, photoId);
        var result = await _mediator.Send(command, ct);

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }
}
