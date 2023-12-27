using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Adani.SuperApp.Airport.Feature.Forex.Platform.Models.ForexImportantInfoModel;

namespace Adani.SuperApp.Airport.Feature.Forex.Platform.Services
{
    public interface IImportantInformation
    {
        ForexImportantInfoList GetForexImportantInfo(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}
