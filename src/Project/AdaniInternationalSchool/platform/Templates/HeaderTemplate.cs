using Sitecore.Data;

namespace Project.AdaniInternationalSchool.Website.Templates
{
    public static class HeaderTemplate
    {
        public static class Fields
        {
            public static readonly ID LogoSrc = new ID("{42210DBB-7A36-4C52-A662-AE0FDDE09D96}");
            public static readonly ID LogoSrcSmall = new ID("{C8C0D5A1-9758-419C-AA14-5CC37DB54517}");
            public static readonly ID LogoSrcHamburger = new ID("{5B97B583-864F-4714-85C0-4340E24ABB54}");
            public static readonly ID HamburgerBG = new ID("{56CD7616-D76F-46A7-A5B2-B83AE2C6735E}");
            public static readonly ID LogoAlt = new ID("{62CA037C-7D2B-442C-A2FE-BF879A5B2AC7}");
        }

        public static class Contact
        {
            public static class Fields
            {
                public static readonly ID Url = new ID("{F5971EAC-5C2C-4134-A2C0-5A365DF4CD5F}");
                public static readonly ID Label = new ID("{5999D23A-51CC-4FA0-8073-A11B2777AF87}");
                public static readonly ID Type = new ID("{6C17B216-9AC4-44EB-B2E4-227B94842F28}");
            }
        }

        public static class Navigation
        {
            public static class Fields
            {
                public static readonly ID Label = new ID("{6AB801DE-412A-4C62-85E4-0477A7578BAF}");
                public static readonly ID Url = new ID("{105F8A07-5379-41C2-90B8-6AB552B33E86}");
                public static readonly ID Target = new ID("{A9AFC729-3FEF-4BDA-B15E-2612F916FDA8}");
                public static readonly ID IsActive = new ID("{3586458F-791F-4BA3-9485-0B5F18C4B823}");
                public static readonly ID IsHighLighted = new ID("{B6D766C7-5010-43DE-8DD7-B865FD19A808}");
                public static readonly ID HighlightLabel = new ID("{D373AF13-1736-43A6-A3BF-10A2C98E751F}");

            }
        }
    }
}
