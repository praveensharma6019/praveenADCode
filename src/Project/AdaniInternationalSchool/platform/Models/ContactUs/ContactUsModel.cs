using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class ContactUsModel
    {
        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Mobile Number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter position you want to apply for")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Please enter your existing Organization")]
        public string Organization { get; set; }

        [Required(ErrorMessage = "Please enter your total Experience")]
        public string Experience { get; set; }

        [Required(ErrorMessage = "Please upload resume")]
        public HttpPostedFileBase Resume { get; set; }

        [Required(ErrorMessage = "Please accept the terms.")]
        public bool Agreement { get; set; }
    }

    public class PositionInterested
    {
        public string PositionInterestedName { get; set; }
        public int PositionInterestedValue { get; set; }
    }
    public class TotalExperience
    {
        public string TotalExperienceName { get; set; }
        public int TotalExperienceValue { get; set; }
    }
}