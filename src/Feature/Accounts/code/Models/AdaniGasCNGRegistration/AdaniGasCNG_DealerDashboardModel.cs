using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models.AdaniGasCNGRegistration
{
    [Serializable]
    public class AdaniGasCNG_DealerDashboardModel
    {
        public AdaniGasCNG_DealerDashboardModel()
        {
            RegisteredInquiryList = new List<RegisteredInquiryDetails>();
        }
        public string CustomerRegistrationNo { get; set; }
        public string DealerMobileNo { get; set; }
        public string DealerName { get; set; }
        public string DealerId { get; set; }
        public string Message { get; set; }
        public HttpPostedFileBase VehicleInsuranceFile { get; set; }
        public HttpPostedFileBase RC_BookFile { get; set; }
        public HttpPostedFileBase AadharCardFile { get; set; }
        public HttpPostedFileBase PAN_CardFile { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PageInfo { get; set; }
        public string CreatedBy { get; set; }
        public bool IsValidRegistrationNo { get; set; }
        public List<RegisteredInquiryDetails> RegisteredInquiryList { get; set; }
    }
    [Serializable]
    public class RegisteredInquiryDetails
    {        
        public string DealerId { get; set; }
        public Guid CustomerGUID { get; set; }
        public string Name { get; set; }
        public string CustomerRegnNo { get; set; }
        public bool IsBSVI { get; set; }
        public string VehicleType { get; set; }
        public string YearOfPurchase { get; set; }
        public string MobileNo { get; set; }
        public string VehicleCompany { get; set; }
        public string vehicleModel { get; set; }
        public string VehicleNo { get; set; }
        public string EnquiryNoForDealer { get; set; }
        public string CNG_KitNumber { get; set; }
        public string VehicleInsureanceFileLink { get; set; }
        public string RC_BookFileLink { get; set; }
        public string AadharCardFileLink { get; set; }
        public string PAN_CardFileLink { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdateHistory { get; set; }
        public List<Documents> DocumentsList { get; set; }
    }
    [Serializable]
    public class AdaniGasCNG_DealerRegisterModel
    {
        public AdaniGasCNG_DealerRegisterModel()
        {
            RegisteredInquiryList = new List<RegisteredInquiryDetails>();
        }
        public bool hasError { get; set; }
        public string EnquiryNoForDealer { get; set; }
        public Guid CustomerGUID { get; set; }
        public string CustomerRegistrationNo { get; set; }
        public string DealerMobileNo { get; set; }
        public string DealerName { get; set; }
        public string DealerId { get; set; }
        public string Message { get; set; }
        public string CNG_KitNumber { get; set; }
        public string VehicleInsureanceFileLink { get; set; }
        public HttpPostedFileBase VehicleInsuranceFile { get; set; }
        public string RC_BookFileLink { get; set; }
        public HttpPostedFileBase RC_BookFile { get; set; }
        public string AadharCardFileLink { get; set; }
        public HttpPostedFileBase AadharCardFile { get; set; }
        public string PAN_CardFileLink { get; set; }
        public HttpPostedFileBase PAN_CardFile { get; set; }
        public string CNG_CylinderCertiFileLink { get; set; }
        public HttpPostedFileBase CNG_CylinderCertiFile { get; set; }
        public string InvoiceFileLink { get; set; }
        public HttpPostedFileBase InvoiceFileFile { get; set; }
        public string RTO_App_ReceiptFileLink { get; set; }
        public HttpPostedFileBase RTO_App_ReceiptFile { get; set; }
        public string RTO_CertiFileLink { get; set; }
        public HttpPostedFileBase RTO_CertiFile { get; set; }
        public string SignedSchemeDocFileLink { get; set; }
        public HttpPostedFileBase SignedSchemeDocFile { get; set; }
        public bool IsVerifiedByDealer { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PageInfo { get; set; }
        public string OTP { get; set; }
        public string CreatedBy { get; set; }
        public string CurrentStatus { get; set; }
        public bool IsValidRegistrationNo { get; set; }
        public bool IsSavedIntoDatabase { get; set; }
        public List<Documents> DocumentsList { get; set; }
        public List<RegisteredInquiryDetails> RegisteredInquiryList { get; set; }
    }
    [Serializable]
    public class CNGDocuments
    {
        public Guid CustomerGUID { get; set; }
        public string CustomerRegistrationNo { get; set; }
        public HttpPostedFileBase DocFile { get; set; }
        public Guid Id { get; set; }
        public string DocName { get; set; }
        public string DocDownloadLink { get; set; }
        public string DocumentBase64 { get; set; }
        public string DocType { get; set; }
        public byte[] DocData { get; set; }
        public string DocContentType { get; set; }
        public string Message { get; set; }
        public string DocDescritption { get; set; }
        public bool Editable { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string ReviewedBy { get; set; }
        public bool SavedIntoDB { get; set; }
    }
}