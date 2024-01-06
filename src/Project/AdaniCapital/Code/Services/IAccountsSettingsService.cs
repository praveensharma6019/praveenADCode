using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.AdaniCapital.Website.Services
{
    public interface IAccountsSettingsService
    {
        //string GetPageLink(Item contextItem, ID fieldID);
        //MailMessage GetForgotPasswordMailTemplate();

        string GetPageLinkOrDefault(Item contextItem, ID field, Item defaultItem = null);
        //Guid? GetRegistrationOutcome(Item contextItem);
    }
}
