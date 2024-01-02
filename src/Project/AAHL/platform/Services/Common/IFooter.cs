using Project.AAHL.Website.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.AAHL.Website.Services.Common
{
    public interface IFooter
    {
        FooterModel GetFooter(Rendering rendering);
    }
}
