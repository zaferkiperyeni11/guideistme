using IstGuide.Application.Features.Districts.Queries.GetAllDistricts;
using MediatR;

namespace IstGuide.Application.Features.Districts.Queries.GetPopularDistricts;

public record GetPopularDistrictsQuery : IRequest<IReadOnlyList<DistrictDto>>;
