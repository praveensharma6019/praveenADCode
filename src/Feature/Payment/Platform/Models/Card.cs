using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.Payment.Platform.Models
{
    public class Card
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string SmallIcon { get; set; }

        public string LargeIcon { get; set; }

        public string RegexForCardNumber { get; set; }

        public string CVVLength { get; set; }

        public bool IsNameMandatory { get; set; }

        public bool IsShowInPage { get; set; }
        public string PackageName { get; set; }
        public string TnC { get; set; }
        public string tnCDC { get; set; }
    }
}