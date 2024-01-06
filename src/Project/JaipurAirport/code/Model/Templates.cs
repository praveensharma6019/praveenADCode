using Sitecore.Data;
using System;

namespace Sitecore.JaipurAirport.Website.Model
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
                Templates.MailTemplate.ID = new ID("{0C383773-52F6-463C-B8C1-AE374DE7D2FA}");
                Templates.MailTemplate.EnvelopeUser = new ID("{6BAAF4FD-31C8-45D4-89E3-A80709A7BE51}");
                Templates.MailTemplate.AdminUser = new ID("{F73F7F82-C155-42F3-83F6-35F52E03EB15}");
                Templates.MailTemplate.EnvelopeUserOnTenderClose = new ID("{E7392401-4996-4AC4-9DC2-DA2E6C7D12E3}");
                Templates.MailTemplate.EnvelopeUserReminderForTenderClose = new ID("{D302556F-7717-4F7D-B8E1-CBF7A94ABEC8}");
                Templates.MailTemplate.CorrigendumCreate = new ID("{101A86F6-FB53-4821-B053-E89DD2BABA57}");
                Templates.MailTemplate.ContactUs = new ID("{D6C52163-01C2-480F-A069-AA5C1B9954F7}");
                Templates.MailTemplate.PQApproval = new ID("{85E5218A-F81D-4709-8809-0EB83F310954}");
                Templates.MailTemplate.PQRejection = new ID("{9B887693-94F9-4128-8A49-10054E16EC94}");
                Templates.MailTemplate.sendForPQApproval = new ID("{93084872-3F82-4E32-9289-9B2ADCFAF3B6}");
                Templates.MailTemplate.sendForTenderDocApproval = new ID("{05F4C26D-C58C-4296-9DBB-80AD13E363A4}");
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
                Templates.Tender.TenderLogin = new ID("{4A4D9CBF-B9E4-4CF4-B3D5-F6EB877568CC}");
                Templates.Tender.TenderList = new ID("{7DD6DDF3-F4AC-4F86-AFC2-9E35117C7C46}");
                Templates.Tender.TenderDetails = new ID("{0576C83D-49E1-4BAE-9153-47C30DD90FB7}");
                Templates.Tender.AdminTenderListing = new ID("{AE4C6D0F-B1B9-4986-82F1-3D3D594FAF8B}");
                Templates.Tender.AdminUserTenderListing = new ID("{540DF1DB-D5E3-4BE1-A00E-C734A02500C1}");
                Templates.Tender.AdminTenderDetail = new ID("{25990C1D-7FCD-496B-A1EB-F486C8568734}");
                Templates.Tender.AdminTenderUpdate = new ID("{529AB105-BDC8-4083-8BA5-90E81FCDB22C}");
                Templates.Tender.TenderCorrigendum = new ID("{7DCB0790-28BB-4208-AF2B-FC651FC55E44}");
                Templates.Tender.AdminTenderCreate = new ID("{CAA3E21F-DA19-41CF-8902-7560D16A3345}");
                Templates.Tender.EnvelopeUserCreate = new ID("{CF0882C7-8103-4966-BF90-00590FE0A83F}");
                Templates.Tender.EnvelopeUserListing = new ID("{CBFF76A1-1A71-4AD0-A492-866590A93898}");
                Templates.Tender.CorrigendumTenderListing = new ID("{59A4B432-EF0D-47B4-A878-D12A8559637E}");
                Templates.Tender.CorrigendumTenderCreate = new ID("{69676ED4-4AF5-48EE-94E3-0234C9B05A72}");
                Templates.Tender.CorrigendumTenderUpdate = new ID("{60BC9FC0-E27A-4E21-B177-AF9611611081}");
                Templates.Tender.AdminUser = new ID("{331A4A14-70DD-4379-A4F9-8E265F183166}");
                Templates.Tender.AdminUserCreate = new ID("{CDCE6339-1BBB-4A17-90F3-957F33612806}");
            }
        }
    }
}