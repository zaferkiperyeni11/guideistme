using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResultDto>>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResultDto>>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<AuthResultDto>> Handle(LoginCommand request, CancellationToken ct)
    {
        return await _identityService.AuthenticateAsync(request.Email, request.Password, ct);
    }
}
