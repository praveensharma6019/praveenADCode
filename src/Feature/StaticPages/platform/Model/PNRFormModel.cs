using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model
{
    public class PNRFormModel
    {
        public PNRFormLabels Labels { get; set; }

        public PNRFormInput Input { get; set; }
    }

    public class PNRFormLabels
    {
        public string FormTitle { get; set; }

        public string FirstNameLabel { get; set; }

        public string LastNameLabel { get; set; }

        public string PNR { get; set; }

        public string ContactNumberLabel { get; set; }

        public string ButtonText { get; set; }

        public string Description { get; set; }

        public string FooterDescription { get; set; }

        public string EmailLabel { get; set; }
    }
}