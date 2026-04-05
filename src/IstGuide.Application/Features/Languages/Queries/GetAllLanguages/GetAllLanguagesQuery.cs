using MediatR;

namespace IstGuide.Application.Features.Languages.Queries.GetAllLanguages;

public record GetAllLanguagesQuery : IRequest<IReadOnlyList<LanguageDto>>;

public class LanguageDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string NativeName { get; set; } = default!;
    public string? FlagIconUrl { get; set; }
}
