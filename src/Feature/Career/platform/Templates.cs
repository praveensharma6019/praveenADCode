using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Adani.SuperApp.Realty.Feature.Career.Platform
{
    public static class Templates
    {

        #region BaseTemplate
        public static class Image
        {
            public static readonly ID TemplateID = new ID("{2C704E65-FE58-4052-BBB4-5D93EFCDC46B}");
            public static readonly string TemplateName = "IImage";
            public static class FieldsID
            {
                public static readonly ID Image = new ID("{0983BE5E-A609-41A5-9FE0-DDF0D65A36C9}");
            }
            public static class FieldsName
            {
                public static readonly string Image = "Image";
            }
        }

        public static class ITitle
        {
            public static readonly ID TemplateID = new ID("{6FF34C27-81B2-47A3-A2B9-6C0143CD3ADD}");
            public static readonly string TemplateName = "ITitle";
            public static class FieldsID
            {
                public static readonly ID Title = new ID("{F572F6EC-021F-4A86-BED4-4DD15C980E8A}");
            }
            public static class FieldsName
            {
                public static readonly string Title = "Title";
            }
        }
        public static class ISubTitle
        {
            public static readonly ID TemplateID = new ID("{C422F1E5-737B-44FE-9132-ACCEC5ABFDC5}");
            public static readonly string TemplateName = "ISubTitle";
            public static class FieldsID
            {
                public static readonly ID SubTitle = new ID("{C095C5FC-6D9E-47BC-B37B-1387C4800265}");
            }
            public static class FieldsName
            {
                public static readonly string SubTitle = "SubTitle";
            }
        }
        public static class IDescription
        {
            public static readonly ID TemplateID = new ID("{1D45045B-1CED-429F-BF4A-1EEDBA67A88C}");
            public static readonly string TemplateName = "IDescription";
            public static class FieldsID
            {
                public static readonly ID Description = new ID("{1F902CB6-743F-44F7-BDF4-160E2477EC73}");
            }
            public static class FieldsName
            {
                public static readonly string Description = "Description";
            }
        }

        public static class IBody
        {
            public static readonly ID TemplateID = new ID("{E5CAE71E-064D-4053-90CB-8720159A2DE5}");
            public static readonly string TemplateName = "IBody";
            public static class FieldsID
            {
                public static readonly ID Body = new ID("{5D8C0707-6E04-4051-9B8D-76E8A2D6483A}");
            }
            public static class FieldsName
            {
                public static readonly string Body = "Body";
            }
        }

        public static class ISummary
        {
            public static readonly ID TemplateID = new ID("{9D520C02-7A11-480D-8518-F5EDD7DCD343}");
            public static readonly string TemplateName = "ISummary";
            public static class FieldsID
            {
                public static readonly ID Summary = new ID("{6BB388D8-DEDF-432F-A0C0-C20AD0CC8A0D}");
            }
            public static class FieldsName
            {
                public static readonly string Summary = "Summary";
            }
        }
        public static class ILink
        {
            public static readonly ID TemplateID = new ID("{C5B2D402-6B0C-4A1F-A80D-E70D29E670D9}");
            public static readonly string TemplateName = "ILink";
            public static class FieldsID
            {
                public static readonly ID Link = new ID("{8CEDC322-2AA6-4EC6-8800-DF54977EC81B}");
            }
            public static class FieldsName
            {
                public static readonly string Link = "Link";
            }
        }

        #endregion

        #region Career
        /// <summary>
        /// Opening Page : (_HasPageContent - {D1B7CD65-6321-46D4-8210-0031CA094037} &
        ///                 Jobs Opening - {C79E9C24-34E4-4791-A7C3-15753B603F51})
        /// </summary>
        public static class Careers
        {
            public static readonly ID TemplateID = new ID("{C79E9C24-34E4-4791-A7C3-15753B603F51}");
            public static class Fields
            {
                public static readonly ID linkID = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
                public static readonly string linkFieldName = "link";
                public static readonly ID RoleID = new ID("{67B9D44E-39F2-49EB-BF1A-5D11F1285510}");
                public static readonly string RoleFieldName = "Role";
                public static readonly ID DepartmentID = new ID("{24AE375C-EBB4-45DB-826D-D1CB6D90938D}");
                public static readonly string DepartmentFieldName = "Department";
                public static readonly ID LocationtextID = new ID("{85692B7B-14F2-411A-90E9-683B0EA7D15B}");
                public static readonly string LocationFieldName = "Location";
                public static readonly ID PostingdateID = new ID("{35A4C3C3-00D3-4D22-86BE-2245CE41BDAB}");
                public static readonly string PostingdateFieldName = "Postingdate";

            }
        }
        /// <summary>
        /// JobAnchors : ((_HasPageContent - {D1B7CD65-6321-46D4-8210-0031CA094037})
        /// </summary>
        public static class JobsAnchors
        {
            public static readonly ID TemplateID = new ID("{D1B7CD65-6321-46D4-8210-0031CA094037}");
            public static class Fields
            {
                public static readonly ID TitleID = new ID("{0AC3BD57-994A-4A0B-80E1-673890B34336}");
                public static readonly string TitleFieldName = "Title";
                public static readonly ID linkID = new ID("{A3519257-0EB9-466F-8028-C047FB293533}");
                public static readonly string linkFieldName = "link";

            }
        }
        public static class EmployeeCare
        {
            public static readonly ID TemplateID = new ID("{4A421518-7B4A-4D43-AF9B-94E17E00EC9F}");
            public static class Fields
            {
                public static readonly ID headingID = new ID("{5179186C-B95E-4E97-95AB-7958721A9AEB}");
                public static readonly string headingFieldName = "heading";
                public static readonly ID subheadingID = new ID("{89B0A8ED-0EE8-4512-B518-AB2C4C2A0B9E}");
                public static readonly string subheadingFieldName = "subheading";
                public static readonly ID TitleID = new ID("{36DFD09C-9819-48CD-A9BA-DA74CBD686AF}");
                public static readonly string TitleName = "Title";

            }
        }

        #endregion

        #region AboutCareer

        public static class AboutCareer
        {
            public static readonly ID TemplateID = new ID("{D891CC1E-4A6A-4515-8CD2-C1BF51FAB356}");
            public static readonly string TemplateName = "AboutCareer";

        }

        public static class AboutCareerFolder
        {
            public static readonly ID TemplateID = new ID("{F0B73B38-E5B0-438D-8274-5C88D8AD2B87}");
            public static readonly string TemplateName = "AboutCareerFolder";

        }

        #endregion

    }
}
