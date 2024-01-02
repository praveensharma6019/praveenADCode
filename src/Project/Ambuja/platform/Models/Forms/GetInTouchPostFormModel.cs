using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.AmbujaCement.Website.Models.Forms
{
    public class GetInTouchPostFormModel
    {
        [Required(ErrorMessage = "Please enter First Name")]
        public string firstName { get; set; }
        [Required(ErrorMessage = "Please enter Last Name")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "Please enter Mobile Number")]
        public string phoneNo { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string email { get; set; }

        [Required(ErrorMessage = "Please enter what you are Looking for")]
        public string lookingFor { get; set; }

        [Required(ErrorMessage = "Please enter your Query")]
        public string queryType { get; set; }

        [Required(ErrorMessage = "Please enter your State")]
        public string state { get; set; }

        [Required(ErrorMessage = "Please enter your City")]
        public string district { get; set; }

        [Required(ErrorMessage = "Please accept the terms.")]
        public bool termsAndConditions { get; set; }

        //[Required(ErrorMessage = "Please enter OTP")]
        public string otp { get; set; }

        public DateTimeOffset ExpireAt { get; set; }
    }

    public class lookingFor
    {
        public string LookingforName { get; set; }
        public int LookingforValue { get; set; }
    }
    public class queryType
    {
        public string QuerytypeName { get; set; }
        public int QuerytypeValue { get; set; }
    }
    public class state
    {
        public string StateName { get; set; }
        public int StateValue { get; set; }
    }
    public class district
    {
        public string CityName { get; set; }
        public int CityValue { get; set; }
    }
}
