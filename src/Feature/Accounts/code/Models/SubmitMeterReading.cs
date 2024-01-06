using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class SubmitMeterReading
    {
        [MaxLength(12)]
        [MinLength(1)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "CA number must be numeric")]
        public string CANumber { get; set; }
        public string Source { get; set; }
        public List<MeterReadingDetail> MeterList { get; set; }
        public List<MeterAttachment> MeterAttachments { get; set; }
        public bool IsSubmitted{ get; set; }
        public string Result { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string MobileNumber { get; set; }
    }

    [Serializable]
    public class MeterReadingDetail
    {
        [MaxLength(15)]
        [MinLength(1)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Meter number must be numeric")]
        public string MeterNumber { get; set; }
        [MaxLength(10)]
        [MinLength(10)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string MobileNumber { get; set; }
        public string MeterReading { get; set; }
        public string SMRD { get; set; }
        // [RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "Invalid date format.")]
        [DataType(DataType.Date)]
        public DateTime MeterReadingDate { get; set; }
        public HttpPostedFileBase[] File { get; set; }
    }
    [Serializable]
    public class MeterAttachment
    {
        public Guid Id { get; set; }
        public byte[] FileByte { get; set; }
        public string FileCT { get; set; }
        public string FileName { get; set; }
    }
    }