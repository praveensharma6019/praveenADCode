using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using Sitecore.AdaniConneX.Website.Helpers;
using Sitecore.AdaniConneX.Website.Models;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Shell.Framework.Commands.Imager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.AdaniConneX.Website.sitecore.admin.AdaniConneX
{
    public partial class AllFormLeadData : System.Web.UI.Page
    {
        string EncryptionKey = "Tl;jld@456763909QPwOeiRuTy873XY7";
        string EncryptionIV = "CEIVRAJWquG8iiMw";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            AdaniConnexContactFormDataContext rdb = new AdaniConnexContactFormDataContext();
            try
            {
                lblErroMsg.Text = "";
                lblErroMsg.Visible = false;
                DateTime fromdate, todate;
                fromdate = DateTime.Parse(TextBoxFrom.Text);
                todate = DateTime.Parse(TextBoxTo.Text);
                if (System.Convert.ToDateTime(TextBoxTo.Text) < System.Convert.ToDateTime(TextBoxFrom.Text))
                {
                    lblErroMsg.Text = "Please select proper date";
                    lblErroMsg.Visible = true;
                    return;
                }
                if (drptype.SelectedValue == "Select form type")
                {
                    lblErroMsg.Text = "Please select proper form type";
                    lblErroMsg.Visible = true;
                    return;
                }
                else if (drptype.SelectedValue == "ContactUs")
                {
                    var datarecord = from rc in rdb.AdaniConnex_ContactForms
                                     where ((rc.SubmittedDate.Value.Date >= fromdate) && (rc.SubmittedDate.Value.Date <= todate) && rc.FormType.ToLower().Trim() == drptype.SelectedValue.ToLower().Trim())
                                     select rc;

                    if (System.Convert.ToDateTime(TextBoxTo.Text) == System.Convert.ToDateTime(TextBoxFrom.Text))
                    {
                        datarecord = from rc in rdb.AdaniConnex_ContactForms
                                     where rc.SubmittedDate.Value.Date == fromdate && rc.FormType.ToLower().Trim() == drptype.SelectedValue.ToLower().Trim()
                                     select rc;
                    }
                    foreach (var record in datarecord)
                    {
                        var originalRecord = datarecord.FirstOrDefault(r => r.Id == record.Id);
                        if (originalRecord != null)
                        {
                            originalRecord.Email = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Email, EncryptionIV);
                            originalRecord.Contact = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Contact, EncryptionIV);
                        }
                    }
                    DataTable dt = ToDataTable(datarecord.ToList());
                    DownloadExcel(dt);
                }
                else if (drptype.SelectedValue == "GetInTouch")
                {
                    var datarecord = from rc in rdb.AdaniConnex_ContactForms
                                     where ((rc.SubmittedDate.Value.Date >= fromdate) && (rc.SubmittedDate.Value.Date <= todate) && rc.FormType.ToLower().Trim() == drptype.SelectedValue.ToLower().Trim())
                                     select rc;

                    if (System.Convert.ToDateTime(TextBoxTo.Text) == System.Convert.ToDateTime(TextBoxFrom.Text))
                    {
                        datarecord = from rc in rdb.AdaniConnex_ContactForms
                                     where rc.SubmittedDate.Value.Date == fromdate && rc.FormType.ToLower().Trim() == drptype.SelectedValue.ToLower().Trim()
                                     select rc;
                    }
                    foreach (var record in datarecord)
                    {
                        var originalRecord = datarecord.FirstOrDefault(r => r.Id == record.Id);
                        if (originalRecord != null)
                        {
                            originalRecord.Email = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Email, EncryptionIV);
                            originalRecord.Contact = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Contact, EncryptionIV);
                        }
                    }
                    DataTable dt = ToDataTable(datarecord.ToList());
                    DownloadExcel(dt);
                }
                else if (drptype.SelectedValue == "ConnectWithHR")
                {
                    var datarecord = from rc in rdb.AdaniConnex_ContactForms
                                     where ((rc.SubmittedDate.Value.Date >= fromdate) && (rc.SubmittedDate.Value.Date <= todate) && rc.FormType.ToLower().Trim() == drptype.SelectedValue.ToLower().Trim())
                                     select rc;

                    if (System.Convert.ToDateTime(TextBoxTo.Text) == System.Convert.ToDateTime(TextBoxFrom.Text))
                    {
                        datarecord = from rc in rdb.AdaniConnex_ContactForms
                                     where rc.SubmittedDate.Value.Date == fromdate && rc.FormType.ToLower().Trim() == drptype.SelectedValue.ToLower().Trim()
                                     select rc;
                    }
                    foreach (var record in datarecord)
                    {
                        var originalRecord = datarecord.FirstOrDefault(r => r.Id == record.Id);
                        if (originalRecord != null)
                        {
                            originalRecord.Email = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Email, EncryptionIV);
                            originalRecord.Contact = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Contact, EncryptionIV);
                        }
                    }
                    DataTable dt = ToDataTable(datarecord.ToList());
                    DownloadExcel(dt);
                }
                else if (drptype.SelectedValue == "WhitePaper")
                {
                    var datarecord = from rc in rdb.AdaniConnex_WhitePaperForms
                                     where ((rc.SubmittedDate.Value.Date >= fromdate) && (rc.SubmittedDate.Value.Date <= todate))
                                     select rc;

                    if (System.Convert.ToDateTime(TextBoxTo.Text) == System.Convert.ToDateTime(TextBoxFrom.Text))
                    {
                        datarecord = from rc in rdb.AdaniConnex_WhitePaperForms
                                     where rc.SubmittedDate.Value.Date == fromdate
                                     select rc;
                    }
                    foreach (var record in datarecord)
                    {
                        var originalRecord = datarecord.FirstOrDefault(r => r.Id == record.Id);
                        if (originalRecord != null)
                        {
                            originalRecord.Email = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Email, EncryptionIV);
                            originalRecord.Contact = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Contact, EncryptionIV);
                        }
                    }
                    DataTable dt = ToDataTable(datarecord.ToList());
                    DownloadExcel(dt);
                }
                else if (drptype.SelectedValue == "Ebook_form")
                {
                    var datarecord = from rc in rdb.AdaniConnex_TakeAtourForms
                                     where ((rc.SubmittedDate.Value.Date >= fromdate) && (rc.SubmittedDate.Value.Date <= todate) && rc.FormType.ToLower().Trim() == drptype.SelectedValue.ToLower().Trim())
                                     select rc;

                    if (System.Convert.ToDateTime(TextBoxTo.Text) == System.Convert.ToDateTime(TextBoxFrom.Text))
                    {
                        datarecord = from rc in rdb.AdaniConnex_TakeAtourForms
                                     where rc.SubmittedDate.Value.Date == fromdate && rc.FormType.ToLower().Trim() == drptype.SelectedValue.ToLower().Trim()
                                     select rc;
                    }
                    foreach (var record in datarecord)
                    {
                        var originalRecord = datarecord.FirstOrDefault(r => r.Id == record.Id);
                        if (originalRecord != null)
                        {
                            originalRecord.Email = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Email, EncryptionIV);
                            originalRecord.Contact = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Contact, EncryptionIV);
                        }
                    }
                    DataTable dt = ToDataTable(datarecord.ToList());
                    DownloadExcel(dt);
                }
                else if (drptype.SelectedValue == "JoinUs")
                {
                    var datarecord = from rc in rdb.AdaniConnex_JoinusForms
                                     where ((rc.SubmittedDate.Date >= fromdate) && (rc.SubmittedDate.Date <= todate))
                                     select rc;

                    if (System.Convert.ToDateTime(TextBoxTo.Text) == System.Convert.ToDateTime(TextBoxFrom.Text))
                    {
                        datarecord = from rc in rdb.AdaniConnex_JoinusForms
                                     where rc.SubmittedDate.Date == fromdate
                                     select rc;
                    }
                    foreach (var record in datarecord)
                    {
                        var originalRecord = datarecord.FirstOrDefault(r => r.Id == record.Id);
                        if (originalRecord != null)
                        {
                            originalRecord.Email = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Email, EncryptionIV);
                            originalRecord.Contact = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Contact, EncryptionIV);
                        }
                    }
                    DataTable dt = ToDataTable(datarecord.ToList());
                    DownloadExcel(dt);
                }

                

            }
            catch (Exception ex)
            {
                lblErroMsg.Text = "There is some technically problem. Please contact administrator.";
                Sitecore.Diagnostics.Log.Error("Form data for Contact us -Export Error  " + ex.Message, this);
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            AdaniConnexContactFormDataContext rdb = new AdaniConnexContactFormDataContext();
            try
            {

                lblErroMsg.Text = "";
                lblErroMsg.Visible = false;
                DateTime fromdate, todate;
                fromdate = DateTime.Parse(TextBoxFrom.Text);
                todate = DateTime.Parse(TextBoxTo.Text);
                if (System.Convert.ToDateTime(TextBoxTo.Text) < System.Convert.ToDateTime(TextBoxFrom.Text))
                {
                    lblErroMsg.Text = "Please select proper date";
                    lblErroMsg.Visible = true;
                    return;
                }


                #region JoinUs
                var joinUsRecords = from rc in rdb.AdaniConnex_JoinusForms
                                    where ((rc.SubmittedDate.Date >= fromdate) && (rc.SubmittedDate.Date <= todate))
                                    select rc;

                if (System.Convert.ToDateTime(TextBoxTo.Text) == System.Convert.ToDateTime(TextBoxFrom.Text))
                {
                    joinUsRecords = from rc in rdb.AdaniConnex_JoinusForms
                                    where rc.SubmittedDate.Date == fromdate
                                    select rc;
                }

                foreach (var record in joinUsRecords)
                {
                    var originalRecord = joinUsRecords.FirstOrDefault(r => r.Id == record.Id);
                    if (originalRecord != null)
                    {
                        originalRecord.Email = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Email, EncryptionIV);
                        originalRecord.Contact = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Contact, EncryptionIV);
                    }
                }
                DataTable joinUsRecordsTable = ToDataTable(joinUsRecords.ToList());

                #endregion
                #region Ebook
                var ebookRecords = from rc in rdb.AdaniConnex_TakeAtourForms
                                   where ((rc.SubmittedDate.Value.Date >= fromdate) && (rc.SubmittedDate.Value.Date <= todate) && rc.FormType.ToLower().Trim() == "Ebook_form")
                                   select rc;
                if (System.Convert.ToDateTime(TextBoxTo.Text) == System.Convert.ToDateTime(TextBoxFrom.Text))
                {
                    ebookRecords = from rc in rdb.AdaniConnex_TakeAtourForms
                                   where rc.SubmittedDate.Value.Date == fromdate && rc.FormType.ToLower().Trim() == "Ebook_form"
                                   select rc;
                }
                foreach (var record in ebookRecords)
                {
                    var originalRecord = ebookRecords.FirstOrDefault(r => r.Id == record.Id);
                    if (originalRecord != null)
                    {
                        originalRecord.Email = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Email, EncryptionIV);
                        originalRecord.Contact = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Contact, EncryptionIV);
                    }
                }
                DataTable ebookRecordsTable = ToDataTable(ebookRecords.ToList());

                #endregion
                #region WhitePaper
                var whitePaperRecords = from rc in rdb.AdaniConnex_WhitePaperForms
                                        where ((rc.SubmittedDate.Value.Date >= fromdate) && (rc.SubmittedDate.Value.Date <= todate))
                                        select rc;
                if (System.Convert.ToDateTime(TextBoxTo.Text) == System.Convert.ToDateTime(TextBoxFrom.Text))
                {
                    whitePaperRecords = from rc in rdb.AdaniConnex_WhitePaperForms
                                        where rc.SubmittedDate.Value.Date == fromdate
                                        select rc;
                }
                foreach (var record in whitePaperRecords)
                {
                    var originalRecord = whitePaperRecords.FirstOrDefault(r => r.Id == record.Id);
                    if (originalRecord != null)
                    {
                        originalRecord.Email = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Email, EncryptionIV);
                        originalRecord.Contact = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Contact, EncryptionIV);
                    }
                }
                DataTable whitePaperRecordsTable = ToDataTable(whitePaperRecords.ToList());
                #endregion
                #region contactRecordTable => HR, Contact, Get In Touch
                var contactRecord = from rc in rdb.AdaniConnex_ContactForms
                                    where ((rc.SubmittedDate.Value.Date >= fromdate) && (rc.SubmittedDate.Value.Date <= todate) && (rc.FormType.ToLower().Trim() == "ConnectWithHR" || rc.FormType.ToLower().Trim() == "GetInTouch" || rc.FormType.ToLower().Trim() == "ContactUs"))
                                    select rc;
                if (System.Convert.ToDateTime(TextBoxTo.Text) == System.Convert.ToDateTime(TextBoxFrom.Text))
                {
                    contactRecord = from rc in rdb.AdaniConnex_ContactForms
                                    where rc.SubmittedDate.Value.Date == fromdate && (rc.FormType.ToLower().Trim() == "ConnectWithHR" || rc.FormType.ToLower().Trim() == "GetInTouch" || rc.FormType.ToLower().Trim() == "ContactUs")
                                    select rc;
                }
                foreach (var record in contactRecord)
                {
                    var originalRecord = contactRecord.FirstOrDefault(r => r.Id == record.Id);
                    if (originalRecord != null)
                    {
                        originalRecord.Email = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Email, EncryptionIV);
                        originalRecord.Contact = EncryptionServiceHelper.DecryptString(EncryptionKey, record.Contact, EncryptionIV);
                    }
                }
                DataTable contactRecordTable = ToDataTable(contactRecord.ToList());

                #endregion

                #region Add Tables to Data Set
                DataSet ds = new DataSet();
                ds.Tables.Add(contactRecordTable);
                ds.Tables.Add(whitePaperRecordsTable);
                ds.Tables.Add(ebookRecordsTable);
                ds.Tables.Add(joinUsRecordsTable);
                #endregion

                #region Download Table
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(ds);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= AdaniConnexLeadsData.xlsx");
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        wb.SaveAs(mStream);
                        mStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                lblErroMsg.Text = "There is some technically problem. Please contact administrator.";
                Sitecore.Diagnostics.Log.Error("Form data for Contact us -Export Error  " + ex.Message, this);
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
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=AdaniConnexLeadsData.csv");
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
                Response.AddHeader("content-disposition", "attachment;filename= AdaniConnexLeadsData.xlsx");
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