namespace IstGuide.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public class GuideNotFoundException : DomainException
{
    public GuideNotFoundException(Guid id) : base($"Guide with id {id} not found.") { }
    public GuideNotFoundException(string slug) : base($"Guide with slug '{slug}' not found.") { }
}

public class ReviewNotFoundException : DomainException
{
    public ReviewNotFoundException(Guid id) : base($"Review with id {id} not found.") { }
}

public class ContactRequestNotFoundException : DomainException
{
    public ContactRequestNotFoundException(Guid id) : base($"Contact request with id {id} not found.") { }
}
