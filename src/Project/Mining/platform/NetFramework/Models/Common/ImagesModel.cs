using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Project.Mining.Website.Models.Common
{
   
    public class ImagesModel
    {
        [SitecoreFieldAttribute(FieldId = "{92E4236C-2DC8-45D3-BCAD-2656E0BA77E1}")]
        public virtual Image Image { get; set; }

        [SitecoreFieldAttribute(FieldId = "{5C3B1428-340D-46D5-8A9B-12BC0966BF36}")]
        public virtual Image MobileImage { get; set; }

        [SitecoreFieldAttribute(FieldId = "{263C8542-9167-4F9E-B758-178C90B9862F}")]
        public virtual Image TabletImage { get; set; }

        [SitecoreFieldAttribute(FieldId = "{FE82BAEC-7C9A-4DC6-8329-46042EC1C18F}")]
        public virtual string ImageTitle { get; set; }
    }
}