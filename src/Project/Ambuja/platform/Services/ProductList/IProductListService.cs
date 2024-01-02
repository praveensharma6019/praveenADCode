using Project.AmbujaCement.Website.Models.AboutUsPage;
using Project.AmbujaCement.Website.Models.ProductList;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.Services.AboutUs
{
    public interface IProductListService
    {
        ProductListModel GetProductListModel(Rendering rendering);
    }
}
