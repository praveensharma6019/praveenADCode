using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform
{
    public static class Templates
    {
        public static class NameValueCollection
        {
            public const string NameValueFolder = "{979C4694-EF8D-4D7F-9017-9E3E9903359B}";
            public const string nameValue = "{407FC9F0-CC99-475B-8F51-541094377FBB}";
            public static class Fields
            {
                public static readonly ID NavigationTitle = new ID("{32CFF90D-4FDF-4402-A364-21199E88753D}");
            }
        }

        public static class NavigationRoot
        {
            public static readonly ID Id = new ID("{D7F870BC-89D8-4F17-95EC-59ACFD7DA05C}");

            public static class Fields
            {
                public static readonly ID HeaderLogo = new ID("{FEE16E6B-0823-44FB-ACD2-F56DB2011AA3}");
                public static readonly ID FooterCopyright = new ID("{F6098004-9129-41E4-A3B0-604A7583C26D}");
            }
        }
        public static class AirportNavigation
        {
            public static readonly ID LinkTemplateID = new ID("{4DC30130-56B5-4E49-BB2D-6FC7EA5465DB}");
            public static readonly ID LinkFolderTemplateID = new ID("{7E944CF9-AAB7-427A-AD75-5A798175BC02}");
            public static readonly ID ImageWithLinkTemplateID = new ID("{7FB58A89-6634-4483-B9E4-E29199FFBDFB}");

            /// <summary>
            /// footer navigation items
            /// </summary>
            public static readonly ID seoContentItemID = new ID("{20DF286C-199A-435F-B88D-B884CFAAD24E}");
            public static readonly ID servicesItemID = new ID("{24F55884-1308-42E4-85B5-986637910EE8}");
            public static readonly ID brandItemID = new ID("{E0AF67B7-5518-4B91-AF20-292CF8441DCD}");
            public static readonly ID MainNavigationItemID = new ID("{3A3AAF78-1488-4934-9381-F02AB2BB456C}");
            public static readonly ID TravelHelpItemID = new ID("{2382E05C-85B8-4EE5-AC62-D676B3AD5CFB}");
            public static readonly ID FlightInformationItemID = new ID("{3CB7A649-89DA-41CB-8CB6-5076B83E22AD}");
            public static readonly ID AdaniBusinessesItemID = new ID("{E85C563F-29E7-47B8-AFC3-44CAD139B06B}");
            public static readonly ID CompanyItemID = new ID("{6E36B5A9-7F73-4A12-91D0-EDAF5EF973B5}");
            public static readonly ID HelpAndSupportItemID = new ID("{FDC0BF4B-AAE9-4063-92C3-8EEC38344E10}");
            public static readonly ID paymentItemID = new ID("{9679C35A-EB3C-49A7-BF77-C352B45117D2}");
            public static readonly ID FooterSocialLinksItemID = new ID("{8677BE9F-463A-4C9D-A0BD-C27B2C2B3A0B}");
            public static readonly ID DownloadItemID = new ID("{65EE9F38-EC8A-4818-86C0-1C422FF27A94}");
            public static readonly ID FooterCopyRightItemID = new ID("{5F39675A-F63B-41BC-938C-2DBDFFCE1967}");
            public static readonly ID FooterContactUsItemID = new ID("{116E514F-8A68-4257-8E54-C494E57E5A8C}");
            public static readonly ID BottomNavItemID = new ID("{150006DA-068B-4D4F-9D4C-431CB0415B8C}");
            public static readonly ID footerBottomNavID = new ID("{50E5DB7A-E94B-414D-B424-C5CC04D8943B}");

          

            /// <summary>
            /// Header navigation items
            /// </summary>
            public static readonly ID TopNavigationID = new ID("{078AB32D-B56B-4AF6-A748-9129A9D4122C}");
            public static readonly ID HamburgerMenuID = new ID("{83CF38F9-551D-48D4-9A43-B207348B2345}");
            public static readonly ID AdaniAirportID = new ID("{10ACCC7E-A9B5-4845-8D9C-2DCAB20FEB30}");
            public static readonly ID OtherID = new ID("{0EDD8E68-71DD-4C5A-B3A6-58F1B983778A}");
            public static readonly ID AdaniBusinessesID = new ID("{9119EC22-0AB8-47ED-BD7B-E7AAD06EADBC}");
            public static readonly ID PrimaryHeaderMenuID = new ID("{126A2B76-CD52-45B7-B623-A2891700F04E}");
            public static readonly ID SearchSuggesterID = new ID("{24563F33-F1BA-47F2-B7F3-288C328E457B}");
            public static readonly ID UserAccountID = new ID("{CADB6CEF-D159-42D2-A5AE-D96513996962}");
            public static readonly ID MyAccountsID = new ID("{10C0AFF5-E835-4622-80A0-8097066B7047}");
            public static readonly ID LogoDropdownID = new ID("{023F5A2A-2317-4A03-B286-C136672C3856}");
            public static readonly ID AirportListID = new ID("{BF639FA4-0FDE-406D-B616-279181D0D069}");
            public static readonly ID HeaderLogoID = new ID("{2017BE48-39DD-4F5F-9153-7447C7050555}");
            public static readonly ID QuickLinksID = new ID("{DEA0B602-C55B-4F06-9B08-D2555E3F175C}");


        }
        public static class NavigationItem
        {
            public static readonly ID Id = new ID("{C231FBB4-DCDB-4708-99BC-94760F222CC5}");
        }
    }
}