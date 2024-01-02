using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Repositories
{
    public interface IFeedbackRepository
    {
        FeedbackFormResponse GetSubmitFormDetails(FeedbackForm feedback);

        FeedbackFormResponse GetIncidentforSubmitForm(FeedbackForm feedback);

        SubmitViewModel GetSubmitFormItem(Item item, SubmitViewModel submitViewModel);
    }
}
