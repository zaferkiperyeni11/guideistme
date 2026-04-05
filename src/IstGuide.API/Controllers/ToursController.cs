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
        var tours = await _context.Tours.ToListAsync();
        return Ok(new { succeeded = true, value = tours });
    }

    [HttpPost]
    public async Task<IActionResult> CreateTour([FromBody] Tour tour)
    {
        tour.Slug = tour.Title.ToLower().Replace(" ", "-");
        _context.Tours.Add(tour);
        await _context.SaveChangesAsync(default);
        return Ok(new { succeeded = true, value = tour });
    }
}
