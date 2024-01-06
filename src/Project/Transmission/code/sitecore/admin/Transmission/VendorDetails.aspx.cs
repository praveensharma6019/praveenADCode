using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;

namespace Sitecore.Transmission.Website.sitecore.admin.Transmission
{
    public partial class VendorDetails : System.Web.UI.Page
    {
        TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            //logout.ServerClick += new EventHandler(LogOut_Click);
            string RegNo = Request.QueryString["id"].ToString();
            var InquiryRecord = (from rc in rdb.TransmissionVendorEnquiryForms
                                 where (rc.RegistrationNo == RegNo)
                                 select rc).FirstOrDefault();
            registrationNo.Text = InquiryRecord.RegistrationNo;
            ComName.Text = InquiryRecord.CompanyName;
            name.Text = InquiryRecord.Name;
            EmailId.Text = InquiryRecord.Email;
            Message.Text = InquiryRecord.Message;
            ContactNo.Text = InquiryRecord.MobileNo;
            if (InquiryRecord.CurrentStatusPhase2 != null)
                CurrentStatus.Text = InquiryRecord.CurrentStatusPhase2;
            else
                CurrentStatus.Text = InquiryRecord.CurrentStatusPhase1;

            var RemarkHistory = InquiryRecord.ActionRemark != null ? InquiryRecord.ActionRemark.Split('|').ToList() : null;
            var UpdateOnHistory = InquiryRecord.UpdateDateTimeHistory != null ? InquiryRecord.UpdateDateTimeHistory.Split('|').ToList() : null;
            var UpdatedByHistory = InquiryRecord.StatusUpdateHistory != null ? InquiryRecord.StatusUpdateHistory.Split('|').ToList() : null;
            if (InquiryRecord.CurrentStatusPhase1.ToLower() == "pending for review")
            {
                selectStatus.Items.Add(new ListItem("Approve at Level 1", "Approved at Level 1"));
                selectStatus.Items.Add(new ListItem("Not suitable at Level 1", "Not suitable at Level 1"));
                Level1.Attributes.Add("style", "display:none;");
                Level2.Attributes.Add("style", "display:none;");
                Level3.Attributes.Add("style", "display:none;");
                
            }
            else if (InquiryRecord.CurrentStatusPhase1.ToLower() == "not suitable at level 1")
            {                
                Level2.Attributes.Add("style", "display:none;");
                Level3.Attributes.Add("style", "display:none;");
                L1Remark.Text = RemarkHistory != null && RemarkHistory.Count > 0 ? RemarkHistory[0] : "N/A";
                L1Status.Text = InquiryRecord.CurrentStatusPhase1;
                L1UpdatedBy.Text = UpdatedByHistory != null && UpdatedByHistory.Count > 0 ? UpdatedByHistory[0] : "N/A";
                L1UpdatedOn.Text = UpdateOnHistory != null && UpdateOnHistory.Count > 0 ? UpdateOnHistory[0] : "N/A";
            }
            else if(InquiryRecord.CurrentStatusPhase1.ToLower() == "approved at level 1" && InquiryRecord.CurrentStatusPhase2.ToLower() == "pending for review at level 2")
            {
                selectStatus.Items.Add(new ListItem("Approve at Level 2", "Approved at Level 2"));
                selectStatus.Items.Add(new ListItem("Reject at Level 2", "Rejected at Level 2"));
                selectStatus.Items.Add(new ListItem("Reassess after 6 Months", "Reassess after 6 Months"));
                Level2.Attributes.Add("style", "display:none;");
                Level3.Attributes.Add("style", "display:none;");
                L1Remark.Text = RemarkHistory != null && RemarkHistory.Count > 0 ? RemarkHistory[0]:"N/A";
                L1Status.Text = InquiryRecord.CurrentStatusPhase1;
                L1UpdatedBy.Text = UpdatedByHistory != null && UpdatedByHistory.Count > 0 ? UpdatedByHistory[0] : "N/A";
                L1UpdatedOn.Text = UpdateOnHistory != null && UpdateOnHistory.Count > 0 ? UpdateOnHistory[0] : "N/A";
            }
            else if (InquiryRecord.CurrentStatusPhase2.ToLower() == "approved at level 2" || InquiryRecord.CurrentStatusPhase2.ToLower() == "rejected at level 2")
            {
                Level3.Attributes.Add("style", "display:none;");
                L1Remark.Text = RemarkHistory != null && RemarkHistory.Count > 0 ? RemarkHistory[0] : "N/A";
                L2Remark.Text = RemarkHistory != null && RemarkHistory.Count > 0 ? RemarkHistory[1] : "N/A";
                L1Status.Text = InquiryRecord.CurrentStatusPhase1;
                L2Status.Text = InquiryRecord.CurrentStatusPhase2;
                L1UpdatedBy.Text = UpdatedByHistory != null && UpdatedByHistory.Count > 0 ? UpdatedByHistory[0] : "N/A";
                L2UpdatedBy.Text = UpdatedByHistory != null && UpdatedByHistory.Count > 1 ? UpdatedByHistory[1] : "N/A";
                L1UpdatedOn.Text = UpdateOnHistory != null && UpdateOnHistory.Count > 0 ? UpdateOnHistory[0] : "N/A";
                L2UpdatedOn.Text = UpdateOnHistory != null && UpdateOnHistory.Count > 1 ? UpdateOnHistory[1] : "N/A";
            }
            else if (InquiryRecord.CurrentStatusPhase2.ToLower() == "reassess after 6 months")
            {
                selectStatus.Items.Add(new ListItem("Approve after reassessment", "Approved after reassessment"));
                selectStatus.Items.Add(new ListItem("Reject after reassessment", "Rejected after reassessment"));
                Level3.Attributes.Add("style", "display:none;");
                L1Remark.Text = RemarkHistory != null && RemarkHistory.Count > 0 ? RemarkHistory[0] : "N/A";
                L2Remark.Text = RemarkHistory != null && RemarkHistory.Count > 1 ? RemarkHistory[1] : "N/A";
                L1Status.Text = InquiryRecord.CurrentStatusPhase1;
                L2Status.Text = InquiryRecord.CurrentStatusPhase2;
                L1UpdatedBy.Text = UpdatedByHistory != null && UpdatedByHistory.Count > 0 ? UpdatedByHistory[0] : "N/A";
                L2UpdatedBy.Text = UpdatedByHistory != null && UpdatedByHistory.Count > 1 ? UpdatedByHistory[1] : "N/A";
                L1UpdatedOn.Text = UpdateOnHistory != null && UpdateOnHistory.Count > 0 ? UpdateOnHistory[0] : "N/A";
                L2UpdatedOn.Text = UpdateOnHistory != null && UpdateOnHistory.Count > 1 ? UpdateOnHistory[1] : "N/A";
            }
            else
            {
                L1Remark.Text = RemarkHistory != null && RemarkHistory.Count > 0 ? RemarkHistory[0] : "N/A";
                L2Remark.Text = RemarkHistory != null && RemarkHistory.Count > 1 ? RemarkHistory[1] : "N/A";
                L3Remark.Text = RemarkHistory != null && RemarkHistory.Count > 2 ? RemarkHistory[2] : "N/A";
                L1Status.Text = InquiryRecord.CurrentStatusPhase1;
                L2Status.Text = InquiryRecord.CurrentStatusPhase2;
                L3Status.Text = InquiryRecord.CurrentStatusPhase2;
                L1UpdatedBy.Text = UpdatedByHistory != null && UpdatedByHistory.Count > 0 ? UpdatedByHistory[0] : "N/A";
                L2UpdatedBy.Text = UpdatedByHistory != null && UpdatedByHistory.Count > 1 ? UpdatedByHistory[1] : "N/A";
                L3UpdatedBy.Text = UpdatedByHistory != null && UpdatedByHistory.Count > 2 ? UpdatedByHistory[2] : "N/A";
                L1UpdatedOn.Text = UpdateOnHistory != null && UpdateOnHistory.Count > 0 ? UpdateOnHistory[0] : "N/A";
                L2UpdatedOn.Text = UpdateOnHistory != null && UpdateOnHistory.Count > 1 ? UpdateOnHistory[1] : "N/A";
                L3UpdatedOn.Text = UpdateOnHistory != null && UpdateOnHistory.Count > 2 ? UpdateOnHistory[2] : "N/A";
            }
            if(InquiryRecord.CurrentStatusPhase2 != null)
            {
                if (InquiryRecord.CurrentStatusPhase2.ToLower() == "approved at level 2" || InquiryRecord.CurrentStatusPhase2.ToLower() == "rejected at level 2")
                {
                    Level3.Attributes.Add("style", "display:none;");
                }
            }
            
