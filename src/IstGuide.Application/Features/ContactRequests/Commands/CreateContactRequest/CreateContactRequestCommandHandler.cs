using IstGuide.Application.Common.Exceptions;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Common;
using IstGuide.Domain.Entities;
using IstGuide.Domain.Events;
using IstGuide.Domain.Repositories;
using MediatR;

namespace IstGuide.Application.Features.ContactRequests.Commands.CreateContactRequest;

public class CreateContactRequestCommandHandler : IRequestHandler<CreateContactRequestCommand, Result<Guid>>
{
    private readonly IContactRequestRepository _contactRequestRepository;
    private readonly IGuideRepository _guideRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateContactRequestCommandHandler(
        IContactRequestRepository contactRequestRepository,
        IGuideRepository guideRepository,
        IUnitOfWork unitOfWork)
    {
        _contactRequestRepository = contactRequestRepository;
        _guideRepository = guideRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateContactRequestCommand request, CancellationToken ct)
    {
        var guide = await _guideRepository.GetByIdAsync(request.GuideId, ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.Guide), request.GuideId);

        var contactRequest = new ContactRequest
        {
            GuideId = request.GuideId,
            VisitorName = request.VisitorName,
            VisitorEmail = request.VisitorEmail,
            VisitorPhone = request.VisitorPhone,
            Message = request.Message,
            PreferredDate = request.PreferredDate,
            GroupSize = request.GroupSize,
            Source = request.Source
        };

        contactRequest.AddDomainEvent(new ContactRequestCreatedEvent(contactRequest.Id, contactRequest.GuideId));

        await _contactRequestRepository.AddAsync(contactRequest, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<Guid>.Success(contactRequest.Id);
    }
}
