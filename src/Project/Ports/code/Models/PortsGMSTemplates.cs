using Sitecore.Data;
using System;

namespace Sitecore.Ports.Website.Models
{
    [Serializable]
    public struct PortsGMSTemplates
    {
        

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
                PortsGMSTemplates.MailTemplate.ID = new ID("{325689B9-BC26-4F37-A8A6-8FE278F3CC0B}");
                PortsGMSTemplates.MailTemplate.EnvelopeUser = new ID("{1B6F1142-F7C9-4649-9045-2D13A77CFB9F}");
                PortsGMSTemplates.MailTemplate.AdminUser = new ID("{AAACDDBA-C713-412B-A4F2-99D5EBFB50A3}");
                PortsGMSTemplates.MailTemplate.EnvelopeUserOnTenderClose = new ID("{7C44384D-64C5-47F9-9106-1C759ED0F157}");
                PortsGMSTemplates.MailTemplate.EnvelopeUserReminderForTenderClose = new ID("{B5D2ED2F-E9EE-49FD-94F2-E68D1CED4E8B}");
                PortsGMSTemplates.MailTemplate.CorrigendumCreate = new ID("{5777282C-FCB2-4C08-8B78-6BB59C533FC4}");
                PortsGMSTemplates.MailTemplate.ContactUs = new ID("{D6C52163-01C2-480F-A069-AA5C1B9954F7}");
                PortsGMSTemplates.MailTemplate.PQApproval = new ID("{7A394273-079D-4CB7-A7BA-F89377BB172F}");
                PortsGMSTemplates.MailTemplate.PQRejection = new ID("{33643DF7-E631-4D42-AF4F-F912787E864A}");
                PortsGMSTemplates.MailTemplate.sendForPQApproval = new ID("{2397A3FB-D387-4564-8382-AB0AD036B3DA}");
                PortsGMSTemplates.MailTemplate.sendForTenderDocApproval = new ID("{8423E95D-FD6B-415F-8468-C69EAD1DBB74}");
            }

            public struct Fields
            {
                public readonly static ID From;

                public readonly static ID Subject;

                public readonly static ID Body;

