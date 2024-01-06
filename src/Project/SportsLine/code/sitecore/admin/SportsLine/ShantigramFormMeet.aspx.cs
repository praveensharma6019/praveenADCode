using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.SportsLine.Website.sitecore.admin.SportsLine
{
    public partial class ShantigramFormMeet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SportsLineContactUsFormDataContext rdb = new SportsLineContactUsFormDataContext();
          try
            {
                this.lblErroMsg.Text = "";
                this.lblErroMsg.Visible = false;
                DateTime dateTime = DateTime.Parse(this.TextBoxFrom.Text);
                DateTime dateTime1 = DateTime.Parse(this.TextBoxTo.Text);
                if (System.Convert.ToDateTime(this.TextBoxTo.Text) >= System.Convert.ToDateTime(this.TextBoxFrom.Text))
                {
                    IQueryable<SportsLineRegistrationForm> transmissioncostcalculator =
                        from rc in rdb.SportsLineRegistrationForms
                        where (rc.FormSubmitOn.Value.Date >= dateTime) && (rc.FormSubmitOn.Value.Date <= dateTime1)
                        select rc;
                    if (System.Convert.ToDateTime(this.TextBoxTo.Text) == System.Convert.ToDateTime(this.TextBoxFrom.Text))
                    {
                        transmissioncostcalculator =
                            from rc in rdb.SportsLineRegistrationForms
                            where rc.FormSubmitOn.Value.Date == dateTime
                            select rc;
                    }
                    DataTable dataTable = this.ToDataTable<SportsLineRegistrationForm>(transmissioncostcalculator.ToList<SportsLineRegistrationForm>());
                    this.DatatableToCSV(dataTable, base.Server.MapPath("~/sitecore/admin/SportsLine/SampleFile/Form-Records.csv"));
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
    }
}