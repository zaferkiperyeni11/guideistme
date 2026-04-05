using IstGuide.Application.Common.Models;
using MediatR;

namespace IstGuide.Application.Features.Tours.Commands.DeleteTour;

public record DeleteTourCommand(Guid TourId) : IRequest<Result>;
