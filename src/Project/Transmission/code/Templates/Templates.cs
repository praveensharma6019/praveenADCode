using Sitecore.Data;
using System;

namespace Sitecore.Transmission.Website
{
    public class Templates
    {
        public Templates()
        {
        }

        public struct MailConfiguration
        {
            public readonly static ID MailConfigurationItemID;
            

            static MailConfiguration()
            {
                Templates.MailConfiguration.MailConfigurationItemID = new ID("{2A97CD09-6807-46F4-A548-F02F2B7163CF}");

            }

            public struct costCalculator
            {
                public readonly static ID costCalculatorRegistration = new ID("{85D1B4E7-9F8E-49FD-AD2B-99E1F496D0D2}");
               
                public struct Fields
                {
                    public static readonly ID From = new ID("{8605948C-60FB-46B8-8AAA-4C52561B53BC}");
                    public static readonly ID Subject = new ID("{0F45DF05-546F-462D-97C0-BA4FB2B02564}");
                    public static readonly ID Body = new ID("{1519CCAD-ED26-4F60-82CA-22079AF44D16}");
                }
            }

            public struct ReviewDate
            {
                public readonly static ID ReviewDateID = new ID("{BC0025AD-84E6-4270-B03A-9D0762C0524D}");
                public struct Fields
                {
                    public static readonly ID From = new ID("{8605948C-60FB-46B8-8AAA-4C52561B53BC}");
                    public static readonly ID Subject = new ID("{0F45DF05-546F-462D-97C0-BA4FB2B02564}");
                    public static readonly ID Body = new ID("{1519CCAD-ED26-4F60-82CA-22079AF44D16}");
                }
            }

            public struct MailConfigurationFields
            {
                public readonly static ID Customer_SubjectName;

                public readonly static ID Customer_MailFrom;

                public readonly static ID CCRegistration_MailFrom;

                public readonly static ID Customer_SuccessMessage;

                public readonly static ID Customer_FailureMessage;

                public readonly static ID Vendor_L1ApproveMessage;
                public readonly static ID Vendor_L1RejectMessage;
                public readonly static ID Vendor_L2ApproveMessage;
                public readonly static ID Vendor_L2RejectMessage;
                public readonly static ID Vendor_L2ReassessMessage;
                public readonly static ID Vendor_ReassessAproveMessage;
                public readonly static ID Vendor_ReassessRejectMessage;

                public readonly static ID Officials_SubjectName;

                public readonly static ID Officials_RecipientMail;

                public readonly static ID Officials_Message;

                public readonly static ID Officials_MailFrom;

                static MailConfigurationFields()
                {
                    Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName = new ID("{6554B2DA-AB71-4169-AF56-0835658653F9}");
                    Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom = new ID("{20F0CCAA-5CFD-4A5F-AF14-E63A2480A20D}");
                    Templates.MailConfiguration.MailConfigurationFields.Customer_SuccessMessage = new ID("{28D4E48D-BA53-4690-953B-F46F853C4D1C}");
                    Templates.MailConfiguration.MailConfigurationFields.Customer_FailureMessage = new ID("{97CCDED8-36A3-471A-B91C-79137244D5B4}");
                    Templates.MailConfiguration.MailConfigurationFields.Vendor_L1ApproveMessage = new ID("{D2A4ACD0-B333-4134-BC2F-73995BB9FDFE}");
                    Templates.MailConfiguration.MailConfigurationFields.Vendor_L1RejectMessage = new ID("{3719BC9E-F248-4B16-998C-7C45397921F0}");
                    Templates.MailConfiguration.MailConfigurationFields.Vendor_L2ApproveMessage = new ID("{0E53F1FD-EE26-4D8F-BF74-534659803656}");
                    Templates.MailConfiguration.MailConfigurationFields.Vendor_L2RejectMessage = new ID("{E841CDFF-0EC3-4B5B-8329-4546C1716B81}");
                    Templates.MailConfiguration.MailConfigurationFields.Vendor_L2ReassessMessage = new ID("{8436F8F2-BF99-4274-87AC-0F7ED1330E11}");
                    Templates.MailConfiguration.MailConfigurationFields.Vendor_ReassessAproveMessage = new ID("{49D10AA1-44C3-442C-8E01-DBEEC14E676C}");
                    Templates.MailConfiguration.MailConfigurationFields.Vendor_ReassessRejectMessage = new ID("{54BD092A-43C5-4FD6-BA07-F07A99AC2E11}");
                    Templates.MailConfiguration.MailConfigurationFields.Customer_FailureMessage = new ID("{97CCDED8-36A3-471A-B91C-79137244D5B4}");
                    Templates.MailConfiguration.MailConfigurationFields.Officials_SubjectName = new ID("{8DE4C939-B9EC-468D-83BC-5D2AAF06B446}");
                    Templates.MailConfiguration.MailConfigurationFields.Officials_RecipientMail = new ID("{0DA3645F-1A67-4C68-B46D-8E4C023811E8}");
                    Templates.MailConfiguration.MailConfigurationFields.Officials_Message = new ID("{F8C12894-9505-4A1F-818C-18FB174490B5}");
                    Templates.MailConfiguration.MailConfigurationFields.Officials_MailFrom = new ID("{F37E48D6-0A96-4F17-9250-8EB3D2D6CB99}");
                }
            }
        }
    }
}