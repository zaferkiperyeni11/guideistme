namespace IstGuide.Application.Common.Interfaces;

public interface IWhatsAppService
{
    string GenerateWhatsAppUrl(string phoneNumber);
    string GenerateWhatsAppUrlWithMessage(string phoneNumber, string message);
}
