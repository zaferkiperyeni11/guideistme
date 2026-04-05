using IstGuide.Application.Features.Auth.Commands.Login;
using IstGuide.Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IstGuide.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Kullanıcı Girişi (JWT Token Alır)</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        
        if (!result.Succeeded)
            return Unauthorized(new { errors = result.Errors });

        return Ok(result);
    }

    /// <summary>Yeni Kullanıcı Kaydı</summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct = default)
    {
        var result = await _mediator.Send(command, ct);
        
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors });

        return Ok(result);
    }
}
