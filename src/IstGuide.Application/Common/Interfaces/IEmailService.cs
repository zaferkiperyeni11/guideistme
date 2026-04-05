namespace IstGuide.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default);
    Task SendTemplateEmailAsync(string to, string templateName, object model, CancellationToken ct = default);
}
