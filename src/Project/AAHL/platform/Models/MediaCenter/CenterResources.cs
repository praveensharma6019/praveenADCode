using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.MediaCenter
{
    public class CenterResources
    {
        [SitecoreChildren]
        public virtual IEnumerable<ResourcesItem> Items { get; set; }
    }
    public class ResourcesItem
    {
        [SitecoreField]
        public virtual string Heading { get; set; }
        [SitecoreField]
        public virtual string Description { get; set; }
        [SitecoreField]
        public virtual bool Isactive { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
        [SitecoreField("allCategories")]
        public virtual IEnumerable<CategoriesChilditems> allCategories { get; set; }
        [SitecoreField("allMedia")]
        public virtual IEnumerable<CategoriesChilditems> allMedia { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<ResourcesChilditems> Childitems { get; set; }
        [SitecoreField]
        public virtual string BtnText { get; set; }
    }
    public class ResourcesChilditems : ImageModel
    {
        [SitecoreField]
        public virtual bool Isvideo { get; set; }
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
        //[SitecoreField("categoriestype")]
        //public virtual IEnumerable<CategoriesChilditems> categoriestype { get; set; }
        //[SitecoreField("mediatype")]
        //public virtual IEnumerable<CategoriesChilditems> mediatype { get; set; }
        [SitecoreField]
        public virtual string categoriestype { get; set; }
        [SitecoreField]
        public virtual string mediatype { get; set; }
        [SitecoreField]
        public virtual Link VideoSource { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<MediaChilditems> subItems { get; set; }
    }
    public class MediaChilditems : ImageModel
    {
        [SitecoreField]
        public virtual bool Isvideo { get; set; }
        [SitecoreField]
        public virtual Link VideoSource { get; set; }
        [SitecoreField]
        public virtual string Class { get; set; }
        [SitecoreField]
        public virtual string Next { get; set; }
        [SitecoreField]
        public virtual string Previous { get; set; }
    }
    public class CategoriesChilditems
    {
        [SitecoreField]
        public virtual Link LinkUrl { get; set; }
        [SitecoreField]
        public virtual string Heading { get; set; }
    }
}