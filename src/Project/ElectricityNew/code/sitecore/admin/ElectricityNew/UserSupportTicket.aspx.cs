using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.ElectricityNew.Website.sitecore.admin.Electricity
{
    public partial class UserSupportTicket : System.Web.UI.Page
    {
        TenderDataContext dbcontext = new TenderDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErroMsg.Visible = false;
            lblSuccessMsg.Visible = false;
            if (!string.IsNullOrEmpty(txtaccountforuser.Text))
            {
                string accountNumber = txtaccountforuser.Text.Trim();
                BindGrid(accountNumber);
            }
            else
            {
                BindGrid("");
            }
        }


        protected void btnSearchUser_Click(object sender, EventArgs e)
        {
            try
            {
                string accountNumber = txtaccountforuser.Text.Trim();
                BindGrid(accountNumber);
            }
            catch (Exception ex)
            {
                lblErroMsg.Text = "There is some technically problem. Please contact administrator.";
                lblErroMsg.Visible = true;
                Sitecore.Diagnostics.Log.Error("Support Form  - Error  " + ex.Message, this);
                BindGrid("");
            }
        }

        protected void gvSecondaryAcList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSecondaryAcList.PageIndex = e.NewPageIndex;
            if (!string.IsNullOrEmpty(txtaccountforuser.Text))
            {
                string accountNumber = txtaccountforuser.Text.Trim();
                BindGrid(accountNumber);
            }
        }

        private void BindGrid(string accountNumber)
        {
            DataSet dsSecondaryAcNums = new DataSet();
            //var secondarylist = dbcontext.UserProfiles.Where(s => s.IsPrimary == false && s.UserName == username).ToList();
            var secondarylist = dbcontext.UserProfiles.Where(s => s.AccountNumber == accountNumber).ToList();
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
            dt.Columns.Add("UserName");
            dt.Columns.Add("Email");
            dt.Columns.Add("Mobile");


            // Add rows.
            foreach (var item in list)
            {
                var row = dt.NewRow();
                row["Id"] = item.Id.ToString();
                row["AccountNumber"] = item.AccountNumber;
                row["MeterNumber"] = item.MeterNumber;
                row["AccountType"] = item.IsPrimary == true ? "Primary" : "Secondary";
                row["UserName"] = item.UserName;
                row["Email"] = item.Email;
                row["Mobile"] = item.Mobile;
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}