                static Fields()
                {
                    PortsGMSTemplates.MailTemplate.Fields.From = new ID("{8605948C-60FB-46B8-8AAA-4C52561B53BC}");
                    PortsGMSTemplates.MailTemplate.Fields.Subject = new ID("{0F45DF05-546F-462D-97C0-BA4FB2B02564}");
                    PortsGMSTemplates.MailTemplate.Fields.Body = new ID("{1519CCAD-ED26-4F60-82CA-22079AF44D16}");
                }
            }
        }
        public struct GMSFlags {
            public readonly static string Registration;
            public readonly static string Booking;
            public readonly static string BookingOnBehalf;
            public readonly static string RePendings;
            public readonly static string ReApproved;
            public readonly static string ReRejected;
            public readonly static string ReOverdue;
            public readonly static string BookingOpen;
            public readonly static string BookingReOpen;
            public readonly static string BookingClosed;
            public readonly static string BookingDraft;
            public readonly static string Completed;
            public readonly static string Submit;
            public readonly static string Response;


            static GMSFlags() {
                PortsGMSTemplates.GMSFlags.Registration = "Registration";
                PortsGMSTemplates.GMSFlags.Booking = "Booking";
                PortsGMSTemplates.GMSFlags.BookingOnBehalf = "BookingOnBehalf";
                PortsGMSTemplates.GMSFlags.RePendings = "Pending";
                PortsGMSTemplates.GMSFlags.ReRejected = "Rejected";
                PortsGMSTemplates.GMSFlags.ReApproved = "Approved";
                PortsGMSTemplates.GMSFlags.ReOverdue = "Overdue";
                PortsGMSTemplates.GMSFlags.BookingOpen = "Open";
                PortsGMSTemplates.GMSFlags.BookingReOpen = "Re-Open";
                PortsGMSTemplates.GMSFlags.BookingClosed = "Closed";
                PortsGMSTemplates.GMSFlags.BookingDraft = "Draft";
                PortsGMSTemplates.GMSFlags.Completed = "Completed";
                PortsGMSTemplates.GMSFlags.Submit = "Submit";
                PortsGMSTemplates.GMSFlags.Response = "Responsed";
            }
        }
        public struct GrievanceStatus
        {
            public readonly static string Level0;
            public readonly static string Level1;
            public readonly static string Level2;
            public readonly static string Level3;
            public readonly static string ReassignLevel0;
            public readonly static string Reassign;
            public readonly static string ReassignRequestLevel0;
            public readonly static string Reopen;
            public readonly static string Completed;




            static GrievanceStatus()
            {
                PortsGMSTemplates.GrievanceStatus.Level1 = "Assign to Level0";
                PortsGMSTemplates.GrievanceStatus.Level1 = "Assign to Level1";
                PortsGMSTemplates.GrievanceStatus.Level2 = "Assign to Level2";
                PortsGMSTemplates.GrievanceStatus.Level3 = "Assign to Level3";
                PortsGMSTemplates.GrievanceStatus.ReassignLevel0 = "Re-assign to Level0";
                PortsGMSTemplates.GrievanceStatus.ReassignRequestLevel0 = "Re-assign request to Level0";
                PortsGMSTemplates.GrievanceStatus.Reassign = "Re-assign";
                PortsGMSTemplates.GrievanceStatus.Reopen = "Re-Open";
                PortsGMSTemplates.GrievanceStatus.Completed = "Closed";

            }
        }
        public struct UserType {
            public readonly static string Level0;
            public readonly static string Level1;
            public readonly static string Level2;
            public readonly static string Level3;
            public readonly static string SusCell;
            public readonly static string Stakeholder;
            public readonly static string Admin;

            static UserType(){

                PortsGMSTemplates.UserType.Level0 = "level0";
                PortsGMSTemplates.UserType.Level1 = "level1";
                PortsGMSTemplates.UserType.Level2 = "level2";
                PortsGMSTemplates.UserType.Level3 = "level3";
                PortsGMSTemplates.UserType.SusCell = "sustainability";
                PortsGMSTemplates.UserType.Stakeholder = "stakeholder";
                PortsGMSTemplates.UserType.Admin = "admin";
            }

        }
       

        public struct Grievance
        {
            public readonly static ID GrievanceLogin;

            public readonly static ID GrievanceRegistration;

            public readonly static ID GrievanceBooking;

            public readonly static ID GrievanceBookingOnBehalf;
            public readonly static ID GrievanceBookingOnBehalf0;

            public readonly static ID GrievanceAddUser;
            public readonly static ID GrievanceUpdateUser;

            public readonly static ID GrievanceManageUser;

            public readonly static ID GrievanceStakHolderDashbord;

            public readonly static ID GrievanceAdminDashbord;
            public readonly static ID GrievanceAdminGrievanceView;
            public readonly static ID GrievanceStakeHolderGrievanceView;

            public readonly static ID GrievanceLevel0Dashbord;
            public readonly static ID GrievanceLevel1Dashbord;

            public readonly static ID GrievanceLevel2Dashbord;

            public readonly static ID GrievanceLevel3Dashbord;

            public readonly static ID GrievanceLevel0Reply;
            public readonly static ID GrievanceLevel1Reply;

            public readonly static ID GrievanceLevel2Reply;

            public readonly static ID GrievanceLevel3Reply;

            public readonly static ID GrievanceMatrix;

            public readonly static ID GrievanceRelocation;

            public readonly static ID GrievanceCorprateUserRegistration;

            public readonly static ID GrievanceBookingOnBehalfReview;
            public readonly static ID GrievanceBookingOnBehalfSubmission;
            public readonly static ID PortsGmsGrievanceBookingOnBehalfStackholderSubmission;

            static Grievance()
            {
                PortsGMSTemplates.Grievance.GrievanceLogin = new ID("{2500B622-ACD3-40DE-88E9-67D560BCC4CD}");
                PortsGMSTemplates.Grievance.GrievanceRegistration = new ID("{02F239B8-3503-4A53-AD26-58DF4188B990}");
                PortsGMSTemplates.Grievance.GrievanceCorprateUserRegistration = new ID("{174EA9B0-AC3A-4C7A-95F2-BF4EDBB89AE1}");
                PortsGMSTemplates.Grievance.GrievanceAddUser = new ID("{5E4C3B3D-407E-4EEC-86AC-291C3B410933}");
                PortsGMSTemplates.Grievance.GrievanceUpdateUser = new ID("{46590A46-3BD6-4953-A739-84AEC5DD69E2}");
                PortsGMSTemplates.Grievance.GrievanceBooking = new ID("{669D8010-CBA0-462D-9E72-3A5952A2AA9B}");
                PortsGMSTemplates.Grievance.GrievanceAdminDashbord = new ID("{62D2A778-1A83-4217-B260-4608DE5CF7CD}");
                PortsGMSTemplates.Grievance.GrievanceAdminGrievanceView = new ID("{3DD4006C-57D1-4A59-A7FC-B4ED6C7E88A8}");
                PortsGMSTemplates.Grievance.GrievanceStakeHolderGrievanceView = new ID("{4FFE7A35-E3AF-4108-8A11-42B1E8574C2A}");

                PortsGMSTemplates.Grievance.GrievanceStakHolderDashbord = new ID("{E8622073-F867-442C-97B3-6B26D68F071E}");
                PortsGMSTemplates.Grievance.GrievanceLevel0Dashbord = new ID("{48AA83D2-5BC9-4F24-89B1-53BC36B42C37}");
                PortsGMSTemplates.Grievance.GrievanceLevel1Dashbord = new ID("{B1FA82A9-C0B7-48B1-A8EA-F0D8BCC589A7}");
                PortsGMSTemplates.Grievance.GrievanceLevel2Dashbord = new ID("{5634D5F0-D027-4B8A-8048-7B0F3E462C17}");
                PortsGMSTemplates.Grievance.GrievanceLevel3Dashbord = new ID("{307A4DAF-1B34-4381-959D-68DDDB12079F}");
                PortsGMSTemplates.Grievance.GrievanceBookingOnBehalf = new ID("{96108962-92A9-40E6-A2FD-EA10F97F1359}");
                PortsGMSTemplates.Grievance.GrievanceBookingOnBehalf0 = new ID("{F00823AE-017C-4459-9F03-01687806DF62}");
                PortsGMSTemplates.Grievance.GrievanceManageUser = new ID("{FA389A47-4FBC-4DC8-9C58-E99A92C2D4F8}");
                PortsGMSTemplates.Grievance.GrievanceLevel0Reply = new ID("{E3122C20-05D5-4960-A613-D342E89643BB}");
                PortsGMSTemplates.Grievance.GrievanceLevel1Reply = new ID("{E9B9C1BE-1862-4F54-985F-77B2531CA2C4}");
                PortsGMSTemplates.Grievance.GrievanceLevel2Reply = new ID("{9D0D9D0E-9A4F-4E50-83A5-2A775CE0EAEA}");
                PortsGMSTemplates.Grievance.GrievanceLevel3Reply = new ID("{4C44FB51-2874-4C07-AEA6-7B04C95F8764}");
                PortsGMSTemplates.Grievance.GrievanceBookingOnBehalfReview = new ID("{84DAAB56-028B-46D1-80F5-4475047C0437}");
                PortsGMSTemplates.Grievance.PortsGmsGrievanceBookingOnBehalfStackholderSubmission = new ID("{92C7D495-4417-45EA-8D74-1ECF8F9217A9}");

            }
        }


        public struct MasterData
        {
            public readonly static ID GrievanceNature;

           

            static MasterData()
            {
                PortsGMSTemplates.MasterData.GrievanceNature = new ID("{39DA8970-3944-4F15-9A7D-7E9157ACB4F2}");
                
            }
        }
    }
}