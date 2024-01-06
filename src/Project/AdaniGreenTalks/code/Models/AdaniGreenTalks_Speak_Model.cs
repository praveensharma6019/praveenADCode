using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website.Models
{
    public class AdaniGreenTalks_Speak_Model
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string NomineeFirstName { get; set; }

        public string NomineeLastName { get; set; }

        public string NomineeEmail { get; set; }

        public string ContactNumber { get; set; }

        public bool checknominatingmyself { get; set; }        

        public string City { get; set; }

        public string Country { get; set; }
        
        public string Takeaway { get; set; }

        public string linkforarticle { get; set; }

        public string linkaudioorvideo { get; set; }
        

        public string Gender { get; set; }
        public string Goal { get; set; }

        public DateTime SubmittedDate { get; set; }

        public string FormType { get; set; }

        public string FormUrl { get; set; }

        public string googleCaptchaToken { get; set; }

        public string fileUploadPhoto { get; set; }
        public string fileUploadbiograph { get; set; }
        public string fileOriginalConcept { get; set; }

        public string fileUploadPhotoName { get; set; }
        public string fileUploadbiographName { get; set; }
        public string fileOriginalConceptName { get; set; }
    }
}