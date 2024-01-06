using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Adani.Website.Models
{
    public class AdaniMarketCapModel
    {        
        public string MarketCapID { get; set; }
        public string MarketCapValue { get; set; }        
    }
    public class AdaniMarketCapListModel
    {
        public AdaniMarketCapListModel()
        {
            MarketList = new List<AdaniMarketCapModel>();
        }
        public List<AdaniMarketCapModel> MarketList { get; set; }
    }
}