using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.LucknowAirport.Website.Model
{
    public class LKOAirportContactUs
    {

        [Required(ErrorMessage = "Please enter valid Name"), MaxLength(100)]
        [RegularExpression(@"^[A-Za-z\s]*$", ErrorMessage = "Fullname is not valid.")]
        public string Fullname
        {
            get;
            set;
        }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter email ID")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email
        {
            get;
            set;
        }
        public int ContactType
        {
            get;
            set;
        }
        [Required(ErrorMessage = " Please enter contact number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact Number is not valid.")]
        public string ContactNo
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Please enter Message"), MaxLength(500)]
        [RegularExpression(@"^[A-Za-z0-9;,.?_\-!@&:""'\/\\ ]{0,500}$", ErrorMessage = "Message is not valid, special characters not allowed")]
        public string Message
        {
            get;
            set;
        }
        public string reResponse
        {
            get;
            set;
        }
        public DateTime FormSubmitDate
        {
            get;
            set;
        }
        public string IPAddress { get; set; }
    }
}