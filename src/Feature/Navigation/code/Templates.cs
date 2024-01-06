namespace Sitecore.Feature.Navigation
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct NavigationRoot
        {
            public static readonly ID ID = new ID("{F9F4FC05-98D0-4C62-860F-F08AE7F0EE25}");
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
                public static readonly ID Body = new ID("{D74F396D-5C5E-4916-BD0A-BFD58B6B1967}");
                public static readonly ID ShowInNewTab = new ID("{10FF37C5-B368-4FAA-89C6-48432D9EBAA0}");
                public static readonly ID Link = new ID("{719E9180-72AB-4592-BE8B-B24A3F28554C}");
                public static readonly ID IsExternalLink = new ID("{40AC25E7-2AFB-4D57-A831-7855C433E1C8}");
                public static readonly ID IsVisible = new ID("{DA2F4A46-CF34-4E5E-A1D2-E1F2E1A221DD}");
            }
        }

        public struct Link
        {
            public static readonly ID ID = new ID("{A16B74E9-01B8-439C-B44E-42B3FB2EE14B}");

            public struct Fields
            {
                public static readonly ID Link = new ID("{FE71C30E-F07D-4052-8594-C3028CD76E1F}");
            }
        }

        public struct LinkMenuItem
        {
            public static readonly ID ID = new ID("{18BAF6B0-E0D6-4CCE-9184-A4849343E7E4}");

            public struct Fields
            {
                public static readonly ID Icon = new ID("{2C24649E-4460-4114-B026-886CFBE1A96D}");
                public static readonly ID DividerBefore = new ID("{4231CD60-47C1-42AD-B838-0A6F8F1C4CFB}");
                public static readonly ID Image = new ID("{ECC76733-D0D9-48D6-B55E-293FE5B13EA6}");
                public static readonly ID ShortDesc = new ID("{92A73427-1B36-4BCF-8F6F-9F72C350C65D}");
                public static readonly ID BGCSS = new ID("{4CB29455-78C6-4998-8A83-73C19AE52474}");
                public static readonly ID LinkToSection = new ID("{2DE66FB6-840D-4708-9D00-CEAF062955FA}");
                public static readonly ID ActiveImage = new ID("{C0947EAD-9FAC-4F79-940F-5E5B33B9930A}");
                public static readonly ID TagLine = new ID("{5D8603E0-F2C0-4958-A0E0-B4378564FD1B}");
                public static readonly ID VideoThumb = new ID("{6E5E40E5-EE0D-449F-85DF-9D077CDBF44C}");
                public static readonly ID Title = new ID("{1B483E91-D8C4-4D19-BA03-462074B55936}");
            }
        }

        public struct SummeryContent
        {
            public struct Fields
            {
                public static readonly ID Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
            }
        }

        public struct LinkMenu
        {
            public static readonly ID ID = new ID("{AC7394D5-2AA7-4FDB-A7D8-B1B87F9FA661}");

            public struct Fields
            {
                public static readonly ID Heading = new ID("{785E82D3-DB24-403E-B5FA-1EE58E3DCCF6}");
                public static readonly ID Tabcss = new ID("{959C772E-2BD5-4A66-B269-15F8492DC125}");
                public static readonly ID ContainerCSS = new ID("{523E9539-71B6-4739-BF8C-F275A13B2100}");
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

        public struct MyAccount
        {
            public static readonly ID ID = new ID("{A470C361-1147-4A81-BC7B-7ED8C895303F}");
        }

        public struct BillingAndPaymentAdaniGas
        {
            public static readonly ID ID = new ID("{40BF49E9-8F5B-48E5-B022-820E97CD9661}");
        }

        public struct AdminTender
        {
            public static readonly ID ID = new ID("{E9C023A6-DA97-44AD-98EC-8AC04354237E}");
        }


        /* Media */
        public struct HasMediaSelector
        {
            public static readonly ID ID = new ID("{AE4635AF-CFBF-4BF6-9B50-00BE23A910C0}");

            public struct Fields
            {
                public static readonly ID MediaSelector = new ID("{72EA8682-24D2-4BEB-951C-3E2164974105}");
                public static readonly ID IsBanner = new ID("{3F78146D-F214-4625-881D-99CF24F9340E}");
            }
        }

        public struct HasMediaImage
        {
            public static readonly ID ID = new ID("{FAE0C913-1600-4EBA-95A9-4D6FD7407E25}");

            public struct Fields
            {
                public static readonly ID Image = new ID("{9F51DEAD-AD6E-41C2-9759-7BE17EB474A4}");
                public static readonly ID ImageTitle = new ID("{63DDA48B-B0CB-45A7-9A1B-B26DDB41009B}");

            }
        }


        public struct RealtyResidentail
        {
            public static readonly ID ID = new ID("{B34DAAA4-B562-4063-96B3-42F430BA418E}");

            public struct Fields
            {
                public static readonly ID ProjectStatus = new ID("{C36CF680-7445-486D-9F4D-948F22BF91B1}");
                public static readonly ID Township = new ID("{83BD2517-2A27-4D92-B97B-128F9C014907}");
                public static readonly ID Title = new ID("{C30A013F-3CC8-4961-9837-1C483277084A}");
                public static readonly ID Location = new ID("{C8FF2B82-90B4-4828-888C-96E63434D844}");
                public static readonly ID Image = new ID("{9492E0BB-9DF9-46E7-8188-EC795C4ADE44}");
            }
        }

        public struct RealtyCommercial
        {
            public static readonly ID ID = new ID("{6E504458-4747-4A0E-8A46-483F03E8D0C4}");

            public struct Fields
            {
                public static readonly ID ProjectStatus = new ID("{11772132-EF04-4104-9BFC-6378B6832D65}");
                public static readonly ID Township = new ID("{8AE049B1-0D45-4F09-9C72-FF7B451CAD8B}");
                public static readonly ID Title = new ID("{C30A013F-3CC8-4961-9837-1C483277084A}");
                public static readonly ID Location = new ID("{7AB2A9A5-8AC1-45CF-8D57-936EB686B8BA}");
                public static readonly ID Image = new ID("{9492E0BB-9DF9-46E7-8188-EC795C4ADE44}");
            }
        }

        public struct RealtyResidentailMenuRoot
        {
            public static readonly ID ID = new ID("{62716968-A105-4C20-A69E-E326D93F4013}");
        }

        public struct RealtyCommercialMenuRoot
        {
            public static readonly ID ID = new ID("{72C91D98-9C05-46CE-B62E-A2B63A22AF0B}");
        }

        public struct RealtyReadyToMove
        {
            public static readonly ID ID = new ID("{A8A307E5-93A6-46E5-A585-1F45F1395E64}");

        }

        public struct RealtyUnderContruction
        {
            public static readonly ID ID = new ID("{4BCC6033-29AB-42E3-8ED7-486D479B79DF}");
        }

        public struct RealtyLocations
        {
            public static readonly ID ID = new ID("{3742F700-308E-4A97-B524-4274E2ACCBB1}");
            public static readonly ID RootItem = new ID("{C5E1D984-1FDA-432F-8B67-C5ED0A796E3C}");
            public struct Fields
            {
                public static readonly ID Text = new ID("{E4059B29-8F8D-4F8B-B265-4F9917EFC4B6}");
            }
        }

        public struct RealtyProjectStatus
        {
            public struct Fields
            {
                public static readonly ID Text = new ID("{E4059B29-8F8D-4F8B-B265-4F9917EFC4B6}");
                public static readonly ID Style = new ID("{A791F095-2521-4B4D-BEF9-21DDA221F608}");
            }
        }

        public struct RealtyTownship
        {
            public static readonly ID ID = new ID("{4BCC6033-29AB-42E3-8ED7-486D479B79DF}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{C30A013F-3CC8-4961-9837-1C483277084A}");
            }
        }


        public struct RealtyMenuMedia
        {
            public static readonly ID ID = new ID("{F0369E1A-9030-4A05-A09C-EF09B664ED27}");

            public static readonly ID RootMenu = new ID("{956F99A6-0321-43FD-A456-36B35AFCF132}");
            public struct Fields
            {
                public static readonly ID Style = new ID("{A791F095-2521-4B4D-BEF9-21DDA221F608}");
                public static readonly ID ThumbImage = new ID("{4FF62B0A-D73B-4436-BEA2-023154F2FFC4}");
            }
        }

        public struct RealtyClub
        {
            public static readonly ID ID = new ID("{1E47FD88-9B08-49DD-A5EB-07DF7044BC7D}");
        }
        public struct Footer
        {
            public static readonly ID GlobalRootFolderID = new ID("{E6216C2B-C981-44EA-9970-7B9B75B59B61}");
            public struct Fields
            {
                public static readonly ID LetsConnect = new ID("{CD20C879-01E9-4AD1-86D0-8F2FF73B66E7}");
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



        public struct FooterNav
        {
            public static readonly ID ID = new ID("{684DDE1A-891F-4430-A521-D4C643BDD760}");

            public struct Fields
            {
                public static readonly ID Name =new ID("{1174708E-3E27-49B9-B041-0FFA7B5CA767}");
                public static readonly ID FontColor = new ID("{C58C2749-84B4-4644-AD4F-D4C2C9628E5D}");
                public static readonly ID Link = new ID("{955D0A2F-10EE-4975-B38C-E1A2E8D21BE3}");

                public static readonly ID Image = new ID("{110A0DF3-8A4F-4452-8B55-2B06112660FC}");

                public static readonly ID IsEnable = new ID("{6DB81347-30FB-4C3C-A4D1-359AD99FA7C6}");
            }
        }
        public struct SitemapListing
        {
            public static readonly ID ID = new ID("{E1F4A5EA-1E25-4302-8B59-B4F5C4B3BFD6}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{02C0AB6A-CA1E-4FE7-9755-449BC127CAB2}");
                public static readonly ID SubTitle = new ID("{69C694ED-AF70-4A9A-9DC3-2B13AFD2373C}");

                public static readonly ID Title1 = new ID("{00D43FC1-3858-4F5C-9592-E575A2CC6744}");
                public static readonly ID Listing1 = new ID("{8660FF7F-FA4A-4A3E-8CDF-FBF1D1EF71DE}");

                public static readonly ID Title2 = new ID("{0640A2CA-5FDC-4D6F-8BC7-BE7DFCA7D0C3}");
                public static readonly ID Listing2 = new ID("{7A6A0E0A-AD38-4D92-8B9A-52D70D862B92}");

                public static readonly ID Title3 = new ID("{ECB3D2C5-F3BC-42BE-BC00-8E7B0266EAA0}");
                public static readonly ID Listing3 = new ID("{EB13FEB7-D36D-4F3F-B907-B8392E00D572}");

                public static readonly ID Title4 = new ID("{1E6523A1-6875-48D5-8A87-B23DB1C30431}");
                public static readonly ID Listing4 = new ID("{DD0912D7-7B23-4F4E-B9D9-7BF47CBFF529}");

                public static readonly ID Title5 = new ID("{C07EF262-B212-45CE-8FD8-CCC4CC5E5A04}");
                public static readonly ID Listing5 = new ID("{34243A0A-6E05-44AD-9948-16A5E8EA70C7}");

                public static readonly ID Title6 = new ID("{3D6FD4E2-92F0-4D72-80CC-BF71C4F01E57}");
                public static readonly ID Listing6 = new ID("{7C0C02E2-0E26-4F2E-990A-D413BF41A6CD}");

                public static readonly ID Title7 = new ID("{A9C4601E-22C0-4C9B-998C-A50133F4B920}");
                public static readonly ID Listing7 = new ID("{BD4CDB74-A6F8-4341-B4FB-481DBD0B6755}");

                public static readonly ID Title8 = new ID("{1D7A565F-5C14-40A4-A49F-65D1B5F5F5D2}");
                public static readonly ID Listing8 = new ID("{63BD892F-864D-4E5E-BA2B-6CA8946FE8D3}");




            }
        }





        public struct FooterNavListing
        {
            public static readonly ID ID = new ID("{F7562229-D176-43D8-84BA-21F1D128A519}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{7531CE03-8B2F-4EF1-AAD3-A0485E09E27E}");
                public static readonly ID ColumnName1 = new ID("{250EE076-E15E-4597-A704-BC6D167E02AF}");
                public static readonly ID ColumnList1 = new ID("{838146F6-805A-4349-9A9B-7100FD1351F9}");

                public static readonly ID ColumnName2 = new ID("{6BD2FBF1-6503-4A6C-91DA-779695A549F2}");

                public static readonly ID ColumnList2 = new ID("{7E4752D5-3AC5-408F-8CD9-C34B343FE109}");


                public static readonly ID ColumnName3 = new ID("{2024C77D-06EA-4A36-844F-3B7334AC01B2}");

                public static readonly ID ColumnList3 = new ID("{2313D8AC-3E41-43FA-9967-9B877A5D7A69}");

                public static readonly ID ColumnName4 = new ID("{734BF0F5-01F6-43ED-ACF8-11121948D5AB}");

                public static readonly ID ColumnList4 = new ID("{471CF123-877B-4534-8E17-9979AE6BBDA8}");
            }
        }







        public struct FooterCopyright
        {
            public static readonly ID ID = new ID("{6BD29BCA-C30C-41D8-842F-94C606830A7F}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{601174D0-A002-466D-8A34-C7125BE0196B}");
                public static readonly ID CopyRightList = new ID("{B57E0D68-3F48-4ABA-B2CF-44095A3AE0F4}");

            }
        }

        public struct SocialLink
        {
            public static readonly ID ID = new ID("{D51FB0E0-3585-4ED7-9E39-639DBD871185}");

            public struct Fields
            {
                public static readonly ID SocailLinksList = new ID("{250F6DD2-4B74-4299-B1D9-1EFACDF207B1}");
                public static readonly ID SocailStoreList = new ID("{825E334C-B521-43B0-B508-B43661BC3880}");

            }
        }


    }
}