using Sitecore.Data;
using System;

namespace Sitecore.Electricity.Website.Model
{
    [Serializable]
    public struct Templates
    {
        public struct ItemList
        {
            public static readonly ID MeterReadingItemList = new ID("{85593468-0842-4548-976C-E8EDC2CC03D1}");
        }

        public struct MeterReadingProperties
        {
            public static readonly ID CYCLE = new ID("{AA5B9814-94E4-407C-8F3C-C0733CA9B82B}");
            public static readonly ID BILLMONTH = new ID("{13BE92FE-CD21-429B-9087-FC69BD537F40}");
            public static readonly ID METERREADINGDATE = new ID("{A00BB8C1-0247-46D7-951A-50D1DFED6509}");
            public static readonly ID FULLMETERREADINGDATE = new ID("{C7260B34-2FCC-429E-BADC-6290834C8930}");
            public static readonly ID PROPOSEDDELIVERYDATE = new ID("{1A932655-7320-420F-9FF0-12848EB3A9DF}");
            public static readonly ID PROPOSEDDUEDATE_RESI = new ID("{07E0DECA-A490-4534-8B3E-97500759C217}");
            public static readonly ID PROPOSEDDUEDATE_COM = new ID("{DF874E79-07AC-48B6-BEFC-1C82706812FE}	");
        }

        public struct PaymentTransaction
        {
            public static readonly ID PaymentTransactioTemplateId = new ID("{9453CBE0-3F4E-4D0B-A7D8-E7CC389C86C8}");
        }

        public struct Datasource
        {
            public static readonly ID PaymentGateway = new ID("{809E9EB6-637F-4858-8DBA-5284F2C03133}");
            public static readonly ID PaymentMode = new ID("{90B1C345-0FF6-4C1B-A7F7-04D78E65B515}");
            public static readonly ID PaymentStatus = new ID("{79F22EF6-F41D-4C6C-8E62-34497EF2E892}");
            public static readonly ID EnvelopeType = new ID("{7AAED8AD-2FB4-4E71-B1C9-F18FEC51032C}");
        }

        public struct SwitchToGreen
        {
            public static readonly ID SwitchToGreen_ThankYouPage = new ID("{1A442D88-124D-4A44-9EDD-D85330461825}");
            public static readonly ID SwitchToGreen_EV_ThankYouPage = new ID("{2E7A6A67-2BAB-4DB4-AFDC-F5BDCD5F5F1C}");
            public static readonly ID SwitchToGreen_RequestPage = new ID("{C20B9118-5E33-4A89-8571-1D0DB736D3E8}");
        }

        public struct ITSR
        {
            public static readonly ID ITSRLogin = new ID("{8114B182-532F-400A-851C-63D55CC94750}");
            public static readonly ID ITSRCreateBidderForm = new ID("{D16D79A5-401A-4E73-9058-354021E355A4}");
            public static readonly ID ITSRUpdateBidderForm = new ID("{0B1B064D-8089-46CE-A04B-E55183B806A6}");
            public static readonly ID ITSRBidderFormListing = new ID("{B92DD7A2-4C27-4731-920B-F132EC02F31B}");
            public static readonly ID ITSRBidderFormSubmissionListing = new ID("{BC2A0C35-F07E-4D01-9747-67D203C7BA0D}");
            public static readonly ID ITSRBidderFormSubmission = new ID("{060788AC-D029-4F68-A7C4-BDEDF4B9ED16}");
            public static readonly ID ITSRBidderFormSubmissionAdminView = new ID("{F814900D-4ED1-4F26-B0AC-41DDCDEBC357}");

            public static readonly ID ManageUsersFormItem = new ID("{D3591238-864F-484D-9C24-DCC54D236B6A}");
            public static readonly ID CreateUsersFormItem = new ID("{162E6E4C-35B7-4D1A-958D-8FE16FD29C93}");
            public static readonly ID DeleteUsersFormItem = new ID("{98CCA670-786A-41E8-AE2C-6C5C54FE8042}");
            public static readonly ID UpdateUsersFormItem = new ID("{FBB7630E-09A5-44A5-9C27-22949AF8E86E}");
        }
        public struct Tender
        {

            public static readonly ID TenderCorrigendum = new ID("{F18AE4AA-6578-460F-B426-60CAB7DC26A8}");
            public static readonly ID TenderDetails = new ID("{45960EF0-4C7B-4900-B3C9-39C4B82591E3}");
            public static readonly ID AdminTenderListing = new ID("{9F1E0ED9-F38A-4C7C-9D8F-1B46114064D5}");
            public static readonly ID AdminUserTenderListing = new ID("{934CD15C-A90C-4888-AD48-10719C018E36}");
            public static readonly ID AdminTenderDetail = new ID("{50DE68FE-454B-4CD0-AF6F-C148D8D19639}");
            public static readonly ID AdminTenderCreate = new ID("{CAA3E21F-DA19-41CF-8902-7560D16A3345}");
            public static readonly ID AdminTenderUpdate = new ID("{24E680EB-D3B5-417F-978C-FE818C64A608}");
            public static readonly ID EnvelopeUserCreate = new ID("{774966CB-D901-468F-926A-69AFAD7D318A}");
            public static readonly ID EnvelopeUserListing = new ID("{C93BFD56-D125-4864-8E18-39ABE364CB17}");
            public static readonly ID CorrigendumTenderListing = new ID("{14986580-ACCB-4F85-AAC9-B040F12BE65D}");
            public static readonly ID CorrigendumTenderCreate = new ID("{F6C45377-AA23-475F-AC63-ACE2075A6DAE}");
            public static readonly ID CorrigendumTenderUpdate = new ID("{52A1932B-A34B-46D5-B861-982B10E7AB6C}");
            public static readonly ID TenderLogin = new ID("{FB10F927-F4B1-4A7A-B42D-BF89E5BA0183}");
        }

        public struct FeedbackModel
        {
            public static readonly ID LandingPage = new ID("{41767778-C426-4EA6-9BC9-C52E31592223}");
            public static readonly ID ThankYouPage = new ID("{DA206689-BFF9-4ECB-AD22-1709FEEA1B2B}");
        }

        public struct CONAndEncryptionSettings
        {
            public static readonly ID ID = new ID("{23BE9863-AD11-4B39-9E2D-39D20F819053}");

            public struct CONSettings
            {
                public static readonly ID ServiceCallUserID = new ID("{A531DC6A-BF62-40C2-A695-5612BAEA67B7}");
                public static readonly ID ServiceCallUserPassword = new ID("{93F419C8-C444-4032-B9BA-C1CB08E25C42}");

                public static readonly ID EncryptionKey = new ID("{D59D5BBF-0D29-4A52-87A0-0CD44111F1FD}");

                public static readonly ID StatusUpdateAPIUserId = new ID("{824F0500-5A27-4A74-BA31-950D8C56CD24}");
                public static readonly ID StatusUpdateAPIUserPassword = new ID("{E7CCFB14-AC11-4B1C-A756-DDD7A27EC21D}");
            }

            public struct SelfMeterReadingSettings
            {
                public static readonly ID EncryptionKey = new ID("{23B0D635-1418-449B-89B2-8F41D1E366E4}");
            }

            public struct EMISettings
            {
                public static readonly ID EncryptionKey = new ID("{AB5AE0AF-9BFB-4C2A-AAB7-321192F7A109}");
            }

            public struct DownloadBillSettings
            {
                public static readonly ID EncryptionKey = new ID("{BB0AEE6B-07F0-4DD9-977F-DC1FAF85556F}");
            }

            
        }
    }
}