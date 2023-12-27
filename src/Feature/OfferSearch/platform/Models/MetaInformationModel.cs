
using System.ServiceModel.Description;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models
{
    public class MetaInformation
    {
        public string tabTitle { get; set; }
        public MetaData data { get; set; }

        public MetaInformation() 
        {
            data= new MetaData();
        }
    }

    public class MetaData
    {
        public string metaTitle { get; set; }
        public string metaDescription { get; set; }
        public string keywords { get; set; }
        public string canonical { get; set; }
        public string viewport { get; set; }
        public string robots { get; set; }
        public string oG_Title { get; set; }
        public string oG_Image { get; set; }
        public string oG_Description { get; set; }
        public string richTextTitle { get; set; }
        public string richTextBody { get; set; }
    }
}