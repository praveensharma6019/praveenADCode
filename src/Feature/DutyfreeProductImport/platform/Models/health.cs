using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.DutyFreeProductImport.Platform.Models
{
    public class health
    {
        public List<Prod> Mainsheet { get; set; }

    }
    public class Prod
    {
        public string SR_NO { get; set; }
        public string Material_Group { get; set; }
        public string Category { get; set; }
        public string Sub_Category { get; set; }
        public string Brand { get; set; }
        public string Sku_Code { get; set; }
        public string Sku_Name { get; set; }
        public string Sku_Description { get; set; }
        public string Product_Highlights { get; set; }
        public string Key_Ingredients { get; set; }
        public string Benefits { get; set; }
        public string How_To_Use { get; set; }

        public string Safety_Final { get; set; }

        public string Flavour { get; set; }

        public string Form { get; set; }
        public string Manufacturer { get; set; }

        public string About_Brand { get; set; }
        public string Sku_Size { get; set; }
        public string Size_Unit { get; set; }
        public string Width { get; set; }
        public string Width_Unit { get; set; }
        public string Height { get; set; }
        public string Height_Unit { get; set; }
        public string Weight { get; set; }
        public string Unit_Weight { get; set; }
        public string Frame_Colour_Front { get; set; }
        public string Frame_Colour_Temple { get; set; }
        public string Lens_Colour { get; set; }
        public string Material { get; set; }
        public string Gender { get; set; }
        public string Material_Fitting_Name { get; set; }
        public string Bucket { get; set; }
        public int Recommended { get; set; }
        public string sold { get; set; }
      

    }

    public class SEOName
    {
        public string SKUcode { get; set; }
        public string SKUname { get; set; }
        public string Slug { get; set; }

    }
}