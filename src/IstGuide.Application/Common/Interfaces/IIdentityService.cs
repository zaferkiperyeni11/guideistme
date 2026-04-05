using IstGuide.Application.Common.Models;

namespace IstGuide.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<AuthResultDto>> AuthenticateAsync(string email, string password, CancellationToken ct = default);
    Task<Result<Guid>> RegisterUserAsync(string email, string password, string firstName, string lastName, CancellationToken ct = default);
}

public class AuthResultDto
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string[] Roles { get; set; } = Array.Empty<string>();
}
