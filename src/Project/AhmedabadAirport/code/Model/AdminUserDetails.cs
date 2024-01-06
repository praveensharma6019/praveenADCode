using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Sitecore.AhmedabadAirport.Website.Model
{
    public class AdminUserDetails
    {
        public string Company
        {
            get;
            set;
        }

        [EmailAddress(ErrorMessage = "Please Enter Valid EmailAddress.")]
        [Required(ErrorMessage = "Please Enter Valid EmailAddress.")]
        public string Email
        {
            get;
            set;
        }

        public List<AdminUserName> EnvelopNameCheckboxs
        {
            get;
            set;
        }

        public string EnvelopRight
        {
            get;
            set;
        }

        public string FaxNo
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        [RegularExpression("^((\\+)?(\\d{2}[-])?(\\d{10}){1})?(\\d{11}){0,1}?$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [Required(ErrorMessage = "Please Enter Valid Mobile Number.")]
        public string MobileNo
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Name Field Cannot be Blank.")]
        public string Name
        {
            get;
            set;
        }

        public string SelectTenderId
        {
            get;
            set;
        }

        public List<SelectListItem> TenderList
        {
            get;
            set;
        }

        public string TenderNumber
        {
            get;
            set;
        }

        public string UserId
        {
            get;
            set;
        }

        public string UserType
        {
            get;
            set;
        }

        public AdminUserDetails()
        {
        }
    }
}