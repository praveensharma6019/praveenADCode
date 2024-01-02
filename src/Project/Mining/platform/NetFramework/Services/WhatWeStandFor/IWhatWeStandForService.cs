using Project.Mining.Website.Models;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Services.Banner
{
    public interface IWhatWeStandForService
    {
        WhatWeStandForModel GetWhatWeStandFor(Rendering rendering);
    }
}
