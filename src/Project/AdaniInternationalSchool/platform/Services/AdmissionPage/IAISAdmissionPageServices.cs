using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Project.AdaniInternationalSchool.Website.AdmissionPage
{
    public interface IAISAdmissionPageServices
    {
        AdmissionOverview GetAdmissionOverview(Sitecore.Mvc.Presentation.Rendering rendering);
        AdmissionUpdatesModel GetAdmissionupdates(Sitecore.Mvc.Presentation.Rendering rendering);
        AdmissionCardsRoot GetAdmissionCards(Sitecore.Mvc.Presentation.Rendering rendering);
        Root GetCards(Sitecore.Mvc.Presentation.Rendering rendering);
        SeoData GetSeoData(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}
