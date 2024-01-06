using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniHousing.Website
{
    public class Templates
    {
        public struct AccountsSettings
        {

            public static readonly ID ID = new ID("{59D216D1-035C-4497-97B4-E3C5E9F1C06B}");

            public struct Fields
            {

                public static readonly ID AccountsDetailsPage = new ID("{ED71D374-8C33-4561-991D-77482AE01330}");
                public static readonly ID LoginPage = new ID("{60745023-FFD5-400E-8F80-4BCA9F2ABB29}");
                public static readonly ID ForgotPasswordPage = new ID("{F3CD2BB8-472B-4DF0-87C0-A13098E391CA}");
                public static readonly ID AfterLoginPage = new ID("{B128E2B3-3865-4F1C-A147-5F248676D3F5}");
            }
        }
        public struct MailConfiguration
        {
            public static readonly ID MailConfigurationItemID = new ID("{C96C473F-BCDC-4AA5-B39E-1202B2843E1B}");
            public struct MailConfigurationFields
            {
                public static readonly ID Customer_SubjectName = new ID("{CDF6553C-7E22-4A14-A14A-EBC5D413DCD6}");
                public static readonly ID Customer_MailFrom = new ID("{CDF73E66-8178-4158-9B80-3A4D91AA0D42}");
                public static readonly ID Customer_SuccessMessage = new ID("{5C315A91-18CA-4CDB-8CE6-E0859E73F3D1}");
                public static readonly ID Customer_FailureMessage = new ID("{BE8D0AD6-1DAA-404F-AF74-60FC60B78936}");
                public static readonly ID Officials_SubjectName = new ID("{52C5009E-2E90-4B04-8DC7-17BBD00727A8}");
                public static readonly ID Officials_RecipientMail = new ID("{B1EAAE29-CA00-4532-87F4-8A449FE861A4}");
                public static readonly ID Officials_Message = new ID("{B2C6627B-B9D0-45EC-90FA-4326DA2D86BA}");
                public static readonly ID Officials_MailFrom = new ID("{86A167D4-17F3-401C-9B32-9039368F526E}");
            }
        }
        public struct FeedbackMailConfiguration
        {
            public static readonly ID MailConfigurationItemID = new ID("{B95FD541-EFB7-4378-8C37-0965FD1E6DEB}");
            public struct MailConfigurationFields
            {
                public static readonly ID Customer_SubjectName = new ID("{CDF6553C-7E22-4A14-A14A-EBC5D413DCD6}");
                public static readonly ID Customer_MailFrom = new ID("{CDF73E66-8178-4158-9B80-3A4D91AA0D42}");
                public static readonly ID Customer_SuccessMessage = new ID("{5C315A91-18CA-4CDB-8CE6-E0859E73F3D1}");
                public static readonly ID Customer_FailureMessage = new ID("{BE8D0AD6-1DAA-404F-AF74-60FC60B78936}");
                public static readonly ID Officials_SubjectName = new ID("{52C5009E-2E90-4B04-8DC7-17BBD00727A8}");
                public static readonly ID Officials_RecipientMail = new ID("{B1EAAE29-CA00-4532-87F4-8A449FE861A4}");
                public static readonly ID Officials_Message = new ID("{B2C6627B-B9D0-45EC-90FA-4326DA2D86BA}");
                public static readonly ID Officials_MailFrom = new ID("{86A167D4-17F3-401C-9B32-9039368F526E}");
            }
        }
        public struct ContactUSSubject
        {
            public static readonly ID ContactUsSubjectListItemID = new ID("{99A5ADF4-3540-4BE6-AF7A-F10441D522C7}");
        }
        public struct AfterLoginSupportSubject
        {
            public static readonly ID SubjectListItemID = new ID("{D0E6D0FE-8869-40E8-9669-DC3F82A40EB1}");
        }
        public struct ApplyForLoanDropdown
        {
            public static readonly ID ProductListItem = new ID("{5F182C7E-E909-4CF7-B8F1-B547A50DA44E}");
            //public static readonly ID EnquirySourceList = new ID("{6CDEB6A4-0297-420B-B18B-68EDD67716F6}");
            //public static readonly ID OccupationList = new ID("{9E2B381F-2E9B-4F6D-85ED-23302258BCEA}");
        }
        public struct SitecoreItems
        {
            public static readonly ID ApplyForLoanPage = new ID("{96B479B1-E5C1-40F9-963A-798F37A7953B}");
            public static readonly ID ThankyouPage = new ID("{A53B8FF0-A2FA-4F24-8DB6-2F9C6B56F7FF}");
        }
    }
}