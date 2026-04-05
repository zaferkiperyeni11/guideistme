using IstGuide.Application.Common.Interfaces;
using IstGuide.Application.Common.Models;
using IstGuide.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IstGuide.Application.Features.ContactRequests.Commands.UpdateContactRequestStatus;

public record UpdateContactRequestStatusCommand : IRequest<Result>
{
    public Guid RequestId { get; init; }
    public ContactRequestStatus Status { get; init; }
}

public class UpdateContactRequestStatusCommandHandler : IRequestHandler<UpdateContactRequestStatusCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactRequestStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateContactRequestStatusCommand request, CancellationToken ct)
    {
        var contactRequest = await _context.ContactRequests
            .FirstOrDefaultAsync(x => x.Id == request.RequestId, ct)
            ?? throw new ApplicationException("İletişim talebi bulunamadı.");

        contactRequest.Status = request.Status;
        contactRequest.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

        return Result.Success();
    }
}
