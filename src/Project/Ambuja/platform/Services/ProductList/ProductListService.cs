using Glass.Mapper.Sc.Web.Mvc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models.AboutUsPage;
using Project.AmbujaCement.Website.Models.ProductList;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AmbujaCement.Website.Services.AboutUs
{
    public class ProductListService : IProductListService
    {
        private readonly IMvcContext _mvcContext;
        public ProductListService(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public ProductListModel GetProductListModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getProductListModel = _mvcContext.GetDataSourceItem<ProductListModel>();
                return getProductListModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

       
    }
}