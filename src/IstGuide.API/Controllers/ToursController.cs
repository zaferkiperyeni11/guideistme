using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IstGuide.Domain.Entities;
using IstGuide.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/api/v1/tours")]
public class ToursController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public ToursController(IApplicationDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetTours()
    {
        var tours = await _context.Tours
            .Select(t => new { t.Id, t.Title, t.Description, t.Price, t.Duration, t.IsActive, t.GuideId, t.DistrictId, t.Slug })
            .ToListAsync();
        return Ok(new { succeeded = true, value = tours });
    }

    [HttpPost]
    public async Task<IActionResult> CreateTour([FromBody] CreateTourDto dto)
    {
        var tour = new Tour
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            Duration = dto.Duration,
            GuideId = dto.GuideId,
            Slug = dto.Title.ToLower().Replace(" ", "-"),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tours.Add(tour);
        await _context.SaveChangesAsync(default);
        return Ok(new { succeeded = true, value = tour.Id });
    }
}

public class CreateTourDto
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public string Duration { get; set; } = default!;
    public Guid GuideId { get; set; }
}