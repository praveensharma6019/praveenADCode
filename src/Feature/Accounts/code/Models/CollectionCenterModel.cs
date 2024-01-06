using System.ComponentModel.DataAnnotations;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Sitecore.Feature.Accounts.Models
{
    public class CollectionCenterModel
    {
        public CollectionCenterModel()
        {
            CollectioTypeList = new List<SelectListItem>();
            CityList = new List<SelectListItem>();
            AreaList = new List<SelectListItem>();
            CollectionCenterList = new List<Centers>();
        }

        [Display(Name = nameof(CollectionTypeCaption), ResourceType = typeof(CollectionCenterModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(CollectionCenterModel))]
        public string CollectionType { get; set; }

        [Display(Name = nameof(CityCaption), ResourceType = typeof(CollectionCenterModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(CollectionCenterModel))]
        public string City { get; set; }

        [Display(Name = nameof(AreaCaption), ResourceType = typeof(CollectionCenterModel))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(CollectionCenterModel))]
        public string Area { get; set; }
        public List<SelectListItem> CollectioTypeList { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> AreaList { get; set; }
        public List<Centers> CollectionCenterList { get; set; }
        public string ReturnViewMessage { get; set; }

        public static string CollectionTypeCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Collection Type");
        public static string CityCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "City");
        public static string AreaCaption => DictionaryPhraseRepository.Current.Get("/Accounts/", "Area");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Required", "Please select value for {0}");
    }

    public class Centers
    {
        public string IvRadius { get; set; }
        public string LocNo { get; set; }
        public string City { get; set; }
        public string NearArea { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string LocType { get; set; }
        public string Blocked { get; set; }
        public string AddrLine1 { get; set; }
        public string AddrLine2 { get; set; }
        public string AddrLine3 { get; set; }
        public string AddrLine4 { get; set; }
        public string AddrLine5 { get; set; }
        public string AddrLine6 { get; set; }
        public string AddrLine7 { get; set; }
        public string AddrLine8 { get; set; }
        public string PinCode { get; set; }
        public string Landline { get; set; }
        public string Landline2 { get; set; }
        public string MobileNo { get; set; }
        public string MobileNo2 { get; set; }
        public string FlagDom { get; set; }
        public string FlagInd { get; set; }
        public string FlagCom { get; set; }

    }
}