using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class NavigationModel:LinkItemModel
    {
        public NavigationModel()
        {
            SubMenu = new List<NavMenuItemModel>();
        }

        public bool IsActive { get; set; }
        public bool IsHighLighted { get; set; }
        public string HighlightLabel { get; set; }
        public List<NavMenuItemModel> SubMenu { get; set; }
    }


}