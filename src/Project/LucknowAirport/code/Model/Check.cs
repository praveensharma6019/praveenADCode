using System;
using System.Runtime.CompilerServices;
using System.Web;

namespace Sitecore.LucknowAirport.Website.Model
{
    [Serializable]
    public class Check
    {
        public DateTime Adv_Date
        {
            get;
            set;
        }

        public string Business
        {
            get;
            set;
        }

        public DateTime Closing_Date
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string DocumentPath
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public HttpPostedFileBase[] Files
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public bool IsChecked
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public string NITNo
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public Check()
        {
        }
    }
}