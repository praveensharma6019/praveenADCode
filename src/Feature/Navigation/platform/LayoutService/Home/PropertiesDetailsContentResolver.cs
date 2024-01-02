using Adani.SuperApp.Realty.Feature.Navigation.Platform.Services;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using static Adani.SuperApp.Realty.Feature.Navigation.Platform.Templates;
namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.LayoutService
{
    public class PropertiesDetailsContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        protected readonly INavigationRootResolver RootResolver;

        public PropertiesDetailsContentResolver(INavigationRootResolver rootResolver)
        {
            RootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var root = RootResolver.GetPropertyDetails(this.GetContextItem(rendering, renderingConfig));
            List<Item> newitem = new List<Item>();
            //string urlParam = rendering.Parameters["Location"]; 
            var commonItem = Sitecore.Context.Database.GetItem(commonData.ItemID);
            var commoffer = !string.IsNullOrEmpty(commonItem.Fields[commonData.Fields.commOffer].Value.ToString()) ? commonItem.Fields[commonData.Fields.commOffer].Value.ToString() : "";
            var resOffer = !string.IsNullOrEmpty(commonItem.Fields[commonData.Fields.ressOffer].Value.ToString()) ? commonItem.Fields[commonData.Fields.ressOffer].Value.ToString() : "";
            string urlParam = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("Location");
            urlParam = !string.IsNullOrEmpty(urlParam) ? urlParam.Replace("property-in-", "") : "";
            if (!string.IsNullOrEmpty(urlParam))
            {
                List<string> storagevariabl = new List<string>();
                var listodcity = root.Where(e => !e.Parent.Name.ToLower().Contains(urlParam.ToLower())).Select(e => e.Parent).ToList();
                string combindedString = "";
                //var commOffer = rendering.Parameters["commOffer"];
                //if (!string.IsNullOrEmpty(commOffer) && commOffer.ToLower().Contains(urlParam.ToLower()))
                //{
                //    commOffer = commOffer.ToLower().Replace(urlParam.ToLower(), string.Empty);
                //    rendering.Parameters["commOffer"] = commOffer;
                //}
                var count = 1;
                var listcount = listodcity.GroupBy(x => x.Name).ToList().Count();
                foreach (var item in listodcity)
                {
                    if (!storagevariabl.Contains(item.Name.ToLower()))
                    {
                        if (listcount <= 2)
                        {
                            if (count != listcount || listcount == 1)
                            {
                                combindedString = combindedString + " " + item.Name;
                            }
                            else
                            {
                                combindedString = combindedString + " & " + item.Name + ".";
                            }
                        }
                        else
                        {

                            if (count == listcount - 1)
                            {
                                combindedString = combindedString + " " + item.Name;
                            }
                            else if (count != listcount || listcount == 1)
                            {
                                combindedString = combindedString + " " + item.Name + ",";
                            }
                            else
                            {
                                combindedString = combindedString + " & " + item.Name + ".";
                            }
                        }
                        storagevariabl.Add(item.Name.ToLower());
                        var childOfcities1 = item.Children.Where(x => x.Fields[ResidentialProjects.Fields.ProjectStatusID].Value != null && x.Fields[ResidentialProjects.Fields.ProjectStatusID].Value == "{9E9581BB-84C0-422A-81D4-10498CD90F38}");
                        newitem.AddRange(childOfcities1);
                        var childOfcities2 = item.Children.Where(x => x.Fields[ResidentialProjects.Fields.ProjectStatusID].Value != null && x.Fields[ResidentialProjects.Fields.ProjectStatusID].Value != "{9E9581BB-84C0-422A-81D4-10498CD90F38}");
                        newitem.AddRange(childOfcities2);
                        count = count + 1;
                    }
                }
                newitem = newitem.OrderBy(x => x.Fields[ResidentialProjects.Fields.Site_StatusID].Value).Where(x => x.Fields[ResidentialProjects.Fields.Site_StatusID].Value != "").ToList();
                var id = root.Where(x => x.TemplateID == ResidentialProjects.TemplateID).Select(x => x.TemplateID).FirstOrDefault();
                if (id == ResidentialProjects.TemplateID)
                {
                    return new
                    {
                        param = listodcity.Select(y => new
                        {
                            ressOffer = resOffer + combindedString,
                        }).FirstOrDefault(),
                        data = newitem.Where(x => x.TemplateID == ResidentialProjects.TemplateID).Select(x => new
                        {
                            propertyID = x.ID.ToString(),
                            link = Helper.GetLinkURL(x, ResidentialProjects.Fields.linkFieldName) != null ?
                                     Helper.GetLinkURL(x, ResidentialProjects.Fields.linkFieldName) : "",
                            linktarget = Helper.GetLinkURLTargetSpace(x, ResidentialProjects.Fields.linkFieldName) != null ?
                                      Helper.GetLinkURLTargetSpace(x, ResidentialProjects.Fields.linkFieldName) : "",
                            logo = !string.IsNullOrEmpty(Helper.GetImageSource(x, ResidentialProjects.Fields.Property_LogoFieldName)) ?
                                       Helper.GetImageSource(x, ResidentialProjects.Fields.Property_LogoFieldName) : "",
                            logotitle = Helper.GetImageDetails(x, ResidentialProjects.Fields.Property_LogoFieldName) != null ?
                                       Helper.GetImageDetails(x, ResidentialProjects.Fields.Property_LogoFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "",
                            logoalt = Helper.GetImageDetails(x, ResidentialProjects.Fields.Property_LogoFieldName) != null ?
                                       Helper.GetImageDetails(x, ResidentialProjects.Fields.Property_LogoFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "",
                            src = !string.IsNullOrEmpty(Helper.GetImageSource(x, ResidentialProjects.Fields.ImageFieldName)) ?
                                       Helper.GetImageSource(x, ResidentialProjects.Fields.ImageFieldName) : "",
                            mobileimage = !string.IsNullOrEmpty(Helper.GetImageSource(x, ResidentialProjects.Fields.mobileimageFieldName)) ?
                                       Helper.GetImageSource(x, ResidentialProjects.Fields.mobileimageFieldName) : "",
                            imgalt = Helper.GetImageDetails(x, ResidentialProjects.Fields.ImageFieldName) != null ?
                                       Helper.GetImageDetails(x, ResidentialProjects.Fields.ImageFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "",
                            title = x.Fields[ResidentialProjects.Fields.TitleFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.TitleFieldName].Value : "",
                            imgtitle = Helper.GetImageDetails(x, ResidentialProjects.Fields.ImageFieldName) != null ?
                                       Helper.GetImageDetails(x, ResidentialProjects.Fields.ImageFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "",
                            location = x.Fields[ResidentialProjects.Fields.LocationFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.LocationFieldName].Value : "",
                            type = x.Fields[ResidentialProjects.Fields.TypeFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.TypeFieldName].Value : "",
                            imgtype = x.Fields[ResidentialProjects.Fields.imgtypeFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.imgtypeFieldName].Value : "",
                            propertyType = x.Fields[ResidentialProjects.Fields.Property_TypeFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.Property_TypeFieldName].Value : "",
                            status = x.Fields[ResidentialProjects.Fields.Site_StatusieldName].Value != null ? x.Fields[ResidentialProjects.Fields.Site_StatusieldName].Value : "",
                            latitude = x.Fields[ResidentialProjects.Fields.LatitudeID].Value != null ? x.Fields[ResidentialProjects.Fields.LatitudeID].Value : "",
                            logitude = x.Fields[ResidentialProjects.Fields.LongitudeID].Value != null ? x.Fields[ResidentialProjects.Fields.LongitudeID].Value : "",
                        })
                    };
                }
                else
                {
                    return new
                    {

                        param = listodcity.Select(y => new
                        {
                            ressOffer = string.Format(commoffer, combindedString),
                            //"We also offer commercial and retail space in" + combindedString + ". Take a look.",
                        }).FirstOrDefault(),
                        data = newitem.Where(x => x.TemplateID == CommercialProjects.TemplateID).Select(x => new
                        {
                            propertyID = x.ID.ToString(),
                            link = Helper.GetLinkURL(x, ResidentialProjects.Fields.linkFieldName) != null ?
                                     Helper.GetLinkURL(x, ResidentialProjects.Fields.linkFieldName) : "",
                            linktarget = Helper.GetLinkURLTargetSpace(x, ResidentialProjects.Fields.linkFieldName) != null ?
                                      Helper.GetLinkURLTargetSpace(x, ResidentialProjects.Fields.linkFieldName) : "",
                            logo = !string.IsNullOrEmpty(Helper.GetImageSource(x, ResidentialProjects.Fields.Property_LogoFieldName)) ?
                                       Helper.GetImageSource(x, ResidentialProjects.Fields.Property_LogoFieldName) : "",
                            logotitle = Helper.GetImageDetails(x, ResidentialProjects.Fields.Property_LogoFieldName) != null ?
                                       Helper.GetImageDetails(x, ResidentialProjects.Fields.Property_LogoFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "",
                            logoalt = Helper.GetImageDetails(x, ResidentialProjects.Fields.Property_LogoFieldName) != null ?
                                       Helper.GetImageDetails(x, ResidentialProjects.Fields.Property_LogoFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "",
                            src = !string.IsNullOrEmpty(Helper.GetImageSource(x, ResidentialProjects.Fields.ImageFieldName)) ?
                                       Helper.GetImageSource(x, ResidentialProjects.Fields.ImageFieldName) : "",
                            mobileimage = !string.IsNullOrEmpty(Helper.GetImageSource(x, ResidentialProjects.Fields.mobileimageFieldName)) ?
                                       Helper.GetImageSource(x, ResidentialProjects.Fields.mobileimageFieldName) : "",
                            imgalt = Helper.GetImageDetails(x, ResidentialProjects.Fields.ImageFieldName) != null ?
                                       Helper.GetImageDetails(x, ResidentialProjects.Fields.ImageFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "",
                            title = x.Fields[ResidentialProjects.Fields.TitleFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.TitleFieldName].Value : "",
                            imgtitle = Helper.GetImageDetails(x, ResidentialProjects.Fields.ImageFieldName) != null ?
                                       Helper.GetImageDetails(x, ResidentialProjects.Fields.ImageFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "",
                            location = x.Fields[ResidentialProjects.Fields.LocationFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.LocationFieldName].Value : "",
                            type = x.Fields[ResidentialProjects.Fields.TypeFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.TypeFieldName].Value : "",
                            imgtype = x.Fields[ResidentialProjects.Fields.imgtypeFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.imgtypeFieldName].Value : "",
                            propertyType = x.Fields[ResidentialProjects.Fields.Property_TypeFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.Property_TypeFieldName].Value : "",
                            status = x.Fields[ResidentialProjects.Fields.Site_StatusieldName].Value != null ? x.Fields[ResidentialProjects.Fields.Site_StatusieldName].Value : "",
                            latitude = x.Fields[ResidentialProjects.Fields.LatitudeID].Value != null ? x.Fields[ResidentialProjects.Fields.LatitudeID].Value : "",
                            logitude = x.Fields[ResidentialProjects.Fields.LongitudeID].Value != null ? x.Fields[ResidentialProjects.Fields.LongitudeID].Value : "",
                        })
                    };
                }
            }

            else
            {
                var childOfcities1 = root.Where(x => x.Fields[ResidentialProjects.Fields.ProjectStatusID].Value != null && x.Fields[ResidentialProjects.Fields.ProjectStatusID].Value == "{9E9581BB-84C0-422A-81D4-10498CD90F38}" && x.Fields[ResidentialProjects.Fields.ProjectStatusID].Value != "");
                newitem.AddRange(childOfcities1);
                var childOfcities2 = root.Where(x => x.Fields[ResidentialProjects.Fields.ProjectStatusID].Value != null && x.Fields[ResidentialProjects.Fields.ProjectStatusID].Value != "{9E9581BB-84C0-422A-81D4-10498CD90F38}" && x.Fields[ResidentialProjects.Fields.ProjectStatusID].Value != "");
                newitem.AddRange(childOfcities2);
                return new
                {

                    data = newitem.Select(x => new
                    {
                        propertyID = x.ID.ToString(),
                        link = Helper.GetLinkURL(x, ResidentialProjects.Fields.linkFieldName) != null ?
                                    Helper.GetLinkURL(x, ResidentialProjects.Fields.linkFieldName) : "",
                        linktarget = Helper.GetLinkURLTargetSpace(x, ResidentialProjects.Fields.linkFieldName) != null ?
                                      Helper.GetLinkURLTargetSpace(x, ResidentialProjects.Fields.linkFieldName) : "",
                        logo = !string.IsNullOrEmpty(Helper.GetImageSource(x, ResidentialProjects.Fields.Property_LogoFieldName)) ?
                                      Helper.GetImageSource(x, ResidentialProjects.Fields.Property_LogoFieldName) : "",
                        logotitle = Helper.GetImageDetails(x, ResidentialProjects.Fields.Property_LogoFieldName) != null ?
                                      Helper.GetImageDetails(x, ResidentialProjects.Fields.Property_LogoFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "",
                        logoalt = Helper.GetImageDetails(x, ResidentialProjects.Fields.Property_LogoFieldName) != null ?
                                      Helper.GetImageDetails(x, ResidentialProjects.Fields.Property_LogoFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "",
                        src = !string.IsNullOrEmpty(Helper.GetImageSource(x, ResidentialProjects.Fields.ImageFieldName)) ?
                                      Helper.GetImageSource(x, ResidentialProjects.Fields.ImageFieldName) : "",
                        mobileimage = !string.IsNullOrEmpty(Helper.GetImageSource(x, ResidentialProjects.Fields.mobileimageFieldName)) ?
                                       Helper.GetImageSource(x, ResidentialProjects.Fields.mobileimageFieldName) : "",
                        imgalt = Helper.GetImageDetails(x, ResidentialProjects.Fields.ImageFieldName) != null ?
                                      Helper.GetImageDetails(x, ResidentialProjects.Fields.ImageFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "",
                        title = x.Fields[ResidentialProjects.Fields.TitleFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.TitleFieldName].Value : "",
                        imgtitle = Helper.GetImageDetails(x, ResidentialProjects.Fields.ImageFieldName) != null ?
                                      Helper.GetImageDetails(x, ResidentialProjects.Fields.ImageFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "",
                        location = x.Fields[ResidentialProjects.Fields.LocationFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.LocationFieldName].Value : "",
                        type = x.Fields[ResidentialProjects.Fields.TypeFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.TypeFieldName].Value : "",
                        imgtype = x.Fields[ResidentialProjects.Fields.imgtypeFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.imgtypeFieldName].Value : "",
                        propertyType = x.Fields[ResidentialProjects.Fields.Property_TypeFieldName].Value != null ? x.Fields[ResidentialProjects.Fields.Property_TypeFieldName].Value : "",
                        status = x.Fields[ResidentialProjects.Fields.Site_StatusieldName].Value != null ? x.Fields[ResidentialProjects.Fields.Site_StatusieldName].Value : "",
                        latitude = x.Fields[ResidentialProjects.Fields.LatitudeID].Value != null ? x.Fields[ResidentialProjects.Fields.LatitudeID].Value : "",
                        logitude = x.Fields[ResidentialProjects.Fields.LongitudeID].Value != null ? x.Fields[ResidentialProjects.Fields.LongitudeID].Value : "",
                    })
                };
            }
        }
    }
}
