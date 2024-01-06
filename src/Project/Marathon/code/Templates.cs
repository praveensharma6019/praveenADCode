using Sitecore.Data;
using System;

namespace Sitecore.Project.Marathon
{
    public struct Templates
    {
        public struct HasMedia
        {
            public readonly static ID ID;

            static HasMedia()
            {
                Templates.HasMedia.ID = new ID("{A44E450E-BA3F-4FAF-9C53-C63241CC34EB}");
            }

            public struct Fields
            {
                public readonly static ID Title;

                public readonly static ID Description;

                public readonly static ID Thumbnail;

                public readonly static ID Link;

                public readonly static ID SubTitle;

                static Fields()
                {
                    Templates.HasMedia.Fields.Title = new ID("{63DDA48B-B0CB-45A7-9A1B-B26DDB41009B}");
                    Templates.HasMedia.Fields.Description = new ID("{302C9F8D-F703-4F76-A4AB-73D222648232}");
                    Templates.HasMedia.Fields.Thumbnail = new ID("{4FF62B0A-D73B-4436-BEA2-023154F2FFC4}");
                    Templates.HasMedia.Fields.Link = new ID("{EF85F9C6-11B7-40C2-8173-336D79D70E13}");
                    Templates.HasMedia.Fields.SubTitle = new ID("{1F2A968D-040D-44C2-9CF0-949F702A6199}");
                }
            }
        }

        public struct HasMediaImage
        {
            public readonly static ID ID;

            static HasMediaImage()
            {
                Templates.HasMediaImage.ID = new ID("{FAE0C913-1600-4EBA-95A9-4D6FD7407E25}");
            }

            public struct Fields
            {
                public readonly static ID Image;

                static Fields()
                {
                    Templates.HasMediaImage.Fields.Image = new ID("{9F51DEAD-AD6E-41C2-9759-7BE17EB474A4}");
                }
            }
        }

        public struct HasMediaVideo
        {
            public readonly static ID ID;

            static HasMediaVideo()
            {
                Templates.HasMediaVideo.ID = new ID("{5A1B724B-B396-4C48-A833-655CD19018E1}");
            }

            public struct Fields
            {
                public readonly static ID VideoLink;

                static Fields()
                {
                    Templates.HasMediaVideo.Fields.VideoLink = new ID("{2628705D-9434-4448-978C-C3BF166FA1EB}");
                }
            }
        }

        public struct HasPageContent
        {
            public readonly static ID ID;

            static HasPageContent()
            {
                Templates.HasPageContent.ID = new ID("{AF74A00B-8CA7-4C9A-A5C1-156A68590EE2}");
            }

            public struct Fields
            {
                public readonly static ID Title;

                public const string Title_FieldName = "Title";

                public readonly static ID Summary;

                public const string Summary_FieldName = "Summary";

                public readonly static ID Body;

                public const string Body_FieldName = "Body";

                public readonly static ID Image;

                static Fields()
                {
                    Templates.HasPageContent.Fields.Title = new ID("{C30A013F-3CC8-4961-9837-1C483277084A}");
                    Templates.HasPageContent.Fields.Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
                    Templates.HasPageContent.Fields.Body = new ID("{D74F396D-5C5E-4916-BD0A-BFD58B6B1967}");
                    Templates.HasPageContent.Fields.Image = new ID("{9492E0BB-9DF9-46E7-8188-EC795C4ADE44}");
                }
            }
        }

        public struct LinkMenu
        {
            public readonly static ID ID;

            static LinkMenu()
            {
                Templates.LinkMenu.ID = new ID("{AC7394D5-2AA7-4FDB-A7D8-B1B87F9FA661}");
            }

            public struct Fields
            {
                public readonly static ID Heading;

                public readonly static ID Tabcss;

                public readonly static ID ContainerCSS;

                static Fields()
                {
                    Templates.LinkMenu.Fields.Heading = new ID("{785E82D3-DB24-403E-B5FA-1EE58E3DCCF6}");
                    Templates.LinkMenu.Fields.Tabcss = new ID("{959C772E-2BD5-4A66-B269-15F8492DC125}");
                    Templates.LinkMenu.Fields.ContainerCSS = new ID("{523E9539-71B6-4739-BF8C-F275A13B2100}");
                }
            }
        }

        public struct Navigable
        {
            public readonly static ID ID;

            static Navigable()
            {
                Templates.Navigable.ID = new ID("{A1CBA309-D22B-46D5-80F8-2972C185363F}");
            }

            public struct Fields
            {
                public readonly static ID ShowInNavigation;

                public readonly static ID NavigationTitle;

                public readonly static ID ShowChildren;

                public readonly static ID Summary;

                public readonly static ID ShowInNewTab;

                public readonly static ID Link;

                public readonly static ID IsExternalLink;

                public readonly static ID IsVisible;

                static Fields()
                {
                    Templates.Navigable.Fields.ShowInNavigation = new ID("{5585A30D-B115-4753-93CE-422C3455DEB2}");
                    Templates.Navigable.Fields.NavigationTitle = new ID("{1B483E91-D8C4-4D19-BA03-462074B55936}");
                    Templates.Navigable.Fields.ShowChildren = new ID("{68016087-AA00-45D6-922A-678475C50D4A}");
                    Templates.Navigable.Fields.Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
                    Templates.Navigable.Fields.ShowInNewTab = new ID("{10FF37C5-B368-4FAA-89C6-48432D9EBAA0}");
                    Templates.Navigable.Fields.Link = new ID("{719E9180-72AB-4592-BE8B-B24A3F28554C}");
                    Templates.Navigable.Fields.IsExternalLink = new ID("{40AC25E7-2AFB-4D57-A831-7855C433E1C8}");
                    Templates.Navigable.Fields.IsVisible = new ID("{DA2F4A46-CF34-4E5E-A1D2-E1F2E1A221DD}");
                }
            }
        }

