using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.DependencyInjection;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(CategorySearchController),
                typeof(CategorySearchController)));
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(MaterialGroupController),
                typeof(MaterialGroupController)));
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(ProductSearchController),
                typeof(ProductSearchController)));
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(ProductFilterSearchController),
                typeof(ProductFilterSearchController)));
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(ConstraintController),
                typeof(ConstraintController)));
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(ProductByCategoryController),
               typeof(ProductByCategoryController)));
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(FilterSearchController),
               typeof(FilterSearchController)));
            serviceCollection.Replace(ServiceDescriptor.Scoped(typeof(ProductSitemapController), 
               typeof(ProductSitemapController)));
            serviceCollection.AddScoped<Services.IFilterProductsService, Services.FilterProductsService>();
            serviceCollection.AddScoped<Services.IBrandListing, Services.BrandListing>();
            serviceCollection.AddScoped<Services.IDutyfreeExclusive, Services.DutyfreeExclusive>();
            serviceCollection.AddScoped<Services.ISearchItems, Services.SearchItems>();
        }
    }
}