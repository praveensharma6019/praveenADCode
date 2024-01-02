using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.Models
{
    public class ContactDetailsItem
    {
        public ContactDetail phone { get; set; }

        public ContactDetail email { get; set; }

    }

    public class ContactDetail
    {
        public string name { get; set; }
        public string title { get; set; }
        public string richText { get; set; }
    }
}