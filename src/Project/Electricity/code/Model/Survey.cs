using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace Sitecore.Electricity.Website.Model
{
    [Serializable]
    public class Survey
    {

        public Survey()
        {
            TypeofApplianceList = new List<TypeofAppliance>();
        }


        [Display(Name = nameof(FirstNameCaption), ResourceType = typeof(Survey))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Survey))]
        public string Name { get; set; }

        [Display(Name = nameof(CANumberCaption), ResourceType = typeof(Survey))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Survey))]
        [RegularExpression(@"^[0-9]{9,9}$", ErrorMessageResourceName = nameof(InvalidCANumber), ErrorMessageResourceType = typeof(Survey))]
        public string CANumber { get; set; }

        [Display(Name = nameof(EmailCaption), ResourceType = typeof(Survey))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Survey))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(Survey))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = nameof(MobileNumberCaption), ResourceType = typeof(Survey))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Survey))]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(Survey))]
        public string MobileNo { get; set; }

        public List<TypeofAppliance> TypeofApplianceList { get; set; }

        public string Frm { get; set; }

        public static string FirstNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/FirstName", "First Name");
        public static string EmailCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Email", "E-mail id");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");

        public static string InvalidCANumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid CA Number", "Please enter a valid CA Number");
        public static string CANumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/CANumber", "CA Number");
        public static string MobileNumberCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/MobileNumber", "Mobile No.");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
    }
    [Serializable]
    public class TypeofAppliance
    {
        public TypeofAppliance()
        {
            ApplianceList = new List<Appliance>();
        }
        public string Name { get; set; }
        public List<Appliance> ApplianceList { get; set; }
    }
    [Serializable]
    public class Appliance
    {
        public Appliance()
        {
            QuestionsList = new List<Questions>();
        }
        public string Name { get; set; }
        public List<Questions> QuestionsList { get; set; }
    }
    [Serializable]
    public class Questions
    {
        public Questions()
        {
            Option = new List<Options>();
        }
        public string Question { get; set; }
        public List<Options> Option { get; set; }

    }
    [Serializable]
    public class Options
    {
        public Options() { }
        //[Display(Name = nameof(FirstNameCaption), ResourceType = typeof(Survey))]
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Survey))]
        public string Option { get; set; }
        public bool Checked { get; set; }
        public string Response { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
        public static string FirstNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/FirstName", "First Name");
    }
    [Serializable]
    public class SurveyResult
    {
        public SurveyResult()
        {
            SubSurveyResult = new List<SubSurveyResult>();
        }
        public string Name { get; set; }
        public string CANumber { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public List<SubSurveyResult> SubSurveyResult { get; set; }
    }
    [Serializable]
    public class SubSurveyResult
    {
        public SubSurveyResult()
        {
            ApplianceList = new List<ApplianceList>();
        }
        public string Appliancetype { get; set; }
        public List<ApplianceList> ApplianceList { get; set; }
    }
    [Serializable]
    public class ApplianceList
    {
        public ApplianceList()
        { AnswerdQuestionResponse = new List<AnswerdQuestionResponse>(); }
        public string ApplianceName { get; set; }
        public List<AnswerdQuestionResponse> AnswerdQuestionResponse { get; set; }
    }
    [Serializable]
    public class AnswerdQuestionResponse
    {
        public string Response { get; set; }
    }
}