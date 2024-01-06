using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models.AdaniGasCNGRegistration
{
    [Serializable]
    public class AdaniGasCNG_AdminDashboardModel
    {
        public AdaniGasCNG_AdminDashboardModel()
        {
            RegisteredInquiryList = new List<RegisteredInquiryDetails>();
        }
        public string CustomerRegistrationNo { get; set; }
        public string UserMobileNo { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PageInfo { get; set; }
        public string CreatedBy { get; set; }
        public string CurrentAddress { get; set; }
        public string RegisteredAddress { get; set; }
        public string EmailId { get; set; }
        public string YearofPurch { get; set; }
        public bool IsValidRegistrationNo { get; set; }
        public List<RegisteredInquiryDetails> RegisteredInquiryList { get; set; }
    }
    
    [Serializable]
    public class AdaniGasCNG_AdminFormModel
    {
        public AdaniGasCNG_AdminFormModel()
        {
            RegisteredInquiryList = new List<AdaniGasCNG_CustomerRegistration>();
        }
        public string EnquiryNoForDealer { get; set; }
        public Guid CustomerGUID { get; set; }
        public string CustomerRegistrationNo { get; set; }
        public string DealerMobileNo { get; set; }
        public string DealerName { get; set; }
        public string DealerId { get; set; }
        public string UserMobileNo { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string CNG_KitNumber { get; set; }
        public string VehicleInsureanceFileLink { get; set; }
        public bool IsValidVehicleInsureanceFile { get; set; }
        public string RC_BookFileLink { get; set; }
        public bool IsValidRC_BookFile { get; set; }
        public string AadharCardFileLink { get; set; }
        public bool IsValidAadharCardFile { get; set; }
        public string PAN_CardFileLink { get; set; }
        public bool IsValidPAN_CardFile { get; set; }
        public string CNG_CylinderCertiFileLink { get; set; }
        public bool IsValidCNG_CylinderCertiFile { get; set; }
        public string InvoiceFileLink { get; set; }
        public bool IsValidInvoiceFile { get; set; }
        public string RTO_App_ReceiptFileLink { get; set; }
        public bool IsValidRTO_App_ReceiptFile { get; set; }
        public string RTO_CertiFileLink { get; set; }
        public bool IsValidRTO_CertiFile { get; set; }
        public string SignedSchemeDocFileLink { get; set; }
        public bool IsValidSignedSchemeDocFile { get; set; }
        public bool IsVerifiedByDealer { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PageInfo { get; set; }
        public string OTP { get; set; }
        public string CreatedBy { get; set; }
        public string CurrentStatus { get; set; }
        public bool IsValidRegistrationNo { get; set; }
        public bool IsSavedIntoDatabase { get; set; }
        public List<TextValueItem> StatusList { get; set; }
        public string UpdatedStatus { get; set; }
        public List<Documents> DocumentsList { get; set; }
        public List<AdaniGasCNG_CustomerRegistration> RegisteredInquiryList { get; set; }
    }
}