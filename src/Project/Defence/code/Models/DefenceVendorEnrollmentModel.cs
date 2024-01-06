using Sitecore.Data.Items;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sitecore.Defence.Website.Helper;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace Sitecore.Defence.Website.Models
{
    public class DefenceVendorEnrollmentModel
    {
        public HttpPostedFileBase UploadedCompanyProfileDoc
        {
            get;
            set;
        }
        public Guid Id { get; set; }
        public string CompanyProfileDocDLink { get; set; }
        public string RegistrationNo { get; set; }
        public string CompanyFullName
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }
        public string City
        {
            get;
            set;
        }
        public string Pincode
        {
            get;
            set;
        }
        public string ContactPerson
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DefenceVendorEnrollmentModel))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(DefenceVendorEnrollmentModel))]
        [DataType(DataType.EmailAddress)]
        public string EmailId
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DefenceVendorEnrollmentModel))]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(DefenceVendorEnrollmentModel))]
        public string MobileNo
        {
            get;
            set;
        }
        public string TelephoneNo
        {
            get;
            set;
        }
        public string Website
        {
            get;
            set;
        }

        public string GSTIN
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(DefenceVendorEnrollmentModel))]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}", ErrorMessageResourceName = nameof(InvalidPAN), ErrorMessageResourceType = typeof(DefenceVendorEnrollmentModel))]
        public string PanNo
        {
            get;
            set;
        }
        public string Tin
        {
            get;
            set;
        }
        public string ImportExportCode
        {
            get;
            set;
        }

        public string MSME
        {
            get;
            set;
        }
        public string TypeofOwnership
        {
            get;
            set;
        }
        public string NumberofEmployees
        {
            get;
            set;
        }
        public string SectorServed
        {
            get;
            set;
        }
        public string TypeofSupplier
        {
            get;
            set;
        }
        public string SegmentsServed
        {
            get;
            set;
        }
        public string PlatformsServed
        {
            get;
            set;
        }
        public string SupplierTo
        {
            get;
            set;
        }
        public string Clientele1
        {
            get;
            set;
        }
        public string Clientele2
        {
            get;
            set;
        }
        public string Clientele3
        {
            get;
            set;
        }
        public string Clientele4
        {
            get;
            set;
        }
        public string Clientele5
        {
            get;
            set;
        }
        public string CountryandCustomer1
        {
            get;
            set;
        }
        public string CountryandCustomer2
        {
            get;
            set;
        }
        public string CountryandCustomer3
        {
            get;
            set;
        }
        public string Commercial_FY1
        {
            get;
            set;
        }
        public string Commercial_FY2
        {
            get;
            set;
        }
        public string Commercial_FY3
        {
            get;
            set;
        }
        public string SalesValueDandA_FY1
        {
            get;
            set;
        }
        public string SalesValueDandA_FY2
        {
            get;
            set;
        }
        public string SalesValueDandA_FY3
        {
            get;
            set;
        }
        public string SalesValueNonDandA_FY1
        {
            get;
            set;
        }
        public string SalesValueNonDandA_FY2
        {
            get;
            set;
        }
        public string SalesValueNonDandA_FY3
        {
            get;
            set;
        }
        public string AnnualSales_FY1
        {
            get;
            set;
        }
        public string AnnualSales_FY2
        {
            get;
            set;
        }
        public string AnnualSales_FY3
        {
            get;
            set;
        }
        public string QualityCertification1
        {
            get;
            set;
        }
        public string QualityCertification2
        {
            get;
            set;
        }
        public string QualityCertification3
        {
            get;
            set;
        }
        public string QualityCertification4
        {
            get;
            set;
        }
        public string QualityCertification5
        {
            get;
            set;
        }
        public string AnyOtherCertification1
        {
            get;
            set;
        }
        public string AnyOtherCertification2
        {
            get;
            set;
        }
        public string AnyOtherCertification3
        {
            get;
            set;
        }
        public string AnyOtherCertification4
        {
            get;
            set;
        }
        public string AnyOtherCertification5
        {
            get;
            set;
        }
        public bool IsManufacturer
        {
            get;
            set;
        }
        public string ManufacturTypeList
        {
            get;
            set;
        }
        public string ManufacturerSpecification
        {
            get;
            set;
        }
        public bool IsTrader_Distributer_Dealer_Reseller
        {
            get;
            set;
        }
        public string Trade_Distriubtion_Deal_TypeList
        {
            get;
            set;
        }
        public string Trade_Distriubtion_Deal_Specifcation
        {
            get;
            set;
        }
        public bool IsSpclProcess_TestingLabs
        {
            get;
            set;
        }
        public string SpclProcess_TestLabs_TypeList
        {
            get;
            set;
        }
        public string SpclProcess_TestLabsSpecification
        {
            get;
            set;
        }
        public bool IsEngineeringServices
        {
            get;
            set;
        }
        public string EngineeringServicesTypeList
        {
            get;
            set;
        }
        public string EngineeringServicesSpecification
        {
            get;
            set;
        }
        public string SpecifykeyFacilities
        {
            get;
            set;
        }
        public string Place
        {
            get;
            set;
        }
        public DateTime FormDate
        {
            get;
            set;
        }
        public DateTime SubmitOnDate
        {
            get;
            set;
        }
        public string FormName
        {
            get;
            set;
        }
        public string PageInfo
        {
            get;
            set;
        }
        public string SubmittedBy
        {
            get;
            set;
        }
        public string reResponse
        {
            get;
            set;
        }
        public List<string> MSMEList { get; set; }
        public List<string> OwnershipTypeList { get; set; }
        public List<CheckBoxes> SectorServedList { get; set; }
        public List<CheckBoxes> SupplierTypeList { get; set; }
        public List<CheckBoxes> SegmentServedTypeList { get; set; }
        public List<CheckBoxes> DA_PlatformsServedList { get; set; }
        public List<CheckBoxes> SupplierToList { get; set; }
        public IEnumerable<SelectListItem> allManufacturTypeList { get; set; }
        public IEnumerable<SelectListItem> allTrader_D_DTypeList { get; set; }
        public IEnumerable<SelectListItem> allSpclPro_TLTypeList { get; set; }
        public IEnumerable<SelectListItem> allEnggServicesTypeList { get; set; }
        public string[] selectedManufacturType { get; set; }
        public string[] selectedTrader_D_DType { get; set; }
        public string[] selectedSpclPro_TLType { get; set; }
        public string[] selectedEnggServicesType { get; set; }
        public DefenceVendorEnrollmentModel()
        {
            Helper.Helper helper = new Helper.Helper(); 
            //MSMEList = new List<string>();
            MSMEList = helper.GetMSME();
            OwnershipTypeList = helper.GetTypeofOwnership();
            SectorServedList = helper.GetSectorServedList();
            SupplierTypeList = helper.GetSupplierTypeList();
            SegmentServedTypeList = helper.GetSegmentServedTypeList();
            DA_PlatformsServedList =helper.GetDA_PlatformsServedList();
            SupplierToList = helper.GetSupplierToList();
            allManufacturTypeList = helper.GetallManufacturTypeList();
            allTrader_D_DTypeList = helper.GetallTrader_D_DTypeList();
            allSpclPro_TLTypeList = helper.GetallSpclPro_TLTypeList();
            allEnggServicesTypeList = helper.GetallEnggServicesTypeList();
            if(helper.GetFinancialYearsList()!= null && helper.GetFinancialYearsList().Count==3)
            {
                Commercial_FY1 = helper.GetFinancialYearsList()[0];
                Commercial_FY2 = helper.GetFinancialYearsList()[1];
                Commercial_FY3 = helper.GetFinancialYearsList()[2];
            }
        }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Defence/Vendor-Form/Required", "Please enter value for {0}");

        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Defence/Vendor-Form/Invalid Email Address", "Please enter a valid email address");
        public static string InvalidPAN => DictionaryPhraseRepository.Current.Get("/Defence/Vendor-Form/Invalid Pan details", "Please enter a valid PAN details");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("//Defence/Vendor-Form/Invalid Mobile", "Please enter a valid Mobile Number");
    }

    public class CheckBoxes
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
    }
    public class FileUpload1
    {
        public string ErrorMessage { get; set; }
        public decimal filesize { get; set; }
        public string UploadUserFile(HttpPostedFileBase file)
        {
            try
            {
                var supportedTypes = new[] { "pdf", "doc", "docx" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!string.Equals(file.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase) && !string.Equals(file.ContentType, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", StringComparison.OrdinalIgnoreCase) && !string.Equals(file.ContentType, "application/msword", StringComparison.OrdinalIgnoreCase) || Regex.IsMatch(file.FileName, @".*\..*\..*") || file.FileName.Contains(">") || file.FileName.Contains("<"))
                {
                    ErrorMessage = file.FileName + " is invalid file - Only Upload pdf/doc/docx File";
                    return ErrorMessage;
                }
                if (!supportedTypes.Contains(fileExt.ToLower()))
                {
                    ErrorMessage = file.FileName + " is invalid file - Only Upload pdf/doc/docx File";
                    return ErrorMessage;
                }
                else if (file.ContentLength > ((filesize * 1024) * 1024))
                {
                    ErrorMessage = file.FileName + "is too big, file size Should Be UpTo " + filesize + "MB";
                    return ErrorMessage;
                }
                else
                {
                    ErrorMessage = string.Empty;
                    return ErrorMessage;
                }
            }
            catch (Exception)
            {
                ErrorMessage = "Upload Container Should Not Be Empty or Contact Admin";
                return ErrorMessage;
            }
        }
    }
}