using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Guides.Commands.RejectGuide;

public record RejectGuideCommand(Guid GuideId, string Reason) : IRequest<Result>;
