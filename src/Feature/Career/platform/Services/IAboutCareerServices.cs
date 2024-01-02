using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.Career.Platform.Services
{
    public interface IAboutCareerServices
    {
        AboutCareerData GetAboutCareer(Sitecore.Mvc.Presentation.Rendering rendering);
 
    }
}
