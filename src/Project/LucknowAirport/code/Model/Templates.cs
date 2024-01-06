using Sitecore.Data;
using System;

namespace Sitecore.LucknowAirport.Website.Model
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
                Templates.MailTemplate.ID = new ID("{76373E9F-A212-462F-87EA-C6606426CA8B}");
                Templates.MailTemplate.EnvelopeUser = new ID("{B88BD4D4-E41B-4423-A921-19057B9B4FC9}");
                Templates.MailTemplate.AdminUser = new ID("{8B01EAB8-6471-4F2F-A443-BC24770DED89}");
                Templates.MailTemplate.EnvelopeUserOnTenderClose = new ID("{04D23CDC-0021-4FBD-92A7-A921A4C0C2C7}");
                Templates.MailTemplate.EnvelopeUserReminderForTenderClose = new ID("{A22B5B00-E02E-4136-9867-F097E80E2C77}");
                Templates.MailTemplate.CorrigendumCreate = new ID("{B661C8E4-9DB2-4F48-B09E-67B4B0AB9DA5}");
                Templates.MailTemplate.ContactUs = new ID("{D6C52163-01C2-480F-A069-AA5C1B9954F7}");
                Templates.MailTemplate.PQApproval = new ID("{FC176F5E-21B2-4030-B006-A01EC46124EE}");
                Templates.MailTemplate.PQRejection = new ID("{A30FE797-3166-4F81-8D44-784C3DED8B85}");
                Templates.MailTemplate.sendForPQApproval = new ID("{77DEC67C-80A2-4875-BA58-01BCDDFC6CAD}");
                Templates.MailTemplate.sendForTenderDocApproval = new ID("{36A4DF5E-D58D-4F0C-AC3E-34366B7387AC}");
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
                Templates.Tender.TenderLoginOld = new ID("{AFB6671F-10FF-4C67-A089-9ED599896DDC}");
                Templates.Tender.TenderLogin = new ID("{A2E4458F-CEB7-4780-B568-B8A0C3AA2FA3}");
                Templates.Tender.TenderList = new ID("{BA4916E3-3CEA-409C-A8B5-94E07EBA3268}");
                Templates.Tender.TenderDetails = new ID("{B30275A1-D1A8-4FAC-A285-5155A6AB7469}");
                Templates.Tender.AdminTenderListing = new ID("{B2214E48-E02A-4FBD-A3CA-F94B4E769962}");
                Templates.Tender.AdminUserTenderListing = new ID("{F098EC41-2192-4D53-83E7-511E2059B934}");
                Templates.Tender.AdminTenderDetail = new ID("{E04E4955-670D-4501-9C80-DC66812113FF}");
                Templates.Tender.AdminTenderUpdate = new ID("{7D0B559E-996A-4280-A80C-7491F9FB7571}");
                Templates.Tender.TenderCorrigendum = new ID("{89521608-322F-447B-BF10-FF3A9C44A657}");
                Templates.Tender.AdminTenderCreate = new ID("{CAA3E21F-DA19-41CF-8902-7560D16A3345}");
                Templates.Tender.EnvelopeUserCreate = new ID("{A41F80EA-2F32-45E3-960C-312A36AF8A4A}");
                Templates.Tender.EnvelopeUserListing = new ID("{5BF1FBB2-85F0-4E41-BDD3-D89667123C21}");
                Templates.Tender.CorrigendumTenderListing = new ID("{7B112242-9808-447D-8ED3-379427B23771}");
                Templates.Tender.CorrigendumTenderCreate = new ID("{AF53C563-99DA-4BFF-A993-63870CFB3765}");
                Templates.Tender.CorrigendumTenderUpdate = new ID("{9359BBAF-EB33-4845-A3CD-E433B614E9F4}");
                Templates.Tender.AdminUser = new ID("{A528FF43-6E21-4B0E-9EDC-5F5196C0AF3F}");
                Templates.Tender.AdminUserCreate = new ID("{938B7965-2832-4A1D-8E66-8547632BC369}");
            }
        }
    }
}