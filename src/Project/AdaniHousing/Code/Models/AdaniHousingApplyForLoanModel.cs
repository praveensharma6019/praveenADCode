using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using System.Web.Mvc;

namespace Sitecore.AdaniHousing.Website.Models
{
    public class AdaniHousingApplyforLoanModel
    {
        Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
        
        public AdaniHousingApplyforLoanModel()
        {
            Item ProductsItemList = db.GetItem(Templates.ApplyForLoanDropdown.ProductListItem);
            if (ProductsItemList != null)
            {
                ProductsList = ProductsItemList.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value ?? "",
                    Value = x.Fields["Value"].Value ?? ""
                }).ToList();
            }
        }
        public Guid Id
        {
            get;
            set;
        }
        [RegularExpression("^[a-zA-Z0-9. ]*$", ErrorMessageResourceName = "InvalidFirstName", ErrorMessageResourceType = typeof(AdaniHousingApplyforLoanModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniHousingApplyforLoanModel))]
        public string FirstName
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniHousingApplyforLoanModel))]
        [RegularExpression("^[a-zA-Z0-9. ]*$", ErrorMessageResourceName = "InvalidLastName", ErrorMessageResourceType = typeof(AdaniHousingApplyforLoanModel))]
        public string LastName
        {
            get;
            set;
        }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(AdaniHousingApplyforLoanModel))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniHousingApplyforLoanModel))]
        public string EmailID
        {
            get;
            set;
        }
        //[RegularExpression("^([A-Za-z]){5}([0-9]){4}([A-Za-z]){1}$", ErrorMessageResourceName = "InvalidPanNumber", ErrorMessageResourceType = typeof(AdaniHousingApplyforLoanModel))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniHousingApplyforLoanModel))]
        //public string PanNumber
        //{
        //    get;
        //    set;
        //}
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(AdaniHousingApplyforLoanModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniHousingApplyforLoanModel))]
        public string MobileNo
        {
            get;
            set;
        }        
        [RegularExpression(@"^\d{6}(-\d{4})?$", ErrorMessage = "Invalid Postal Code.")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniHousingApplyforLoanModel))]
        public string PinCode
        {
            get;
            set;
        }
        //public string LoanAmount
        //{
        //    get;
        //    set;
        //}
        public string LMS_RequestKey
        {
            get;
            set;
        }
        public string LMS_Response
        {
            get;
            set;
        }
        //public string Tenure
        //{
        //    get;
        //    set;
        //}
        public string LastTabIndex
        {
            get;
            set;
        }
        public List<SelectListItem> ProductsList { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniHousingApplyforLoanModel))]
        public string ProductType
        {
            get;
            set;
        }
        //public List<SelectListItem> EnquirySourceList { get; set; }
        //public string EnquirySource
        //{
        //    get;
        //    set;
        //}
        public bool IsSubmittedToLMS
        {
            get;
            set;
        }
        //public bool IsSubmittedToFreshdesk
        //{
        //    get;
        //    set;
        //}
        public string reResponse
        {
            get;
            set;
        }
        //public string TicketID
        //{
        //    get;
        //    set;
        //}
        public string PageInfo
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
        public string AccessToken
        {
            get;
            set;
        }
        public string TokenType
        {
            get;
            set;
        }
        public static string InvalidEmailAddress
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/ApplyforLoan/Invalid Email Address", "Please enter a valid email address");
            }
        }
        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/ApplyforLoan/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }
        public static string InvalidFirstName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/ApplyforLoan/Invalid Name", "Please enter a valid Name");
            }
        }
        public static string InvalidLastName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/ApplyforLoan/Invalid Name", "Please enter a valid Name");
            }
        }
        public static string InvalidPanNumber
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/ApplyforLoan/Invalid PanNumber", "Please enter a valid PAN Number");
            }
        }
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniHousing/ApplyforLoan/Required", "Please enter value for {0}");
            }
        }
    }
    public class LeadMgmtResponseParam
    {
        public string Key
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }
    }
}