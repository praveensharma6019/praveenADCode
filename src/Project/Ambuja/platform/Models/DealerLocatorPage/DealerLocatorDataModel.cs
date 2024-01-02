using Glass.Mapper.Sc.Configuration.Attributes;
using Project.AmbujaCement.Website.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AmbujaCement.Website.Models.DealerLocatorPage
{
    public class DealerLocatorDataModel
    {
        [SitecoreFieldAttribute(FieldId = "{D837D79F-E76B-4D8F-A336-6F163C76F726}")]
        public virtual int ShowInMobile { get; set; }

        [SitecoreFieldAttribute(FieldId = "{62933FFA-2198-4370-A75D-E763039E6022}")]
        public virtual int ShowInPage { get; set; }

        [SitecoreFieldAttribute(FieldId = "{2BCDAAA2-C7A4-45C2-A7A4-0D01924B4174}")]
        public virtual int ShowInOverlay { get; set; }

        [SitecoreFieldAttribute(FieldId = "{235849BC-E8CD-42C4-AD95-0A4ACEA49229}")]
        public virtual string RedirectUrl { get; set; }

        [SitecoreFieldAttribute(FieldId = "{4A25EC19-0ECF-49F4-AB76-FC966CAE326D}")]
        public virtual DealerLocatorFilterData DealerLocatorFilterData { get; set; }
    }

    public class DealerLocatorFilterData
    {
        [SitecoreFieldAttribute(FieldId = "{1C468078-12C4-41CF-BF89-5728AE352962}")]
        public virtual GtmDetails GtmData { get; set; }

        [SitecoreFieldAttribute(FieldId = "{929AD2D8-B07F-48DE-B50D-285573184470}")]
        public virtual string RedirectUrl { get; set; }

        [SitecoreChildren]
        public virtual IEnumerable<OptionModel> Options { get; set; }

        [SitecoreFieldAttribute(FieldId = "{0DEF0902-65BE-4091-876B-126996D8D8AA}")]
        public virtual Placeholders Placeholders { get; set; }
    }

    public class Placeholders
    {
        [SitecoreFieldAttribute(FieldId = "{6534A819-5887-498A-8CEF-5B64A0FCDCC0}")]
        public virtual string StatePlaceholder { get; set; }

        [SitecoreFieldAttribute(FieldId = "{65A0A145-41C0-40B8-940F-0CE82CC6AF10}")]
        public virtual string CityPlaceholder { get; set; }

        [SitecoreFieldAttribute(FieldId = "{DBAD654B-599F-493E-A19D-918FF7352928}")]
        public virtual string AreaPlaceholder { get; set; }

        [SitecoreFieldAttribute(FieldId = "{E2485725-A9AE-4838-B78A-8636C274C503}")]
        public virtual string FieldName { get; set; }

        [SitecoreFieldAttribute(FieldId = "{AEF7AA36-A693-4A85-9A79-74E90ABF961C}")]
        public virtual string Heading { get; set; }

        [SitecoreFieldAttribute(FieldId = "{EA82288C-5154-4095-A205-0E89A6F2AD55}")]
        public virtual string Description { get; set; }

        [SitecoreFieldAttribute(FieldId = "{010F16A7-4855-44E9-8209-1D55E556C515}")]
        public virtual string ButtonLabel { get; set; }

        [SitecoreFieldAttribute(FieldId = "{8DC1A45D-986D-4E9C-9A6C-10FD3D1FB2F0}")]
        public virtual string SearchLabel { get; set; }
        [SitecoreFieldAttribute(FieldId = "{30080F84-3991-4C11-9018-2114EE6915B0}")]
        public virtual string ErrorMessage { get; set; }
    }
    public class OptionModel : CommonOptionModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<CityOption> CityOptions { get; set; }
    }
    public class CityOption : CommonOptionModel
    {
        [SitecoreChildren]
        public virtual IEnumerable<CommonOptionModel> AreaOptions { get; set; }
    }
}