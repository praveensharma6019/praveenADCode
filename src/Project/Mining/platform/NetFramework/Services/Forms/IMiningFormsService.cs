using Project.Mining.Website.Models.Forms;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services.Forms
{
    public interface IMiningFormsService
    {
        MiningForm GetSubscribeForm(Rendering rendering);
        MiningForm GetRequestACallForm(Rendering rendering);
        MiningForm GetContactForm(Rendering rendering);
    }
}
