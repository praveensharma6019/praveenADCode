using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniCapital.Website.Models
{
    [Serializable]
    public class LoginInfoModel
    {
        public LoginInfoModel()
        {
            getLoansList = new List<getLoansModel>();
            AssociatedLANList = new List<TextValueListItem>();
        }
        [Display(Name = "Loan Account Number")]
        public string LoanAccountNumber { get; set; }
        public static string InvalidAccountNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Login/Invalid Account", "Please enter a valid Loan Account Number");
        //[Display(Name = nameof(MobileCaption), ResourceType = typeof(LoginInfoModel))]
        //[RegularExpression(@"^(?:(?:\+|0{0,2})91(\s*[\ -]\s*)?|[0]?)?[789]\d{9}|(\d[ -]?){10}\d$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(LoginInfoModel))]
        public string MobileNo { get; set; }
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Login/Invalid Mobile", "Please enter a valid Mobile Number");
        public string OTP { get; set; }
        public string Message { get; set; }
        public string LastLoginInfo { get; set; }
        public string PageInfo { get; set; }
        public string ReturnUrl { get; set; }
        [Display(Name = "Remember Me")]
        public bool Rememberme { get; set; }
        public IEnumerable<FedAuthLoginButton> LoginButtons { get; set; }
        public bool IsAccountValid { get; set; }
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Login/Required", "Please enter {0}");
        public bool IsOTPSend { get; set; }
        public string MobileNoOrLoanAccountNumber { get; set; }
        public string MobileNoOrLoanAccountNumberValue { get; set; }
        public static string MobileCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Login/MobileNo", "Mobile Number");
        public List<getLoansModel> getLoansList { get; set; }
        public List<TextValueListItem> AssociatedLANList { get; set; }
    }
    [Serializable]
    public class UserInfoModel
    {
        public string LoanAccountNumber { get; set; }
        
    }
    [Serializable]
    public class TextValueListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    [Serializable]
    public class ReportsModel
    {
        public bool IsRequestValid { get; set; }
        public string PDF_report { get; set; }
        public string Message { get; set; }
    }
    [Serializable]
    public class getLoansModel
    {
        public getLoansModel()
        {
            loanDetailsList = new List<loanDetails>();
            identificationDetailsList = new List<identificationDetails>();
        }
        public string customerNumber { get; set; }
        //public string CustomerLAN { get; set; }
        public string customerName { get; set; }
        public string dateOfBirthOrInception { get; set; }
        public string panNumber { get; set; }
        public string individualCorporateFlag { get; set; }
        public List<loanDetails> loanDetailsList { get; set; }
        public List<identificationDetails> identificationDetailsList { get; set; }

    }
    [Serializable]
    public class loanDetails
    {       
        public string loanId { get; set; }
        public string loanAccountNumber { get; set; }
        public string productName { get; set; }
        public string financedAmount { get; set; }
        public string loanBranchCode { get; set; }
        public string tenure { get; set; }
        public string currencyIsoCode { get; set; }
        public string disbursalDate { get; set; }
        public string agreementDate { get; set; }
        public string productTypeDescription { get; set; }
        public string propertyAddress { get; set; }
        public string loanDisbursalStatus { get; set; }
        public string maturityFlag { get; set; }
        public string nextDueInstallmentAmount { get; set; }
        public string nextDueDate { get; set; }
        public string balanceTenure { get; set; }
        public string numberOfInstallmentUnpaid { get; set; }
        public string amountOverdue { get; set; }
        public string mobileNumber { get; set; }
        public string primaryEmailId { get; set; }
        public string vehicleRegistrationNumber { get; set; }
        public string loanStatus { get; set; }
        public string customerRelationship { get; set; }
        public string effectiveRateOfInterest { get; set; }
        public string interestChargeMode { get; set; }
        public string repaymentFrequency { get; set; }
        public string repaymentDueDay { get; set; }
        public string installmentAmount { get; set; }
        public string contractType { get; set; }
        public string contractSubType { get; set; }
        public string recoveryType { get; set; }
        public string recoverySubType { get; set; }
        public string overDraftType { get; set; }
        public string overDraftFlag { get; set; }
        //for get_loan_details API
        public string maturityDate { get; set; }
        public string disbursedAmount { get; set; }
        public string finalSanctionedAmount { get; set; }
        public string status { get; set; }
        public string principalOutstanding { get; set; }
        public string futurePrincipal { get; set; }
    }
    [Serializable]
    public class identificationDetails
    {
        public string identificationType { get; set; }
        public string identificationNumber { get; set; }
    }
}