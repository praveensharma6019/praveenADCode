namespace Sitecore.ElectricityNew.Website.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;
    using Sitecore.ElectricityNew.Website.Services;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class ApplyFormModel
    {
        public ApplyFormModel()
        {
            SwitchToGreenService objService = new SwitchToGreenService();
            VehicleList = objService.GetVihecleList();
        }

        public List<SwitchToGreenPledge_VehicleMaster> VehicleList { get; set; }

        //Captcha
        public string Captcha_GreenIn { get; set; }
        public string Captcha_GreenOrg { get; set; }
        public string Captcha_EVIn { get; set; }
        public string Captcha_EVSo { get; set; }
        public string Captcha_EVPartner { get; set; }

        //Name
        public static string InvalidName => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Login Name", "Name contains alphabets only");
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ApplyFormModel))]
        public string FullName { get; set; }

        public static string InvalidOrgName => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Org Name", "Invalid Organization name.");
        [RegularExpression("^[a-zA-Z]([a-zA-Z0-9]|[- @/\\.,#&!()])*$", ErrorMessageResourceName = nameof(InvalidOrgName), ErrorMessageResourceType = typeof(ApplyFormModel))]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string OrganizationName { get; set; }

        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ApplyFormModel))]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string FullName_EV { get; set; }

        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ApplyFormModel))]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string FullName_EVS { get; set; }

        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = nameof(InvalidName), ErrorMessageResourceType = typeof(ApplyFormModel))]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string FullName_EVP { get; set; }


        //Email
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ApplyFormModel))]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string EmailId_GreenIn { get; set; }

        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ApplyFormModel))]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string EmailId_GreenOrg { get; set; }

        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ApplyFormModel))]
        [DataType(DataType.EmailAddress)]
        public string EmailId_EVIn { get; set; }

        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ApplyFormModel))]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string EmailId_EVS { get; set; }

        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ApplyFormModel))]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Max length limit exceeded")]
        public string EmailId_EVP { get; set; }

        //Mobile number
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ApplyFormModel))]
        public string MobileNumber { get; set; }

        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ApplyFormModel))]
        public string MobileNumber_EVIn { get; set; }

        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ApplyFormModel))]
        public string MobileNumber_EVS { get; set; }

        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ApplyFormModel))]
        public string MobileNumberEVP { get; set; }

        [RegularExpression(@"^[0-9]{6,6}$", ErrorMessage = "Invalid Zip code")]
        public string ZipCode { get; set; }

        public string VehicleType_In { get; set; }
        public string VehicleType_Org { get; set; }

        public string PledgeType { get; set; }
        public string PledgeId { get; set; }

        public bool IsSignUp_In { get; set; }
        public bool IsSignUp_Org { get; set; }

        public string FormType { get; set; }
    }
}