using Adani.SuperApp.Airport.Feature.Payment.Platform.Models;
using Sitecore.Data.Items;
using System;

namespace Adani.SuperApp.Airport.Feature.Payment.Platform.Services
{
    public interface IPaymentOptionService
    {
        PaymentOptions GetPaymentOptions(Item dataSourceItem, string Type, string ChannelType, string url);
    }
}
