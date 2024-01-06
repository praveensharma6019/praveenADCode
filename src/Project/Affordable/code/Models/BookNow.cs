using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Affordable.Website.Models
{
    public class BookNow
    {
        public string Project_Address { get; set; }
        public string Project_Notes { get; set; }
        public string projectid { get; set; }
        public string msg { get; set; }

        [Display(Name = nameof(FirstNameCaption), ResourceType = typeof(BookNow))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(BookNow))]
        public string Name { get; set; }

        [Display(Name = nameof(EmailCaption), ResourceType = typeof(BookNow))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(BookNow))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(BookNow))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name = nameof(MobileNumberCaption), ResourceType = typeof(BookNow))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(BookNow))]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(BookNow))]
        public string MoblieNo { get; set; }

        public string transaction_id { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceName = nameof(AmountRequired), ErrorMessageResourceType = typeof(BookNow))]
        public double Amount { get; set; }

        public double TotalAmount { get; set; }

        [Display(Name = nameof(projectRequired), ResourceType = typeof(BookNow))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(BookNow))]
        public string selectedProjectID { get; set; }

        public string ProjectName { get; set; }

        [Display(Name = nameof(BuildingRequired), ResourceType = typeof(BookNow))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(BookNow))]
        public string buildingid { get; set; }
        [Display(Name = nameof(FloorRequired), ResourceType = typeof(BookNow))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(BookNow))]
        public string floorid { get; set; }

        [Display(Name = nameof(UnitRequired), ResourceType = typeof(BookNow))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(BookNow))]
        public string unitid { get; set; }

        public Dictionary<string, string> Building { get; set; }
        public Dictionary<string, string> Projects { get; set; }
        public Dictionary<string, string> Floor { get; set; }
        public Dictionary<string, string> Unit { get; set; }



        public static string FirstNameCaption => DictionaryPhraseRepository.Current.Get("/BookNow/FirstName", "First Name");
        public static string EmailCaption => DictionaryPhraseRepository.Current.Get("//Register/Email", "E-mail id");
        public static string Required => DictionaryPhraseRepository.Current.Get("/BookNow/Register/Required", "Please enter a value for {0}");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/BookNow/Invalid Email Address", "Please enter a valid email address");
        public static string BuildingRequired => DictionaryPhraseRepository.Current.Get("/BookNow/Building", "Building");
        public static string FloorRequired => DictionaryPhraseRepository.Current.Get("/BookNow/Floor", "Floor");
        public static string UnitRequired => DictionaryPhraseRepository.Current.Get("/BookNow/Unit", "Unit");
        public static string AmountRequired => DictionaryPhraseRepository.Current.Get("/BookNow/Amount", "Please enter a value bigger than {1}");
        public static string projectRequired => DictionaryPhraseRepository.Current.Get("/BookNow/Project", "Project");


        public static string InvalidCANumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid CA Number", "Please enter a valid CA Number");
        public static string CANumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/CANumber", "CA Number");
        public static string MobileNumberCaption => DictionaryPhraseRepository.Current.Get("/BookNow/MobileNumber", "Mobile No.");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
    }
}