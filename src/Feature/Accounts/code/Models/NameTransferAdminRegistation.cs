using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Feature.Accounts.Validation;
using Sitecore.Foundation.Dictionary.Repositories;
using static Sitecore.Feature.Accounts.Services.NameTransferService;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class NameTransferAdminRegistation
    {
        public List<CheckBoxes> checkCityList2 { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Adminstatus { get; set; }

        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(AdaniGasENachRegistrationModel))]
        public string MobileNumber { get; set; }

        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/CON/Name Transfer Admin Registration/Invalid Email Address", "Please enter a valid email address");
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(NameTransferAdminRegistation))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Username { get; set; }

        public string City { get; set; }

        public List<CheckBoxes> checkCityList { get; set; }

        public string Password { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Role { get; set; }

        public string Captcha { get; set; }

        public List<SelectListItem> CityList { get; set; }

        public List<SelectListItem> GetAdminRegisterNamelist { get; set; }

        public bool msg { get; set; }

        public string NameChangeNewPassword { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("NameChangeNewPassword", ErrorMessage = "New password and confirmation password not matched, Type again !")]
        public string ConfirmNewPassword { get; set; }
        //public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Invalid Email Address", "Please enter a valid email address");

    }

    [Serializable]
    public class NameTransferAdminRegistationSessssion
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string EmployeeId { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string City { get; set; }


    }


}