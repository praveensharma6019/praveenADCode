using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Realty.Website.Model
{
    //public class PropertyInfo
    //{

    //    public PropertyInfo()
    //    {
    //        _propertyDetailList = new List<PropertyDetail>();
    //    }
    //    public string PropertyType { get; set; }
    //    public List<PropertyDetail> _propertyDetailList { get; set; }
    //}

    ////public class PropertyLocation
    ////{
    ////    public PropertyLocation()
    ////    {
    ////        _propertyDetailList = new List<PropertyDetail>();
    ////    }

    ////    public List<PropertyDetail> _propertyDetailList { get; set; }
    ////}
    //public class PropertyDetail
    //{
    //    public string city { get; set; }
    //    public string PropertyName { get; set; }
    //}

    public class PropertyInfo
    {
        #region Property

        public PropertyInfo()
        {
            this.citylist = new List<City>();
        }
        public string PropertyType { get; set; }
        public List<City> citylist { get; set; }
        #endregion
    }

    public class City
    {
        public City()
        {
            this.propertyitemslist = new List<PropertyDetail>();
        }
        public string Nameofcity { get; set; }
        public List<PropertyDetail> propertyitemslist { get; set; }
    }

    public class PropertyDetail
    {
        public string Id { get; set; }
        public string cityname { get; set; }
        public string PropertyName { get; set; }

    }

    public class PropertryData
    {
        public string Title { get; set; }
        public string Overview { get; set; }
        public List<propertyText> Features { get; set; }

        public List<ameinitesdata> Amenities { get; set; }
        public string Brochure { get; set; }
        public List<propertyText> ReraCertification { get; set; }
        public string GoogleMap { get; set; }
        public List<propertyText> Images { get; set; }
    }

    public class propertyText
    {
        public string text { get; set; }
    }

    public class ameinitesdata
    {
        public string text { get; set; }
        public string imageURL { get; set; }
    }
}