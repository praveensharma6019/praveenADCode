using Project.AAHL.Website.Models.OurBelief;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.AAHL.Website.Services.OurBelief
{
    public interface IOurBeliefService
    {
        OurPurposeModel GetOurPurpose(Rendering rendering);
        OurValuesModel GetOurValues(Rendering rendering);
        OurMissionModel GetOurMission(Rendering rendering);
        ServiceExcellenceModel GetServiceExcellence(Rendering rendering);
    }
}
