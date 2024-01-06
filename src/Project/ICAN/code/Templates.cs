using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.ICAN.Website
{
    public class Templates
    {
        public struct MailConfiguration
        {
            public static readonly ID MailConfigurationItemID = new ID("{6701EBE5-DAC4-49B2-A70D-61DB4110E1EC}");
            public struct MailConfigurationFields
            {
                public static readonly ID Customer_SubjectName = new ID("{EB13B15F-54E4-4612-8834-4AB166F24D98}");
                public static readonly ID Customer_MailFrom = new ID("{BC9A114B-6F8C-4D69-9F2F-A5C608D9E09C}");
                public static readonly ID Customer_SuccessMessage = new ID("{83F73F09-FCF1-4D0F-8EE2-63FECC980BE9}");
                public static readonly ID Customer_FailureMessage = new ID("{9CBF6BE5-EA90-4A3B-8419-1A6F55EECC7F}");
                public static readonly ID Officials_SubjectName = new ID("{2CDEE9EE-CF03-494A-8E6C-BDE902210AC1}");
                public static readonly ID Officials_RecipientMail = new ID("{9EA9DF5D-E16B-44BB-818A-9DAEF8CDA823}");
                public static readonly ID Officials_Message = new ID("{5E8C5141-AED4-49D4-B168-C8AD144E0894}");
                public static readonly ID Officials_MailFrom = new ID("{ED8AC668-FB09-450E-B145-D5CF681742C7}");
            }
        }
    }
}