using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.Marathon.Website.Models
{
    [Serializable]
    public class MarathonLoginModel
    {
        [RegularExpression("^[6-9][0-9]{9}$", ErrorMessage = "Please enter a valid contact number")]
        public string Contact
        {
            get;
            set;
        }

        public string Dob
        {
            get;
            set;
        }

        public string Email_id
        {
            get;
            set;
        }

        public string ErrMsg
        {
            get;
            set;
        }

        public string OTP
        {
            get;
            set;
        }
        public string reResponse { get; set; }

        public MarathonLoginModel()
        {
        }
    }
}