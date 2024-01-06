namespace Sitecore.Feature.Accounts
{
    using Sitecore.Data;

    public struct Templates
    {


        public struct Services
        {
            public static readonly ID ID = new ID("{3AF60F7A-C1F5-40E8-A702-DFE88FD59BBC}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{B75CD92D-9492-45DC-B721-77A275971B6A}");

                public static readonly ID ShortTitle = new ID("{2505F2EE-819E-4888-8EA5-15DB4F3156E8}");

                public static readonly ID CssClass = new ID("{90D2E48D-8A40-445F-BC56-9D550DD032B1}");

                public static readonly ID Description = new ID("{99A353AE-36A2-4776-92E4-5E6EA8428E40}");
                public static readonly ID Image = new ID("{898EBA1E-01CB-4970-9E8A-BAEB5EB6C549}");
                public static readonly ID IconCss = new ID("{9394CE4F-9E2F-4BD2-AE87-E07B13415F81}");
                public static readonly ID CTAText = new ID("{847D6120-C3C4-4AC6-8DB5-21AA945B04D6}");
                public static readonly ID CTALink = new ID("{2BDD1A5E-329E-478E-8E91-1BF6B500BBBD}");
            }
        }

        public struct SubmitMeterReadingQA
        {
            public static readonly ID ID = new ID("{D4022313-C064-4590-81F1-97C540D3EB8A}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{09302A83-3B07-4BD1-A9A0-E9736050BD80}");
                public static readonly ID Description = new ID("{018213CC-58C2-474A-ABEB-3E414583F6F6}");
                public static readonly ID ListingProducts = new ID("{93193722-1E33-4DCA-A611-BF0110E1D450}");
            }
        }


        public struct Offers
        {
            public static readonly ID ID = new ID("{2930933A-2F75-41F7-A48E-CB07CCC1A811}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{6B3DEEF9-7B5B-4410-A035-4748BE3294B9}");
                public static readonly ID Description = new ID("{D841F758-F231-4405-82DA-5FC7FC26AC82}");
                public static readonly ID ListingProducts = new ID("{F42853FF-3175-4978-B4FD-5A91E8282F8E}");
            }
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
        public struct AdaniGas
        {
            public static readonly ID DealerDetails = new ID("{5BBA2CDC-BB48-4A64-A506-E3381620B7F1}");
            public static readonly ID NameTransfer = new ID("{7AC56B06-80C8-48F5-9D72-64989578BE77}");
        }
        public struct LECPortal
        {
            public static readonly ID LECPortalHomePage = new ID("{0ED8F553-ED70-4AA5-95E0-B8CDFDD517A1}");
            public static readonly ID LECUserLoginPage = new ID("{CE7E0535-B714-4AC5-BAFF-6BBAF7EBE772}");
            public static readonly ID LECUserRegistrationPage = new ID("{B13BAD09-D324-4B60-8C4C-B3AA1D48E21E}");

            public static readonly ID LECUserProfilePage = new ID("{9E78B1CE-D00A-4E00-9957-2CA4AD175610}");
            public static readonly ID LECChangePasswordPage = new ID("{4F25B727-3BB2-4AF4-8454-DE8ACFD16214}");
            public static readonly ID LECDeregisterPage = new ID("{C2158AC3-3AB0-4865-BDA8-7D2B4C95EA50}");
            public static readonly ID ChangeOfNameLECRegistrationPage = new ID("{CA00831C-34BA-46EB-B094-D7FB9B7B8ED6}");
            public static readonly ID ChangeOfNameLECSubmittedApplicationPage = new ID("{3A493D98-4DD6-4E61-A8EE-6987291E2970}");
            public static readonly ID NewConLECRegistrationPage = new ID("{3C98C3C0-AE76-4736-BC08-9F9B989700A9}");
            public static readonly ID NewConLECSubmittedApplicationPage = new ID("{D3BCFD7F-E4C6-44F6-A9C7-5F8F9409FB3D}");
            public static readonly ID ChangeOfNameLECApplicationFormPage = new ID("{52264A1C-D0D6-4386-95E0-EE5DF05C3E96}");
            public static readonly ID ChangeOfNameLECViewApplicationPage = new ID("{C0498B39-3499-47EF-8D36-B3FDF04E69CC}");
            public static readonly ID ChangeOfNameLECStatusCheckPage = new ID("{177B908E-DB5C-4747-A1E6-44962D1133A4}");
            public static readonly ID LECTestReport = new ID("{CD919C8D-C4AD-4149-90E2-8E86B89C4873}");
        }
        public struct NewConnection
        {
            public static readonly ID LoginPage = new ID("{021AAE51-4338-4436-9D94-1F2A1CB50755}");
            public static readonly ID NewConnectionHomePage = new ID("{BA25BAEB-1D37-46A9-897B-AFC12DC7570A}");
            public static readonly ID IndividualSupplyLoadTwenty = new ID("{45879211-3036-48BA-9426-799635B9988F}");
            public static readonly ID GreenTariffList = new ID("{F81C2069-F681-4BF8-A190-7FC469CD982C}");
            public static readonly ID IndividualSupplyTemporailySupply = new ID("{BA25BAEB-1D37-46A9-897B-AFC12DC7570A}");
            public static readonly ID IndividualSupplyLoadHundred = new ID("{BA25BAEB-1D37-46A9-897B-AFC12DC7570A}");
            public static readonly ID BuildingProjectsNetwordRecommnedation = new ID("{BA25BAEB-1D37-46A9-897B-AFC12DC7570A}");
            public static readonly ID BuildingProjectsConstructingPower = new ID("{BA25BAEB-1D37-46A9-897B-AFC12DC7570A}");
            public static readonly ID BuildingProjectsNewBuilding = new ID("{BA25BAEB-1D37-46A9-897B-AFC12DC7570A}");
            public static readonly ID BuildingProjectsNewBuildingPermanentSupply = new ID("{BA25BAEB-1D37-46A9-897B-AFC12DC7570A}");
            public static readonly ID LoadRevision = new ID("{BA25BAEB-1D37-46A9-897B-AFC12DC7570A}");
            public static readonly ID ContractDemandRevision = new ID("{BA25BAEB-1D37-46A9-897B-AFC12DC7570A}");
            public static readonly ID MeterShifting = new ID("{BA25BAEB-1D37-46A9-897B-AFC12DC7570A}");
            public static readonly ID ApplicationList = new ID("{09946EB0-97EF-44EF-8CB9-56B129FCD5A5}");
            public static readonly ID EmployeePortalLogin = new ID("{1358389D-2942-4BA6-BE90-356427FD5989}");
            public static readonly ID EmployeePortalApprovalPage = new ID("{5D73C974-7040-4842-A2B9-C84B6A656A6E}");
            public static readonly ID EmployeePortalRegistrationPage = new ID("{2C9D9A73-48D7-4C3F-9383-940D595B2ECA}");
            public static readonly ID CheckApplicationStatus = new ID("{3FA6BD14-2885-4BA1-B425-E993039798A2}");
            public static readonly ID EPPPortalregistrationPage = new ID("{6F7B80C0-E1E4-4536-A48D-FAFA9CA4D3AC}");
            public static readonly ID EmployeePortal = new ID("{8B2DE716-20AF-499A-B22C-9DBEBA202AE9}");
            public static readonly ID ActionEPPList = new ID("{1716ADAF-A5A0-4B4B-AF40-A90D86E70D1B}");
            public static readonly ID NormalEPPList = new ID("{E23BF207-75A9-417D-B536-3F1B473C23F1}");
            public static readonly ID AdminRegList = new ID("{54D94AAE-B6F5-42DD-8B59-0A49BF123FD6}");
            public static readonly ID HistoryList = new ID("{4BA1C4F9-C6CB-4D2F-B37C-D69459AA0E87}");
            public static readonly ID LoadCalculator = new ID("{345F8592-4392-4AAF-93BE-BA1A23103FD9}");
            public static readonly ID AdminCreateEmployee = new ID("{6431782E-7855-49B0-8A76-359150B69301}");
            public static readonly ID EmployeePortalForgotPassword = new ID("{EA369007-13CE-4EF4-90B0-FD2A23869D7A}");


        }

        public struct ComplaintPortal
        {
            public static readonly ID ComplaintPortalLoginPage = new ID("{3779ABDF-6CED-4387-9ED9-77743EFCFA77}");
            public static readonly ID ComplaintPortalLoginPageRevamp = new ID("{4ED06227-1990-4E10-8920-41892B9BAF97}");
            public static readonly ID ComplaintPortalHomePage = new ID("{3EAC5001-F309-4AC3-AF93-1511A0603726}");
            public static readonly ID ComplaintPortalHomePageRevamp = new ID("{912BF775-47B7-44D9-99CC-1B3B71D1B1CC}");
            public static readonly ID ComplaintPortalHelpdeskHomePage = new ID("{B22F6956-2CA7-42BC-9A88-CBAA98B75CCA}");
            public static readonly ID ComplaintPortalCGRFHomePage = new ID("{6B782DFD-5726-4688-8BDD-C413E8E0F5FE}");

            public static readonly ID ComplaintPortalFileComplaintPage = new ID("{58DFADFD-0BFB-44CA-A4F4-CB6CC3577FAA}");
            public static readonly ID ComplaintPortalFileComplaintPageRevamp = new ID("{027C011D-DDAA-4B8A-AA5E-0C96E93E499D}");
            public static readonly ID ComplaintPortalFileComplaint2Page = new ID("{C50A970B-0F1F-40BD-A6F8-F9FEBB21D4C0}");
            public static readonly ID ComplaintPortalFileComplaint3Page = new ID("{033D37C2-9E46-4D4B-A7F2-197504B70E21}");

            public static readonly ID ComplaintPortalTrackComplaintPage = new ID("{1E8A099D-86D4-4D2D-8A3C-75D7CC598F47}");
            public static readonly ID ComplaintPortalSubmitSavedComplaintPage = new ID("{6C80E25A-A086-4300-9961-382E28196791}");
            public static readonly ID ComplaintPortalSubmitFeedbackPage = new ID("{F3D4F730-8F85-477C-8100-102FB3B672E4}");

            public static readonly ID ComplaintPortalFileCGRFComplaintPage = new ID("{B169CCC7-870F-4031-9474-637AFD10C5A0}");
            public static readonly ID ComplaintPortalFileCGRFComplaintPageRevamp = new ID("{31BAD8E9-0AD5-4B72-BA11-29988ED1AD56}");
            public static readonly ID ComplaintPortalTrackCGRFComplaintPage = new ID("{EABE417B-5887-4F46-8A9F-77DFCEB26FA0}");
            public static readonly ID ComplaintPortalTrackCGRFComplaintPageRevamp = new ID("{9854FD8F-3DCE-4FC4-8F1C-07D62E536035}");

            //admin pages
            public static readonly ID ComplaintPortalAdminHomePage = new ID("{B78C55EA-BB88-4199-84A3-86D32F3ABBE8}");
            public static readonly ID ComplaintPortalAdminReportsPage = new ID("{58FC1CD8-3008-40E0-9C3C-EBF1F91078B0}");
            public static readonly ID ComplaintPortalAdminFileCGRFComplaintPage = new ID("{BFF7AEE1-C0F0-4FC8-87F2-0A4E09F1CF94}");

            public static readonly ID ComplaintPortalAdminHomePageRevamp = new ID("{316BFA02-0859-4B3B-94B5-AF8DCE831811}");
            public static readonly ID ComplaintPortalAdminReportsPageRevamp = new ID("{95FBBE43-5FAE-422D-9EF6-ADEFCE459B74}");
            public static readonly ID ComplaintPortalAdminFileCGRFComplaintPageRevamp = new ID("{E0FD447F-0D13-421F-854D-C3C68CF8A734}");

            public static readonly ID ComplaintPortalICRSAdminHomePage = new ID("{D00AA8B4-65E4-4B79-94DC-34335694C9D4}");
            public static readonly ID ComplaintPortalICRSAdminHomePageRevamp = new ID("{579F17AA-7FCA-4A45-AE49-258786C70821}");
        }

        public struct AccountsSettings
        {

            public static readonly ID ID = new ID("{59D216D1-035C-4497-97B4-E3C5E9F1C06B}");

            public struct Fields
            {

                public static readonly ID AccountsDetailsPage = new ID("{ED71D374-8C33-4561-991D-77482AE01330}");
                public static readonly ID RegisterPage = new ID("{71962360-10D8-4B98-BB8D-57660CE11127}");
                public static readonly ID LoginPage = new ID("{60745023-FFD5-400E-8F80-4BCA9F2ABB29}");
                public static readonly ID ForgotPasswordPage = new ID("{F3CD2BB8-472B-4DF0-87C0-A13098E391CA}");
                public static readonly ID AfterLoginPage = new ID("{B128E2B3-3865-4F1C-A147-5F248676D3F5}");
                public static readonly ID AfterPVCLoginPage = new ID("{BD2A4408-3190-4717-95CD-8F04ABBCD856}");
                public static readonly ID AfterLogoutPage = new ID("{0F11C57E-A410-448F-9CCF-C1779A285B55}");
                public static readonly ID ForgotPasswordMailTemplate = new ID("{365254C4-1C1C-493A-9710-671574717898}");
                public static readonly ID RegisterOutcome = new ID("{835FA523-D28A-46A2-A589-6AA4A5BF0846}");
                public static readonly ID ResetPassword = new ID("{6019A92C-50DF-47D8-9939-1F2CFBC6E961}");
                public static readonly ID QuickPayBillPage = new ID("{91CF9016-CA0D-41E1-8295-0DE4CD2CE064}");
                public static readonly ID RegistrationThankYouPage = new ID("{31AF85FC-46F3-4BC7-851B-01C96BC1758B}");
                public static readonly ID RegistrationPageAfterValidate = new ID("{65A6C20D-C8A7-4A27-909B-78A9687B7DBE}");
                public static readonly ID RegistrationRevampPageAfterValidate = new ID("{2D389B09-DFD2-429C-A35E-CD16DFEDE929}");
                public static readonly ID RegistrationPage = new ID("{C8FF7DCE-0093-48E6-A4F2-A8C21990CAB2}");
                public static readonly ID RegistrationPageRevamp = new ID("{4BB176DF-85FA-49F8-A1B6-42D1E96CB72B}");
                public static readonly ID AfterLoginPageComplaintPortal = new ID("{38514B57-AD6C-465E-97F8-7CB53F2DEA58}");
                public static readonly ID LoginPageComplaintPortal = new ID("{B2A91565-32A3-4F41-8077-A6CDE92DB146}");

                public static readonly ID ChangeOfNameRegistrationPage = new ID("{B23A6925-D15D-4DA6-A540-3E0DF8AEF2E7}");
                public static readonly ID ChangeOfNameRegistrationPageRevamp = new ID("{D720BAA0-6352-4E8A-9E83-B3CBF88C1D4D}");
                public static readonly ID ChangeOfNameSubmittedApplicationPage = new ID("{DD829423-2EF1-45EA-9645-29BFBCC8E5B0}");
                public static readonly ID ChangeOfNameSubmittedApplicationPageRevamp = new ID("{01C260FB-63F5-4EC7-9543-6463416C55B8}");
                public static readonly ID ChangeOfNameApplicationFormPage = new ID("{9A5FADF7-F845-4C36-950C-840F83FD9F6C}");
                public static readonly ID ChangeOfNameApplicationFormPageRevamp = new ID("{9CC42766-76BE-461E-AC99-BA1ECCDCCE18}");

                public static readonly ID ChangeOfNameLECRegistrationPage = new ID("{8E4B0A8F-7738-4794-84C9-2F86B7F16FD4}");
                //public static readonly ID ChangeOfNameLECSubmittedApplicationPage = new ID("{8115B668-FD1E-4FEE-BAEC-B5026E719BD4}");
                //public static readonly ID ChangeOfNameLECApplicationFormPage = new ID("{C5C316C9-BB17-4E65-9332-D2E6035BE7D3}");

                public static readonly ID ENACHRegistrationPageRevamp = new ID("{22A4EAFB-AEEC-421A-86B2-332D1D6202A5}");
                public static readonly ID ENACHRegistrationPage = new ID("{B27E9C31-EFD1-4BA5-B70C-7262321926EB}");
                public static readonly ID CNGDealerLoginPage = new ID("{7DE8BAB0-2327-4454-83D6-2DB03166D6EA}");
                public static readonly ID CNGDealerAfterLoginPage = new ID("{489FF3BF-68C7-4903-B3AE-C1F7CDBB3110}");
                public static readonly ID CNGAdminUserLoginPage = new ID("{A3637AAE-EFE8-4F0A-B4D7-C0F9E73B4CD1}");
                public static readonly ID CNGAdminUserAfterLoginPage = new ID("{6289F1B3-DC57-4957-B212-45CC0907829A}");

            }
        }

        public struct MailBox
        {
            public static readonly ID OutBoxItemID = new ID("{F0616DBB-7551-4318-B3A5-1F96563A538F}");
            public static readonly ID InboxItemID = new ID("{2ECA1197-F0E5-4DF2-83E4-530659B93314}");

            public struct ComposeItemFields
            {
                public static readonly ID MailId = new ID("{8017F52C-BFE2-4836-83B5-BC1807201468}");

                public static readonly ID FromEmail = new ID("{6A9FDF01-341A-4785-8936-10627949C967}");

                public static readonly ID Subject = new ID("{4358D29C-614E-48C5-B793-62A06302B0F7}");
                public static readonly ID Body = new ID("{844F5079-839D-473B-8ED2-58E3615C0343}");
                public static readonly ID Attachment = new ID("{AB669A31-021F-4F46-8801-ECB72FBB8EF7}");
                public static readonly ID Ondate = new ID("{3DA0F09F-C73C-499A-9645-279967309C6C}");

                public static readonly ID IsDelete = new ID("{EA07AB9F-BF38-4A10-A565-B3DA5B2B7C77}");
                public static readonly ID IsPvc = new ID("{5EE7FD1B-06C1-4389-BF25-89615CD3B646}");
                public static readonly ID IsNonPvc = new ID("{10E71FB3-67F4-4CD8-B395-9EB72B2AD656}");
                public static readonly ID IsTrash = new ID("{BFA40B28-6249-4106-8AA0-9F681B69C790}");
                public static readonly ID UserId = new ID("{A1B436DE-4A65-4E98-ADFA-00174669C71E}");
            }
        }


        public struct MailTemplate
        {
            public static readonly ID ID = new ID("{26DF8F38-7E1B-43D2-85DD-68DF05FA276B}");
            public static readonly ID OTPMailLECPortal = new ID("{E9CE4EF2-142A-426D-8E0B-887AA6C907B8}");
            public static readonly ID OTPLoginMailLECPortal = new ID("{DB089142-781A-4926-9E0E-46E58E77E0EB}");

            public static readonly ID ComplaintSubmitMailComplaintPortal = new ID("{C35C44E5-43AB-461B-94F4-3A2B876DD24E}");
            public static readonly ID GreenPowerOptInEmail = new ID("{FCFDC23A-0538-40D0-9DD6-F4A60201175B}");
            public static readonly ID EODBMailToConsumer = new ID("{F9ECDEDC-6BBC-486D-B3CB-240AABFCA2EC}");

            public static readonly ID LECNewConUserPasswordMail = new ID("{5CAC0D06-88EC-49F5-8120-BA85C7EA0D47}");
            public static readonly ID LECNewConUserReviewMail = new ID("{89E47E72-AAAA-453F-BAAD-DED20128730B}");
            public static readonly ID LECNewConHODApprovalMail = new ID("{7ECA2855-C8D5-4E6D-B755-8604608814FD}");
            public static readonly ID EmployeePortalReviewMail = new ID("{E8CD5245-3D8F-45E5-BFC4-3D043E0F5C83}");
            public static readonly ID EmployeePortalApprovalMail = new ID("{E199728B-A28E-4C6B-BB21-EB6C91E5295D}");
            public static readonly ID EmployeePortalRegistrationMail = new ID("{04BB123E-7C64-43DA-8FE1-A41D5D094A8B}");
            public static readonly ID EmployeePortalForgotPassword = new ID("{EB280263-AD0E-4D34-BD6B-352E9F0BD740}");

            public static readonly ID UserPortalRegistrationMail = new ID("{51B41623-500E-49B5-9673-35B08DD6ECBF}");
            public static readonly ID ITDeclarationType1Email = new ID("{E4AB9502-EC2C-4EBB-9813-6F2865C89324}");
            public static readonly ID ITDeclarationType2Email = new ID("{C732AC8D-D317-4581-9848-1636F1D81590}");
            public static readonly ID ITDeclarationType3Email = new ID("{6E08A5F9-2850-48C3-BFCF-A77E35D8FD58}");

            public static readonly ID ComplaintApprovalMailComplaintPortal = new ID("{F462C6D0-7C1C-4BB9-B766-EA299E4F12A4}");
            public static readonly ID ComplaintApprovalMailToMembersComplaintPortal = new ID("{57812132-3F03-48AE-8A02-1611373182DF}");
            public static readonly ID ComplaintApprovalMailToNodalOfficerComplaintPortal = new ID("{BE092288-D000-4AFC-A7F1-AB26051F4210}");

            public static readonly ID ComplaintReviewMailComplaintPortal = new ID("{6F775766-DB10-4A05-9C25-3895F930C4E3}");
            public static readonly ID ComplaintReviewMailToMembersComplaintPortal = new ID("{936F6810-236B-466C-88A7-C4BFAA09F2BD}");
            public static readonly ID ComplaintReviewMailToSecretaryComplaintPortal = new ID("{936F6810-236B-466C-88A7-C4BFAA09F2BD}");

            public static readonly ID ComplaintSubmitMailComplaintPortalCGRF = new ID("{D7131B17-6C1E-403B-81FD-6FEEC0CEF537}");
            public static readonly ID ComplaintSubmitMailToSecretaryComplaintPortal = new ID("{F6393F6E-239D-487A-996E-683D73B930E3}");
            public static readonly ID ComplaintForwardMailComplaintPortalCGRF = new ID("{386EF585-193A-40CA-803B-EA7681FB75AB}");

            public static readonly ID ComplaintNodalReplyMailComplaintPortalCGRF = new ID("{4E3A8C26-C7D5-4F06-95BF-9C66A8BFEE0F}");
            public static readonly ID ComplaintNodalReplyMailToSecretaryComplaintPortalCGRF = new ID("{03924C25-858D-4BB0-9CF8-0E98F65AB68B}");

            public static readonly ID ComplaintRejoinderMailToSecretaryComplaintPortalCGRF = new ID("{17E5EA17-439F-4CC0-9638-7CB9A4F59C99}");

            public static readonly ID ComplaintHearingScheduledMailComplaintPortalCGRF = new ID("{2DB391DC-2E5A-45E6-8A2A-368658A8767C}");
            public static readonly ID ComplaintClosedMailComplaintPortalCGRF = new ID("{5AB67DC9-0E3E-48D5-A045-683295AAD8F8}");

            public static readonly ID ComplaintAskToReSubmitMailComplaintPortal = new ID("{149B2876-6332-49E0-8CA0-EFA41C328028}");
            public static readonly ID ComplaintAskToReSubmitMailToSecretaryComplaintPortal = new ID("{FEAB4205-D52E-4C51-88CC-870336B432D2}");

            public static readonly ID ComplaintReSubmitMailComplaintPortal = new ID("{3FB89629-0808-431E-8697-5A6C60C7BF27}");
            public static readonly ID ComplaintReSubmitMailToSecretaryComplaintPortal = new ID("{FEAB4205-D52E-4C51-88CC-870336B432D2}");
            public static readonly ID NameTransferCreateAdmin = new ID("{56F1123A-7721-429D-838C-EC4E2E12621B}");
            public static readonly ID ApproveNameTransferApplication = new ID("{B23B1BD4-9D26-4488-A944-3AD83373CF98}");
            public static readonly ID RejectNameTransferApplication = new ID("{1E44D49D-276F-4631-BC69-F1990F8DF453}");
            public static readonly ID AskForAdditionalDetailsAndDocumentNameTransferApplication = new ID("{C5FB5A78-9801-455F-A700-131AFA9DA47B}");
            public static readonly ID NameTransferUpdateAdminDetails = new ID("{D89E7D45-2BFB-4AC7-B313-9286FBA65758}");
            public static readonly ID NameTransferStatusDetails = new ID("{4D535F52-12F5-4B11-AF8B-AEA1EB5039CC}");
            public static readonly ID NameTransferAdminForgotPassword = new ID("{534FA962-42C1-435D-8BFC-AD82D84D8602}");
            public static readonly ID NameTransferChangePasswordAdmin = new ID("{233D189D-1F3D-406F-92F2-D138329A7867}");
            public static readonly ID SMSNameTransferRequest = new ID("{28912185-CFAA-45E4-B483-5DFF19B800CB}");
            public static readonly ID SMSNameTransferApplicationRejected = new ID("{5D903C65-13C9-4271-A8D7-821E28F37FDF}");
            public static readonly ID SMSNameTransferApplicationAdditionalDocRequired = new ID("{F60DAEF4-E75A-4B25-A5EC-6F7908F9EFD9}");
            public static readonly ID SMSNameTransferApplicationSuccessfulClose = new ID("{0D6AA4DF-313E-4101-B330-C9FAA15669D1}");
            public static readonly ID SMSNameTransferApplicationApplicationOnHold = new ID("{12564AE5-C8C1-466A-AFF4-5F871F51460C}");

            public struct Fields
            {
                public static readonly ID From = new ID("{8605948C-60FB-46B8-8AAA-4C52561B53BC}");
                public static readonly ID Subject = new ID("{0F45DF05-546F-462D-97C0-BA4FB2B02564}");
                public static readonly ID Body = new ID("{1519CCAD-ED26-4F60-82CA-22079AF44D16}");
            }
        }

        public struct Interest
        {
            public static readonly ID ID = new ID("{C9B1855E-CA80-4414-B5BA-956CB67DC5A9}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{1FBE5200-2C62-4A32-BA84-CFFE3CF665D3}");
            }
        }

        public struct ProfileSettigs
        {
            public static readonly ID ID = new ID("{2D9AA1E4-3359-4F02-9EAA-5CF972FD990D}");

            public struct Fields
            {
                public static readonly ID UserProfile = new ID("{378B7D87-5775-4EB6-86B7-282D5359B1C6}");
                public static readonly ID InterestsFolder = new ID("{021AA3F7-206F-4ACC-9538-F6D7FE86B168}");
            }
        }

        public struct UserProfile
        {
            public static readonly ID ID = new ID("{696D276B-786A-4D1E-B8BB-8E139DB7BE7C}");

            public struct Fields
            {
                public static readonly ID FirstName = new ID("{E7BC8A3E-3201-4556-B2FF-FF4DB04DB081}");
                public static readonly ID LastName = new ID("{EE21278F-4F83-4A10-8890-66B957F3D312}");
                public static readonly ID PhoneNumber = new ID("{F7A1605F-7BBB-4BC7-BBB4-9E0546648E1D}");
                public static readonly ID Interest = new ID("{A5D0B0AD-CE4E-4E06-B821-F30416B7DEC9}");
            }
        }

        public struct LoginTeaser
        {
            public static readonly ID ID = new ID("{EF38D9E6-313C-491C-8648-79436D10091C}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{D9843186-ED10-47D8-8CC7-511AC670B6B4}");
                public static readonly ID Summary = new ID("{FFAB401B-8D7C-4172-A82A-AE32B7D2C6C1}");
                public static readonly ID LoggedInTitle = new ID("{39AABCF6-426C-4AD1-8B3D-A5135F219455}");
                public static readonly ID LoggedInSummary = new ID("{76183BC1-755C-428B-A35A-1425309990BE}");
            }
        }

        public struct Pages
        {
            public static readonly ID SwitchAccount = new ID("{1ADBE374-0F7F-42AF-B7E6-ED5D1D3FEC24}");
            public static readonly ID DeRegisterAccount = new ID("{F9988E94-02D7-4773-8C97-E03313130828}");
            public static readonly ID ForgotPassword = new ID("{A039B75E-F780-4EBD-A736-5B07FC521846}");
            public static readonly ID ResetPassword = new ID("{31A7014F-ED88-4307-B683-C6ABF05D95E8}");
            public static readonly ID MailDetail = new ID("{B7ACC50D-366A-48AD-B919-EA00E397BE31}");
            public static readonly ID GuestUserMeterReadingDate = new ID("{682E4795-0CC8-427B-9D55-EDF4AEE04251}");
            public static readonly ID PaymentSuccess = new ID("{E7154A16-C19A-4814-9D75-67EB0B9BF886}");
            public static readonly ID PaymentSuccessRevamp = new ID("{BC377F56-595C-4023-A99C-FE89C335AF35}");
            public static readonly ID PaymentPending = new ID("{9CFC1416-E202-4C57-A010-E602DE9052CA}");
            public static readonly ID PaymentPendingRevamp = new ID("{BD383625-9759-42AD-829B-27A2B6A77C0F}");
            public static readonly ID PaymentFailure = new ID("{F6BE84B3-30A7-44F6-B21C-03F9D6D20FEA}");
            public static readonly ID PaymentFailureRevamp = new ID("{958A96DA-0396-4108-A37C-84BF0BF19FA9}");
            public static readonly ID PaymentVDS = new ID("{35809AED-3C9D-44D7-806D-84FC35420874}");
            public static readonly ID GasForgotPassword = new ID("{4F3A4B9C-9CA4-4B0E-AC85-DCE64BA2F986}");
            public static readonly ID GasCngLocate = new ID("{0E379138-B43C-45D0-968D-EE45BD880A3E}");
            public static readonly ID Selfbilling = new ID("{43A988EF-C41D-49DD-A139-B885E74E8EFD}");
			public static readonly ID ResetPasswordRevamp = new ID("{0D736B61-9684-4C16-8D8B-E521A39E54DB}");
            public static readonly ID ForgotPasswordRevamp = new ID("{BB64A512-D34C-421B-B1E8-A369E2CFFD8B}");
            public static readonly ID ManageConnectionsRevamp = new ID("{755E7DB7-B360-446B-860D-19A03A5497AB}");
            public static readonly ID Submityourquery = new ID("{D97E2873-33E3-4272-BC44-A7F2A09D0982}");
        }

        public struct AdaniGasPages
        {
            public static readonly ID PaymentSuccess = new ID("{F6416D83-D878-4EC0-8B5E-C8291CD8E595}");
            public static readonly ID PaymentFailure = new ID("{EF16D5B6-51F4-4E5B-A264-479473F8FECD}");
            public static readonly ID RegistrationThankU = new ID("{2082929F-DD48-43B9-AE62-11C101278C53}");
            public static readonly ID ChangePasswordThankU = new ID("{F2B455D9-567C-4E0E-9C77-9E9A1151F5E0}");
            public static readonly ID NewInquiryCNG = new ID("{DC3217DA-3244-4543-BD0B-83EFF1FCECBB}");
            public static readonly ID NewEnquiry = new ID("{AE1082C5-1821-499D-8C05-1ABD504E4BA2}");
            public static readonly ID ENachRegistrationSuccess = new ID("{B39F7DA7-2575-49D3-A5BB-5ACFDE0FE72C}");
            public static readonly ID ENachRegistrationFailure = new ID("{146A94CD-C2FA-495D-84D4-D85C76566DED}");

            public static readonly ID MyAccount = new ID("{7D5F8D9D-91A2-4D92-AE4C-A1F2F4B919B3}");
            public static readonly ID InstallationInProgress = new ID("{F52E1BED-C5DE-49DE-8844-25B2A15F8C39}");
            public static readonly ID DocumentVerification = new ID("{A37E43F9-2254-4317-9E09-B8F09DD6994A}");
            public static readonly ID ConnectionComepleted = new ID("{FC971FB1-6E7C-4835-9610-4F93F195B900}");
            public static readonly ID Feedback = new ID("{B7A29EC6-A8C4-47AA-81D0-C5FBC97FA156}");
            public static readonly ID GetSchemes = new ID("{7BE4E7EF-B9B7-40B7-8832-E4369BCD60E1}");
            public static readonly ID ERegistration = new ID("{32ED0287-90C6-4378-8DF5-42E75D4CB566}");
        }
        public struct AccountNumber
        {
            public static readonly ID ItemId = new ID("{38FCD543-B37B-46EE-B867-40A000148DDB}");
        }

        public struct ItemList
        {
            public static readonly ID MonthList = new ID("{D8680063-F53E-4E82-B63C-F15FF309DE16}");
            public static readonly ID MeterReadingItemList = new ID("{85593468-0842-4548-976C-E8EDC2CC03D1}");
            public static readonly ID UserPaymentTransaction = new ID("{E44D46EE-EE71-4BEA-8B5C-47654A965052}");
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

        public struct PaymentRequestProperties
        {
            public static readonly ID AccountNumber = new ID("{5F20F10B-8F9E-4805-8F25-22FA1EBDBC2A}");
            public static readonly ID UserId = new ID("{59E440CA-0A17-4D5D-A732-4497F4EA44FE}");
            public static readonly ID OrderId = new ID("{B24390E3-160F-41D5-B139-98F046988F92}");
            public static readonly ID Amount = new ID("{967C370E-5E87-4877-A6A3-78AE3FBA9352}");
            public static readonly ID RequestTime = new ID("{B56AC741-3839-40A5-97DE-977B61670F70}");
            public static readonly ID Checksumkey = new ID("{BACDB4C7-6AE6-45CA-B4AD-E8356590675E}");
            public static readonly ID CurrencyType = new ID("{22274DE3-BB56-4E28-B333-FD8650639D40}");
            public static readonly ID GatewayType = new ID("{DBAF1CF5-9F40-4F5F-8F2C-F4FBA688A2D5}");
            public static readonly ID msg = new ID("{BA6E6D51-8204-4B80-997C-9AADC8BB7B3C}");
            public static readonly ID AdvanceAmmount = new ID("{28540196-DA30-4408-B35C-EB6463A1C24C}");
            public static readonly ID PaymentType = new ID("{77B18137-81FB-4606-9589-4CD6870C7B0D}");
            public static readonly ID UserType = new ID("{44961A3F-6406-4039-B153-376A8B0F190B}");
            public static readonly ID Email = new ID("{372078B5-7A19-4DE8-8CFD-DB513469B108}");
            public static readonly ID Mobile = new ID("{6A1E931A-CB40-4577-B9CC-CBDD6F6A4389}");
        }

        public struct PaymentResponseProperties
        {
            public static readonly ID TransactionId = new ID("{BD7953B5-FAF8-4B9C-A3F4-96758761467B}");
            public static readonly ID Status = new ID("{5448D408-339D-43F2-B8D2-F1632A519C91}");
            public static readonly ID Responsecode = new ID("{0CFC076F-C019-4BA4-AE98-1C5296276410}");
            public static readonly ID Remark = new ID("{A8C6A0C5-109F-4F48-8E64-BEEA29B019AF}");
            public static readonly ID ResponseTime = new ID("{FD382D99-106B-4145-84D2-CD4275D4F60E}");
            public static readonly ID PaymentRef = new ID("{A2AE07CC-B152-4FBE-968C-0ED1F53518EF}");
            public static readonly ID msg = new ID("{1C32393D-0B36-43E0-9420-9A6A995994F3}");
            public static readonly ID PaymentMode = new ID("{F65F9616-4D3B-4CB3-BCFF-C0C4514108D1}");
        }

        public struct Itempath
        {
            public static readonly ID UserNameFolder = new ID("{5193B68E-D8C3-43FE-97D9-F0E81FA0F530}");
            public static readonly ID ItemUnderUserNameFolder = new ID("{9453CBE0-3F4E-4D0B-A7D8-E7CC389C86C8}");
        }

        public struct PaymentConfiguration
        {
            public static readonly ID ID = new ID("{A34DA962-ABA9-4B32-BE68-785C4C9DB7A4}");

            public struct PayTMFields
            {
                public static readonly ID PTM_Request_URL = new ID("{FF1B8668-7E5C-47D5-BB47-1043AB7B72F0}");
                public static readonly ID PTM_Merchant_Key = new ID("{247CD33C-21C7-4DC8-B93B-B71EFB09FB16}");
                public static readonly ID PTM_Merchant_ID = new ID("{9E8E6B0C-2D04-4A4A-AD5F-246700F160BA}");
                public static readonly ID PTM_INDUSTRY_TYPE_ID = new ID("{A22C0A85-D4ED-4D39-89F5-7001103152D5}");
                public static readonly ID PTM_CHANNEL_ID = new ID("{2F13E8A5-A9F9-491F-93C5-B9E7DAF0AA4B}");
                public static readonly ID PTM_WEBSITE = new ID("{98410680-0481-41EB-BA7E-B937C8EAF192}");
                public static readonly ID PTM_Response_URL_B2B = new ID("{2CBC3802-2D57-4286-933E-BAC4138E5F11}");
                public static readonly ID PTM_Response_URL_S2S = new ID("{89267528-3C50-4811-A6CE-6B7BBF18E13F}");
                public static readonly ID PTM_TransactionStatus_URL = new ID("{9A09015B-6952-43D6-A663-179E62A2C26A}");
            }

            public struct HDFCFields
            {
                public static readonly ID HDFC_App_Id = new ID("{751BFE87-DDC7-4A43-B667-E9D6C6829CD9}");
                public static readonly ID HDFC_Secret_Key = new ID("{F943A24F-B6DD-4AC5-AE39-37ADE03F6EB5}");
                public static readonly ID HDFC_Resp_URL_Success = new ID("{F050F537-0A68-4D5C-9C2A-AB05ED8D5D8E}");
                public static readonly ID HDFC_Resp_URL_Error = new ID("{C4011202-2D8E-4662-9E77-FA09782222EA}");
            }

            public struct BillDeskFields
            {
                public static readonly ID BDSK_Request_URL = new ID("{83970294-ECE6-4058-A399-750AA5590B18}");
                public static readonly ID BDSK_Merchant_ID = new ID("{433C47B2-8507-45A8-83D6-DD10752FCD3B}");
                public static readonly ID BDSK_SECURITY_ID = new ID("{996918E4-7452-4DE7-BC22-D6ECB1DD4AEB}");
                public static readonly ID BDSK_CURRENCY_TYPE = new ID("{93D4ADED-038B-404A-99FD-B69FA6283070}");
                public static readonly ID BDSK_Resp_URL_B2B = new ID("{5D153506-E2E8-4ABE-9D9B-5E84D61E9B27}");
                public static readonly ID BDSK_Resp_URL_S2S = new ID("{3F60B06F-A7EB-4533-B0CB-76B2F3E1A767}");
                public static readonly ID BDSK_Req_Msg = new ID("{8247FE49-B941-43BA-8DC5-6B2A215D2C9A}");
                public static readonly ID BDSK_ChecksumKey = new ID("{BA3E3B2C-E796-4120-985C-8A9079D745A3}");
            }

            public struct BillDeskSecurityDepositeFields
            {
                public static readonly ID BDSD_Request_URL = new ID("{83970294-ECE6-4058-A399-750AA5590B18}");
                public static readonly ID BDSD_Merchant_ID = new ID("{433C47B2-8507-45A8-83D6-DD10752FCD3B}");
                public static readonly ID BDSD_SECURITY_ID = new ID("{996918E4-7452-4DE7-BC22-D6ECB1DD4AEB}");
                public static readonly ID BDSD_CURRENCY_TYPE = new ID("{93D4ADED-038B-404A-99FD-B69FA6283070}");
                public static readonly ID BDSD_Resp_URL_B2B = new ID("{5D153506-E2E8-4ABE-9D9B-5E84D61E9B27}");
                public static readonly ID BDSD_Resp_URL_S2S = new ID("{3F60B06F-A7EB-4533-B0CB-76B2F3E1A767}");
                public static readonly ID BDSD_Req_Msg = new ID("{8247FE49-B941-43BA-8DC5-6B2A215D2C9A}");
                public static readonly ID BDSD_ChecksumKey = new ID("{BA3E3B2C-E796-4120-985C-8A9079D745A3}");
            }

            public struct BillDeskVDSFields
            {
                public static readonly ID BDVDS_Request_URL = new ID("{088B6AA2-EFFC-4607-85CF-29E091895202}");
                public static readonly ID BDVDS_Merchant_ID = new ID("{7A530103-284F-4425-8C9E-7ACC5C3572D5}");
                public static readonly ID BDVDS_SECURITY_ID = new ID("{C3DDD3A5-9B9C-467C-B669-4C26448A2026}");
                public static readonly ID BDVDS_CURRENCY_TYPE = new ID("{3B0617A2-3D04-4BBD-82B4-6528463DE5F6}");
                public static readonly ID BDVDS_Resp_URL_B2B = new ID("{A4D01049-0CDB-4DF4-B7DF-E43807D4BD59}");
                public static readonly ID BDVDS_Resp_URL_S2S = new ID("{F46A166A-3831-4AC4-A9BA-03159CDA0E56}");
                public static readonly ID BDVDS_Req_Msg = new ID("{130AB95D-22B3-4D0B-ADAB-716FEE335986}");
                public static readonly ID BDVDS_ChecksumKey = new ID("{E804FB92-0D39-497A-B1DC-0127E3C9A92C}");
            }

            public struct EbixCashFields
            {
                public static readonly ID EBIX_Request_URL = new ID("{115CBF24-152C-4001-BD7A-DEB008D01079}");
                public static readonly ID EBIX_MERCHANT_KEY = new ID("{F96DA234-0450-41B9-ACB9-10ECD6DF4697}");
                public static readonly ID EBIX_CASH_P_NO = new ID("{A656C010-023B-4D34-9F64-2C69871E4007}");
                public static readonly ID EBIX_Response_URL = new ID("{03B93045-52DD-4C61-89AE-621CE2929454}");
                public static readonly ID EBIX_S2S_URL = new ID("{4806DB71-7666-44BD-AB8A-60C5280E6136}");
                public static readonly ID EBIX_Transaction_VerifyURL = new ID("{01FD30DD-CB7B-4F67-A6EF-31B6CE91D77D}");

            }

            public struct PayuMoneyFields
            {
                public static readonly ID PUM_Request_URL = new ID("{ED7B57A9-3EB4-4EA1-BBE3-B613F03E7F06}");
                public static readonly ID PUM_Merchant_Key = new ID("{69BF2814-392F-4336-B709-A56F0489AC76}");
                public static readonly ID PUM_Merchant_Salt = new ID("{18966701-2167-48E2-8BE2-15A1E7EF2272}");
                public static readonly ID PUM_Service_Provider = new ID("{8A2BB971-4AE5-483E-BEFD-65E29316A982}");
                public static readonly ID PUM_Product_Info = new ID("{FE6CBDE7-00CE-4A86-A832-86B63177E091}");
                public static readonly ID PUM_Resp_URL_B2B = new ID("{9A814E7B-9448-41B8-8854-C73E22581C47}");
                public static readonly ID PUM_Resp_URL_S2S = new ID("{25E1BAA5-C818-4005-B484-77DF56891B4D}");
            }

            public struct ICICIFields
            {
                public static readonly ID ICICI_Request_URL = new ID("{CF73D14D-EC1D-4895-BC4C-A02FB5A72387}");
                public static readonly ID ICICI_Response_URL_B2B = new ID("{3BBA2FA9-658F-4278-B2ED-62CBECFB591A}");
                public static readonly ID MerchantCode = new ID("{0ACC8E3D-F7DE-4C1B-BD54-67BC4A48BD06}");
                public static readonly ID SchemeCode = new ID("{79E958D4-4911-45BE-BE13-24C464259B43}");
                public static readonly ID EncryptionKey = new ID("{E590B211-FBB4-4806-9508-1E8F95A6796B}");
                public static readonly ID EncryptionIV = new ID("{E130D1C0-E6EA-47F3-8D55-D14155A54B59}");
                public static readonly ID Currency = new ID("{1CEAC655-C2BE-498F-A986-58D8252722F4}");
                public static readonly ID BankCode = new ID("{C9D6D9A6-B212-4D65-91DB-79D385273E2C}");
                public static readonly ID ICICI_S2S_URL = new ID("{36962298-71BB-4A28-8944-F3BE86A271A0}");
                public static readonly ID ITC = new ID("{2070400E-53D0-4BFB-8916-43946DC29E10}");
            }

            public struct BenowFields
            {
                public static readonly ID BNW_Request_URL = new ID("{0A02DDAF-B76B-4795-98D5-D463AAE91FAA}");
                public static readonly ID BNW_EncryptedString = new ID("{1F0C3C86-04FB-42A5-AB84-277ACB7AAA66}");
                public static readonly ID BNW_MerchantCode = new ID("{73498085-7055-46EC-AF4F-C9D19A448CCC}");
                public static readonly ID BNW_S2S_URL = new ID("{B5980DF7-1F42-4058-8743-C10FE34BFF65}");
                public static readonly ID BNW_Response_URL_B2B = new ID("{C93C348C-DB5C-441B-B8D5-5D8AF7EE6951}");
                public static readonly ID BNW_XEMAIL = new ID("{A70BDAFA-24E3-4496-ABC0-D308FF55694B}");
                public static readonly ID BNW_AuthorizationKey = new ID("{53BBA3E6-51AA-4CC3-9119-DA8BE845E078}");
                public static readonly ID BNW_REFNo = new ID("{12301057-6324-41AA-88CB-DF18E7BD6836}");
                public static readonly ID BNW_PaymentMethod = new ID("{D3DE99F6-C077-444D-A073-F8927CBA91B5}");
                public static readonly ID BNW_Remark = new ID("{22A85A49-5EE8-4B82-AB77-55FCFFB2C2F4}");
                public static readonly ID BNW_ORGID = new ID("{34581D0F-D3C2-417C-B371-0B094E626F0D}");
                public static readonly ID BNW_HashKey = new ID("{8D615883-7537-4C81-995C-18B599950F6C}");
                public static readonly ID BNW_PayerVPA = new ID("{2ADE2F73-4B91-4B28-90F6-77663CA2878E}");
            }

            public struct DBSFields
            {
                public static readonly ID DBS_ORGID = new ID("{88E775A6-052C-4F33-9916-F9BB725BE0C3}");
                public static readonly ID DBS_PayeeVPA = new ID("{E6F48017-71A0-405D-9B32-061C5E5CF347}");
                public static readonly ID DBS_PayeeName = new ID("{F6BCC1FA-927B-4634-BF29-627501EE25E0}");
                public static readonly ID DBS_ResponseCallbackURL = new ID("{C225A1E1-CE28-45E9-B49B-969E30A22E91}");
                public static readonly ID DBS_DefaultTN = new ID("{BEAD6B34-AB7E-48BC-BA28-770C48F0BD2B}");
                public static readonly ID DBS_Currency = new ID("{A9C73CB4-34C5-4C8F-9A16-00E9C82960FF}");
                public static readonly ID DBS_ModeOfTransaction = new ID("{A45591DC-45CD-4E22-B4FB-2D26581181EB}");
                public static readonly ID DBS_ServerPublicKeyPath = new ID("{74F12779-5FD3-43AB-A597-184540527BDB}");
                public static readonly ID DBS_ClientPrivateKeyPath = new ID("{5EA582AE-C793-4C3E-959A-E3CDB8085717}");
                public static readonly ID DBS_ClientSecretKey = new ID("{A53DCB2E-79AF-4ACE-9507-220641CE9C23}");
            }

            public struct CityFields
            {
                public static readonly ID City_PayeeVPA = new ID("{49BD5637-5FCD-4206-A1B3-80C2F1B67770}");
                public static readonly ID City_PayeeName = new ID("{A0B08739-32B6-4795-B60A-4D7C9555896C}");
                public static readonly ID City_ORGID = new ID("{16B07757-27BB-4FD8-9891-34B10230E40A}");
                public static readonly ID City_DefaultTN = new ID("{EF868770-0D79-4F34-AC69-A69DCDB42C95}");
                public static readonly ID City_Currency = new ID("{6582E0B1-3C89-4D2E-8B73-2CA0ED59EDAD}");
                public static readonly ID City_ModeOfTransaction = new ID("{5F60A318-7AAA-435A-AF89-7A2B6BE83FD9}");
            }

            public struct Datasource
            {
                public static readonly ID SecurityDepositPaymentMode = new ID("{7E9DAE49-3E24-4947-B37E-7CE764F75ECA}");
                public static readonly ID VDSPaymentMode = new ID("{FDC122B5-F47F-4396-A99B-816F97B28DD9}");
            }

        }


        public struct PaymentConfigurationRevamp
        {
            public static readonly ID ID = new ID("{7BFF155F-8870-46CF-BB73-E722408E211E}");

            public struct PayTMFields
            {
                public static readonly ID PTM_Request_URL = new ID("{FF1B8668-7E5C-47D5-BB47-1043AB7B72F0}");
                public static readonly ID PTM_Merchant_Key = new ID("{247CD33C-21C7-4DC8-B93B-B71EFB09FB16}");
                public static readonly ID PTM_Merchant_ID = new ID("{9E8E6B0C-2D04-4A4A-AD5F-246700F160BA}");
                public static readonly ID PTM_INDUSTRY_TYPE_ID = new ID("{A22C0A85-D4ED-4D39-89F5-7001103152D5}");
                public static readonly ID PTM_CHANNEL_ID = new ID("{2F13E8A5-A9F9-491F-93C5-B9E7DAF0AA4B}");
                public static readonly ID PTM_WEBSITE = new ID("{98410680-0481-41EB-BA7E-B937C8EAF192}");
                public static readonly ID PTM_Response_URL_B2B = new ID("{2CBC3802-2D57-4286-933E-BAC4138E5F11}");
                public static readonly ID PTM_Response_URL_S2S = new ID("{89267528-3C50-4811-A6CE-6B7BBF18E13F}");
                public static readonly ID PTM_TransactionStatus_URL = new ID("{9A09015B-6952-43D6-A663-179E62A2C26A}");
            }

            public struct HDFCFields
            {
                public static readonly ID HDFC_App_Id = new ID("{751BFE87-DDC7-4A43-B667-E9D6C6829CD9}");
                public static readonly ID HDFC_Secret_Key = new ID("{F943A24F-B6DD-4AC5-AE39-37ADE03F6EB5}");
                public static readonly ID HDFC_Resp_URL_Success = new ID("{F050F537-0A68-4D5C-9C2A-AB05ED8D5D8E}");
                public static readonly ID HDFC_Resp_URL_Error = new ID("{C4011202-2D8E-4662-9E77-FA09782222EA}");
            }

            public struct BillDeskFields
            {
                public static readonly ID BDSK_Request_URL = new ID("{83970294-ECE6-4058-A399-750AA5590B18}");
                public static readonly ID BDSK_Merchant_ID = new ID("{433C47B2-8507-45A8-83D6-DD10752FCD3B}");
                public static readonly ID BDSK_SECURITY_ID = new ID("{996918E4-7452-4DE7-BC22-D6ECB1DD4AEB}");
                public static readonly ID BDSK_CURRENCY_TYPE = new ID("{93D4ADED-038B-404A-99FD-B69FA6283070}");
                public static readonly ID BDSK_Resp_URL_B2B = new ID("{5D153506-E2E8-4ABE-9D9B-5E84D61E9B27}");
                public static readonly ID BDSK_Resp_URL_S2S = new ID("{3F60B06F-A7EB-4533-B0CB-76B2F3E1A767}");
                public static readonly ID BDSK_Req_Msg = new ID("{8247FE49-B941-43BA-8DC5-6B2A215D2C9A}");
                public static readonly ID BDSK_ChecksumKey = new ID("{BA3E3B2C-E796-4120-985C-8A9079D745A3}");
            }

            public struct BillDeskSecurityDepositeFields
            {
                public static readonly ID BDSD_Request_URL = new ID("{83970294-ECE6-4058-A399-750AA5590B18}");
                public static readonly ID BDSD_Merchant_ID = new ID("{433C47B2-8507-45A8-83D6-DD10752FCD3B}");
                public static readonly ID BDSD_SECURITY_ID = new ID("{996918E4-7452-4DE7-BC22-D6ECB1DD4AEB}");
                public static readonly ID BDSD_CURRENCY_TYPE = new ID("{93D4ADED-038B-404A-99FD-B69FA6283070}");
                public static readonly ID BDSD_Resp_URL_B2B = new ID("{5D153506-E2E8-4ABE-9D9B-5E84D61E9B27}");
                public static readonly ID BDSD_Resp_URL_S2S = new ID("{3F60B06F-A7EB-4533-B0CB-76B2F3E1A767}");
                public static readonly ID BDSD_Req_Msg = new ID("{8247FE49-B941-43BA-8DC5-6B2A215D2C9A}");
                public static readonly ID BDSD_ChecksumKey = new ID("{BA3E3B2C-E796-4120-985C-8A9079D745A3}");
            }

            public struct BillDeskVDSFields
            {
                public static readonly ID BDVDS_Request_URL = new ID("{088B6AA2-EFFC-4607-85CF-29E091895202}");
                public static readonly ID BDVDS_Merchant_ID = new ID("{7A530103-284F-4425-8C9E-7ACC5C3572D5}");
                public static readonly ID BDVDS_SECURITY_ID = new ID("{C3DDD3A5-9B9C-467C-B669-4C26448A2026}");
                public static readonly ID BDVDS_CURRENCY_TYPE = new ID("{3B0617A2-3D04-4BBD-82B4-6528463DE5F6}");
                public static readonly ID BDVDS_Resp_URL_B2B = new ID("{A4D01049-0CDB-4DF4-B7DF-E43807D4BD59}");
                public static readonly ID BDVDS_Resp_URL_S2S = new ID("{F46A166A-3831-4AC4-A9BA-03159CDA0E56}");
                public static readonly ID BDVDS_Req_Msg = new ID("{130AB95D-22B3-4D0B-ADAB-716FEE335986}");
                public static readonly ID BDVDS_ChecksumKey = new ID("{E804FB92-0D39-497A-B1DC-0127E3C9A92C}");
            }

            public struct EbixCashFields
            {
                public static readonly ID EBIX_Request_URL = new ID("{115CBF24-152C-4001-BD7A-DEB008D01079}");
                public static readonly ID EBIX_MERCHANT_KEY = new ID("{F96DA234-0450-41B9-ACB9-10ECD6DF4697}");
                public static readonly ID EBIX_CASH_P_NO = new ID("{A656C010-023B-4D34-9F64-2C69871E4007}");
                public static readonly ID EBIX_Response_URL = new ID("{03B93045-52DD-4C61-89AE-621CE2929454}");
                public static readonly ID EBIX_S2S_URL = new ID("{4806DB71-7666-44BD-AB8A-60C5280E6136}");
                public static readonly ID EBIX_Transaction_VerifyURL = new ID("{01FD30DD-CB7B-4F67-A6EF-31B6CE91D77D}");

            }

            public struct PayuMoneyFields
            {
                public static readonly ID PUM_Request_URL = new ID("{ED7B57A9-3EB4-4EA1-BBE3-B613F03E7F06}");
                public static readonly ID PUM_Merchant_Key = new ID("{69BF2814-392F-4336-B709-A56F0489AC76}");
                public static readonly ID PUM_Merchant_Salt = new ID("{18966701-2167-48E2-8BE2-15A1E7EF2272}");
                public static readonly ID PUM_Service_Provider = new ID("{8A2BB971-4AE5-483E-BEFD-65E29316A982}");
                public static readonly ID PUM_Product_Info = new ID("{FE6CBDE7-00CE-4A86-A832-86B63177E091}");
                public static readonly ID PUM_Resp_URL_B2B = new ID("{9A814E7B-9448-41B8-8854-C73E22581C47}");
                public static readonly ID PUM_Resp_URL_S2S = new ID("{25E1BAA5-C818-4005-B484-77DF56891B4D}");
            }

            public struct ICICIFields
            {
                public static readonly ID ICICI_Request_URL = new ID("{CF73D14D-EC1D-4895-BC4C-A02FB5A72387}");
                public static readonly ID ICICI_Response_URL_B2B = new ID("{3BBA2FA9-658F-4278-B2ED-62CBECFB591A}");
                public static readonly ID MerchantCode = new ID("{0ACC8E3D-F7DE-4C1B-BD54-67BC4A48BD06}");
                public static readonly ID SchemeCode = new ID("{79E958D4-4911-45BE-BE13-24C464259B43}");
                public static readonly ID EncryptionKey = new ID("{E590B211-FBB4-4806-9508-1E8F95A6796B}");
                public static readonly ID EncryptionIV = new ID("{E130D1C0-E6EA-47F3-8D55-D14155A54B59}");
                public static readonly ID Currency = new ID("{1CEAC655-C2BE-498F-A986-58D8252722F4}");
                public static readonly ID BankCode = new ID("{C9D6D9A6-B212-4D65-91DB-79D385273E2C}");
                public static readonly ID ICICI_S2S_URL = new ID("{36962298-71BB-4A28-8944-F3BE86A271A0}");
                public static readonly ID ITC = new ID("{2070400E-53D0-4BFB-8916-43946DC29E10}");
            }

            public struct BenowFields
            {
                public static readonly ID BNW_Request_URL = new ID("{0A02DDAF-B76B-4795-98D5-D463AAE91FAA}");
                public static readonly ID BNW_EncryptedString = new ID("{1F0C3C86-04FB-42A5-AB84-277ACB7AAA66}");
                public static readonly ID BNW_MerchantCode = new ID("{73498085-7055-46EC-AF4F-C9D19A448CCC}");
                public static readonly ID BNW_S2S_URL = new ID("{B5980DF7-1F42-4058-8743-C10FE34BFF65}");
                public static readonly ID BNW_Response_URL_B2B = new ID("{C93C348C-DB5C-441B-B8D5-5D8AF7EE6951}");
                public static readonly ID BNW_XEMAIL = new ID("{A70BDAFA-24E3-4496-ABC0-D308FF55694B}");
                public static readonly ID BNW_AuthorizationKey = new ID("{53BBA3E6-51AA-4CC3-9119-DA8BE845E078}");
                public static readonly ID BNW_REFNo = new ID("{12301057-6324-41AA-88CB-DF18E7BD6836}");
                public static readonly ID BNW_PaymentMethod = new ID("{D3DE99F6-C077-444D-A073-F8927CBA91B5}");
                public static readonly ID BNW_Remark = new ID("{22A85A49-5EE8-4B82-AB77-55FCFFB2C2F4}");
                public static readonly ID BNW_ORGID = new ID("{34581D0F-D3C2-417C-B371-0B094E626F0D}");
                public static readonly ID BNW_HashKey = new ID("{8D615883-7537-4C81-995C-18B599950F6C}");
                public static readonly ID BNW_PayerVPA = new ID("{2ADE2F73-4B91-4B28-90F6-77663CA2878E}");
            }

            public struct DBSFields
            {
                public static readonly ID DBS_ORGID = new ID("{88E775A6-052C-4F33-9916-F9BB725BE0C3}");
                public static readonly ID DBS_PayeeVPA = new ID("{E6F48017-71A0-405D-9B32-061C5E5CF347}");
                public static readonly ID DBS_PayeeName = new ID("{F6BCC1FA-927B-4634-BF29-627501EE25E0}");
                public static readonly ID DBS_ResponseCallbackURL = new ID("{C225A1E1-CE28-45E9-B49B-969E30A22E91}");
                public static readonly ID DBS_DefaultTN = new ID("{BEAD6B34-AB7E-48BC-BA28-770C48F0BD2B}");
                public static readonly ID DBS_Currency = new ID("{A9C73CB4-34C5-4C8F-9A16-00E9C82960FF}");
                public static readonly ID DBS_ModeOfTransaction = new ID("{A45591DC-45CD-4E22-B4FB-2D26581181EB}");
                public static readonly ID DBS_ServerPublicKeyPath = new ID("{74F12779-5FD3-43AB-A597-184540527BDB}");
                public static readonly ID DBS_ClientPrivateKeyPath = new ID("{5EA582AE-C793-4C3E-959A-E3CDB8085717}");
                public static readonly ID DBS_ClientSecretKey = new ID("{A53DCB2E-79AF-4ACE-9507-220641CE9C23}");
            }

            public struct CityFields
            {
                public static readonly ID City_PayeeVPA = new ID("{49BD5637-5FCD-4206-A1B3-80C2F1B67770}");
                public static readonly ID City_PayeeName = new ID("{A0B08739-32B6-4795-B60A-4D7C9555896C}");
                public static readonly ID City_ORGID = new ID("{16B07757-27BB-4FD8-9891-34B10230E40A}");
                public static readonly ID City_DefaultTN = new ID("{EF868770-0D79-4F34-AC69-A69DCDB42C95}");
                public static readonly ID City_Currency = new ID("{6582E0B1-3C89-4D2E-8B73-2CA0ED59EDAD}");
                public static readonly ID City_ModeOfTransaction = new ID("{5F60A318-7AAA-435A-AF89-7A2B6BE83FD9}");
            }

            public struct Datasource
            {
                public static readonly ID SecurityDepositPaymentMode = new ID("{BD614821-1713-4D34-B6F9-0C78D0B25483}");
                public static readonly ID VDSPaymentMode = new ID("{75538250-BE2A-40EF-8783-5A70FC9BC1C6}");
            }

            public struct SafeXPayFields
            {
                public static readonly ID SafeXPay_Request_URL = new ID("{DC387D14-7904-4C74-9CAD-ECAEF23BE533}");
                public static readonly ID SafeXPay_Merchant_ID = new ID("{27078134-A2D8-4735-90CA-43762365CF25}");
                public static readonly ID SafeXPay_Aggregator_ID = new ID("{C2B12B50-F031-4F43-B6E1-D60D7804A9AE}");
                public static readonly ID SafeXPay_CURRENCY_TYPE = new ID("{C9E4BB90-9C38-4083-9721-385E08E8EE1B}");
                public static readonly ID SafeXPay_Resp_URL_B2B = new ID("{4FA6AC6D-AECA-4A7E-9A23-B6BA47C61ADF}");
                public static readonly ID SafeXPay_Resp_URL_S2S = new ID("{09699D72-C08D-4B87-9E65-0E34A8D2616B}");
                public static readonly ID SafeXPay_Req_Msg = new ID("{F7A6E9F0-6D86-41FF-B6DB-6B6F3FE1928C}");
                public static readonly ID SafeXPay_ChecksumKey = new ID("{E080EC59-6B77-4DC8-9ACF-2CE5D7043BBF}");
                public static readonly ID SafeXPay_Merchant_Encryption_Key = new ID("{6413B34F-AA72-4C1E-AADE-DBDA4A18C2A8}");
            }

            public struct CashFreeFields
            {
                public static readonly ID CashFree_Key_Id = new ID("{506A1CD4-EAD7-41AA-86A2-4E03F21EA538}");
                public static readonly ID CashFree_Order_Request_URL = new ID("{031D0BC5-4FE9-4FEC-8346-8EE3187A8EF7}");
                public static readonly ID CashFree_secret_Key = new ID("{6F2E400E-286C-466B-8729-BF5A9A407FD6}");
                public static readonly ID CashFree_api_version = new ID("{8B0E4B0A-9B46-4A20-9CFD-0F253710DE19}");
                public static readonly ID CashFree_Content_Type = new ID("{99143497-A35B-4ADD-B4D2-47FCCB51E54D}");
                public static readonly ID CashFree_CURRENCY_TYPE = new ID("{4D265E1A-19D5-4C9D-A4DC-AF7F5D7DE6FB}");
                public static readonly ID CashFree_Return_Url = new ID("{CC6DE3A6-E74B-4AEB-9AB8-52DB318DDBA6}");
                public static readonly ID CashFree_Notify_Url = new ID("{47975642-BA1F-491E-A387-D13D5B30EE9D}");
                public static readonly ID CashFree_timestamp = new ID("{50C78AD8-59A8-41A6-BBDC-CF8A4F3622A2}");
                public static readonly ID CheckSumKey = new ID("{1975A2B3-B9D3-4F8D-99AF-8C6EAB9F9251}");
            }
        }

        public struct PaymentConfigurationAdaniGas
        {
            public static readonly ID ID = new ID("{3AB44E4E-D14A-4653-AE5A-F945B6FD3BF4}");

            public struct PayTMFields
            {
                public static readonly ID PTM_Request_URL = new ID("{86725123-7F2E-42A2-AD9E-663F16EC75FE}");
                public static readonly ID PTM_Merchant_Key = new ID("{A29ACC85-1E5C-4DA6-B847-0A23EA36E318}");
                public static readonly ID PTM_Merchant_ID = new ID("{8C3CE550-332A-4D79-B139-7FAD6F3693F8}");
                public static readonly ID PTM_INDUSTRY_TYPE_ID = new ID("{9B944BA1-B7F8-41DD-857E-E59EA8F19C95}");
                public static readonly ID PTM_CHANNEL_ID = new ID("{C8C99DD1-7D8A-45D6-8CF7-6720C84FFB10}");
                public static readonly ID PTM_WEBSITE = new ID("{D0673665-2B6F-4346-A943-8943850D6F57}");
                public static readonly ID PTM_Response_URL_B2B = new ID("{6FFDE562-9911-4D7C-A526-3C5738E7E101}");
                public static readonly ID PTM_Response_URL_S2S = new ID("{C0E51A84-374C-4985-ACB5-7DCF0A2684C8}");
                public static readonly ID PTM_TransactionStatus_URL = new ID("{C49356E5-0AA7-419D-B74C-E09DADB7675D}");
            }

            public struct BillDeskFields
            {
                public static readonly ID BDSK_Request_URL = new ID("{5DFECDA9-426F-45C3-BD7B-D424257A7655}");
                public static readonly ID BDSK_Merchant_ID = new ID("{E85867B8-D768-406F-83AB-E1F4B8BB909F}");
                public static readonly ID BDSK_SECURITY_ID = new ID("{10A04DE1-CD6E-4E07-A841-A68109EFE3AD}");
                public static readonly ID BDSK_CURRENCY_TYPE = new ID("{52A7233F-C6CF-4F2E-A701-8C1DCAF5BD2B}");
                public static readonly ID BDSK_Resp_URL_B2B = new ID("{1C7DF115-A04A-4799-961C-E5C1DB42589B}");
                public static readonly ID BDSK_Resp_URL_S2S = new ID("{2C8BB5E0-15D5-41A3-B430-DE08CA3A1DC1}");
                public static readonly ID BDSK_Req_Msg = new ID("{04F396BF-DCF0-4FBE-9B7B-4DC4FCA0C2CF}");
                public static readonly ID BDSK_ChecksumKey = new ID("{05FA0F48-1FCE-47AB-AE18-1052C4FCCD46}");
            }
            public struct PayuMoneyFields
            {
                public static readonly ID PUM_Request_URL = new ID("{58D5E1BB-E617-49AF-B735-C87EFBFAEF62}");
                public static readonly ID PUM_Merchant_Key = new ID("{04ECCA89-BD23-48D8-B5AE-F07D7CE4CE2C}");
                public static readonly ID PUM_Merchant_Salt = new ID("{89642DCA-E18F-4E20-8485-4B7B3587E4F5}");
                public static readonly ID PUM_Service_Provider = new ID("{982BC53A-2366-4EF7-9F06-09599A17B5C2}");
                public static readonly ID PUM_Product_Info = new ID("{DD1443ED-3CC7-4250-929D-01CD1C90FDEE}");
                public static readonly ID PUM_Resp_URL_B2B = new ID("{137A1C82-5466-4CD6-8979-CC0056FB7617}");
                public static readonly ID PUM_Resp_URL_S2S = new ID("{D484BD2F-1A90-43D9-993D-1C700ABF8D52}");
            }
            public struct HDFCFields
            {
                public static readonly ID HDFC_Merchant_ID = new ID("{565A03BB-F9F7-4D96-92DD-D4FD93DAC156}");
                public static readonly ID HDFC_Working_Key = new ID("{CE33858E-993E-425B-810F-B5825301D417}");
                public static readonly ID HDFC_Access_Code = new ID("{9365A866-2ACA-4A95-8426-DAF1A35BDD9D}");
                public static readonly ID HDFC_Request_URL = new ID("{FAA68DC6-171F-40A8-8703-E477A37A47E7}");
                public static readonly ID HDFC_Resp_URL_Error = new ID("{5DAFED08-548D-4AD6-96A0-D7020B4C0E7A}");
                public static readonly ID HDFC_Resp_URL_Success = new ID("{88CB24D8-0433-4EB1-81DB-7B46BFE8D3D4}");
                public static readonly ID HDFC_Currency_Type = new ID("{E43FADCC-9D0D-4A10-8B38-1924217A79B4}");
                public static readonly ID HDFC_Req_Param = new ID("{92AED1F9-B2D5-4019-B128-CF6E1CFFECA1}");
            }
            public struct HDFCFieldsForATGL
            {
                public static readonly ID HDFC_Merchant_ID = new ID("{0C836CDE-C850-4EFB-865F-A6A9CAC72F1B}");
                public static readonly ID HDFC_Working_Key = new ID("{0CB6D356-31EE-4F77-B36D-AE39BE8176FD}");
                public static readonly ID HDFC_Access_Code = new ID("{5EE70E4B-E64F-41EF-AD38-E618262F8DAA}");
                public static readonly ID HDFC_Request_URL = new ID("{3FABF8B6-F276-47CD-A6B7-3704EB2368EF}");
                public static readonly ID HDFC_Resp_URL_Error = new ID("{483C7D23-532C-4AC4-B7E0-8B34A4853841}");
                public static readonly ID HDFC_Resp_URL_Success = new ID("{51AA337D-2C1C-4177-922D-57E9881CCC64}");
                public static readonly ID HDFC_Currency_Type = new ID("{6CC26618-BAAE-44F1-BDB1-A086FA964173}");
                public static readonly ID HDFC_Req_Param = new ID("{B94BB46F-8D82-40F2-9E23-C74DC8DDFD0F}");
            }
        }
        public struct AdaniGasENACHRegisterConfiguration
        {
            public static readonly ID ID = new ID("{824B11E3-D9FF-485B-8AB6-29C0A2A7F518}");
            public struct BillDeskENach
            {
                public static readonly ID BDSK_Request_URL = new ID("{FD3C7FE6-78CF-4003-B8BE-3B0EA28FF0D6}");
                public static readonly ID BDSK_Merchant_ID = new ID("{7D1E7030-8149-4425-8988-F8974B97BF79}");
                public static readonly ID BDSK_USER_ID = new ID("{4E978051-0AAD-4464-B8A5-829450A9F3CF}");
                public static readonly ID BDSK_CURRENCY_TYPE = new ID("{8DA829A6-43C0-4F17-9340-625D56F93720}");
                public static readonly ID BDSK_Resp_URL_B2B = new ID("{46772B9A-CF6F-4727-A467-7C1F54D5ECB3}");
                public static readonly ID BDSK_Resp_URL_S2S = new ID("{49B3C4D8-2E3E-4C4E-B3BF-FA5A2334904D}");
                public static readonly ID BDSK_Req_Msg = new ID("{C98BD05E-216A-4970-BDB3-67F70A326D7F}");
                public static readonly ID BDSK_ChecksumKey = new ID("{B6D49AB1-D244-41B5-AA0F-AC4CA773A5B3}");
                public static readonly ID BDSK_BankId = new ID("{659B1727-1392-4B81-891E-D85BEB6BA116}");
                public static readonly ID BDSK_ItemCode = new ID("{9DF09492-26C4-4CBA-90E7-2768FC43EAD0}");
            }
        }
        public struct AdaniGasENACHBilldeskSIDetails
        {
            public static readonly ID ID = new ID("{AE9B9690-347E-43F7-AEAE-3B9BC9665164}");
            public struct BillDeskENachSIDetail
            {
                public static readonly ID Request_String = new ID("{B5A0C7D7-0446-4024-9E84-5E4F7EDF1578}");
                public static readonly ID accountnumber = new ID("{5C9F50D9-F14A-417E-94EA-E3EB86A1F133}");
                public static readonly ID accounttype = new ID("{266B1332-07AA-400B-BE2C-82589923E2E7}");
                public static readonly ID amount = new ID("{CC8B811D-FAAE-4B17-B50B-4A0A012B4528}");
                public static readonly ID amounttype = new ID("{05E196EC-2B3F-42D9-BBAC-9C8A3DA500D4}");
                public static readonly ID startdate = new ID("{136972E2-58EF-48C1-BEA4-D1CF23206414}");
                public static readonly ID enddate = new ID("{8BAD66B2-A22E-4A33-8821-C0E70ECF4DB1}");
                public static readonly ID frequency = new ID("{FD393604-BFB3-4EFC-AF85-F19E1403D305}");
                public static readonly ID Ref1 = new ID("{B6F756B2-01FF-4D5A-B21F-D0A49BA5B4B0}");
                public static readonly ID Ref2_CustomerNo = new ID("{4D4FB6F9-AC79-4A83-94FA-95E50B8A9C22}");
                public static readonly ID customername = new ID("{584DE0AC-B608-49F4-8E0B-8C1547FA6F48}");
                public static readonly ID mandaterefno = new ID("{5891E3D7-1406-4775-BE28-4CF9A537A3C8}");
                public static readonly ID IfscCode = new ID("{D4A71199-973B-4C8E-8046-46FCA8122EB1}");
                public static readonly ID MICR = new ID("{4C5114D4-B564-4612-BCF7-E0BFF201DA2D}");
                public static readonly ID MobileNumber = new ID("{BD0D63B6-0B63-4722-BB61-5FB5D05184BB}");
                public static readonly ID emailID = new ID("{6F771988-A951-4B92-BB32-FA4295E50FB7}");
                public static readonly ID ModeOfRegistration = new ID("{6F247CEE-76F2-47D1-9A00-3C415857ED7E}");
            }
        }
        public struct GasConsumptionPattern
        {
            public struct Datasource
            {
                public static readonly ID GasConsumptionPattern_Year = new ID("{A2ACEEC7-7CE6-4460-8032-CB9106522FE4}");
            }
        }
        public struct GasConsumptionPattern_Quarters
        {
            public struct Datasource
            {
                public static readonly ID GasConsumptionPattern_Quarters = new ID("{005D997D-2F88-4BA8-B755-D9C847C4F57A}");
            }
        }
        public struct ComplaintsQueries
        {
            public struct Datasource
            {
                public static readonly ID ComplaintsQuery_Type = new ID("{3328DBC9-4757-40C3-B048-91B077CECCBC}");
                public static readonly ID ComplaintsQuery_Category = new ID("{DDFD656F-8787-4132-A0ED-D5DF9A6B07F4}");

            }
        }

        public struct AferSalesService
        {
            public struct Datasource
            {
                public static readonly ID AfterSalesService_Request = new ID("{2B285A09-A9A6-47CC-B852-FDB2914DBEB5}");


            }
        }


        public struct PaymentPostToSAP
        {
            public static readonly ID ID = new ID("{0CC18376-7764-4660-AE0E-5A6EBDF902CE}");
            public struct Field
            {
                public static readonly ID ClearingAcct = new ID("{9809D9BD-40D6-4DC9-AC99-289A1D071C8C}");
                public static readonly ID CompanyCode = new ID("{BF1AAEC2-C2E7-4809-94A5-C2C6A40DBBB8}");
                public static readonly ID BusArea = new ID("{5F7740BA-D748-4383-8ACB-F2433ECA4428}");
            }
        }


        public struct RegistrationConfig
        {
            public struct Datasource
            {
                public static readonly ID SecretQuestionList = new ID("{0BEC2F00-2430-440A-9B2A-1792035D159E}");
                public static readonly ID SecretQuestionListRevamp = new ID("{87FFEEC1-9059-41D2-B579-C7B8F33F1A23}");
            }
        }

        public struct PlantCity
        {
            public struct Datasource
            {
                public static readonly ID PlantCityList = new ID("{CCE3405A-87B1-4DB1-B9A5-4216FBD2B4CE}");
            }
        }
        public struct CNGcustomerRegistration
        {
            public struct Datasource
            {
                public static readonly ID States = new ID("{56AC971B-64AC-462C-83F4-7351C709133E}");
                public static readonly ID Cities = new ID("{9175B8FD-652E-4B41-B7AB-0C5AE14072B4}");
                public static readonly ID VehicleTypes = new ID("{58601851-D783-4A13-8AF6-D1935238DE39}");
                public static readonly ID Years = new ID("{EAC3D159-EA57-414D-9847-1555278B206A}");
                public static readonly ID ThreeW_VehicleCompanyModel = new ID("{C1F7FCCE-8AA3-4646-967F-10305962C0EB}");
                public static readonly ID FourW_VehicleCompanyModel = new ID("{CF70712C-6FE4-49E6-918C-1D352FBCD7C1}");
            }
        }
        public struct NewConnectionDataSources
        {
            public struct Datasource
            {
                public static readonly ID PartnerTypeList = new ID("{7C07BC61-B9F1-4639-A057-801218114965}");
                public static readonly ID CityTypeList = new ID("{CC33C458-6B4F-43D5-8F9F-2F81754CD6D5}");
                public static readonly ID ReferenceSource = new ID("{7853D142-D3C4-49E5-A8E3-E8A0007786BF}");
                public static readonly ID TypeOfHouse = new ID("{0979321A-5057-4B23-9D59-F053DE8FC370}");
                public static readonly ID TypeOfCustomer = new ID("{B7419E57-CFC5-49D0-8C23-F9873BBC3315}");
                public static readonly ID TypeOfApplication = new ID("{53B3DA2E-CE74-4249-9C0D-8BCB856767FA}");
                public static readonly ID TypeOfIndustry = new ID("{75FA8C5D-E4EB-4F2A-A3E6-6B3E615B48B7}");
                public static readonly ID CommercialFuelUsing = new ID("{99FD8D0D-9162-4480-B6A5-4D6FAC745354}");
                public static readonly ID IndustrialFuelUsing = new ID("{58D11F58-37C7-49CD-8D0E-BEA65A5FAF2B}");
            }
        }
        public struct PartnerType
        {
            public struct Datasource
            {
                public static readonly ID PartnerTypeList = new ID("{4CE3325B-C9B4-4132-9B2E-B8EBE223BC6A}");
            }
            public struct Fields
            {
                public static readonly ID residential = new ID("{9F7F5EB1-21CB-45D5-954B-317AD38C6D1F}");
                public static readonly ID commercial = new ID("{ED504B06-4DF4-405F-9175-3D10E4C1FD13}");
                public static readonly ID industrial = new ID("{860F99E8-94A5-4C43-A16D-6CFD63F8D0D2}");
            }
        }

        public struct CenterCity
        {
            public struct Datasource
            {
                public static readonly ID CenterCityList = new ID("{A971EBFC-4A59-4A5A-B7FB-F0EEF31B95E3}");
            }
        }
        public struct CenterType
        {
            public struct Datasource
            {
                public static readonly ID CenterTypeList = new ID("{6605E3E3-4FE4-4929-B973-E0D1D7603EC2}");
            }
        }
        public struct GasPricesInCity
        {
            public struct Datasource
            {
                public static readonly ID CenterCityList = new ID("{465932E6-A095-41B1-9CB1-C320E532BEA6}");
            }
        }


        public struct CNG_DODO_Occupation
        {
            public struct Datasource
            {
                public static readonly ID ListItems = new ID("{C464C374-FC3F-4A2F-BE81-5612300BD7B0}");
            }
        }

        public struct CNG_DODO_Salutation
        {
            public struct Datasource
            {
                public static readonly ID ListItems = new ID("{5104550F-3289-45D5-9DD5-14AF8B5EFF7A}");
            }
        }

        public struct CNG_DODO_Source
        {
            public struct Datasource
            {
                public static readonly ID ListItems = new ID("{EC8B12ED-0EF6-4F93-8D30-1F86D6A87EAB}");
            }
        }

        public struct CNG_DODO_State
        {
            public struct Datasource
            {
                public static readonly ID ListItems = new ID("{0576835E-5E49-47BF-B10D-D54B20E2DE2B}");
            }
        }

        public struct CNG_DODO_OMC
        {
            public struct Datasource
            {
                public static readonly ID ListItems = new ID("{CBBDD886-32F8-44C9-9B85-AA3FD768EF35}");
            }
        }

        public struct CNG_DODO_OMCYears
        {
            public struct Datasource
            {
                public static readonly ID ListItems = new ID("{4D62BDD0-E2D9-4A4A-A0B7-F6BD9AD1AB12}");
            }
        }

        public struct NameTransfer
        {
            public static readonly ID NameTransferAdminDashboard = new ID("{F83113E1-A229-4030-BBA4-57061A134E0F}");

            public static readonly ID NameTransferLogin = new ID("{D0F188D9-4F27-452C-9D2A-5109EA9EEE8F}");

            public static readonly ID NameTransferHome = new ID("{84F71B03-2D36-4DE6-866C-AD8A8BAF75DD}");

        }

    }

    public class BenowRequest
    {
        public BenowRequest() { }

        public string EncryptedString { get; set; }
        public string JsonString { get; set; }
    }

    public class PaymentRequest
    {
        public PaymentRequest() { }
        public string MerchantCode { get; set; }
        public decimal Amount { get; set; }
        public string RefNumber { get; set; }
        public string PaymentMethod { get; set; }
        public string PayerVPA { get; set; }
        public string Remarks { get; set; }
        public string DisplayName { get; set; }
    }

}