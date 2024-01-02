using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models
{
    public class CategoriesListModel
    {
        public List<CategoriesModel> CategoriesList { get; set; }
    }

    public class CategoriesModel : ImageModel
    {
        public string ProductName { get; set; }
        public string ImageTitle { get; set; }
        public string ProductCount { get; set; }
        public string Link { get; set; }
        public string LinkTarget { get; set; }
        public bool IsActive { get; set; }
        public GtmDataModel GtmData { get; set; }
    }
}