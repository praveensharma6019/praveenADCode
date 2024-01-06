using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Web.Mvc;
using System;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class ViewPayBill
    {
        private string _currencytype;
        private string _msg;
        private double _advanceammount;

        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidLoginName), ErrorMessageResourceType = typeof(ViewPayBill))]
        public string LoginName { get; set; }
        public string AccountNumber { get; set; }
        public string BookNumber { get; set; }
        public string CycleNumber { get; set; }
        public string Zone { get; set; }
        public string Address { get; set; }
        public string BillMonth { get; set; }
        public string PaymentDueDate { get; set; }
        public string TariffSlab { get; set; }
        public string MeterNumber { get; set; }
        public string UnitsConsumed { get; set; }
        public string TotalCharges { get; set; }
        public string CurrentMonthsBills { get; set; }
        public string BroughtForward { get; set; }
        public string TotalBillAmount { get; set; }
        public string SecurityDeposit { get; set; }
        public decimal AverageVDSAmount { get; set; }
        public decimal PaymentVDSAmount { get; set; }
        public string PANNo { get; set; }
        public string AmountPayable { get; set; }
        public string AmountPayableshow { get; set; }
        public int PaymentGateway { get; set; }
        public string Captcha { get; set; }
        public string OrderId { get; set; }
        public string SecurityDepositAmountType { get; set; }
        public decimal SecurityDepositPartial { get; set; }
        public string Flag { get; set; }

        public string Message { get; set; }
        public bool EMIEligible { get; set; }
        public bool ProceedWithEMI { get; set; }
        public decimal EMIInstallmentAmount { get; set; }
        public decimal EMIOutstandingAmount { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ViewPayBill))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ViewPayBill))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ViewPayBill))]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(ViewPayBill))]
        public string Mobile { get; set; }

        public decimal MaxVDSAmount { get; set; }


        public string SecurityPayment { get; set; }
        public List<SelectListItem> SecurityPaymentList { get; set; }

        public List<SelectListItem> VDSPaymentList { get; set; }

        public string VDSPaymentSelection { get; set; }
        public double AdvanceAmmount
        {
            get
            {
                return this._advanceammount;
            }
            set
            {
                double outputval = 0;
                if (double.TryParse(AmountPayable, out outputval))
                {
                    if (outputval > 0)
                    {
                        this._advanceammount = 0;
                    }
                    else
                    {
                        this._advanceammount = value;
                    }

                }
            }
        }
        public string CurrencyType
        {
            get
            {
                return this._currencytype = "INR";
            }
            set
            {
                this._currencytype = value;
            }
        }
        public string msg
        {
            get
            {
                return this._msg;
            }
            set
            {
                this._msg = value;
            }
        }

        public string ItemID { get; set; }
        public string ResponseStatus { get; set; }
        public string TransactionId { get; set; }
        public string Responsecode { get; set; }
        public string Remark { get; set; }
        public string PaymentRef { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentModeNumber { get; set; }
        public string PaymentType { get; set; }
        public string UserType { get; set; }

        public string Name { get; set; }

        public string TransactionDate { get; set; }
        public bool DueDateGraterthenFourDays { get; set; }

        public string SecurityDepositMsg { get; set; }
        public static string InvalidLoginName => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Login Name", "Please enter a Valid Login Name");

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");

        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");

        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
    }
}