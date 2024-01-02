using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models
{
	public class LoyaltyJourneyModel
	{
        public string title { get; set; }
        public string subTitle { get; set; }
        public string imageSrc { get; set; }
        public string imageAlt { get; set; }

        public List<Rewards> rewardList { get; set; }
    }
}