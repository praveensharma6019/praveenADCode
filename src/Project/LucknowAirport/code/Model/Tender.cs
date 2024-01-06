using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.LucknowAirport.Website;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;

namespace Sitecore.LucknowAirport.Website.Model
{
    [Serializable]
    public class Tender
    {
        public DateTime? AdvDate
        {
            get;
            set;
        }

        public DateTime? ClosingDate
        {
            get;
            set;
        }

        public string CompanyName
        {
            get;
            set;
        }

        [Compare("Password", ErrorMessageResourceName = "ConfirmPasswordMismatch", ErrorMessageResourceType = typeof(Tender))]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPasswordCaption", ResourceType = typeof(Tender))]
        public string ConfirmPassword
        {
            get;
            set;
        }

        public static string ConfirmPasswordCaption
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/ConfirmPasswordCaption", "Retype Password");
            }
        }

        public static string ConfirmPasswordMismatch
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/Confirm Password Mismatch", "Your password confirmation does not match. Please enter a new password.");
            }
        }

        public string DocumentStatus
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope11
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope12
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope13
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope14
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope15
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope21
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope22
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope23
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope24
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope25
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope26
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope27
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope28
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope29
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope3
        {
            get;
            set;
        }

        public HttpPostedFileBase Envelope30
        {
            get;
            set;
        }

        public bool? FinalSubmit
        {
            get;
            set;
        }

        public bool? IsPQVerified
        {
            get;
            set;
        }

        public static string MinimumPasswordLength
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/Minimum Password Length", "Please enter a password with at lease {1} characters");
            }
        }

        public string Mobile
        {
            get;
            set;
        }

        public string NITNo
        {
            get;
            set;
        }

        [DataType(DataType.Password)]
        [Display(Name = "OldPasswordCaption", ResourceType = typeof(Tender))]
        public string OldPassword
        {
            get;
            set;
        }

        public static string OldPasswordCaption
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/OldPasswordCaption", "Change Password");
            }
        }

        [DataType(DataType.Password)]
        [Display(Name = "PasswordCaption", ResourceType = typeof(Tender))]
        public string Password
        {
            get;
            set;
        }

        public static string PasswordCaption
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/PasswordCaption", "New Password");
            }
        }

        public bool? PQApprovalRequired
        {
            get;
            set;
        }

        public string PQDocumentStatus
        {
            get;
            set;
        }

        public bool PQEnvelopeReviewStatus
        {
            get;
            set;
        }

        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Model Messages/Required", "Please enter a value for {0}");
            }
        }

        public string SupportEmailAddress
        {
            get;
            set;
        }

        public List<LKO_TenderDocument> tenderdocument
        {
            get;
            set;
        }

        public string TenderId
        {
            get;
            set;
        }

        public string TenderName
        {
            get;
            set;
        }

        public List<LKO_TenderDocument> tenderPQdocument
        {
            get;
            set;
        }

        public string TenderType
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public List<LKO_UserTenderDocument> userUpladdocument
        {
            get;
            set;
        }

        public Tender()
        {
        }
    }
}