namespace Sitecore.Feature.Media
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct BackgroundType
        {
            public static readonly ID ID = new ID("{55A5BDAD-EB69-40F5-8195-CDA182E48EE4}");

            public struct Fields
            {
                public static readonly ID Class = new ID("{AF6B8E5C-10A2-46BE-8310-407434EC1055}");
            }
        }

        public struct HasMedia
        {
            public static readonly ID ID = new ID("{A44E450E-BA3F-4FAF-9C53-C63241CC34EB}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{63DDA48B-B0CB-45A7-9A1B-B26DDB41009B}");
                public static readonly ID Description = new ID("{302C9F8D-F703-4F76-A4AB-73D222648232}");
                public static readonly ID Thumbnail = new ID("{4FF62B0A-D73B-4436-BEA2-023154F2FFC4}");
                public static readonly ID Link = new ID("{EF85F9C6-11B7-40C2-8173-336D79D70E13}");
                // public static readonly ID SubTitle = new ID("{1F2A968D-040D-44C2-9CF0-949F702A6199}");
                public static readonly ID SubTitle = new ID("{A1F260C7-8EB4-4E87-A654-DC4DFD309F06}");


            }
        }
        public struct SingleText
        {
            public static readonly ID ID = new ID("{3742F700-308E-4A97-B524-4274E2ACCBB1}");

            public struct Fields
            {
                public static readonly ID Text = new ID("{E4059B29-8F8D-4F8B-B265-4F9917EFC4B6}");
                public static readonly ID Image = new ID("{89651F3D-7AF2-4471-A3B8-2385F977A988}");
            }
        }

        public struct HasMediaSelector
        {
            public static readonly ID ID = new ID("{AE4635AF-CFBF-4BF6-9B50-00BE23A910C0}");

            public struct Fields
            {
                public static readonly ID MediaSelector = new ID("{72EA8682-24D2-4BEB-951C-3E2164974105}");
                public static readonly ID IsBanner = new ID("{3F78146D-F214-4625-881D-99CF24F9340E}");
            }
        }



        public struct Services
        {
            public static readonly ID ID = new ID("{3AF60F7A-C1F5-40E8-A702-DFE88FD59BBC}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{B75CD92D-9492-45DC-B721-77A275971B6A}");

                public static readonly ID ShortTitle = new ID("{2505F2EE-819E-4888-8EA5-15DB4F3156E8}");

                public static readonly ID CssClass = new ID("{90D2E48D-8A40-445F-BC56-9D550DD032B1}");

                public static readonly ID Description = new ID("{99A353AE-36A2-4776-92E4-5E6EA8428E40}");
                public static readonly ID Image = new ID("{898EBA1E-01CB-4970-9E8A-BAEB5EB6C549}");
                public static readonly ID IconCss = new ID("{9394CE4F-9E2F-4BD2-AE87-E07B13415F81}");
                public static readonly ID CTAText = new ID("{847D6120-C3C4-4AC6-8DB5-21AA945B04D6}");
                public static readonly ID CTALink = new ID("{2BDD1A5E-329E-478E-8E91-1BF6B500BBBD}");
            }
        }

        public struct MoneySaver
        {
            public static readonly ID ID = new ID("{02B2C45A-D9B2-4E72-A43B-F27117F564C6}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{508CA6E0-FF09-4BD0-B679-2FDEAB673B29}");
                public static readonly ID Description = new ID("{17D090D9-5A7B-4F3E-B6F9-8EBF06F61989}");
                public static readonly ID ListingProducts = new ID("{9CBF7246-3D4D-4A50-B218-6328D16D3163}");
            }
        }

        public struct Products
        {
            public static readonly ID ID = new ID("{03636C6B-6769-43E4-A834-19DF1A8D1164}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{46766116-4BCD-4CD5-B3DE-5B18D8180D3B}");
                public static readonly ID Description = new ID("{8CA6940E-21FB-443F-AF3D-C88AC057C19B}");

                public static readonly ID ShortTitle = new ID("{B33C475D-5397-4E9E-8EA4-6176E4972CEA}");
                public static readonly ID CssClass = new ID("{8DBA5654-92AD-45CE-96C8-257F2AD9F50F}");

            }
        }

        public struct DiscoveryWithAdani
        {
            public static readonly ID ID = new ID("{E685EB74-B362-4385-812D-3526749AF83D}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{BAA87B5D-E5CA-446A-80EA-BFC21341DB14}");
                public static readonly ID Description = new ID("{A6E6CDCE-4C7D-4121-B7E5-8EFF08830DD9}");
                public static readonly ID ListingProducts = new ID("{9F09F16D-9108-4F40-8539-D2FD9BF9D63A}");
            }
        }

        public struct CustomerSays
        {
            public static readonly ID ID = new ID("{31843423-7D50-453E-951C-B5C2D7A8EB22}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{1477201C-5086-4D03-8A5C-FB4FAF010E83}");
                public static readonly ID Description = new ID("{73C700B5-D1E2-4C58-B643-A6BFD8E7638E}");
                public static readonly ID ListingProducts = new ID("{86C8F6E9-3E5A-4CF4-884A-C838C8942D9F}");
            }
        }

        public struct ApplyNewConnection
        {
            public static readonly ID ID = new ID("{125A0F9C-ECF9-427E-A637-9392A9258BAB}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{ADB966AC-A6B3-448A-8665-42C767021803}");
                public static readonly ID Description = new ID("{6A2B35FA-0A75-44CB-B111-C1486A6A0863}");
                public static readonly ID ListingProducts = new ID("{9EF8F7E7-F15C-4561-93A9-ABF66F762DDA}");
            }
        }
        public struct OurProducts
        {
            public static readonly ID ID = new ID("{D9B3022F-1702-42D5-A34B-8E9FCBE102E5}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{28C97743-278D-4020-8132-DE334A8C7CB0}");
                public static readonly ID Description = new ID("{E0FB904C-C5C6-4C11-9B76-4106047DABAC}");
                public static readonly ID ListingProducts = new ID("{3EECA2BB-F288-4BA7-BFD2-FF26627C01B0}");
            }
        }

        public struct Offers
        {
            public static readonly ID ID = new ID("{2930933A-2F75-41F7-A48E-CB07CCC1A811}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{6B3DEEF9-7B5B-4410-A035-4748BE3294B9}");
                public static readonly ID Description = new ID("{D841F758-F231-4405-82DA-5FC7FC26AC82}");
                public static readonly ID ListingProducts = new ID("{F42853FF-3175-4978-B4FD-5A91E8282F8E}");
            }
        }

        public struct Growth
        {
            public static readonly ID ID = new ID("{67ADEFB1-CC26-45CB-9630-8685508510A1}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{5CA2978C-0E24-43B3-AD56-B4AC4609DF95}");
                public static readonly ID Description = new ID("{FFDC8F54-451F-40E1-9908-A6CDB3AA7D27}");
                public static readonly ID ListingProducts = new ID("{30AC1399-CE36-48F0-A933-6971C10D0A33}");
            }
        }

        public struct RewardsAndSaving
        {
            public static readonly ID ID = new ID("{BB253650-B1A4-484C-841D-0928AC7137B2}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{2AE68E5F-4C5C-403C-BA73-BF6B75DD4FB5}");
                public static readonly ID Description = new ID("{36F5629C-EED6-4702-BB8D-476D0FECB861}");
                public static readonly ID ListingProducts = new ID("{31302911-8F66-41F8-B49B-6B2078B483DC}");
            }
        }

        public struct AdaniServices
        {
            public static readonly ID ID = new ID("{DDBDD7CB-E9A5-4E5C-9EED-4FCF76CAC3A2}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{CE1C2F11-3060-414E-B9B6-FE0D8668CCF6}");
                public static readonly ID Description = new ID("{73A991D5-674A-44B2-BBBE-4AD187EDCAA6}");
                public static readonly ID ListingProducts = new ID("{3E88F044-060A-4591-9424-EBCA65117605}");
            }
        }

        public struct OtherInformation
        {
            public static readonly ID ID = new ID("{CAA677AB-956B-46A9-BEBA-700B4528BE5F}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{6724DD65-0F0A-43D4-B39C-22040A0A4822}");
                public static readonly ID Description = new ID("{AB9F4AC1-2E03-4890-AACF-68944924B6FD}");
                public static readonly ID ListingProducts = new ID("{8E0E9266-E00F-41F5-A47B-F6A0ECD101C5}");
            }
        }

        public struct HasMediaImage
        {
            public static readonly ID ID = new ID("{FAE0C913-1600-4EBA-95A9-4D6FD7407E25}");

            public struct Fields
            {
                public static readonly ID Image = new ID("{9F51DEAD-AD6E-41C2-9759-7BE17EB474A4}");
            }
        }

        public struct HasMediaImageinCollectionItem
        {
            public static readonly ID ID = new ID("{F033BF35-888E-4096-A8A7-0F3DA14A8698}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{446FDE9F-EE88-4EFE-A4E4-829A91904B14}");
                public static readonly ID Body = new ID("{C962573F-29B3-40C3-AE48-9D53B789FAC3}");
                public static readonly ID Summary = new ID("{BA89EBBB-B667-4C81-A2AB-087F94A7785C}");
                public static readonly ID Image = new ID("{3EEC7300-DC36-481A-9ED1-64CC32FC84A8}");
                public static readonly ID IsVisible = new ID("{68DC42D4-1129-466B-90B1-3BD8B0DCEF4C}");
                public static readonly ID Date = new ID("{00B5162D-E7EE-4108-A463-F112A2BC5868}");

            }
        }
        public struct HasMediaVideo
        {
            public static readonly ID ID = new ID("{5A1B724B-B396-4C48-A833-655CD19018E1}");

            public struct Fields
            {
                public static readonly ID VideoLink = new ID("{2628705D-9434-4448-978C-C3BF166FA1EB}");
            }
        }

        public struct NewsArticle
        {
            public static readonly ID ID = new ID("{2F75C8AF-35FC-4A88-B585-7595203F442C}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{BD9ECD4A-C0B0-4233-A3CD-D995519AC87B}");
                public const string Title_FieldName = "NewsTitle";

                public static readonly ID Image = new ID("{3437EAAC-6EE8-460B-A33D-DA1F714B5A93}");

                public static readonly ID Date = new ID("{C464D2D7-3382-428A-BCDF-0963C60BA0E3}");

                public static readonly ID Summary = new ID("{9D08271A-1672-44DD-B7EF-0A6EC34FCBA7}");
                public const string Summary_FieldName = "NewsSummary";

                public static readonly ID Body = new ID("{801612C7-5E98-4E3C-80D2-A34D0EEBCBDA}");
                public const string Body_FieldName = "NewsBody";

                public static readonly ID Publication = new ID("{D54D1D05-D24E-456B-9019-D0E78BA10FB4}");
                public const string publication = "Publication";

                public static readonly ID ViewDownloadLink = new ID("{E8419A33-3754-428E-9503-FA67D76BD933}");
                public const string viewDownloadLink = "ViewDownloadLink";
            }
        }

        public struct NewsFolder
        {
            public static readonly ID ID = new ID("{74889B26-061C-4D6A-8CDB-422665FC34EC}");
        }


        public struct TimelineCarousel
        {
            public static readonly ID ID = new ID("{8F9C5A7D-6E60-4E1C-A862-9DE24B9288F3}");
            public struct Fields
            {
                public static readonly ID Year = new ID("{45E3674D-FA78-4515-AA1E-02EEB1919BE4}");
                public static readonly ID Desription = new ID("{B873B3DC-D3E2-4C54-9DBD-64E266107E96}");
                public static readonly ID Image = new ID("{F47E10CE-44B8-4728-BE73-4DFF549DC5AE}");
            }
        }
        public struct HasBannerSelector
        {
            public static readonly ID ID = new ID("{206B7C63-02E0-4B0C-9403-2E0751096A69}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{60AB17E5-F66D-407B-B796-C6D7C8641CD4}");
                public static readonly ID Description = new ID("{68D496BE-F46E-40A1-B06E-A8ED480D420D}");
                public static readonly ID Image = new ID("{3AD864D0-19A0-4A96-BE93-58D93A53924B}");
           
            }

        }

        public struct HasBannerSelectorclubahmd
        {
            public static readonly ID ID = new ID("{2C35125E-CDD6-4A39-800A-097F64ECC248}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{EA00D5C0-56B8-47C6-BAEA-2ED002F8CC85}");
                public static readonly ID Description = new ID("{E16CABF8-A6C8-49E8-8876-9E6F1A5DFBA6}");
                public static readonly ID Image = new ID("{30ECC4F1-7D3F-4E91-A4C7-B3A70726AADC}");

            }

        }

        public struct RichText
        {
            public static readonly ID ID = new ID("{9B94B153-65BE-49BE-9415-1D54A300B52F}");

            public struct Fields
            {
                public static readonly ID Text = new ID("{A62FDB6B-0B49-497B-A95B-8CB88B9BE88E}");
            }
        }

        public struct HasPageContent
        {
            public static readonly ID ID = new ID("{AF74A00B-8CA7-4C9A-A5C1-156A68590EE2}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{C30A013F-3CC8-4961-9837-1C483277084A}");
                public const string Title_FieldName = "Title";
                public static readonly ID Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
                public const string Summary_FieldName = "Summary";
                public static readonly ID Body = new ID("{D74F396D-5C5E-4916-BD0A-BFD58B6B1967}");
                public const string Body_FieldName = "Body";
                public static readonly ID Image = new ID("{9492E0BB-9DF9-46E7-8188-EC795C4ADE44}");
            }
        }


        public struct SubBody
        {
            public static readonly ID Body = new ID("{DA6ED874-F918-47E5-AD6A-B5E93B40B0AF}");
            public const string SubBody_FieldName = "Body";
        }


        public struct Navigable
        {
            public static readonly ID ID = new ID("{A1CBA309-D22B-46D5-80F8-2972C185363F}");

            public struct Fields
            {
                public static readonly ID ShowInNavigation = new ID("{5585A30D-B115-4753-93CE-422C3455DEB2}");
                public static readonly ID NavigationTitle = new ID("{1B483E91-D8C4-4D19-BA03-462074B55936}");
                public static readonly ID ShowChildren = new ID("{68016087-AA00-45D6-922A-678475C50D4A}");
                public static readonly ID Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
                public static readonly ID ShowInNewTab = new ID("{10FF37C5-B368-4FAA-89C6-48432D9EBAA0}");
                public static readonly ID Link = new ID("{719E9180-72AB-4592-BE8B-B24A3F28554C}");
                public static readonly ID IsExternalLink = new ID("{40AC25E7-2AFB-4D57-A831-7855C433E1C8}");
                public static readonly ID IsVisible = new ID("{DA2F4A46-CF34-4E5E-A1D2-E1F2E1A221DD}");
            }
        }

        public struct ProductPage
        {
            public static readonly ID ID = new ID("{AB99E48B-D5AF-4839-90D5-FF94B629F111}");
            public struct Fields
            {
                public static readonly ID ProductName = new ID("{ED6DB8B1-B800-4000-B64A-78C9B497EEEF}");
                public static readonly ID ProductImage = new ID("{94BB9019-34C0-434E-8F7F-99BD05806ADA}");
                public static readonly ID DatasheetLink = new ID("{848AAD81-BFF4-4B74-9981-AEE903FB52B4}");
                public static readonly ID MoreProducts = new ID("{A55460CF-8C0A-407A-A47C-F82A16E7D738}");
                public static readonly ID Certifications = new ID("{7A8717F5-CBC8-42E2-A1A5-91108C2A7D76}");
            }
        }
        public struct ProductType
        {
            public static readonly ID ID = new ID("{D86B0DCB-8004-4F5D-AE35-CA2988523595}");

        }
        public struct KeyPersonMessage
        {
            public static readonly ID ID = new ID("{B0BBB850-5261-42CA-8A74-D351822FE815}");
            public struct Fields
            {
                public static readonly ID Name = new ID("{BF87823B-0B53-4D1E-A9DD-CC8FAE75F369}");
                public static readonly ID Designation = new ID("{5AE49B24-B904-4DF5-8C33-97781D3F8B01}");
                public static readonly ID Thumbnail = new ID("{19E3A0E6-FAC2-4AFA-BBE3-6E4F07B6442E}");
            }
        }
        public struct AboutUSNavigation
        {
            public static readonly ID ID = new ID("{18C90867-EF19-48D5-9AC6-01C578850B84}");
            public struct Fields
            {
                public static readonly ID IsMessage = new ID("{C9B0B0EB-C73F-4E16-B4D0-CDB5EA891E3E}");
            }
        }
        public struct SolarPageTypeNews
        {
            public static readonly ID ID = new ID("{CCBDCDFC-118A-4FC3-A4C7-52350D42385A}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{D11152FB-1AD6-4719-9D6F-D1954E01CC6D}");
                public static readonly ID Summary = new ID("{2ADB29A0-2AC9-4C68-806B-8E21D6A3E51C}");
                public static readonly ID Body = new ID("{AFEC2259-D012-499B-847C-606A5745B833}");
                public static readonly ID Type = new ID("{8665F808-6854-4139-BC99-49FA0D135A3B}");
                public static readonly ID Date = new ID("{D6EB0D1F-4E2D-4308-AD3F-2B558BA07A04}");
                public static readonly ID Author = new ID("{F9F66957-8396-493B-B652-942DBB6B1790}");
                public static readonly ID Image = new ID("{D390B1E8-B0B6-4FE3-9959-F8249F8ADE5B}");
                public static readonly ID isVisible = new ID("{EA50BB1A-1898-44A7-91A9-94CC8C0D6C6E}");
                public static readonly ID LinkUrl = new ID("{70BCC2CC-A232-48B5-B1D7-79DFDE014010}");
            }
        }

        public struct SolarContentTypeLink
        {
            public static readonly ID ID = new ID("{FBFA4ABB-7EDA-4B0E-9D04-2EA17EBC338B}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{70F483B0-0148-4EEC-9B3B-71C7D99BB623}");
                public static readonly ID LinkUrl = new ID("{E60BEE02-A31F-4F76-BE76-D37D6925FC9A}");
            }
        }
        public struct EVCharging
        {
            public static readonly ID ID = new ID("{AB86861A-6030-46C5-B394-E8F99E8B87DB}");
            public struct Fields
            {
                public static readonly ID CTAText = new ID("{28E24EC0-4C8D-40F4-B212-D7C32E884457}");
                public static readonly ID MediaTitle = new ID("{63DDA48B-B0CB-45A7-9A1B-B26DDB41009B}");
                public static readonly ID MediaDescription = new ID("{455A3E98-A627-4B40-8035-E683A0331AC7}");
                public static readonly ID MediaThumbnail = new ID("{4FF62B0A-D73B-4436-BEA2-023154F2FFC4}");
                public static readonly ID MediaLink = new ID("{EF85F9C6-11B7-40C2-8173-336D79D70E13}");
                public static readonly ID SubTitle = new ID("{455A3E98-A627-4B40-8035-E683A0331AC7}");
                public static readonly ID CTALink = new ID("{29906FB0-5E30-4D84-8B54-E078F16D23C8}");
                public static readonly ID MediaImage = new ID("{9F51DEAD-AD6E-41C2-9759-7BE17EB474A4}");
            }
        }

    }
}