namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class AdaniGasENachRegistrationModel 
    {
        public static string InvalidCustomerId => DictionaryPhraseRepository.Current.Get("/Accounts/ENachRegister/Invalid CustomerID", "Customer ID is invalid. Please enter a valid Customer Id.");
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(AdaniGasENachRegistrationModel))]
        [RegularExpression(@"^[0-9]{8,12}$", ErrorMessageResourceName = nameof(InvalidCustomerId), ErrorMessageResourceType = typeof(AdaniGasENachRegistrationModel))]
        public string CustomerID { get; set; }

        public string Name { get; set; }

        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(AdaniGasENachRegistrationModel))]
        public string MobileNo { get; set; }

        public string EmailId { get; set; }
        public string UserType { get; set; }
        //public string PaymentType { get; set; }
        public string CustomerType { get; set; }
        public string reResponse { get; set; }
        public string ResendDisable { get; set; }
        public string CustomerName { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
        public string MessageFlag { get; set; }
        public string OTP_Validity_Minutes { get; set; }
        public string OrderId { get; set; }
        public string ResponseStatus { get; set; }
        public string TransactionId { get; set; }
        public string Responsecode { get; set; }
        public string Remark { get; set; }
        public string BankRefNo { get; set; }
        public string AccountType { get; set; }
        public string TransactionDate { get; set; }
        public string PaymentGateway { get; set; }
        public double Amount { get; set; }
        public string SIAmount { get; set; }
        public string BPUA { get; set; }
        public string OTPNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string UMRN { get; set; }
        public string ErrorStatus { get; set; }
        public string ErrorDescription { get; set; }
        public string ExistingRefNo { get; set; }
        public string ECSCurrentStatus { get; set; }
        public string SapECSMsgFlag { get; set; }
        public string SapECSMsg { get; set; }
        public bool IsSapECSPostError { get; set; }

        public bool IsvalidatAccount { get; set; }

        public bool IsOTPSent { get; set; }
        public bool IsOTPValid { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
    }

    public class AdaniGasENachRedirectionModel
    {
        public string AppID { get; set; }
        public string EntityMerchantKey { get; set; }
        public string ToDebit { get; set; }
        public string Amount { get; set; }
        public string Frequency { get; set; }
        public string DebitType { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailID { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NameInBankRecords1 { get; set; }
        public string NameInBankRecords2 { get; set; }
        public string NameInBankRecords3 { get; set; }
    }

    public class ValidatedOTPModel
    {
        public string Mobile { get; set; }
        public bool IsOTPValid { get; set; }
        public string OTP { get; set; }
        public string Message { get; set; }
        public string Msg_Flag { get; set; }
        public bool IsError { get; set; }
    }
}