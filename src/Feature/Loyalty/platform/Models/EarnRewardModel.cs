using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models
{
    public class EarnRewardModel
    {
        public string Title { get; set; }
        public List<Object> rewardStepList { get; set; }
    }

    public class RewardStep
    {
        public string rewardsSteps { get; set; }
    }
}