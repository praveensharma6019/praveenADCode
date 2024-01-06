using Sitecore.Data;
using System;

namespace Sitecore.AhmedabadAirport.Website.Model
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
                Templates.MailTemplate.ID = new ID("{325689B9-BC26-4F37-A8A6-8FE278F3CC0B}");
                Templates.MailTemplate.EnvelopeUser = new ID("{1B6F1142-F7C9-4649-9045-2D13A77CFB9F}");
                Templates.MailTemplate.AdminUser = new ID("{AAACDDBA-C713-412B-A4F2-99D5EBFB50A3}");
                Templates.MailTemplate.EnvelopeUserOnTenderClose = new ID("{7C44384D-64C5-47F9-9106-1C759ED0F157}");
                Templates.MailTemplate.EnvelopeUserReminderForTenderClose = new ID("{B5D2ED2F-E9EE-49FD-94F2-E68D1CED4E8B}");
                Templates.MailTemplate.CorrigendumCreate = new ID("{5777282C-FCB2-4C08-8B78-6BB59C533FC4}");
                Templates.MailTemplate.ContactUs = new ID("{D6C52163-01C2-480F-A069-AA5C1B9954F7}");
                Templates.MailTemplate.PQApproval = new ID("{7A394273-079D-4CB7-A7BA-F89377BB172F}");
                Templates.MailTemplate.PQRejection = new ID("{33643DF7-E631-4D42-AF4F-F912787E864A}");
                Templates.MailTemplate.sendForPQApproval = new ID("{2397A3FB-D387-4564-8382-AB0AD036B3DA}");
                Templates.MailTemplate.sendForTenderDocApproval = new ID("{8423E95D-FD6B-415F-8468-C69EAD1DBB74}");
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
            public readonly static ID TenderLoginOld;

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
                Templates.Tender.TenderLoginOld = new ID("{84829838-7FD5-4EB4-A9AB-B8A0B7512914}");
                Templates.Tender.TenderLogin = new ID("{8BA1215D-20A1-4F64-8F6D-B2C638F636AF}");
                Templates.Tender.TenderList = new ID("{B6E4ECA1-EB86-44FB-90E4-39CE2F87F215}");
                Templates.Tender.TenderDetails = new ID("{C69FBBF7-0ED0-4B61-AA21-2E1442E2A570}");
                Templates.Tender.AdminTenderListing = new ID("{3177B6FC-D331-4DFF-AE80-A48513D85C35}");
                Templates.Tender.AdminUserTenderListing = new ID("{59C96E93-5C09-4C86-922F-A6DDE1BFBBD9}");
                Templates.Tender.AdminTenderDetail = new ID("{BF2C2655-D255-48A3-A1C8-A848135652D8}");
                Templates.Tender.AdminTenderUpdate = new ID("{492DCFB1-C634-4B08-8E88-EB6E1422811D}");
                Templates.Tender.TenderCorrigendum = new ID("{3ED43951-9B95-4BC4-BFE5-EED7C46A8F12}");
                Templates.Tender.AdminTenderCreate = new ID("{CAA3E21F-DA19-41CF-8902-7560D16A3345}");
                Templates.Tender.EnvelopeUserCreate = new ID("{42A485CD-7D10-419C-8636-22FF08BB9245}");
                Templates.Tender.EnvelopeUserListing = new ID("{F45D6F69-B918-46CD-B263-5997E22B590C}");
                Templates.Tender.CorrigendumTenderListing = new ID("{B822892D-3EE5-4F76-83BF-F7A927278449}");
                Templates.Tender.CorrigendumTenderCreate = new ID("{DBF94E3C-F1F2-4986-8A18-F86EA379CAAB}");
                Templates.Tender.CorrigendumTenderUpdate = new ID("{AFDD5393-16E5-45B6-B44F-21893A47C757}");
                Templates.Tender.AdminUser = new ID("{E4E3450F-4D6E-4EE0-A048-B791B70B8BC1}");
                Templates.Tender.AdminUserCreate = new ID("{4671F6A3-3F9E-4870-B96D-E2769059475A}");
            }
        }
    }
}