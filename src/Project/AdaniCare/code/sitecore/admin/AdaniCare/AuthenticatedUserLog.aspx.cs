using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Collections;
using Sitecore.Data;
using System.Net;
using System.IO;
using Sitecore.Resources.Media;
using Sitecore.Configuration;
using Sitecore.SecurityModel;
using Sitecore.Links;
using Sitecore;
using System.Text.RegularExpressions;
using Sitecore.AdaniCare.Website.Models;
using System.Linq.Expressions;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions;
using System.Reflection;
using System.Diagnostics;
using System.Text;

namespace Sitecore.AdaniCare.Website.sitecore.admin.AdaniCare
{
    public partial class AuthenticatedUserLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            lblErroMsg.Text = "";
            lblErroMsg.Visible = false;
            if (!IsPostBack)
            {

            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            UserDataContextDataContext dc = new UserDataContextDataContext();

            try
            {
                lblErroMsg.Text = "";
                lblErroMsg.Visible = false;

                var predicateQuery =dc.AuthenticatedUsersLogs.AsQueryable();
                if (!string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
                {
                    lblErroMsg.Text = "Please select To date.";
                    lblErroMsg.Visible = true;
                    return;
                }

                if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
                {
                    if (System.Convert.ToDateTime(txtToDate.Text) < System.Convert.ToDateTime(txtFromDate.Text))
                    {
                        lblErroMsg.Text = "Please select proper date";
                        lblErroMsg.Visible = true;
                       
                        return;
                    }
                    predicateQuery = predicateQuery.Where(p => p.CreatedDate >= System.Convert.ToDateTime(txtFromDate.Text) && p.CreatedDate <= System.Convert.ToDateTime(txtToDate.Text));
                }

                if (predicateQuery != null && predicateQuery.Any())
                {

                    lblErroMsg.Text = "Records download Successfully";

                    var AuthenticatedUserLog = predicateQuery.ToList().Select(x => new AuthenticatedUsersLog()
                    {
                        ID=x.ID,
                       ConsumerName=x.ConsumerName,
                       AccountNumber=x.AccountNumber,
                       MobileNumber=x.MobileNumber,
                       ConsumerEmail=x.ConsumerEmail,
                       CreatedDate=x.CreatedDate,
                        

                    }).ToList();

                    DataTable dt = ToDataTable(AuthenticatedUserLog);
                    string savefilepath = Server.MapPath("~/sitecore/admin/AdaniCare/SampleFile/AuthenticatedUsersLog.csv");
                    DatatableToCSV(dt, savefilepath);
                }
                else
                {
                    lblErroMsg.Text = "No Records found.";
                    lblErroMsg.Visible = true;
                    return;
                }

            }
            catch (Exception ex)
            {
                lblErroMsg.Text = "There is some technically problem. Please contact administrator.";
                Sitecore.Diagnostics.Log.Error("Admin Payment History data - Error  " + ex.Message, this);
            }
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public void DatatableToCSV(DataTable dtDataTable, string strFilePath)
        {
            #region New Method

            Stopwatch stw = new Stopwatch();
            stw.Start();
            StringBuilder sb = new StringBuilder();
            //Column headers  
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sb.Append(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(Environment.NewLine);
            //Column Values
            foreach (DataRow dr in dtDataTable.Rows)
            {
                sb.AppendLine(string.Join(",", dr.ItemArray));
            }
            File.WriteAllText(strFilePath, sb.ToString());
            stw.Stop();
            DownLoad(strFilePath);

            #endregion
        }


        public void DownLoad(string FName)
        {
            string path = FName;
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (file.Exists)
            {
                string filename = "Claimed_offers" + DateTime.Now.ToString();
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=AuthenticatedUsersLog.csv");
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.End();
            }
            else
            {
                Response.Write("This file does not exist.");
            }

        }
    }
}