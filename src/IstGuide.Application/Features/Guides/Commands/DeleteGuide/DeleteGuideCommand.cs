using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Guides.Commands.DeleteGuide;

public record DeleteGuideCommand(Guid GuideId) : IRequest<Result>;
