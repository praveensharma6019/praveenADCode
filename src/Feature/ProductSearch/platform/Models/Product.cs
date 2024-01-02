using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models
{
    public class Product
    {
        public string material_group_name { get; set; }
        public string category_name { get; set; }
        public string brand_name { get; set; }
        public string sub_category_name { get; set; }
        public string flemingo_sku_code { get; set; }
        public string sku_name { get; set; }
        public int saleable_quantity { get; set; }

        public int length { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int volume { get; set; }
        public int unit_weight { get; set; }

       // public int unit_weight { get; set; }


        /*
        
          
            
            "inventory_type_id": 1,
            "supplier_name": "N/A",
            "threshold_limlit": 10,
            "age_of_product_for_liquor": 30,
            "gender": "1",
            "status": true,
            "image1": "02N02001.png",
            "image2": null,
            "meta_title": null,
            "meta_description": null,
            "meta_keyword": null,
            "meta_tag": null,
            "price": 800,
            "stock_in_hand": 50,
            "threshold_quantity": 3,
            "promotions": [],
            "effect_from": "2022-02-18T12:00:38.446",
            "effect_to": "1900-01-01T12:25:44.1833333",
            "store_code": "MDFS02",
            "airport_code": "BOM"
        */
    }
}