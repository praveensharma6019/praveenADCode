using Adani.Foundation.Messaging.Models;
using System.Collections.Generic;

namespace Adani.Foundation.Messaging.Services.Email
{
    public interface IEmailService
    {
        bool Send(EmailData emailData, IEnumerable<KeyValuePair<string, string>> data);
    }
}
