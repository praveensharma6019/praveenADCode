using Sitecore.Data;
using System;

namespace Sitecore.TrivandrumAirport.Website.Model
{
    [Serializable]
    public struct Templates
    {
        public struct Datasource
        {
            public readonly static ID PaymentGateway;

            public readonly static ID PaymentMode;

            public readonly static ID PaymentStatus;

            public readonly static ID EnvelopeType;

            static Datasource()
            {
                Templates.Datasource.PaymentGateway = new ID("{809E9EB6-637F-4858-8DBA-5284F2C03133}");
                Templates.Datasource.PaymentMode = new ID("{90B1C345-0FF6-4C1B-A7F7-04D78E65B515}");
                Templates.Datasource.PaymentStatus = new ID("{79F22EF6-F41D-4C6C-8E62-34497EF2E892}");
                Templates.Datasource.EnvelopeType = new ID("{8E21D182-70E0-4F81-BADF-6BAD2B027201}");
            }
        }

        public struct ItemList
        {
            public readonly static ID MeterReadingItemList;

            static ItemList()
            {
                Templates.ItemList.MeterReadingItemList = new ID("{85593468-0842-4548-976C-E8EDC2CC03D1}");
            }
        }

        public struct MailTemplate
        {
            public readonly static ID ID;

            public readonly static ID EnvelopeUser;

            public readonly static ID AdminUser;

            public readonly static ID EnvelopeUserOnTenderClose;

            public readonly static ID EnvelopeUserReminderForTenderClose;

            public readonly static ID CorrigendumCreate;

            public readonly static ID ContactUs;

            public readonly static ID PQApproval;

            public readonly static ID PQRejection;

            public readonly static ID sendForPQApproval;

            public readonly static ID sendForTenderDocApproval;

            static MailTemplate()
            {
                Templates.MailTemplate.ID = new ID("{7BB9D998-6B64-4B87-9FB5-8A9B03519F16}");
                Templates.MailTemplate.EnvelopeUser = new ID("{17E2EC0A-C5A1-4625-8DFF-A2D5A0ECA789}");
                Templates.MailTemplate.AdminUser = new ID("{AD02ADA1-5704-4268-8C04-2F98EA7EDD83}");
                Templates.MailTemplate.EnvelopeUserOnTenderClose = new ID("{8EA06F2B-25A2-4FD8-8293-0374C57B33BF}");
                Templates.MailTemplate.EnvelopeUserReminderForTenderClose = new ID("{EAC940F8-9B38-4693-B963-B4954F299D1B}");
                Templates.MailTemplate.CorrigendumCreate = new ID("{9FBB69E8-1835-4FA2-828A-DB2806E4742D}");
                Templates.MailTemplate.ContactUs = new ID("{D6C52163-01C2-480F-A069-AA5C1B9954F7}");
                Templates.MailTemplate.PQApproval = new ID("{0B8AE7E4-64CC-4876-AFA1-18CFDF14E94F}");
                Templates.MailTemplate.PQRejection = new ID("{BF55537D-D7F2-411B-9D1A-56F7C83A5676}");
                Templates.MailTemplate.sendForPQApproval = new ID("{5D1C04B9-2738-4EBC-9803-D574D6A877EB}");
                Templates.MailTemplate.sendForTenderDocApproval = new ID("{E549F0A9-CEB2-4A9A-AF7C-3917DB0183D8}");
            }

            public struct Fields
            {
                public readonly static ID From;

                public readonly static ID Subject;

                public readonly static ID Body;

                static Fields()
                {
                    Templates.MailTemplate.Fields.From = new ID("{8605948C-60FB-46B8-8AAA-4C52561B53BC}");
                    Templates.MailTemplate.Fields.Subject = new ID("{0F45DF05-546F-462D-97C0-BA4FB2B02564}");
                    Templates.MailTemplate.Fields.Body = new ID("{1519CCAD-ED26-4F60-82CA-22079AF44D16}");
                }
            }
        }

        public struct MeterReadingProperties
        {
            public readonly static ID CYCLE;

            public readonly static ID BILLMONTH;

            public readonly static ID METERREADINGDATE;

            public readonly static ID FULLMETERREADINGDATE;

            public readonly static ID PROPOSEDDELIVERYDATE;

            public readonly static ID PROPOSEDDUEDATE_RESI;

            public readonly static ID PROPOSEDDUEDATE_COM;

