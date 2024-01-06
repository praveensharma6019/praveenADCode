using Sitecore.Diagnostics;
using Sitecore.Feature.Accounts.Models;
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

namespace Sitecore.ElectricityNew.Website.sitecore.admin.Electricity
{
    public partial class GreenPower : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            TenderDataContext rdb = new TenderDataContext();
            try
            {
                this.lblErroMsg.Text = "";
                this.lblErroMsg.Visible = false;
                DateTime dateTime = DateTime.Parse(this.TextBoxFrom.Text);
                DateTime dateTime1 = DateTime.Parse(this.TextBoxTo.Text);
                if (System.Convert.ToDateTime(this.TextBoxTo.Text) >= System.Convert.ToDateTime(this.TextBoxFrom.Text))
                {
                    IQueryable<GreenPowerOptIn> greenpoweropt =
                        from rc in rdb.GreenPowerOptIns
                        where (rc.CreatedDate.Date >= dateTime) && (rc.CreatedDate.Date <= dateTime1)
                        select rc;
                    if (System.Convert.ToDateTime(this.TextBoxTo.Text) == System.Convert.ToDateTime(this.TextBoxFrom.Text))
                    {
                        greenpoweropt =
                            from rc in rdb.GreenPowerOptIns
                            where rc.CreatedDate.Date == dateTime
                            select rc;
                        var GreenPowerOptInModels = greenpoweropt.ToList().Select(x => new GreenPowerOptInModels()
                        {
                            Id = x.Id.ToString(),
                         AccountNumber=x.AccountNumber,
                         MobileNumber=x.MobileNumber,
                         EmailId=x.EmailId,
                         FacebookId=x.FacebookId,
                         TwitterId=x.TwitterId,
                        OptInFlagCurrentOrNextBilling=x.OptInFlagCurrentOrNextBilling,
                         IsIPledge =x.IPledge.HasValue  ,
                         IsPicCaptured=x.IsPicCaptured.HasValue,
                         ImageName=x.ImageName,
                         ImageLink=x.ImageLink,
                         imageLinkContentType=x.ImageContentType,
                            OptInBillingFrom = x.OptInBillingFrom,
                            PercentageOptIn=x.PercentageOptIn,
                            CreatedDate=x.CreatedDate



                        }).ToList();
                        DataTable dt = ToDataTable(GreenPowerOptInModels);
                        string savefilepath = Server.MapPath("~/sitecore/admin/Electricity/SampleFile/Form-Records.csv");
                        DatatableToCSV(dt, savefilepath);
                    }
                    else
                    {
                        greenpoweropt =
                        from rc in rdb.GreenPowerOptIns
                        where (rc.CreatedDate.Date >= dateTime) && (rc.CreatedDate.Date <= dateTime1)
                        select rc;
                        var GreenPowerOptInModels = greenpoweropt.ToList().Select(x => new GreenPowerOptInModels()
                        {
                            Id = x.Id.ToString(),
                            AccountNumber = x.AccountNumber,
                            MobileNumber = x.MobileNumber,
                            EmailId = x.EmailId,
                            FacebookId = x.FacebookId,
                            TwitterId = x.TwitterId,
                            OptInFlagCurrentOrNextBilling = x.OptInFlagCurrentOrNextBilling,
                            IsIPledge = x.IPledge.HasValue,
                            IsPicCaptured = x.IsPicCaptured.HasValue,
                            ImageName = x.ImageName,
                            ImageLink = x.ImageLink,
                            imageLinkContentType = x.ImageContentType,
                            OptInBillingFrom = x.OptInBillingFrom,
                            PercentageOptIn = x.PercentageOptIn,
                            CreatedDate = x.CreatedDate



                        }).ToList();
                        DataTable dt = ToDataTable(GreenPowerOptInModels);
                        string savefilepath = Server.MapPath("~/sitecore/admin/Electricity/SampleFile/Form-Records.csv");
                        DatatableToCSV(dt, savefilepath);
                      
                    }
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
            PropertyInfo[] propertyInfoArray = Props;
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {
                    if ((Props[i].Name.ToLower() == "UploadedFileType" ? false : Props[i].Name.ToLower() != "imagelink"))
                    {
                        values[i] = Props[i].GetValue(item, null);
                    }
                    else if (Props[i].GetValue(item, null) == null)
                    {
                        values[i] = Props[i].GetValue(item, null);
                    }
                    else
                    {
                        values[i] = string.Concat("https://www.adanielectricity.com", Props[i].GetValue(item, null));
                    }
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}