﻿using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public interface IHowItWorkSteps
    {
        HowItWorkStepsWidgets GetStepDetails(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}