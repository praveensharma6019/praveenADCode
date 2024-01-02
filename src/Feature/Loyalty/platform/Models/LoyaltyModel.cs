using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models
{
	public class LoyaltyModel
	{
        public string title { get; set; }
        public string imageSrc { get; set; }
        public string imageAlt { get; set; }
        public string ctaLink { get; set; }
        public string ctaText { get; set; }
        public List<Rewards> rewardList { get; set; }

        public string loyaltyWelcomeMessage { get; set; }
        public string appImageSrc { get; set; }
        public string appImageAlt { get; set; }
    }
}