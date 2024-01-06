using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.Data;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using DocumentFormat.OpenXml.Office.CustomUI;
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
using Sitecore.Electricity.Website.Model;
using System.Linq.Expressions;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions;
using System.Reflection;
using System.Diagnostics;
using System.Text;

namespace Sitecore.Electricity.Website.sitecore.admin.Electricity
{
    public partial class PaymentData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErroMsg.Text = "";
            lblErroMsg.Visible = false;
            if (!IsPostBack)
            {
                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                ListItem objselect = new ListItem() { Text = "Please Select", Value = "0" };

                var itemInfo = db.GetItem(Templates.Datasource.PaymentGateway);
                var gatewayList = itemInfo.GetChildren().ToList().Select(x => new ListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();
                gatewayList.Insert(0, objselect);
                drpPaymentGateway.DataSource = gatewayList;
                drpPaymentGateway.DataTextField = "Text";
                drpPaymentGateway.DataValueField = "Value";
                drpPaymentGateway.DataBind();

                var paymentModelist = db.GetItem(Templates.Datasource.PaymentMode);
                var paymentModeList = paymentModelist.GetChildren().ToList().Select(x => new ListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();
                paymentModeList.Insert(0, objselect);
                drpPaymentType.DataSource = paymentModeList;
                drpPaymentType.DataTextField = "Text";
                drpPaymentType.DataValueField = "Value";
                drpPaymentType.DataBind();

                var statusList = db.GetItem(Templates.Datasource.PaymentStatus);
                var PaymentStatusList = statusList.GetChildren().ToList().Select(x => new ListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();
                PaymentStatusList.Insert(0, objselect);
                drpStatus.DataSource = PaymentStatusList;
                drpStatus.DataTextField = "Text";
                drpStatus.DataValueField = "Value";
                drpStatus.DataBind();
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            TenderDataContext dc = new TenderDataContext();

            try
            {
                lblErroMsg.Text = "";
                lblErroMsg.Visible = false;

                var predicateQuery = dc.PaymentHistories.AsQueryable();
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
                    predicateQuery = predicateQuery.Where(p => p.Created_Date >= System.Convert.ToDateTime(txtFromDate.Text) && p.Created_Date <= System.Convert.ToDateTime(txtToDate.Text));
                }
                if (drpPaymentGateway.SelectedValue != "0")
                {
                    predicateQuery = predicateQuery.Where(p => p.GatewayType.Contains(drpPaymentGateway.SelectedItem.Text));
                }
                if (drpPaymentType.SelectedValue != "0")
                {
                    predicateQuery = predicateQuery.Where(p => p.PaymentType.Contains(drpPaymentType.SelectedItem.Text));
                }
                if (drpStatus.SelectedValue != "0")
                {
                    predicateQuery = predicateQuery.Where(p => p.Status.Contains(drpStatus.SelectedItem.Text));
                }




                if (predicateQuery != null && predicateQuery.Any())
                {

                    lblErroMsg.Text = "Records download Successfully";


                    var paymentHistroy = predicateQuery.ToList().Select(x => new PaymentSearchResult()
                    {
                        AccountNumber = x.AccountNumber,
                        TransactionId = !string.IsNullOrEmpty(x.TransactionId) ? x.GatewayType + "-" + x.TransactionId.ToString() + " " : string.Empty,
                        OrderId = x.OrderId,
                        Amount = x.Amount,
                        Status = x.Status,
                        PaymentGateWay = x.GatewayType,
                        PaymentMode = x.PaymentMode,
                        PaymentType = x.PaymentType,
                        UserType = x.UserType,
                        UserId = x.UserId,
                        RequestTime = x.RequestTime,
                        ResponseTime = x.ResponseTime,
                        CreateDate=x.Created_Date,
                        ResponseMsg=x.ResponseMsg
                    }).ToList();

                    DataTable dt = ToDataTable(paymentHistroy);
                    string savefilepath = Server.MapPath("~/sitecore/admin/Electricity/SampleFile/Sample_Payment_data.csv");
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
            #region Old Method
            //StreamWriter sw = new StreamWriter(strFilePath, false);
            ////headers  
            //for (int i = 0; i < dtDataTable.Columns.Count; i++)
            //{
            //    sw.Write(dtDataTable.Columns[i]);
            //    if (i < dtDataTable.Columns.Count - 1)
            //    {
            //        sw.Write(",");
            //    }
            //}
            //sw.Write(sw.NewLine);
            //foreach (DataRow dr in dtDataTable.Rows)
            //{
            //    for (int i = 0; i < dtDataTable.Columns.Count; i++)
            //    {
            //        if (!System.Convert.IsDBNull(dr[i]))
            //        {
            //            string value = dr[i].ToString();
            //            if (value.Contains(','))
            //            {
            //                value = String.Format("\"{0}\"", value);
            //                sw.Write(value);
            //            }
            //            else
            //            {
            //                sw.Write(dr[i].ToString());
            //            }
            //        }
            //        if (i < dtDataTable.Columns.Count - 1)
            //        {
            //            sw.Write(",");
            //        }
            //    }
            //    sw.Write(sw.NewLine);
            //}
            //sw.Close();
            //DownLoad(strFilePath);
            #endregion

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
                Response.AddHeader("Content-Disposition", "attachment; filename=Payment_Transaction.csv");
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