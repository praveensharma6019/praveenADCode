using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.SportsLine.Website
{
    public class Templates
    {
        public struct MailConfiguration
        {
            public static readonly ID MailConfigurationItemID = new ID("{CDE1171A-88A4-44F6-A342-10F2CB237DBC}");
            public struct MailConfigurationFields
            {
                public static readonly ID Customer_SubjectName = new ID("{C931B57B-4E85-4301-B572-F804D9263F0C}");
                public static readonly ID Customer_MailFrom = new ID("{FB95B522-BB68-4C6C-B054-DB4D23B6B598}");
                public static readonly ID Customer_SuccessMessage = new ID("{E5A75352-FF65-4E4E-AE21-E8F12BD87CB8}");
                public static readonly ID Customer_FailureMessage = new ID("{409902E7-4914-4EE6-B224-FF05F3A34978}");
                public static readonly ID Officials_SubjectName = new ID("{B037D824-5F03-414D-B498-02C6ED33B3DB}");
                public static readonly ID Officials_RecipientMail = new ID("{83F083C7-16EB-4888-992D-22187A99659B}");
                public static readonly ID Officials_Message = new ID("{021B006A-FD4D-49C2-BD0C-15F0D61E13F8}");
                public static readonly ID Officials_MailFrom = new ID("{4FB22C7D-C7EB-4130-A7CA-6FFC7C709D13}");
            }
        }
    }
}