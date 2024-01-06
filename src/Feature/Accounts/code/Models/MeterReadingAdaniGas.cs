using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class MeterReadingAdaniGas
    {
        public string CustomerID { get; set; }
        public string MeterNumber { get; set; }
        public string PreviousMeterReading { get; set; }
        public string PreviousMeterReadingDateandTime { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(MeterReadingAdaniGas))]
        public string MeterReading { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(MeterReadingAdaniGas))]
        public string ReadingDateandTime { get; set; }

        public string ReturnViewMessage { get; set; }
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
    }

    [Serializable]
    public class SelfBillingAdaniGas
    {
        public SelfBillingAdaniGas()
        {
            MeterReading1 = 0;
            MeterReading2 = 0;
            MeterReading3 = 0;
            MeterReading4 = 0;
            MeterReading5 = 0;
            MeterReading6 = 0;
            MeterReading7 = 0;
            MeterReading8 = 0;
        }
        public string BusinessPartnerNumber { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string ResendDisable { get; set; }
        public string OTPNumber { get; set; }
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
        public string PreviousMeterReading { get; set; }
        public string PreviousMeterReadingDateandTime { get; set; }
        public string NextMeterReadingDateandTime { get; set; }
        public string CONTRACTNO { get; set; }
        public string DEV_CAT { get; set; }
        public bool IsSuccessful { get; set; }
        public string MRIDNUMBER { get; set; }
        public string SERNR { get; set; }
        public string ISTABLART { get; set; }
        public string FilePath { get; set; }

        public string ExternalMessage { get; set; }
        public string Message { get; set; }
        public string PDFData { get; set; }
        public bool IsRelease { get; set; }

        public string MeterReadingImagePath { get; set; }

        [Required]
        public HttpPostedFileBase MeterReadingImage { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(MeterReadingAdaniGas))]
        public decimal MeterReading { get; set; }


        public decimal MeterReading1 { get; set; }
        public decimal MeterReading2 { get; set; }
        public decimal MeterReading3 { get; set; }
        public decimal MeterReading4 { get; set; }
        public decimal MeterReading5 { get; set; }
        public decimal MeterReading6 { get; set; }
        public decimal MeterReading7 { get; set; }
        public decimal MeterReading8 { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(MeterReadingAdaniGas))]
        public string ReadingDateandTime { get; set; }

        public string ReturnViewMessage { get; set; }
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
    }

    public class SelfBillingRequest
    {
        public string MODE { get; set; }
        public string CONTRACTACCOUNT { get; set; }
        public string DEVICE { get; set; }
        public string MRRESULT { get; set; }
        public string METERIMAGE { get; set; }
        public string Token { get; set; }
        public string MRDATE { get; set; }
        public string MRTIME { get; set; }
        public string MRIDNUMBER { get; set; }
        public string PARTNERNAME { get; set; }
        public string CONTRACTNO { get; set; }
        public string SHMRDATE { get; set; }
        public string PRV_MR_DATE { get; set; }
        public string PRV_RESULT { get; set; }
        public string DEV_CAT { get; set; }
    }
}