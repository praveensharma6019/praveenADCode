using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Services
{
    public interface ICallBackRepository
    {
        CallBackFormLabels GetCallBackFormItem(Item currentItem);
        List<ApiResponse> GetEmailResponse(CallBackFormInput input);
    }
}
