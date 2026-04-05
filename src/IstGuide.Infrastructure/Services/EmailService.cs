using IstGuide.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace IstGuide.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        _logger.LogInformation("[EmailService] To: {To}, Subject: {Subject}, Body: {Body}", to, subject, body);
        return Task.CompletedTask;
    }

    public Task SendTemplateEmailAsync(string to, string templateName, object model, CancellationToken ct = default)
    {
        _logger.LogInformation("[EmailService] To: {To}, Template: {TemplateName}, Model: {Model}", to, templateName, model);
        return Task.CompletedTask;
    }
}