            static MeterReadingProperties()
            {
                Templates.MeterReadingProperties.CYCLE = new ID("{AA5B9814-94E4-407C-8F3C-C0733CA9B82B}");
                Templates.MeterReadingProperties.BILLMONTH = new ID("{13BE92FE-CD21-429B-9087-FC69BD537F40}");
                Templates.MeterReadingProperties.METERREADINGDATE = new ID("{A00BB8C1-0247-46D7-951A-50D1DFED6509}");
                Templates.MeterReadingProperties.FULLMETERREADINGDATE = new ID("{C7260B34-2FCC-429E-BADC-6290834C8930}");
                Templates.MeterReadingProperties.PROPOSEDDELIVERYDATE = new ID("{1A932655-7320-420F-9FF0-12848EB3A9DF}");
                Templates.MeterReadingProperties.PROPOSEDDUEDATE_RESI = new ID("{07E0DECA-A490-4534-8B3E-97500759C217}");
                Templates.MeterReadingProperties.PROPOSEDDUEDATE_COM = new ID("{DF874E79-07AC-48B6-BEFC-1C82706812FE}\t");
            }
        }

        public struct PaymentTransaction
        {
            public readonly static ID PaymentTransactioTemplateId;

            static PaymentTransaction()
            {
                Templates.PaymentTransaction.PaymentTransactioTemplateId = new ID("{9453CBE0-3F4E-4D0B-A7D8-E7CC389C86C8}");
            }
        }

        public struct Tender
        {
            public readonly static ID TenderLogin;

            public readonly static ID TenderList;

            public readonly static ID TenderDetails;

            public readonly static ID AdminTenderListing;

            public readonly static ID AdminUserTenderListing;

            public readonly static ID AdminTenderDetail;

            public readonly static ID AdminTenderUpdate;

            public readonly static ID TenderCorrigendum;

            public readonly static ID AdminTenderCreate;

            public readonly static ID EnvelopeUserCreate;

            public readonly static ID EnvelopeUserListing;

            public readonly static ID CorrigendumTenderListing;

            public readonly static ID CorrigendumTenderCreate;

            public readonly static ID CorrigendumTenderUpdate;

            public readonly static ID AdminUser;

            public readonly static ID AdminUserCreate;

            static Tender()
            {
                Templates.Tender.TenderLogin = new ID("{1A34BE14-1EC8-47AD-BE7C-A1DC8A9604F8}");
                Templates.Tender.TenderList = new ID("{09ABCE1D-3811-483D-AA33-3283C408B24D}");
                Templates.Tender.TenderDetails = new ID("{8FCC54FB-5400-4DF6-B2AF-600D40A1ED02}");
                Templates.Tender.AdminTenderListing = new ID("{B8A2DB11-F73F-4E0B-8EFC-80E91DDB5778}");
                Templates.Tender.AdminUserTenderListing = new ID("{E20AFD40-CCDD-4BDF-B622-841103EC7775}");
                Templates.Tender.AdminTenderDetail = new ID("{71CAFC35-C559-43FE-9BB0-D3D94245085A}");
                Templates.Tender.AdminTenderUpdate = new ID("{22685A1B-08FC-4EC9-B4D3-B2194EE0A4AE}");
                Templates.Tender.TenderCorrigendum = new ID("{969422DE-F712-4013-B034-6A0CFECD8494}");
                Templates.Tender.AdminTenderCreate = new ID("{CAA3E21F-DA19-41CF-8902-7560D16A3345}");
                Templates.Tender.EnvelopeUserCreate = new ID("{E75A65FD-C1D7-45B7-ADBF-32E9E68EDC2A}");
                Templates.Tender.EnvelopeUserListing = new ID("{21041D96-7516-488F-B33C-8CCA4F88BDB1}");
                Templates.Tender.CorrigendumTenderListing = new ID("{659F8021-0693-46C9-A55E-CCBEAE8E7126}");
                Templates.Tender.CorrigendumTenderCreate = new ID("{1586281D-414D-4BE2-985B-88CA8406689E}");
                Templates.Tender.CorrigendumTenderUpdate = new ID("{4FC7A2FB-4BF1-4C4D-A22C-F04EE69E0931}");
                Templates.Tender.AdminUser = new ID("{4BA79DF4-7B9C-45E2-B8F7-82413F527D3F}");
                Templates.Tender.AdminUserCreate = new ID("{49E0999E-81D3-4EDA-9A07-47CC01653860}");
            }
        }
    }
}