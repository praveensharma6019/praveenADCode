using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public class PortsGMSGrievanceBookingModel
    {
        public string SubmitGcptchares { set; get; }
        public Guid Id
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Nature is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Nature { set; get; }

        [Required(ErrorMessage = "Nature is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Location { set; get; }

        [Required(ErrorMessage = "Location is Required.")]
        [StringLength(255, ErrorMessage = "Maximum 150 characters allowed")]
        public string Subject { set; get; }

        [Required(ErrorMessage = "Subject is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string Company { set; get; }

        [Required(ErrorMessage = "Company is Required.")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string WhoImpacted { set; get; }
        public string Brief { set; get; }
        public string On_Behalf { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Mobile { set; get; }
        public string DOB
        {
            get;
            set;
        }
        public string Gender
        {
            get;
            set;
        }
        public string Department
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }
        public Guid GirevanceId { set; get; }
        public string Icd { set; get; }
        public bool Status { set; get; }

        public string PointMan { set; get; }
        public string BuisnessGroup { set; get; }
        public string SiteHead { set; get; }
        public string UserType { set; get; }
        public string AssignedLevel { set; get; }
        public string AssignedState { set; get; }


        public string SaveAsDraft
        {
            get;
            set;
        }
        public DateTime? Modified_Date
        {
            get;
            set;
        }

        public DateTime? Created_Date
        {
            get;
            set;
        }
       public HttpPostedFileBase File { get; set; }
    }
}