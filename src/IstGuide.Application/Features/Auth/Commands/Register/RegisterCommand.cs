using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Auth.Commands.Register;

public record RegisterCommand : IRequest<Result<Guid>>
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<Guid>>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken ct)
    {
        return await _identityService.RegisterUserAsync(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            ct);
    }
}