        public struct PaymentConfiguration
        {
            public readonly static ID Id;

            public readonly static ID DonationId;

            static PaymentConfiguration()
            {
                Templates.PaymentConfiguration.Id = new ID("{C5C26D3E-FC5A-4676-81DB-FD4D48266EA4}");
                Templates.PaymentConfiguration.DonationId = new ID("{657A2A60-A43E-41FD-A5F4-38AA36636E60}");
            }

            public struct Insta_Mojo
            {
                public readonly static ID Insta_client_id;

                public readonly static ID Insta_client_secret;

                public readonly static ID Insta_Endpoint;

                public readonly static ID Insta_Auth_Endpoint;

                public readonly static ID grant_type;

                public readonly static ID Insta_Redirect_Url;

                static Insta_Mojo()
                {
                    Templates.PaymentConfiguration.Insta_Mojo.Insta_client_id = new ID("{4B86309B-15A6-417C-A99E-313AAD179B5C}");
                    Templates.PaymentConfiguration.Insta_Mojo.Insta_client_secret = new ID("{87D9609A-08E5-4D9D-A8A7-C55203D182F5}");
                    Templates.PaymentConfiguration.Insta_Mojo.Insta_Endpoint = new ID("{FF8966BC-76B6-4CE9-97F9-97CEEE78A49F}");
                    Templates.PaymentConfiguration.Insta_Mojo.Insta_Auth_Endpoint = new ID("{4022D7D5-F3A2-4B65-9E1E-B488580E5B76}");
                    Templates.PaymentConfiguration.Insta_Mojo.grant_type = new ID("{5F45F186-7924-415F-AC1A-88E63D0AAB84}");
                    Templates.PaymentConfiguration.Insta_Mojo.Insta_Redirect_Url = new ID("{650E9DA2-A45A-40A4-BFA7-795695E0524A}");
                }
            }
        }

        public struct MarathonRun
        {
            public readonly static ID PhysicalRunAgeLimit;

            public readonly static ID RemotRunAgeLimit;



            static MarathonRun()
            {
                Templates.MarathonRun.PhysicalRunAgeLimit = new ID("{6B0DCBFE-D405-424B-9A93-4AD52224109B}");
                Templates.MarathonRun.RemotRunAgeLimit = new ID("{1675BF07-AFA0-4325-A242-D58A01534E54}");

            }

        }


    }

    public struct SMSTemplate
    {
        public static readonly ID OTP = new ID("{EC266F6C-BC9C-4E14-87B8-6D05B3AFA05B}");
        public static readonly ID RegistrationConfirmationSMS = new ID("{8FD0D0C0-BA57-4C25-B967-7A13118FB828}");


        public struct Fields
        {
            public static readonly ID Body = new ID("{52892CEE-7B40-406C-9BC0-3B53C88F9B09}");
        }
    }
    public struct EMailTemplate
    {
        public static readonly ID CharityBib = new ID("{79E74FA7-2924-422E-B3CE-F52FEE5ABACD}");
        public static readonly ID AcknowledgementMail = new ID("{BBEDFF31-FEE2-428C-A722-37BAB7991C3D}");
        public static readonly ID DonationMail = new ID("{4F77A542-3C13-4530-B395-C89B7F9F3BF2}");

        public struct Fields
        {
            public static readonly ID To = new ID("{6046077E-B73F-4AC0-9668-1630B55029F0}");
            public static readonly ID From = new ID("{45482122-C460-41D8-8743-F489437CA64D}");
            public static readonly ID Subject = new ID("{D43B9D9B-C789-48FD-8116-B6439301290C}");
            public static readonly ID Body = new ID("{52892CEE-7B40-406C-9BC0-3B53C88F9B09}");
        }
    }
    public struct AgeValidater
    {
        public static readonly ID AgeGroup = new ID("{3874EB38-DE2A-4CFB-B6BE-407406865DFB}");


        public struct Fields
        {
            public static readonly ID RaceCategory = new ID("{5E4003DC-C623-44D8-9BBD-19C006B5D2D2}");
            public static readonly ID Age = new ID("{370C4775-1D92-44AF-B7E6-CAE7A82F7F46}");
        }
    }

    public struct RaceType
    {
        public static readonly ID Normal = new ID("{3874EB38-DE2A-4CFB-B6BE-407406865DFB}");
        public static readonly ID Charity = new ID("{3874EB38-DE2A-4CFB-B6BE-407406865DFB}");
    }
    public struct RaceTime
    {
        public static readonly ID RaceTimeFolder = new ID("{F2EB4E8D-D08C-4DE3-9266-BA90979BD661}");
        public struct Fields
        {
            public static readonly ID RaceCategory = new ID("{C2C8580C-6041-40B6-AD4D-29A99E616818}");
            public static readonly ID ReportingTime = new ID("{CFF55CE2-1671-4E16-829B-2C85143F6F42}");
            public static readonly ID FlagOffTime = new ID("{F3B286DC-6646-4729-9A45-0031E07FEEC5}");
        }
    }
    public struct RaceDetails
    {
        public static readonly ID Normal = new ID("{0FAABDDF-9C8E-4283-B499-9853674E97D4}");
        public static readonly ID Charity = new ID("{7B220E91-F649-4E9C-8DDA-A72451876EDE}");
    }
}