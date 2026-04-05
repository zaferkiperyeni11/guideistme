using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Guides.Commands.ToggleFeaturedGuide;

public record ToggleFeaturedGuideCommand(Guid GuideId) : IRequest<Result<bool>>;
