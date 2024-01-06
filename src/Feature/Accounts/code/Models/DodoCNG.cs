using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Feature.Accounts.Infrastructure.Pipelines;

namespace Sitecore.Feature.Accounts.Models
{
    public class DodoCNG
    {
        public DodoCNG()
        {
            GeoAreaList = new List<SelectListItem>();
            IsDownloadable = false;
        }
        public bool IsDownloadable { get; set; }
        public string DownloadLink { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string StateVal { get; set; }
        public List<SelectListItem> StateList { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string GeoAreaVal { get; set; }
        public List<SelectListItem> GeoAreaList { get; set; }
        public string salutationVal { get; set; }
        public List<SelectListItem> salutationList { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string FirstName { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string MiddleName { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string LastName { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string Address1 { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string Address2 { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string Address3 { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string CityOfAddress { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string DistrictOfAddress { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string StateOfAddress { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        [Display(Name = nameof(PincodeCaption), ResourceType = typeof(DodoCNG))]
        [RegularExpression(@"\d{6}", ErrorMessageResourceName = nameof(InvalidPincode), ErrorMessageResourceType = typeof(DodoCNG))]
        public string PincodeOfAddress { get; set; }

        #region Contact number
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        //[Display(Name = nameof(MobileCaption), ResourceType = typeof(DodoCNG))]
        //[RegularExpression(@"^(?:(?:\+|0{0,2})91(\s*[\ -]\s*)?|[0]?)?[789]\d{9}|(\d[ -]?){10}\d$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(DodoCNG))]
        public string MobileNumber { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string LandlineNumber { get; set; }

        [Display(Name = nameof(EmailCaption), ResourceType = typeof(DodoCNG))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(DodoCNG))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string OccupationVal { get; set; }
        public List<SelectListItem> OccupationList { get; set; }
        public string briefOccupation { get; set; }
        #endregion

        #region Details of land owned by you to set up CNG Station
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string PlotNo { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string Road { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string Landmark { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string RoadGoingTowards { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string City { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string District { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string State { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        [Display(Name = nameof(PincodeCaption), ResourceType = typeof(DodoCNG))]
        [RegularExpression(@"\d{6}", ErrorMessageResourceName = nameof(InvalidPincode), ErrorMessageResourceType = typeof(DodoCNG))]
        public string Pincode { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string PlotType { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string Plotsize { get; set; }
        #endregion

        #region Photographs of Land
        [Required(ErrorMessage = "Please select file.")]
        [ValidateFile(ErrorMessage = "Please select a png,Jpeg or gif image up to 1MB")]
        public HttpPostedFileBase FrontViewPhoto { get; set; }
        [Required(ErrorMessage = "Please select file.")]
        [ValidateFile(ErrorMessage = "Please select a png,Jpeg or gif image up to 1MB")]
        public HttpPostedFileBase LeftsideEntryPlot { get; set; }
        [Required(ErrorMessage = "Please select file.")]
        [ValidateFile(ErrorMessage = "Please select a png,Jpeg or gif image up to 1MB")]
        public HttpPostedFileBase RightsideExitPlot { get; set; }
        [Required(ErrorMessage = "Please select file.")]
        [ValidateFile(ErrorMessage = "Please select a png,Jpeg or gif image up to 1MB")]
        public HttpPostedFileBase InsideOutViewPlot { get; set; }
        #endregion

        #region Plot co-ordinates
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        #endregion

        #region Name of Nearest Petrol Pump from the land
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string NameoffillingStation { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string DistancefromLandInKM { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string PetrolIndustryPrevExp { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string ProposedEntity { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DodoCNG))]
        public string DODOformatSourceVal { get; set; }
        public List<SelectListItem> DODOformatSourceList { get; set; }
        public bool TermCondition1 { get; set; }
        public bool TermCondition2 { get; set; }
        public bool TermCondition3 { get; set; }
        #endregion
        public string ReturnViewMessage { get; set; }

        public string OMCVal { get; set; }
        public List<SelectListItem> OMCList { get; set; }

        public string OMCYear { get; set; }
        public List<SelectListItem> OMCYearList { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Required", "This field is required");
        public static string MobileCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Feeedback/ContactNumber", "Mobile Number");
        public static string EmailCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Email", "Email");
        public static string PincodeCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Edit Profile/Pincode", "PincodeOfAddress");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Feedback/Invalid Contact", "Please enter a valid Mobile Number");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");
        public static string InvalidPincode => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Pincode", "Please enter a valid Pincode");

    }
}