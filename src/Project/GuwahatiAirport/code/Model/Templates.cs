using Sitecore.Data;
using System;

namespace Sitecore.GuwahatiAirport.Website.Model
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
                Templates.MailTemplate.ID = new ID("{217811C7-0139-4EFF-8F0F-8CFD810BB609}");
                Templates.MailTemplate.EnvelopeUser = new ID("{FAF02DD9-5307-4697-A869-713BB8117D12}");
                Templates.MailTemplate.AdminUser = new ID("{166B45B9-8EC7-47A0-91E5-6204F3990349}");
                Templates.MailTemplate.EnvelopeUserOnTenderClose = new ID("{5761801C-77C0-4B2B-BE14-846EEBE66D30}");
                Templates.MailTemplate.EnvelopeUserReminderForTenderClose = new ID("{E5BCC5A1-3FCB-4838-80DE-C4B6FEC4BF78}");
                Templates.MailTemplate.CorrigendumCreate = new ID("{EDACB211-89B0-4935-9CBD-040755EB1117}");
                Templates.MailTemplate.ContactUs = new ID("{D6C52163-01C2-480F-A069-AA5C1B9954F7}");
                Templates.MailTemplate.PQApproval = new ID("{7D4EFEE3-02E0-45EC-94DB-46C148C9F6F7}");
                Templates.MailTemplate.PQRejection = new ID("{F0BB8618-BA72-4ABC-A5A9-C337503650A4}");
                Templates.MailTemplate.sendForPQApproval = new ID("{3A9C3F9E-ACE6-4005-A099-FD02105DF46A}");
                Templates.MailTemplate.sendForTenderDocApproval = new ID("{A4112531-0487-4184-8F8E-8400F0896A5D}");
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
                Templates.Tender.TenderLogin = new ID("{7A68790D-C9F0-4E9A-9960-4347E9BB6BD1}");
                Templates.Tender.TenderList = new ID("{2930EBA3-CA70-4E97-8701-BD81FDDC5E26}");
                Templates.Tender.TenderDetails = new ID("{0C47A6FD-8D96-4FA9-BCB5-778E36A24B78}");
                Templates.Tender.AdminTenderListing = new ID("{3B4E588A-1AB5-4695-8BF9-087CDC879A5A}");
                Templates.Tender.AdminUserTenderListing = new ID("{9C191F32-3BAE-4D60-9A3C-F90C3D87616B}");
                Templates.Tender.AdminTenderDetail = new ID("{B5779099-1F2D-458F-A5F4-80939EE62311}");
                Templates.Tender.AdminTenderUpdate = new ID("{6BC715CB-83BF-4CF8-9E24-3B1DD535C64B}");
                Templates.Tender.TenderCorrigendum = new ID("{B893234F-693F-4C82-B9B6-A930A2DFDBB6}");
                Templates.Tender.AdminTenderCreate = new ID("{CAA3E21F-DA19-41CF-8902-7560D16A3345}");
                Templates.Tender.EnvelopeUserCreate = new ID("{3A3DE28C-DA71-4A8D-BEB7-A3063828997F}");
                Templates.Tender.EnvelopeUserListing = new ID("{5345A0C5-FCBA-4599-8A6E-78D370EC411F}");
                Templates.Tender.CorrigendumTenderListing = new ID("{9C69FAAA-5285-417A-B9B1-D188A28934DB}");
                Templates.Tender.CorrigendumTenderCreate = new ID("{74D4A076-EBA9-40D5-911B-D069685F61C5}");
                Templates.Tender.CorrigendumTenderUpdate = new ID("{168EA4EA-8840-4895-9011-A65BD924ED44}");
                Templates.Tender.AdminUser = new ID("{E9386336-ED10-41A3-AC2B-A45E272B1AFB}");
                Templates.Tender.AdminUserCreate = new ID("{D0A9A676-2D96-42CA-8A90-D514921C8C1E}");
            }
        }
    }
}