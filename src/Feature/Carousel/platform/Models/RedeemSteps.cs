using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class RedeemSteps
    {
        public string ComponentTitle { get; set; }
        public List<howtoredeemStep> howtoredeemSteps { get; set; }
    }

    public class howtoredeemStep
    {
        public string redeemTitle { get; set; }
        public string redeemDescription { get; set; }

        public string redeemDescriptionApp { get; set; }

        public string redeemDesktopImage { get; set; }

        public string redeemMobileImage { get; set; }

        public string redeemlinkText { get; set; }
        public string redeemlinkUrl { get; set; }
    }
}