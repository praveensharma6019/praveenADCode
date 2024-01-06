namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class ChangeOfNameApplicationsStatusModel
    {
        public string LECMobileNumber { get; set; }
        public string LECRegistrationNumber { get; set; }
        public bool IsOTPSent { get; set; }
        public bool IsvalidatAccount { get; set; }
        public string OTPNumber { get; set; }
        public bool IsValidatedOTP { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[Required]
        public string StartDate { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[Required]
        public string EndDate { get; set; }

        public List<CONApplicationDetail> CONApplicationsList { get; set; }
    }
}