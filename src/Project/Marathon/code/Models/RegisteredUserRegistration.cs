using Sitecore.Marathon.Website.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Marathon.Website.Models
{
    public class RegisteredUserRegistration
    {
        public string RunMode { get; set; }

        [RegularExpression("^[0-9a-z A-Z.-]{3,15}$", ErrorMessage = "Please enter a valid Reference Code.")]
        public string ReferenceCode { get; set; }

        [RegularExpression("^[a-z A-Z]{3,50}$", ErrorMessage = "Please select a valid run type.")]
        public string RunType { get; set; }

        [RegularExpression("^[0-9.a-z A-Z]{3,50}$", ErrorMessage = "Please enter a valid Race Distance.")]
        public string RaceDistance { get; set; }

        [RegularExpression("^[0-9a-z A-Z]{3,50}$", ErrorMessage = "Please enter a valid employee  id")]
        public string EmployeeID { get; set; }

        [RegularExpression("^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,17}$", ErrorMessage = "Please enter a valid employee email id")]
        public string EmployeeEmailId { get; set; }

        [DecimalValidator(ErrorMessage = "Please enter a valid donation amount")]
        [DonationAmount(ErrorMessage = "Please enter a valid donation amount")]
        public decimal DonationAmount { get; set; }

        [RegularExpression("^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Please enter a valid PAN Number.")]
        public string PANNumber { get; set; }
        public bool TaxExemptionCertificate { get; set; }
        [RegularExpression("^[0-9.a-z A-Z]{3,500}$", ErrorMessage = "Please enter a valid Tax Exemption Cause.")]
        public string TaxExemptionCause { get; set; }

        public string reResponse { get; set; }

    }
}