using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using System.IO;
//using System.Dat.Entity.Core.Objects;

namespace Sitecore.Realty.Website.sitecore.admin.Realty
{
    public partial class FormsData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var cityList = new List<ListItem>();

                cityList.Add(new ListItem
                {
                    Text = "All",
                    Value = "All"
                });
                cityList.Add(new ListItem
                {
                    Text = "Ahmedabad",
                    Value = "Ahmedabad"
                });
                cityList.Add(new ListItem
                {
                    Text = "Gurgaon",
                    Value = "Gurgaon"
                });
                cityList.Add(new ListItem
                {
                    Text = "Mumbai",
                    Value = "Mumbai"
                });
                cityList.Add(new ListItem
                {
                    Text = "Pune",
                    Value = "Pune"
                });

                drpCity.DataSource = cityList;
                drpCity.DataTextField = "Text";
                drpCity.DataValueField = "Value";
                drpCity.DataBind();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            RealtyDataContext rdb = new RealtyDataContext();
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
                var datarecord = from rc in rdb.ConsolidateFormDatas
                             where ((rc.FormSubmitOn.Value.Date >= fromdate) && (rc.FormSubmitOn.Value.Date <= todate))
                             select rc;
                if (drpCity.SelectedItem.Text != "All")
                {
                    datarecord = from rc in rdb.ConsolidateFormDatas
                                     where ((rc.FormSubmitOn.Value.Date >= fromdate) && (rc.FormSubmitOn.Value.Date <= todate)) && rc.Lead_City.ToLower().Trim() == drpCity.SelectedItem.Text.ToLower().Trim()
                                     select rc;
                }
               
                if (System.Convert.ToDateTime(TextBoxTo.Text) == System.Convert.ToDateTime(TextBoxFrom.Text))
                {
                     datarecord = from rc in rdb.ConsolidateFormDatas
                                     where rc.FormSubmitOn.Value.Date == fromdate
                                     select rc;

                    if (drpCity.SelectedItem.Text != "All")
                    {
                        datarecord = from rc in rdb.ConsolidateFormDatas
                                     where rc.FormSubmitOn.Value.Date == fromdate && rc.Lead_City.ToLower().Trim() == drpCity.SelectedItem.Text.ToLower().Trim()
                                     select rc;
                    }
                }
                DataTable dt = ToDataTable(datarecord.ToList()) ;
                string savefilepath = Server.MapPath("~/sitecore/admin/Realty/SampleFile/Form-Records.csv");
                DatatableToCSV(dt, savefilepath);

            }
            catch (Exception ex)
            {

                lblErroMsg.Text = "There is some technically problem. Please contact administrator.";
                Sitecore.Diagnostics.Log.Error("Form data -Export Error  " + ex.Message, this);
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
                Response.AddHeader("Content-Disposition", "attachment; filename=Form-Records.csv");
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