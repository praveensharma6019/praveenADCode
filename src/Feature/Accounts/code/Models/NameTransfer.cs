using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Web;
using Newtonsoft.Json;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class NameTransfer
    {

        public string CustomerID { get; set; }

        public string MeterNumberID { get; set; }

    }

    [Serializable]
    public class ByCustomerId
    {
        public ByCustomerId()
        {
            AreaList = new List<SelectListItem>();
            CityList = new List<SelectListItem>();
            ApartmentComplexList = new List<SelectListItem>();
            HouseNumberList = new List<SelectListItem>();
            OTPNumber = null;
            IsOTPSent = false;
            IsOTPValid = false;
            IsvalidatAccount = false;
            Captcha = null;
            CustomerID = null;
            CustomerName = null;
            CustomerPartnerNumber = null;
            CustomerAddress = null;
            MeterNumber = null;
            MsgFlag = null;
            Message = null;
            CustomerValidationType = null;
            RegisterNameTransferList = new List<NameTransferRequestDetail>();
            
        }

        public List<NameTransferStatus> NameTransferstatusList { get; set; }
        public List<string> NameTransferCheckStatusList { get; set; }
        public List<SelectListItem> AreaList { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> ApartmentComplexList { get; set; }
        public List<SelectListItem> HouseNumberList { get; set; }
        public string HouseNumber { get; set; }
        public string Reg_Str_Grp { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string ApartmentComplex { get; set; }

        public List<NameTransferAdminRegistration> adminregistrationlist { get; set; }

        [Required]
        public string InputByUser { get; set; }
        public Guid id { get; set; }
        public string ProceedWith { get; set; }
        //[Required]
        public string OTPNumber { get; set; }
        public bool CustomerIdValidated { get; set; }
        public bool IsOTPSent { get; set; }
        public bool IsOTPValid { get; set; }
        public bool IsvalidatAccount { get; set; }
        public string Captcha { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPartnerNumber { get; set; }
        public string CustomerAddress { get; set; }
        public string MeterNumber { get; set; }
        public string MsgFlag { get; set; }
        public string Message { get; set; }
        public string CustomerValidationType { get; set; }
        public string ConnectionType { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ByCustomerId))]
        public string MobileNumber { get; set; }
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");

        public string PreMobileNumber { get; set; }

        public string EmailId { get; set; }
        public string Address { get; set; }
        public string Amount { get; set; }
        [Required]
        public string ApplicationType { get; set; }
        [Required]
        public string SocietyType { get; set; }
        public string GasSupply { get; set; }
       
        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Comment { get; set; }

        public HttpPostedFileBase IndexBuilder { get; set; }
        public string IndexBuilderId { get; set; }
        public HttpPostedFileBase PossessionletterBuilder { get; set; }
        public string PossessionletterBuilderId { get; set; }
        public HttpPostedFileBase PhotoIDBuilder { get; set; }
        public string PhotoIDBuilderId { get; set; }
        public HttpPostedFileBase CoownerBuilder { get; set; }
        public string CoownerBuilderId { get; set; }
        public HttpPostedFileBase SignedIDproof { get; set; }
        public string SignedIDproofId { get; set; }

        public HttpPostedFileBase DeathCertificateDemise { get; set; }
        public string DeathCertificateDemiseId { get; set; }
        public HttpPostedFileBase DocumentaryDemise { get; set; }
        public string DocumentaryDemiseId { get; set; }
        public HttpPostedFileBase IDProofDemise { get; set; }
        public string IDProofDemiseId { get; set; }
        public HttpPostedFileBase NOCDemise { get; set; }
        public string NOCDemiseId { get; set; }
        public HttpPostedFileBase SignedIDproofDemise { get; set; }
        public string SignedIDproofDemiseId { get; set; }


        public HttpPostedFileBase FirstRegisteredHousingSocietypropertyresale { get; set; }
        public string FirstRegisteredHousingSocietypropertyresaleId { get; set; }
        public HttpPostedFileBase SecondRegisteredHousingSocietypropertyresale { get; set; }
        public string SecondRegisteredHousingSocietypropertyresaleId { get; set; }
        public HttpPostedFileBase NOCRegisteredHousingSocietypropertyresale { get; set; }
        public string NOCRegisteredHousingSocietypropertyresaleId { get; set; }
        public HttpPostedFileBase SignedIDproofRegisteredHousingSocietypropertyresale { get; set; }
        public string SignedIDproofRegisteredHousingSocietypropertyresaleId { get; set; }


        public HttpPostedFileBase FirstUnregisteredHousingSocietypropertyresale { get; set; }
        public string FirstUnregisteredHousingSocietypropertyresaleId { get; set; }
        public HttpPostedFileBase SecondUnregisteredHousingSocietypropertyresale { get; set; }
        public string SecondUnregisteredHousingSocietypropertyresaleId { get; set; }
        public HttpPostedFileBase IDProofUnregisteredHousingSocietypropertyresale { get; set; }
        public string IDProofUnregisteredHousingSocietypropertyresaleId { get; set; }
        public HttpPostedFileBase NOCUnregisteredHousingSocietypropertyresale { get; set; }
        public string NOCUnregisteredHousingSocietypropertyresaleId { get; set; }
        public HttpPostedFileBase SignedIDproofUnregisteredHousingSocietypropertyresale { get; set; }
        public string SignedIDproofUnregisteredHousingSocietypropertyresaleId { get; set; }


        public HttpPostedFileBase DeathCertificateDemiseUnregisteredHousing { get; set; }
        public string DeathCertificateDemiseUnregisteredHousingId { get; set; }

        public HttpPostedFileBase MunicipalCorporationTaxBillDemiseUnregisteredHousing { get; set; }
        public string MunicipalCorporationTaxBillDemiseUnregisteredHousingId { get; set; }
        public HttpPostedFileBase ElectricityBillDemiseUnregisteredHousing { get; set; }
        public string ElectricityBillDemiseUnregisteredHousingId { get; set; }
        public HttpPostedFileBase OtherDemiseUnregisteredHousing { get; set; }
        public string OtherDemiseUnregisteredHousingId { get; set; }

        public HttpPostedFileBase IDProofDemiseUnregisteredHousing { get; set; }
        public string IDProofDemiseUnregisteredHousingId { get; set; }
        public HttpPostedFileBase NOCDemiseUnregisteredHousing { get; set; }
        public string NOCDemiseUnregisteredHousingId { get; set; }
        public HttpPostedFileBase SignedIDprooDemiseUnregisteredHousing { get; set; }
        public string SignedIDprooDemiseUnregisteredHousingID { get; set; }

        public HttpPostedFileBase isAdditionalDocument { get; set; }
        public string isAdditionalDocumentID { get; set; }

        public string stepOne { get; set; }
        public bool stepTwo { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public QuickPayNameTransfer quickPayNameTransfer { get; set; }

        public List<NameTransferRequestDetail> RegisterNameTransferList { get; set; }

        public decimal PayableTotalAmount { get; set; }
        public decimal ProcessingAmount { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string RegisterdConsumerName { get; set; }

        public string TransactionNumber { get; set; }

        public string ApprovedComment { get; set; }
        public string ShowApprovedComment { get; set; }
        
        public string RejectedComment { get; set; }
        public string ShowRejectedComment { get; set; }
        
        public string AdditionalDetailsComment { get; set; }
        public string ShowAdditionalDetailsComment { get; set; }

        public string ReopenApplicationComment { get; set; }

        [Required]
        public bool isAdditionalDocumentRequired { get; set; }
        public string ShowisAdditionalDocumentRequired { get; set; }

        public bool isAdditionalPaymentRequired { get; set; }
        public int AddAdditionalPayment { get; set; }

        public int TotalRequest { get; set; }
        public int TotalSubmittedApplication { get; set; }
        public int TotalPaymentDoneApplication { get; set; }
        public int TotalApprovedApplication { get; set; }
        public int TotalRejectedApplication { get; set; }
        public int TotalResubmittedApplication { get; set; }
        public int Total_Additional_Details_document_Required { get; set; }
        public int Total_Additional_Details_document_Payment_Required { get; set; }
        public int Total_Application_Submited_But_Payment_Required { get; set; }
        public int Total_Additional_Payment_Done_Required { get; set; }

        public string AdminCityName { get; set; }

        public string PreStatus { get; set; }
        public string RequestNumber { get; set; }
        public string OtherDetails { get; set; }

        public string DocumentStatus1 { get; set; }
        public string DocumentStatus2 { get; set; }
        public string DocumentStatus3 { get; set; }
        public string DocumentStatus4 { get; set; }
        public string DocumentStatus5 { get; set; }
        public string DocumentStatus6 { get; set; }
        public string DocumentStatus7 { get; set; }
        public string DocumentStatus8 { get; set; }
        public string DocumentStatus9 { get; set; }
        public string DocumentStatus10 { get; set; }
        public string DocumentStatus11 { get; set; }
        public string DocumentStatus12 { get; set; }
        public string DocumentStatus13 { get; set; }
        public string DocumentStatus14 { get; set; }
        public string DocumentStatus15 { get; set; }
        public string DocumentStatus16 { get; set; }
        public string DocumentStatus17 { get; set; }
        public string DocumentStatus18 { get; set; }
        public string DocumentStatus19 { get; set; }
        public string DocumentStatus0 { get; set; }
        public string DocumentStatus20 { get; set; }
        public string DocumentStatus21 { get; set; }
        public string DocumentStatus22 { get; set; }
        public string DocumentStatus23 { get; set; }
        public string DocumentStatus24 { get; set; }
        public string DocumentStatus25 { get; set; }
        public string DocumentStatus26 { get; set; }

        public string AdminCommentForRejectDocument1 { get; set; }
        public string AdminCommentForRejectDocument2 { get; set; }
        public string AdminCommentForRejectDocument3 { get; set; }
        public string AdminCommentForRejectDocument4 { get; set; }
        public string AdminCommentForRejectDocument5 { get; set; }
        public string AdminCommentForRejectDocument6 { get; set; }
        public string AdminCommentForRejectDocument7 { get; set; }
        public string AdminCommentForRejectDocument8 { get; set; }
        public string AdminCommentForRejectDocument9 { get; set; }
        public string AdminCommentForRejectDocument10 { get; set; }
        public string AdminCommentForRejectDocument11 { get; set; }
        public string AdminCommentForRejectDocument12 { get; set; }
        public string AdminCommentForRejectDocument13 { get; set; }
        public string AdminCommentForRejectDocument14 { get; set; }
        public string AdminCommentForRejectDocument15 { get; set; }
        public string AdminCommentForRejectDocument16 { get; set; }
        public string AdminCommentForRejectDocument17 { get; set; }
        public string AdminCommentForRejectDocument18 { get; set; }
        public string AdminCommentForRejectDocument19 { get; set; }
        public string AdminCommentForRejectDocument0 { get; set; }
        public string AdminCommentForRejectDocument20 { get; set; }
        public string AdminCommentForRejectDocument21 { get; set; }
        public string AdminCommentForRejectDocument22 { get; set; }
        public string AdminCommentForRejectDocument23 { get; set; }
        public string AdminCommentForRejectDocument24 { get; set; }
        public string AdminCommentForRejectDocument25 { get; set; }
        public string AdminCommentForRejectDocument26 { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public List<NameTransferRequestDetail> SearchApplicationNameTransferList { get; set; }

        public List<SelectListItem> NameTransferRequestStatus { get; set; }

        public string CustomerEmailId { get; set; }

        public string IndexBuilder_content_type { get; set; }
        public string PossessionletterBuilder_content_type { get; set; }
        public string PhotoIDBuilder_content_type { get; set; }
        public string CoownerBuilder_content_type { get; set; }
        public string SignedIDproof_content_type { get; set; }

        public string DeathCertificateDemise_content_type { get; set; }
        public string DocumentaryDemise_content_type { get; set; }
        public string IDProofDemise_content_type { get; set; }
        public string NOCDemise_content_type { get; set; }
        public string SignedIDproofDemise_content_type { get; set; }

        public string MunicipalCorporationTaxBillDemiseUnregisteredHousing_content_type { get; set; }
        public string ElectricityBillDemiseUnregisteredHousing_content_type { get; set; }
        public string OtherDemiseUnregisteredHousing_content_type { get; set; }
        public string DeathCertificateDemiseUnregisteredHousing_content_type { get; set; }
        public string IDProofDemiseUnregisteredHousing_content_type { get; set; }
        public string NOCDemiseUnregisteredHousing_content_type { get; set; }
        public string SignedIDproofDemiseUnregisteredHousing_content_type { get; set; }

        public string FirstRegisteredHousingSocietypropertyresale_content_type { get; set; }
        public string SecondRegisteredHousingSocietypropertyresale_content_type { get; set; }
        public string NOCRegisteredHousingSocietypropertyresale_content_type { get; set; }
        public string SignedIDproofRegisteredHousingSocietypropertyresale_content_type { get; set; }

        public string FirstUnregisteredHousingSocietypropertyresale_content_type { get; set; }
        public string SecondUnregisteredHousingSocietypropertyresale_content_type { get; set; }
        public string IDProofUnregisteredHousingSocietypropertyresale_content_type { get; set; }
        public string NOCUnregisteredHousingSocietypropertyresale_content_type { get; set; }
        public string SignedIDproofUnregisteredHousingSocietypropertyresale_content_type { get; set; }

        public string ClosedApplicationComment { get; set; }


    }
}
