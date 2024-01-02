using Project.AmbujaCement.Website.Models;
using Project.AmbujaCement.Website.Models.Common;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services
{
    public interface IFooterServices
    {
        Footer GetFooter(Rendering rendering);
    }
}
