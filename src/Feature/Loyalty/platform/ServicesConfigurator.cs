using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform
{
	public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Services.ILoyalty, Services.Loyalty>();
            serviceCollection.AddTransient<Services.ILoyaltyRewardJourney, Services.LoyaltyRewardJourney>();
            serviceCollection.AddTransient<Services.IEarnReward, Services.EarnRewards>();
            serviceCollection.AddTransient<Services.IEarn2XProduct, Services.Earn2XLoyaltyProduct>();
            serviceCollection.AddTransient<Services.ISocialMediaLoyalty, Services.SocialMediaLoyalty>();
            serviceCollection.AddTransient<Services.ITerminal, Services.Terminal>();
            serviceCollection.AddScoped<Services.IRewardsList, Services.RewardsList>();
        }
    }
}