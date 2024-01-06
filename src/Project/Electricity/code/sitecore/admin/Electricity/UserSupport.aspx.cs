using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Electricity.Website.sitecore.admin.Electricity
{
    public partial class UserSupport : System.Web.UI.Page
    {
        TenderDataContext dbcontext = new TenderDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErroMsg.Visible = false;
            lblSuccessMsg.Visible = false;
            lblmsg.Text = string.Empty;
            if (!string.IsNullOrEmpty(txtuserName.Text))
            {
                string username = "electricity\\" + txtuserName.Text.Trim();
                BindGrid(username);
            }
            else
            {
                BindGrid("");
            }
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    var useravailable = new UserProfile();
                    string _username = string.Empty, _primaryaccountNumber = string.Empty, _meternumber = string.Empty;

                    #region Update Email in SiteCore User Profile.
                    if (!string.IsNullOrEmpty(txtuserName.Text))
                    {
                        _username = "electricity\\" + txtuserName.Text.Trim();
                    }
                    if (Security.Accounts.User.Exists(_username))
                    {
                        var sitecoreprofile = Security.Accounts.User.FromName(_username, true);
                        sitecoreprofile.Profile.Email = txtEmail.Text;
                        sitecoreprofile.Profile.Save();
                        Sitecore.Diagnostics.Log.Info("Support Form  - Email Update for User Account number:  " + txtAccountNumber.Text + " Username:" + _username, this);
                    }
                    #endregion

                    #region Insert record into the custom DB if record not available (check existing user using Meter Number,Account Number and UserName).
                    if (!string.IsNullOrEmpty(txtAccountNumber.Text))
                    {
                        _primaryaccountNumber = txtAccountNumber.Text;
                    }
                    if (!string.IsNullOrEmpty(txtMeterNumber.Text))
                    {
                        _meternumber = txtMeterNumber.Text;
                    }
                    if ((dbcontext.UserProfiles.Any(s => s.UserName == _username && s.IsPrimary == true)) && chkIsPrimary.Checked)
                    {
                        var userprofile = dbcontext.UserProfiles.Where(s => s.UserName == _username && s.IsPrimary == true).FirstOrDefault();
                        Sitecore.Diagnostics.Log.Info("Support Form  - User Profile update for Username:  " + _username, this);
                        Sitecore.Diagnostics.Log.Info("Support Form  - User Profile update for Accountnumber:  " + userprofile.AccountNumber, this);
                        userprofile.Email = txtEmail.Text;
                        userprofile.Mobile = txtMobile.Text;
                        userprofile.AccountNumber = _primaryaccountNumber;
                        userprofile.MeterNumber = _meternumber;
                        userprofile.ModifiedBy = "Support";
                        userprofile.Modified_Date = DateTime.Now;
                        dbcontext.SubmitChanges();
                        Sitecore.Diagnostics.Log.Info("Support Form  - User Profile update:  " + _primaryaccountNumber + "," + _meternumber + "," + txtMobile.Text + "," + txtEmail.Text, this);
                        lblSuccessMsg.Visible = true;
                        //lblErroMsg.Text = "One records already exist as primary account number";
                        //lblErroMsg.Visible = true;
                        BindGrid("");
                    }
                    else if ((!dbcontext.UserProfiles.Any(s => s.UserName == _username && s.IsPrimary == true)) && !chkIsPrimary.Checked)
                    {
                        lblErroMsg.Text = "No one records exist as primary account number, set first account as primary account";
                        lblErroMsg.Visible = true;
                        BindGrid("");
                    }
                    else
                    {
                        if (!dbcontext.UserProfiles.Any(s => s.UserName == _username && s.AccountNumber == _primaryaccountNumber))
                        {
                            UserProfile entity = new UserProfile()
                            {
                                Id = Guid.NewGuid(),
                                UserName = _username,
                                Email = txtEmail.Text,
                                Mobile = txtMobile.Text,
                                AccountNumber = _primaryaccountNumber,
                                MeterNumber = _meternumber,
                                IsPrimary = chkIsPrimary.Checked,
                                status = true,
                                Created_Date = DateTime.Now,
                                CreatedBy = "Support"
                            };
                            dbcontext.UserProfiles.InsertOnSubmit(entity);
                            dbcontext.SubmitChanges();
                            lblSuccessMsg.Visible = true;
                        }
                        else
                        {
                            lblErroMsg.Text = "Records already exist with given user name and account number";
                            lblErroMsg.Visible = true;
                            BindGrid("");
                        }
                    }
                    ResetForm();
                    #endregion 
                }
                else
                {
                    lblErroMsg.Text = "Inputs are not in correct format";
                    lblErroMsg.Visible = true;
                    BindGrid("");
                }
            }
            catch (Exception ex)
            {
                lblErroMsg.Text = "There is some technically problem. Please contact administrator.";
                lblErroMsg.Visible = true;
                Sitecore.Diagnostics.Log.Error("Support Form  - Error  " + ex.Message, this);
                ResetForm();
                BindGrid("");
            }
        }

        protected void btnSearchUser_Click(object sender, EventArgs e)
        {
            try
            {


                UserProfile profile = null;
                #region Serach Record From Custom DB or Sitecore Items based on UserName
                string _username = string.Empty, _email = string.Empty, _primaryaccountNumber = string.Empty, _meternumber = string.Empty;


                if (!string.IsNullOrEmpty(txtemailforuser.Text) && !string.IsNullOrEmpty(txtaccountforuser.Text))
                {
                    profile = dbcontext.UserProfiles.FirstOrDefault(s => s.Email == txtemailforuser.Text && s.AccountNumber == txtaccountforuser.Text && s.IsPrimary == true);
                    // txtemailforuser.Text = string.Empty;
                    // txtaccountforuser.Text = string.Empty;
                    txtuserName.Text = string.Empty;
                }
                else if (!string.IsNullOrEmpty(txtuserName.Text))
                {
                    _username = "electricity\\" + txtuserName.Text.Trim();
                    profile = dbcontext.UserProfiles.FirstOrDefault(s => s.UserName == _username && s.IsPrimary == true);
                    txtemailforuser.Text = string.Empty;
                    txtaccountforuser.Text = string.Empty;

                }
                if (profile != null)
                {
                    _username = profile.UserName.Split('\\').LastOrDefault();
                    txtuserName.Text = _username;
                    txtAccountNumber.Text = profile.AccountNumber;
                    txtMeterNumber.Text = profile.MeterNumber;
                    txtEmail.Text = profile.Email;
                    txtMobile.Text = profile.Mobile;
                    chkIsPrimary.Checked = true;
                    lblmsg.Text = "User avaliable.";

                    if (dbcontext.UserProfiles.Any(s => s.UserName == "electricity\\" + _username))
                    {
                        BindGrid("electricity\\" + _username);
                    }
                    else
                    {
                        BindGrid("");
                    }
                }
                // Note : Search Records from Sitecore User Items.
                else if (profile == null && !string.IsNullOrEmpty(_username) && Security.Accounts.User.Exists(_username))
                {
                    var sitecoreprofile = Security.Accounts.User.FromName(_username, true);
                    _email = sitecoreprofile.Profile.Email;
                    txtAccountNumber.Text = string.Empty;
                    txtMeterNumber.Text = string.Empty;
                    txtEmail.Text = _email;
                    txtMobile.Text = sitecoreprofile.Profile.GetCustomProperty("Mobile Number");
                    lblmsg.Text = "User avaliable in Sitecore. But not in DB.";
                }
                else
                {

                    lblErroMsg.Text = "No record exist in system.";
                    if (profile == null && !string.IsNullOrEmpty(txtemailforuser.Text) && !string.IsNullOrEmpty(txtaccountforuser.Text))
                    {
                        lblmsg.Text = "User not avaliable in DB. Please try with UserName.";
                    }
                    else if (profile == null && !string.IsNullOrEmpty(_username) && Security.Accounts.User.Exists(_username))
                    {
                        lblmsg.Text = "User not avaliable in DB and Sitecore.";
                    }
                    else
                    {
                        lblmsg.Text = "User not avaliable.";
                    }
                    lblErroMsg.Visible = true;
                    ResetForm();
                    BindGrid("");
                }

                #endregion
            }
            catch (Exception ex)
            {
                ResetForm();
                lblErroMsg.Text = "There is some technically problem. Please contact administrator.";
                lblErroMsg.Visible = true;
                Sitecore.Diagnostics.Log.Error("Support Form  - Error  " + ex.Message, this);
                BindGrid("");
            }
        }


        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string _username = "electricity\\" + txtuserName.Text.Trim();
            string Id = gvSecondaryAcList.DataKeys[e.RowIndex].Values[0].ToString();

            var entity = dbcontext.UserProfiles.FirstOrDefault(s => s.Id == new Guid(Id));
            //var entity = dbcontext.UserProfiles.FirstOrDefault(s => s.AccountNumber == AccNo && s.UserName == _username && s.MeterNumber == s.MeterNumber && s.IsPrimary == isprimary);
            dbcontext.UserProfiles.DeleteOnSubmit(entity);
            dbcontext.SubmitChanges();

            Sitecore.Diagnostics.Log.Info("Support Form  - Deleted Entry:  " + entity.AccountNumber + "," + entity.MeterNumber + "," + entity.UserName + "," + entity.IsPrimary, this);

            if (!string.IsNullOrEmpty(txtuserName.Text))
            {
                BindGrid(_username);
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.Cells[3].Controls[0] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this Account for this User?');";
            }
        }

        protected void gvSecondaryAcList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSecondaryAcList.PageIndex = e.NewPageIndex;
            if (!string.IsNullOrEmpty(txtuserName.Text))
            {
                string username = "electricity\\" + txtuserName.Text.Trim();
                BindGrid(username);
            }
        }


        private void BindGrid(string username)
        {
            DataSet dsSecondaryAcNums = new DataSet();
            //var secondarylist = dbcontext.UserProfiles.Where(s => s.IsPrimary == false && s.UserName == username).ToList();
            var secondarylist = dbcontext.UserProfiles.Where(s => s.UserName == username).ToList();
            var dt = ConvertListToDataTable(secondarylist);

            gvSecondaryAcList.DataSource = dt;
            gvSecondaryAcList.DataBind();
        }
        private DataTable ConvertListToDataTable(List<UserProfile> list)
        {
            // New table.
            DataTable dt = new DataTable();
            // Add columns.
            dt.Columns.Add("Id");
            dt.Columns.Add("AccountNumber");
            dt.Columns.Add("MeterNumber");
            dt.Columns.Add("AccountType");

            // Add rows.
            foreach (var item in list)
            {
                var row = dt.NewRow();
                row["Id"] = item.Id.ToString();
                row["AccountNumber"] = item.AccountNumber;
                row["MeterNumber"] = item.MeterNumber;
                row["AccountType"] = item.IsPrimary == true ? "Primary" : "Secondary";
                dt.Rows.Add(row);
            }
            return dt;
        }
        private void ResetForm()
        {
            txtaccountforuser.Text = string.Empty;
            txtemailforuser.Text = string.Empty;
            txtuserName.Text = string.Empty;
            txtAccountNumber.Text = string.Empty;
            txtMeterNumber.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtMobile.Text = string.Empty;

        }
    }
}