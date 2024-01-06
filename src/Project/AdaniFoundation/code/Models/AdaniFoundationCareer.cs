using System;
using System.Runtime.CompilerServices;
using System.Web;

namespace Sitecore.AdaniFoundation.Website.Models
{
    public class AdaniFoundationCareer
    {
        public string ContactNo
        {
            get;
            set;
        }

        public string Current_Organization
        {
            get;
            set;
        }

        public string CurrentCTC
        {
            get;
            set;
        }

        public string CurrentDesignation
        {
            get;
            set;
        }

        public string Education
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string Experience
        {
            get;
            set;
        }

        public string FormType
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string PageInfo
        {
            get;
            set;
        }

        public string reResponse
        {
            get;
            set;
        }

        public HttpPostedFileBase ResumeAttachment
        {
            get;
            set;
        }

        public DateTime SubmitOn
        {
            get;
            set;
        }

        public AdaniFoundationCareer()
        {
        }
    }
}