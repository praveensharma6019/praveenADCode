using Adani.Foundation.Messaging.Models;
using System.Collections.Generic;

namespace Adani.Foundation.Messaging.Services.SMS
{
    public interface ISMSService
    {
        bool Send(string recipient, string messageBody, IEnumerable<KeyValuePair<string, string>> data);
        bool SendSMSFarmpik(string mobile, FarmpikSMSModel data);
    }
}
