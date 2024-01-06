using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.GyaanGalaxy.Website
{
    public class Templates
    {
        public struct MailConfiguration
        {
            public static readonly ID MailConfigurationItemID = new ID("{D7CF1545-6C27-4581-B552-02838AC76EDB}");
            public struct MailConfigurationFields
            {
                public static readonly ID Customer_SubjectName = new ID("{1CE8F867-6B45-4451-BF8F-2186EF6344DE}");
                public static readonly ID Customer_MailFrom = new ID("{99690686-390B-4FEC-A984-07ABCE6C5CA2}");
                public static readonly ID Customer_SuccessMessage = new ID("{A20F6C20-9ED7-4676-8463-24F695BC6689}");
                public static readonly ID Customer_FailureMessage = new ID("{CF4A3371-4C8F-444E-BF9F-9278E521F016}");
                public static readonly ID Officials_SubjectName = new ID("{50D7F5CE-213F-42D0-9E97-AFAD5A882F72}");
                public static readonly ID Officials_RecipientMail = new ID("{7A01E374-DB81-4EFC-A138-3432AC428B61}");
                public static readonly ID Officials_Message = new ID("{61A465AD-9D3F-4B7E-A5B1-8B6FF2ADE849}");
                public static readonly ID Officials_MailFrom = new ID("{EE132F56-ED16-41B0-B3C6-A7783119F019}");
            }
        }


        public struct MailConfigurationSubmission
        {
            public static readonly ID MailConfigurationSubmissionItemID = new ID("{97936E57-A4EB-44E1-9B42-3182EE314D86}");
            public struct MailConfigurationSubmissionFields
            {
                public static readonly ID Customer_SubjectName = new ID("{CD4136CA-DC41-43B8-BA70-E73A6C52230D}");
                public static readonly ID Customer_MailFrom = new ID("{2FCDBD09-652A-4CE5-917D-99B4FCA21E1A}");
                public static readonly ID Customer_SuccessMessage = new ID("{C103F954-9DDE-4A91-AAE9-1B581465BE76}");
                public static readonly ID Customer_FailureMessage = new ID("{8A4608C3-460D-4C34-AEFE-D27B7561E131}");
                public static readonly ID Officials_SubjectName = new ID("{1AE38650-F404-49D0-9A07-31D9E15504F4}");
                public static readonly ID Officials_RecipientMail = new ID("{E94587E4-80A7-44B1-830F-2537A39265D1}");
                public static readonly ID Officials_Message = new ID("{2E6B5224-5049-47EB-A968-FE5A9E69CCFA}");
                public static readonly ID Officials_MailFrom = new ID("{8E0DA2D1-8AF2-4F65-8940-C16F950963A9}");
            }
        }
    }
}