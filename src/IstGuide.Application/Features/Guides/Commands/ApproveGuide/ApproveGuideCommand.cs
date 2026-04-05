using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Guides.Commands.ApproveGuide;

public record ApproveGuideCommand(Guid GuideId) : IRequest<Result>;
