using IstGuide.Domain.Common;

namespace IstGuide.Domain.ValueObjects;

public class PhoneNumber : ValueObject
{
    public string CountryCode { get; }
    public string Number { get; }

    public PhoneNumber(string countryCode, string number)
    {
        CountryCode = countryCode;
        Number = number;
    }

    public string ToWhatsAppUrl() => $"https://wa.me/{CountryCode.TrimStart('+')}{Number}";
    public string ToFormatted() => $"{CountryCode} {Number}";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CountryCode;
        yield return Number;
    }
}
