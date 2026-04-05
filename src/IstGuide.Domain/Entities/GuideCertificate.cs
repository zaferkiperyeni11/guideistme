using IstGuide.Domain.Common;

namespace IstGuide.Domain.Entities;

public class GuideCertificate : BaseEntity
{
    public Guid GuideId { get; set; }
    public Guide Guide { get; set; } = default!;
    public string CertificateName { get; set; } = default!;
    public string? IssuingAuthority { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? DocumentUrl { get; set; }
    public bool IsVerified { get; set; }
}
