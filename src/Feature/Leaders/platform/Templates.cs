using Sitecore.Data;

namespace Adani.SuperApp.Realty.Feature.Leaders.Platform
{
    public static class Templates
    {

        #region BaseTemplate
        public static class Image
        {
            public static readonly ID TemplateID = new ID("{2C704E65-FE58-4052-BBB4-5D93EFCDC46B}");
            public static readonly ID ThumbImageID = new ID("{3ECF3CDF-0E2B-41A5-B715-0508A3DC5899}");
            public static readonly string ThumbImageName = "thumb";
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

        #endregion

        #region LeadersData

        public static class LeadersData
        {

            public static readonly ID TemplateID = new ID("{9968A4E8-89DF-45A0-B3B2-EA3FCB1406DB}");
            public static readonly string TemplateName = "LeadersData";
            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID Name = new ID("{BC40052C-4198-4352-8BAA-1088A754E7CD}");
                    public static readonly ID Quote = new ID("{6324B285-4EF5-45C9-8F9F-194B22D1D2B2}");
                    public static readonly ID Designation = new ID("{69EBF323-9B3B-4D90-8E01-C1417C034519}");
                }
                public static class FieldsName
                {
                    public static readonly string Name        = "Name";
                    public static readonly string Quote       = "Quote";
                    public static readonly string Designation = "Designation";
                }
            }
        }

        public static class LeadersDataFolder
        {
            public static class FieldsID
            {
                public static readonly ID Title = new ID("{3D1BE184-7C18-4C29-84C1-A88F662AC32C}");               
            }
            public static class FieldsName
            {
                public static readonly string Title = "Title";
                
            }
            public static readonly ID TemplateID = new ID("{0E303CD6-6694-4D0C-9B44-41E4BC2F1F68}");
            public static readonly string TemplateName = "LeadersDataFolder";

        }

        #endregion

        #region Achievement
        public static class Achievement
        {
            public static readonly ID TemplateID = new ID("{3C4C50F2-6676-47B6-9C0D-83BDBA84EA97}");
            public static readonly string TemplateName = "Achievement";

            public static class Fields
            {
                public static class FieldsID
                {
                    public static readonly ID Start = new ID("{58AA0A28-CA3A-4B0B-B9F6-2A17643CA355}");
                    public static readonly ID Delay = new ID("{164A588A-DDA0-4330-B837-6A9CFF3FB875}");
                }
                public static class FieldsName
                {
                    public static readonly string Start = "Start";
                    public static readonly string Delay = "Delay";                 
                }
            }
        }
        public static class AchievementsFolder
        {
            public static readonly ID TemplateID = new ID("{3CEBB0C2-F2F1-45B0-9D94-F22B1459D106}");
            public static readonly string TemplateName = "AchievementsFolder";
        }

        #endregion
    }
}