using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.EV.Project.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.EV.Project.Services
{
    public interface IArticleServices
    {
        ArticleBannerCarousel GetBannerCarousel(Sitecore.Mvc.Presentation.Rendering rendering);
        ArticleFeaturedFilters GetArticleFeaturedFilters(Sitecore.Mvc.Presentation.Rendering rendering);
        ArticleVideoGalleryCarousel GetArticleVideoGalleryCarousel(Sitecore.Mvc.Presentation.Rendering rendering);
        ArticleFeaturedCardModel ArticleFeaturedCardList(Sitecore.Mvc.Presentation.Rendering rendering);
        ArticleDetailsDiscoverMore ArticleDetailsDiscoverMore(Sitecore.Mvc.Presentation.Rendering rendering);
        Faq GetFaq(Sitecore.Mvc.Presentation.Rendering rendering);
        LegalNavbar GetLegalNavbar(Sitecore.Mvc.Presentation.Rendering rendering);
        AddVehicle GetAddVehicle(Sitecore.Mvc.Presentation.Rendering rendering);
        AddVehicleForm GetAddVehicleForm(Sitecore.Mvc.Presentation.Rendering rendering);
        FaqContactUs GetFaqContactUs(Sitecore.Mvc.Presentation.Rendering rendering);
        LegalTermsAndCondition GetLegalTermsAndCondition(Sitecore.Mvc.Presentation.Rendering rendering);
        LegalPrivacyPolicy GetLegalPrivacyPolicy(Sitecore.Mvc.Presentation.Rendering rendering);
        ArticleDetails ArticleDetailsSocialMediaLinks(Sitecore.Mvc.Presentation.Rendering rendering);

    }
}
