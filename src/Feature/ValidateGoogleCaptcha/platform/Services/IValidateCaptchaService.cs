using Sitecore.Feature.ValidateGoogleCaptcha.Models;
using System.Threading.Tasks;

namespace Sitecore.Feature.ValidateGoogleCaptcha.Services
{
    public interface IValidateCaptchaService
    {
        bool VerifyCaptcha(CaptchaData captchaData);
        bool IsV3CaptchValid(CaptchaData captchaData);
    }
}
