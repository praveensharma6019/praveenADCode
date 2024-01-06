using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Realty.Website.Model;

namespace Sitecore.Realty.Website.Services
{
    public interface IPaymentServices
    {
        void StorePaymentRequest(PropertyCollection model);
        void StorePaymentResponse(PropertyCollection model);
        string BillDeskTransactionRequestAPIRequestPost(PropertyCollection Model);

        string GetHMACSHA256(string text, string key);
    }
}