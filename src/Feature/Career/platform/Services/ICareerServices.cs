using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.Career.Platform.Services
{
    public interface ICareerServices
    {
        JobOpeningsList GetJobOpeningsList(Sitecore.Mvc.Presentation.Rendering rendering);
        JobsAnchorsList GetJobsAnchorsList(Sitecore.Mvc.Presentation.Rendering rendering);
        EmployeeCareList GetemployeeCareList(Sitecore.Mvc.Presentation.Rendering rendering);


    }
}
