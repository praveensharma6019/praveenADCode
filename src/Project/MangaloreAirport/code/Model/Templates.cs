using Sitecore.Data;
using System;

namespace Sitecore.MangaloreAirport.Website.Model
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
                Templates.MailTemplate.ID = new ID("{C97A2F5A-4CBF-45F0-9DC1-E00E4FE4C71B}");
                Templates.MailTemplate.EnvelopeUser = new ID("{BF2CC482-4E1F-4D6E-99DB-B58852FA0F78}");
                Templates.MailTemplate.AdminUser = new ID("{E75EA0BB-AB7E-433F-95D0-D3E019157B56}");
                Templates.MailTemplate.EnvelopeUserOnTenderClose = new ID("{D3FC6EFE-FF7C-436D-A64C-712E80688138}");
                Templates.MailTemplate.EnvelopeUserReminderForTenderClose = new ID("{3858400B-E15F-4251-A8D2-44C10248A1DF}");
                Templates.MailTemplate.CorrigendumCreate = new ID("{687EDDAD-A225-4886-807E-C22B88CBEA9D}");
                Templates.MailTemplate.ContactUs = new ID("{D6C52163-01C2-480F-A069-AA5C1B9954F7}");
                Templates.MailTemplate.PQApproval = new ID("{1A9CF79F-8793-4F37-8322-AB4B98461D66}");
                Templates.MailTemplate.PQRejection = new ID("{F9660982-371A-43CE-93D4-47D4444AA679}");
                Templates.MailTemplate.sendForPQApproval = new ID("{C711909E-0FBF-4CE4-893F-D4DE8029FE16}");
                Templates.MailTemplate.sendForTenderDocApproval = new ID("{F9132BAC-6265-4B79-8ED6-EC11786E70B5}");
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
            public readonly static ID TenderLoginOld;

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
                Templates.Tender.TenderLoginOld = new ID("{6ACDF431-0AF0-4BEA-A85E-9B7CDE8A9CE8}");
                Templates.Tender.TenderLogin = new ID("{5D57DEE8-D543-4017-A136-2BB32D6FD235}");
                Templates.Tender.TenderList = new ID("{4CC04F7B-7C05-4B98-A1A5-D2B4482F8505}");
                Templates.Tender.TenderDetails = new ID("{9646F91C-37C5-48E1-A347-BDF477861712}");
                Templates.Tender.AdminTenderListing = new ID("{82018BE6-6BC2-4287-A43C-9B62D8316A49}");
                Templates.Tender.AdminUserTenderListing = new ID("{7C637474-B3A3-4744-AF2A-C808D2AEDEDE}");
                Templates.Tender.AdminTenderDetail = new ID("{67886758-66E3-441E-86D1-927E67120664}");
                Templates.Tender.AdminTenderUpdate = new ID("{E9BD4F02-E126-458D-B1CF-01D96C3D8B12}");
                Templates.Tender.TenderCorrigendum = new ID("{C180FF61-8F3E-456B-BB6B-EAF3BF3C2B44}");
                Templates.Tender.AdminTenderCreate = new ID("{CAA3E21F-DA19-41CF-8902-7560D16A3345}");
                Templates.Tender.EnvelopeUserCreate = new ID("{025FE05E-6E92-4E4A-AAA6-69F2DE85E9E2}");
                Templates.Tender.EnvelopeUserListing = new ID("{EB7258A2-1688-4CD7-BEC3-108910D71950}");
                Templates.Tender.CorrigendumTenderListing = new ID("{423A3226-449A-46AF-9DE2-7D1EA89752F5}");
                Templates.Tender.CorrigendumTenderCreate = new ID("{B837CA4E-6E97-4AB5-A122-1A0B6B165A67}");
                Templates.Tender.CorrigendumTenderUpdate = new ID("{144508BF-A094-4781-9038-CB0E500622F2}");
                Templates.Tender.AdminUser = new ID("{22942302-0120-4C3B-90A0-7459895F8949}");
                Templates.Tender.AdminUserCreate = new ID("{9BD21AEC-D83C-4842-9788-C238D27C1AD8}");
            }
        }
    }
}