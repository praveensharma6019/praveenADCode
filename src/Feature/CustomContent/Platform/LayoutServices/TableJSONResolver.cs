using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.LayoutServices
{
    public class TableJSONResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ITableJSON table;

        public TableJSONResolver(ITableJSON _table)
        {
            table = _table;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {                    
            return table.GetTableDataList(rendering);
        }
    }
}