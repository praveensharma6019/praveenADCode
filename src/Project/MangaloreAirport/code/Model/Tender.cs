namespace Sitecore.MangaloreAirport.Website.Model
{
    using Sitecore.Foundation.Dictionary.Repositories;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;
    using System.Web;

    [Serializable]
    public class Tender
    {
        public string UserName { get; set; }

        public string CompanyName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public bool? IsPQVerified { get; set; }

        public string TenderId { get; set; }

        public string NITNo { get; set; }

        public string TenderName { get; set; }

        public DateTime? AdvDate { get; set; }

        public DateTime? ClosingDate { get; set; }

        public string DocumentStatus { get; set; }

        public string PQDocumentStatus { get; set; }

        public bool? FinalSubmit { get; set; }

        public bool? PQApprovalRequired { get; set; }

        public string TenderType { get; set; }

        public string SupportEmailAddress { get; set; }

        public bool PQEnvelopeReviewStatus { get; set; }

        public List<TenderDocument> tenderdocument { get; set; }

        public List<TenderDocument> tenderPQdocument { get; set; }

        public List<UserTenderDocument> userUpladdocument { get; set; }

        public HttpPostedFileBase Envelope11 { get; set; }

        public HttpPostedFileBase Envelope12 { get; set; }

        public HttpPostedFileBase Envelope13 { get; set; }

        public HttpPostedFileBase Envelope14 { get; set; }

        public HttpPostedFileBase Envelope15 { get; set; }

        public HttpPostedFileBase Envelope21 { get; set; }

        public HttpPostedFileBase Envelope22 { get; set; }

        public HttpPostedFileBase Envelope23 { get; set; }

        public HttpPostedFileBase Envelope24 { get; set; }

        public HttpPostedFileBase Envelope25 { get; set; }

        public HttpPostedFileBase Envelope26 { get; set; }

        public HttpPostedFileBase Envelope27 { get; set; }

        public HttpPostedFileBase Envelope28 { get; set; }

        public HttpPostedFileBase Envelope29 { get; set; }

        public HttpPostedFileBase Envelope30 { get; set; }

        public HttpPostedFileBase Envelope3 { get; set; }

        [Display(Name = "OldPasswordCaption", ResourceType = typeof(Sitecore.MangaloreAirport.Website.Model.Tender)), DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "PasswordCaption", ResourceType = typeof(Sitecore.MangaloreAirport.Website.Model.Tender)), DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "ConfirmPasswordCaption", ResourceType = typeof(Sitecore.MangaloreAirport.Website.Model.Tender)), DataType(DataType.Password), Compare("Password", ErrorMessageResourceName = "ConfirmPasswordMismatch", ErrorMessageResourceType = typeof(Sitecore.MangaloreAirport.Website.Model.Tender))]
        public string ConfirmPassword { get; set; }

        public static string Required =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/Required", "Please enter a value for {0}");

        public static string PasswordCaption =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/PasswordCaption", "New Password");

        public static string OldPasswordCaption =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/OldPasswordCaption", "Change Password");

        public static string ConfirmPasswordCaption =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/ConfirmPasswordCaption", "Retype Password");

        public static string ConfirmPasswordMismatch =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/Confirm Password Mismatch", "Your password confirmation does not match. Please enter a new password.");

        public static string MinimumPasswordLength =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/Minimum Password Length", "Please enter a password with at lease {1} characters");
    }
}

