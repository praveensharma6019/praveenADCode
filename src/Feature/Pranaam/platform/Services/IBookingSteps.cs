using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public interface IBookingSteps
    {
        ServiceStepslwidgets GetPranaamSteps(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}