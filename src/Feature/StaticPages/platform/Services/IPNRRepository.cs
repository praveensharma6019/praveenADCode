
using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Services
{
    public interface IPNRRepository
    {
        PNRFormLabels GetPNRFormItem(Item currentItem);
        PNRFormResponse GetPNRResponse(PNRFormInput input);
        bool IsReCaptchValid(string responseData);
    }
}
