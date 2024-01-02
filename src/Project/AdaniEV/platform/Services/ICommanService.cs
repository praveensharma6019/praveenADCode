using Adani.EV.Project.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.EV.Project.Services
{
    public interface ICommanService
    {
        HeaderNavBarModel GetHeaderNavBarModel(Rendering rendering);

        FooterBannerModel GetFooterBannerModel(Rendering rendering);
        FooterNavigationModel GetFooterNavigationModel(Rendering rendering);
        SocialMediaLinksModel GetSocialMediaLinksModel(Rendering rendering);

        CopyrightModel GetCopyrightModel(Rendering rendering);
        LanguageModel GetLanguageModel(Rendering rendering);

    }
}
