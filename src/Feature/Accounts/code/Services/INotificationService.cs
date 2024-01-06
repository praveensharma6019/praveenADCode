using System.IO;
using System.Web;

namespace Sitecore.Feature.Accounts.Services
{
    public interface INotificationService
    {
        void SendPassword(string email, string newPassword);
        void SendPasswordResetLink(string email, string userName, string redirectUrl);

        bool SendComposeMail(string to, string subject, string body, byte[] attachedfile,string fileName);

        bool SendBill(string to, string subject, string body, MemoryStream ms, string fileName, string from);
    }
}