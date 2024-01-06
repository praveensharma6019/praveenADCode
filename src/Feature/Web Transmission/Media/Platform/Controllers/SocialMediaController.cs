using System.Collections.Generic;
using System.Web.Mvc;
using Adani.BAU.Transmission.Feature.Media.Platform.Models;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Collections;
using Sitecore.Mvc.Presentation;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Controllers
{
    public class SocialMediaController : Controller
    {
        Helper obj = new Helper();
        // GET: SocialMedia
        public ActionResult SocialMedia()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            SocialMediaList model = new SocialMediaList();
            try
            {
                if (dataSource !=null && dataSource.HasChildren)
                {
                    model.socialMediaList = ParseDatasourceMediaItems(dataSource.Children);
                }
            }
            catch (System.Exception ex)
            {

                string msg = ex.Message;
            }

            return View("~/views/SocialMedia/SocialMedia.cshtml",model);
        }

        private List<SocialMediaModel> ParseDatasourceMediaItems(ChildList children)
        {
            List<SocialMediaModel> objSocialMediaList = new List<SocialMediaModel>();
            SocialMediaModel objSocialMedia = null;
            foreach(Sitecore.Data.Items.Item SocialMediaItem in children)
            {
                objSocialMedia = new SocialMediaModel();
                objSocialMedia.LinkText = obj.GetLinkText(SocialMediaItem, "Link");
                objSocialMedia.LinkURL = obj.GetLinkURL(SocialMediaItem, "Link");
                objSocialMedia.LinkTarget = obj.GetLinkTarget(SocialMediaItem, "Link");
                objSocialMedia.ImageAltText = obj.GetImageAlt(SocialMediaItem, "Image");
                objSocialMedia.ImageSrc = obj.GetImageURL(SocialMediaItem, "Image");
                objSocialMediaList.Add(objSocialMedia);

            }
            return objSocialMediaList;
        }
    }
}