            if (InquiryRecord.InqTicketStatus.ToLower() == "closed")
            {
                SelectInqDiv.Attributes.Add("style", "display:none;");
                RemarkInqDiv.Attributes.Add("style", "display:none;");
                Updatebtn.Attributes.Add("style", "display:none;");
            }
            MessageType.Text = InquiryRecord.MessageType;
            InqCreatedOn.Text = InquiryRecord.CreatedOn.ToString();
            
            Session["RegistrationNo"] = InquiryRecord.RegistrationNo;
        }
        protected void UpdateInq_Click(object sender, EventArgs e)
        {
            var RegNo = (string)(Session["RegistrationNo"]);
            var InquiryRecord = (from rc in rdb.TransmissionVendorEnquiryForms
                                 where (rc.RegistrationNo == RegNo)
                                 select rc).FirstOrDefault();
            //if(string.IsNullOrEmpty(Remarks.Text))
            //{
            //    LabelError.Text = "Please enter any remarks to update status.";
            //    return;
            //   // InquiryRecord.ActionRemark = "";
            //}
          
            InquiryRecord.ActionRemark = InquiryRecord.ActionRemark != null? InquiryRecord.ActionRemark + " | " + Remarks.Text: Remarks.Text;
            

            if (selectStatus.SelectedIndex != 0)
            {
                Sitecore.Data.Database targetDB = Sitecore.Configuration.Factory.GetDatabase("web");
                Item mailconfig = targetDB.GetItem(Templates.MailConfiguration.MailConfigurationItemID);
                if (InquiryRecord.CurrentStatusPhase1 != null && selectStatus.SelectedValue.ToLower() == "approved at level 1")
                {
                    InquiryRecord.CurrentStatusPhase1 = selectStatus.SelectedValue;
                    InquiryRecord.CurrentStatusPhase2 = "Pending for Review at Level 2";
                    Log.Info("Vendor Validated mail sending to client", this);
                    //Send mail
                    string CustomerFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
                    string CustomerTo = InquiryRecord.Email;
                    string CustomerSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
                    string value = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Vendor_L1ApproveMessage].Value;
                    string CustomerMailBody = value;
                    CustomerMailBody = CustomerMailBody.Replace("$name", InquiryRecord.Name);
                    CustomerMailBody = CustomerMailBody.Replace("$companyname", InquiryRecord.CompanyName);
                    CustomerMailBody = CustomerMailBody.Replace("$mobile", InquiryRecord.MobileNo);
                    CustomerMailBody = CustomerMailBody.Replace("$mail", InquiryRecord.Email);
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", InquiryRecord.RegistrationNo);
                    CustomerMailBody = CustomerMailBody.Replace("$remarks", InquiryRecord.ActionRemark);
                    CustomerMailBody = CustomerMailBody.Replace("$currentStatus", selectStatus.SelectedValue);
                    if (!this.sendEmail(CustomerTo, CustomerSubject, CustomerMailBody, CustomerFrom))
                    {
                        Log.Info("Sending mail to customer is Failed", this);
                    }
                    else
                    {
                        Log.Info("Sending mail to customer is Successfull", this);
                    }
                }
                else if (InquiryRecord.CurrentStatusPhase1 != null && selectStatus.SelectedValue.ToLower() == "not suitable at level 1")
                {
                    InquiryRecord.CurrentStatusPhase1 = selectStatus.SelectedValue;
                    InquiryRecord.InqTicketStatus = "Closed";
                    //Send mail
                    Log.Info("Vendor Validated mail sending to client", this);
                    string CustomerFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
                    string CustomerTo = InquiryRecord.Email;
                    string CustomerSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
                    string value = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Vendor_L1RejectMessage].Value;
                    string CustomerMailBody = value;
                    CustomerMailBody = CustomerMailBody.Replace("$name", InquiryRecord.Name);
                    CustomerMailBody = CustomerMailBody.Replace("$companyname", InquiryRecord.CompanyName);
                    CustomerMailBody = CustomerMailBody.Replace("$mobile", InquiryRecord.MobileNo);
                    CustomerMailBody = CustomerMailBody.Replace("$mail", InquiryRecord.Email);
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", InquiryRecord.RegistrationNo);
                    CustomerMailBody = CustomerMailBody.Replace("$remarks", InquiryRecord.ActionRemark);
                    CustomerMailBody = CustomerMailBody.Replace("$currentStatus", selectStatus.SelectedValue);
                    if (!this.sendEmail(CustomerTo, CustomerSubject, CustomerMailBody, CustomerFrom))
                    {
                        Log.Info("Sending mail to customer is Failed", this);
                    }
                    else
                    {
                        Log.Info("Sending mail to customer is Successfull", this);
                    }
                }
                else if (InquiryRecord.CurrentStatusPhase2 != null && selectStatus.SelectedValue.ToLower() == "approved at level 2")
                {
                    InquiryRecord.CurrentStatusPhase2 = selectStatus.SelectedValue;
                    InquiryRecord.InqTicketStatus = "Closed";
                    //Send Mail
                    Log.Info("Vendor Validated mail sending to client", this);
                    string CustomerFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
                    string CustomerTo = InquiryRecord.Email;
                    string CustomerSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
                    string value = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Vendor_L2ApproveMessage].Value;
                    string CustomerMailBody = value;
                    CustomerMailBody = CustomerMailBody.Replace("$name", InquiryRecord.Name);
                    CustomerMailBody = CustomerMailBody.Replace("$companyname", InquiryRecord.CompanyName);
                    CustomerMailBody = CustomerMailBody.Replace("$mobile", InquiryRecord.MobileNo);
                    CustomerMailBody = CustomerMailBody.Replace("$mail", InquiryRecord.Email);
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", InquiryRecord.RegistrationNo);
                    CustomerMailBody = CustomerMailBody.Replace("$remarks", InquiryRecord.ActionRemark);
                    CustomerMailBody = CustomerMailBody.Replace("$currentStatus", selectStatus.SelectedValue);
                    if (!this.sendEmail(CustomerTo, CustomerSubject, CustomerMailBody, CustomerFrom))
                    {
                        Log.Info("Sending mail to customer is Failed", this);
                    }
                    else
                    {
                        Log.Info("Sending mail to customer is Successfull", this);
                    }
                }
                else if (InquiryRecord.CurrentStatusPhase2 != null && selectStatus.SelectedValue.ToLower() == "rejected at level 2")
                {
                    InquiryRecord.CurrentStatusPhase2 = selectStatus.SelectedValue;
                    InquiryRecord.InqTicketStatus = "Closed";
                    //Send Mail
                    Log.Info("Vendor Validated mail sending to client", this);
                    string CustomerFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
                    string CustomerTo = InquiryRecord.Email;
                    string CustomerSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
                    string value = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Vendor_L2RejectMessage].Value;
                    string CustomerMailBody = value;
                    CustomerMailBody = CustomerMailBody.Replace("$name", InquiryRecord.Name);
                    CustomerMailBody = CustomerMailBody.Replace("$companyname", InquiryRecord.CompanyName);
                    CustomerMailBody = CustomerMailBody.Replace("$mobile", InquiryRecord.MobileNo);
                    CustomerMailBody = CustomerMailBody.Replace("$mail", InquiryRecord.Email);
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", InquiryRecord.RegistrationNo);
                    CustomerMailBody = CustomerMailBody.Replace("$remarks", InquiryRecord.ActionRemark);
                    CustomerMailBody = CustomerMailBody.Replace("$currentStatus", selectStatus.SelectedValue);
                    if (!this.sendEmail(CustomerTo, CustomerSubject, CustomerMailBody, CustomerFrom))
                    {
                        Log.Info("Sending mail to customer is Failed", this);
                    }
                    else
                    {
                        Log.Info("Sending mail to customer is Successfull", this);
                    }
                }
                else if (InquiryRecord.CurrentStatusPhase2 != null && selectStatus.SelectedValue.ToLower() == "reassess after 6 months")
                {
                    InquiryRecord.CurrentStatusPhase2 = selectStatus.SelectedValue;
                    InquiryRecord.InqTicketStatus = "Open";
                    //Send Mail
                    Log.Info("Vendor Validated mail sending to client", this);
                    string CustomerFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
                    string CustomerTo = InquiryRecord.Email;
                    string CustomerSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
                    string value = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Vendor_L2ReassessMessage].Value;
                    string CustomerMailBody = value;
                    CustomerMailBody = CustomerMailBody.Replace("$name", InquiryRecord.Name);
                    CustomerMailBody = CustomerMailBody.Replace("$companyname", InquiryRecord.CompanyName);
                    CustomerMailBody = CustomerMailBody.Replace("$mobile", InquiryRecord.MobileNo);
                    CustomerMailBody = CustomerMailBody.Replace("$mail", InquiryRecord.Email);
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", InquiryRecord.RegistrationNo);
                    CustomerMailBody = CustomerMailBody.Replace("$remarks", InquiryRecord.ActionRemark);
                    CustomerMailBody = CustomerMailBody.Replace("$currentStatus", selectStatus.SelectedValue);
                    if (!this.sendEmail(CustomerTo, CustomerSubject, CustomerMailBody, CustomerFrom))
                    {
                        Log.Info("Sending mail to customer is Failed", this);
                    }
                    else
                    {
                        Log.Info("Sending mail to customer is Successfull", this);
                    }
                }
                else if (InquiryRecord.CurrentStatusPhase2 != null && selectStatus.SelectedValue.ToLower() == "approved after reassessment")
                {
                    InquiryRecord.CurrentStatusPhase2 = selectStatus.SelectedValue;
                    InquiryRecord.InqTicketStatus = "Closed";
                    //Send Mail
                    Log.Info("Vendor Validated mail sending to client", this);
                    string CustomerFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
                    string CustomerTo = InquiryRecord.Email;
                    string CustomerSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
                    string value = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Vendor_ReassessAproveMessage].Value;
                    string CustomerMailBody = value;
                    CustomerMailBody = CustomerMailBody.Replace("$name", InquiryRecord.Name);
                    CustomerMailBody = CustomerMailBody.Replace("$companyname", InquiryRecord.CompanyName);
                    CustomerMailBody = CustomerMailBody.Replace("$mobile", InquiryRecord.MobileNo);
                    CustomerMailBody = CustomerMailBody.Replace("$mail", InquiryRecord.Email);
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", InquiryRecord.RegistrationNo);
                    CustomerMailBody = CustomerMailBody.Replace("$remarks", InquiryRecord.ActionRemark);
                    CustomerMailBody = CustomerMailBody.Replace("$currentStatus", selectStatus.SelectedValue);
                    if (!this.sendEmail(CustomerTo, CustomerSubject, CustomerMailBody, CustomerFrom))
                    {
                        Log.Info("Sending mail to customer is Failed", this);
                    }
                    else
                    {
                        Log.Info("Sending mail to customer is Successfull", this);
                    }
                }
                else if (InquiryRecord.CurrentStatusPhase2 != null && selectStatus.SelectedValue.ToLower() == "rejected after reassessment")
                {
                    InquiryRecord.CurrentStatusPhase2 = selectStatus.SelectedValue;
                    InquiryRecord.InqTicketStatus = "Closed";
                    //Send Mail
                    Log.Info("Vendor Validated mail sending to client", this);
                    string CustomerFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
                    string CustomerTo = InquiryRecord.Email;
                    string CustomerSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
                    string value = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Vendor_ReassessRejectMessage].Value;
                    string CustomerMailBody = value;
                    CustomerMailBody = CustomerMailBody.Replace("$name", InquiryRecord.Name);
                    CustomerMailBody = CustomerMailBody.Replace("$companyname", InquiryRecord.CompanyName);
                    CustomerMailBody = CustomerMailBody.Replace("$mobile", InquiryRecord.MobileNo);
                    CustomerMailBody = CustomerMailBody.Replace("$mail", InquiryRecord.Email);
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", InquiryRecord.RegistrationNo);
                    CustomerMailBody = CustomerMailBody.Replace("$remarks", InquiryRecord.ActionRemark);
                    CustomerMailBody = CustomerMailBody.Replace("$currentStatus", selectStatus.SelectedValue);
                    if (!this.sendEmail(CustomerTo, CustomerSubject, CustomerMailBody, CustomerFrom))
                    {
                        Log.Info("Sending mail to customer is Failed", this);
                    }
                    else
                    {
                        Log.Info("Sending mail to customer is Successfull", this);
                    }
                }
            }
            else
            {
                LabelError.Text = "Please select any option.";
                return;
            }
            InquiryRecord.LastUpdatedOn = DateTime.Now;
            InquiryRecord.LastUpdatedBy = Sitecore.Context.User.Name.Replace("sitecore\\", "");
            InquiryRecord.StatusUpdateHistory = InquiryRecord.StatusUpdateHistory != null? InquiryRecord.StatusUpdateHistory + " | "+Sitecore.Context.User.Name.Replace("sitecore\\", "") : Sitecore.Context.User.Name.Replace("sitecore\\","");
            InquiryRecord.UpdateCount = InquiryRecord.UpdateCount != null ? InquiryRecord.UpdateCount.Value + 1 : 1;
            InquiryRecord.UpdateDateTimeHistory = InquiryRecord.UpdateDateTimeHistory != null ? InquiryRecord.UpdateDateTimeHistory+" | "+ DateTime.Now.ToString() : DateTime.Now.ToString();
            rdb.SubmitChanges();
            Session["RegistrationNo"] = InquiryRecord.RegistrationNo;
            Response.Redirect("/sitecore/admin/transmission/VendorDetails.aspx?id="+InquiryRecord.RegistrationNo);
        }
        public bool sendEmail(string to, string subject, string body, string from)
        {
            bool flag;
            bool status = false;
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(from)
                };
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                ContentType ct = new ContentType("application/pdf");
                mail.From = new MailAddress(from);
                MainUtil.SendMail(mail);
                status = true;
                flag = status;
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Console.WriteLine(ex.Message, "sendEmail - ");
                Log.Error(ex.Message, "sendEmail - ");
                flag = status;
            }
            return flag;
        }
        protected void LogOut_Click(object sender, EventArgs e)
        {
            if(Sitecore.Context.User.IsAuthenticated)
                FormsAuthentication.SignOut();
            Response.Redirect("/Sitecore/login?ReturnUrl=%2fsitecore%2fadmin%2ftransmission%2fVendorAdminDashboard.aspx");
        }
    }
}