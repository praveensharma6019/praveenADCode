using ClosedXML.Excel;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Marathon.Website.sitecore.admin.Marathon
{
    public partial class DownloadData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void DownloadDataClick(object sender, EventArgs e)
        {
            try
            {
                Log.Info("Download user data start", "");
                AhmedabadMarathonRegistrationDataContext dataContext = new AhmedabadMarathonRegistrationDataContext();
                DateTime fromdate, todate;
                fromdate = DateTime.Parse(TextBoxFrom.Text);
                List<AhmedabadMarathonRegistration> datarecord = new List<AhmedabadMarathonRegistration>();
                todate = DateTime.Parse(TextBoxTo.Text);
                if (System.Convert.ToDateTime(TextBoxTo.Text) < System.Convert.ToDateTime(TextBoxFrom.Text))
                {
                    lblErroMsg.Text = "Please select proper date";
                    lblErroMsg.Visible = true;
                    return;
                }
                if (PaymentStatus.SelectedValue == "")
                {
                    datarecord = dataContext.AhmedabadMarathonRegistrations.Where(x => (x.FormSubmitOn >= fromdate) && (x.FormSubmitOn <= todate)).OrderBy(x => x.FormSubmitOn).ToList();
                }
                else
                {
                    datarecord = dataContext.AhmedabadMarathonRegistrations.Where(x => (x.FormSubmitOn >= fromdate) && (x.FormSubmitOn <= todate) && (x.PaymentStatus.ToLower().Trim().Equals(PaymentStatus.SelectedValue.ToLower().Trim()))).OrderBy(x => x.FormSubmitOn).ToList();
                }
                DataTable dt = ConvertToDataTable(datarecord.ToList());
                DownloadExcel(dt);
            }
            catch (Exception ex)
            {
                Log.Error("Download user data exception" + ex, "");
            }
        }
        public DataTable ConvertToDataTable<T>(List<T> items)
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
        public void DownloadExcel(DataTable Tableref)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(Tableref);
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= AhmedabadMarathonData.xlsx");
                using (MemoryStream mStream = new MemoryStream())
                {
                    wb.SaveAs(mStream);
                    mStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }
}