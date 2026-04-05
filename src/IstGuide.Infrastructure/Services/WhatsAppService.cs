using System.Web;
using IstGuide.Application.Common.Interfaces;

namespace IstGuide.Infrastructure.Services;

public class WhatsAppService : IWhatsAppService
{
    public string GenerateWhatsAppUrl(string phoneNumber)
    {
        var cleanNumber = phoneNumber.TrimStart('+');
        return $"https://wa.me/{cleanNumber}";
    }

    public string GenerateWhatsAppUrlWithMessage(string phoneNumber, string message)
    {
        var cleanNumber = phoneNumber.TrimStart('+');
        var encodedMessage = HttpUtility.UrlEncode(message);
        return $"https://wa.me/{cleanNumber}?text={encodedMessage}";
    }
}
