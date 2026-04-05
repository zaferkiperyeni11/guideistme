using IstGuide.Domain.Common;

namespace IstGuide.Domain.Events;

public record GuideRegisteredEvent(Guid GuideId) : IDomainEvent;
public record GuideApprovedEvent(Guid GuideId) : IDomainEvent;
public record GuideRejectedEvent(Guid GuideId, string Reason) : IDomainEvent;
public record ReviewSubmittedEvent(Guid ReviewId, Guid GuideId) : IDomainEvent;
public record ContactRequestCreatedEvent(Guid RequestId, Guid GuideId) : IDomainEvent;
