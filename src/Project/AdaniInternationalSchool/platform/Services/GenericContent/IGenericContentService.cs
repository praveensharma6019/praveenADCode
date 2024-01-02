using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Services
{
    public interface IGenericContentService
    {
        object Render(Rendering rendering);
    }
}