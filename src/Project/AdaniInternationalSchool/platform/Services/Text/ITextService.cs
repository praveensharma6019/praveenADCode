using Project.AdaniInternationalSchool.Website.Models;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Services.Text
{
    public interface ITextService
    {
        Dictionary<string, string> Render(Rendering rendering);
    }
}