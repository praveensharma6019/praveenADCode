using Sitecore.Feature.FormsExtensions.Models;
using System.Threading.Tasks;

namespace Feature.FormsExtensions.Business.ReCaptcha
{
    public interface IReCaptchaService
    {
        Task<bool> Verify(string response, string SecretKey);
        bool VerifySync(string response, string SecretKey);
        bool IsV3CaptchValid(string response, string SecretKey);

    }
}
