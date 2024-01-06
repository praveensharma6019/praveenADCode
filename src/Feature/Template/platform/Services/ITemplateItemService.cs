using Sitecore.Feature.Template.Models;
using System.Collections.Generic;

namespace Sitecore.Feature.Template.Services
{
    public interface ITemplateItemService
    {
        bool CreateItem(TemplateData templateData);
        bool UpdateItem(TemplateData templateData);
    }
}