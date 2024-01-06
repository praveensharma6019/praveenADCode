using System;
using System.Runtime.CompilerServices;

namespace Sitecore.Adani.Website.Models
{
    public class AdaniContactModal
    {
        public string Email
        {
            get;
            set;
        }

        public DateTime FormSubmitOn
        {
            get;
            set;
        }

        public string FormType
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string Mobile
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string OTP
        {
            get;
            set;
        }

        public string PageInfo
        {
            get;
            set;
        }

        public string SubjectType
        {
            get;
            set;
        }
        public string reResponse { get; set; }
        public AdaniContactModal()
        {
        }
    }
}