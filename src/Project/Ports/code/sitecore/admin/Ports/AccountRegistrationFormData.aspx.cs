using Sitecore.Diagnostics;
using Sitecore.Ports.Website;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Sitecore.Ports.Website.sitecore.admin.Ports
{
    public partial class AccountRegistrationFormData : System.Web.UI.Page
    {

        protected void Button1_Click(object sender, EventArgs e)
        {
            AccountRegistrationFormDataContext accountRegistrationFormDataContext = new AccountRegistrationFormDataContext();
            try
            {
                this.lblErroMsg.Text = "";
                this.lblErroMsg.Visible = false;
                DateTime dateTime = DateTime.Parse(this.TextBoxFrom.Text);
                DateTime dateTime1 = DateTime.Parse(this.TextBoxTo.Text);
                if (System.Convert.ToDateTime(this.TextBoxTo.Text) >= System.Convert.ToDateTime(this.TextBoxFrom.Text))
                {
                    IQueryable<PortsAccountRegistrationForm> portsAccountRegistrationForms =
                        from rc in accountRegistrationFormDataContext.PortsAccountRegistrationForms
                        where (rc.SubmitOnDate.Value.Date >= dateTime) && (rc.SubmitOnDate.Value.Date <= dateTime1)
                        select rc;
                    if (System.Convert.ToDateTime(this.TextBoxTo.Text) == System.Convert.ToDateTime(this.TextBoxFrom.Text))
                    {
                        portsAccountRegistrationForms =
                            from rc in accountRegistrationFormDataContext.PortsAccountRegistrationForms
                            where rc.SubmitOnDate.Value.Date == dateTime
                            select rc;
                    }
                    DataTable dataTable = this.ToDataTable<PortsAccountRegistrationForm>(portsAccountRegistrationForms.ToList<PortsAccountRegistrationForm>());
                    this.DatatableToCSV(dataTable, base.Server.MapPath("~/sitecore/admin/Ports/SampleFile/Form-Records.csv"));
                }
                else
                {
                    this.lblErroMsg.Text = "Please select proper date";
                    this.lblErroMsg.Visible = true;
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                this.lblErroMsg.Text = "There is some technically problem. Please contact administrator.";
                Log.Error(string.Concat("Form data -Export Error  ", exception.Message), this);
            }
        }

        public void DatatableToCSV(DataTable dtDataTable, string strFilePath)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                stringBuilder.Append(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Append(Environment.NewLine);
            foreach (DataRow row in dtDataTable.Rows)
            {
                stringBuilder.AppendLine(string.Join(",", row.ItemArray));
            }
            File.WriteAllText(strFilePath, stringBuilder.ToString());
            stopwatch.Stop();
            this.DownLoad(strFilePath);
        }

        public void DownLoad(string FName)
        {
            FileInfo fileInfo = new FileInfo(FName);
            if (!fileInfo.Exists)
            {
                base.Response.Write("This file does not exist.");
            }
            else
            {
                base.Response.Clear();
                base.Response.AddHeader("Content-Disposition", "attachment; filename=Form-Records.csv");
                base.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                base.Response.ContentType = "application/octet-stream";
                base.Response.WriteFile(fileInfo.FullName);
                base.Response.End();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] propertyInfoArray = properties;
            for (int i = 0; i < (int)propertyInfoArray.Length; i++)
            {
                PropertyInfo propertyInfo = propertyInfoArray[i];
                dataTable.Columns.Add(propertyInfo.Name);
            }
            foreach (T item in items)
            {
                object[] value = new object[(int)properties.Length];
                for (int j = 0; j < (int)properties.Length; j++)
                {
                    if ((properties[j].Name.ToLower() == "agreementwithprincipal" || properties[j].Name.ToLower() == "certificationofincorporation" || properties[j].Name.ToLower() == "memorandumarticleassociation" || properties[j].Name.ToLower() == "drivinglicense" || properties[j].Name.ToLower() == "pancard" || properties[j].Name.ToLower() == "acknowledgementgstregistration" || properties[j].Name.ToLower() == "municipallicence" || properties[j].Name.ToLower() == "aeolicence" || properties[j].Name.ToLower() == "customdpdpermission" ? false : properties[j].Name.ToLower() != "cancelledcheque"))
                    {
                        value[j] = properties[j].GetValue(item, null);
                    }
                    else if (properties[j].GetValue(item, null) == null)
                    {
                        value[j] = properties[j].GetValue(item, null);
                    }
                    else
                    {
                        value[j] = string.Concat("https://adaniproduction-cd.azurewebsites.net", properties[j].GetValue(item, null));
                    }
                }
                dataTable.Rows.Add(value);
            }
            return dataTable;
        }
    }
}