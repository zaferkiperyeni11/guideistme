namespace IstGuide.Domain.Enums;

public enum Gender
{
    Male = 0,
    Female = 1,
    Other = 2,
    PreferNotToSay = 3
}

public enum LanguageProficiency
{
    Native = 0,
    Fluent = 1,
    Intermediate = 2,
    Basic = 3
}

public enum ReviewStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}

public enum ContactRequestStatus
{
    New = 0,
    Viewed = 1,
    Replied = 2,
    Converted = 3,
    Closed = 4
}

public enum ContactSource
{
    Website = 0,
    WhatsApp = 1,
    Direct = 2,
    Referral = 3
}
