using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniCapital.Website
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
            public static readonly ID MailConfigurationItemID = new ID("{34D9B0B0-2F02-4E6E-98DD-18CCBA668635}");
            public struct MailConfigurationFields
            {
                public static readonly ID Customer_SubjectName = new ID("{9546FD7E-3D75-49E3-A73A-3C3554EFBEBE}");
                public static readonly ID Customer_MailFrom = new ID("{64D8E1FB-B38D-4E1A-86B1-C0377E7E53A9}");
                public static readonly ID Customer_SuccessMessage = new ID("{261B36D5-2728-4D9B-8365-EF920B0828B1}");
                public static readonly ID Customer_FailureMessage = new ID("{B291F791-D74D-477C-90EE-AF69636C9336}");
                public static readonly ID Officials_SubjectName = new ID("{9DB5B41E-EED7-40CC-B1D8-E014156DC69A}");
                public static readonly ID Officials_RecipientMail = new ID("{B1ED2584-81B7-4346-987D-30A62918F16D}");
                public static readonly ID Officials_Message = new ID("{A44AD3EC-3CA1-4787-9015-36D7072690E7}");
                public static readonly ID Officials_MailFrom = new ID("{2DCD84F6-86DB-47B6-AB5F-5F3250974129}");
            }
        }
        public struct FeedbackMailConfiguration
        {
            public static readonly ID MailConfigurationItemID = new ID("{56B3076B-F79E-4C5B-B3CF-5E1D09073648}");
            public struct MailConfigurationFields
            {
                public static readonly ID Customer_SubjectName = new ID("{9546FD7E-3D75-49E3-A73A-3C3554EFBEBE}");
                public static readonly ID Customer_MailFrom = new ID("{64D8E1FB-B38D-4E1A-86B1-C0377E7E53A9}");
                public static readonly ID Customer_SuccessMessage = new ID("{261B36D5-2728-4D9B-8365-EF920B0828B1}");
                public static readonly ID Customer_FailureMessage = new ID("{B291F791-D74D-477C-90EE-AF69636C9336}");
                public static readonly ID Officials_SubjectName = new ID("{9DB5B41E-EED7-40CC-B1D8-E014156DC69A}");
                public static readonly ID Officials_RecipientMail = new ID("{B1ED2584-81B7-4346-987D-30A62918F16D}");
                public static readonly ID Officials_Message = new ID("{A44AD3EC-3CA1-4787-9015-36D7072690E7}");
                public static readonly ID Officials_MailFrom = new ID("{2DCD84F6-86DB-47B6-AB5F-5F3250974129}");
            }
        }
        public struct ContactUSSubject
        {
            public static readonly ID ContactUsSubjectListItemID = new ID("{31499C7B-8945-4703-8BD5-14D9831DEA35}");
        }
        public struct AfterLoginSupportSubject
        {
            public static readonly ID SubjectListItemID = new ID("{81D096E0-B113-4BCC-B343-1ED61205BFF0}");
        }
        public struct ApplyForLoanDropdown
        {
            public static readonly ID ProductListItem = new ID("{7520660B-536C-4179-96C2-20983E6ABDC8}");
            //public static readonly ID EnquirySourceList = new ID("{6CDEB6A4-0297-420B-B18B-68EDD67716F6}");
            //public static readonly ID OccupationList = new ID("{9E2B381F-2E9B-4F6D-85ED-23302258BCEA}");
        }
        public struct SitecoreItems
        {
            public static readonly ID ApplyForLoanPage = new ID("{3222F3A9-EA49-48F6-8508-4146A5E2B8E9}");
            public static readonly ID ThankyouPage = new ID("{A4AD6383-451F-4475-B5A2-E6790E7C1AD0}");
        }
    }
}