using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Links.UrlBuilders;
using Sitecore.Resources.Media;

namespace Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.CustomResolvers
{
    public class CustomLinkProvider : ItemUrlBuilder
    {
		public CustomLinkProvider(DefaultItemUrlBuilderOptions defaultOptions) : base(defaultOptions)
		{
		}

		public override string Build(Item item, ItemUrlBuilderOptions options)
		{
			options.SiteResolving = Settings.Rendering.SiteResolving;			
			// For items using any other template, use default url builder logic.
			return base.Build(item, options).ToLower();
		}
	}

}