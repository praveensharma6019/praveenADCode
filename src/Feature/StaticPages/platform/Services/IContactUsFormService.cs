
using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Services
{
    public interface IContactUsFormService
    {
        ContactUsForm GetContactUsForm(Item dataSourceItem);
    }
}
