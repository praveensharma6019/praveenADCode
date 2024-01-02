using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class NavMenuItemModel : LinkItemModel
    {
        public NavMenuItemModel()
        {
            SubMenu = new List<NavMenuItemModel>();
        }

        public List<NavMenuItemModel> SubMenu { get; set; }
    }


}