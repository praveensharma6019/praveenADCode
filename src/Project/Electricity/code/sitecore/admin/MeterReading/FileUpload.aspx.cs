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

namespace Sitecore.Electricity.Website.sitecore.admin.MeterReading
{
    public partial class FileUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {
            try
            {
                lblErroMsg.Visible = false;
                lblSuccessMsg.Visible = false;

                if (fuExcelselection.HasFile)
                {
                    string savefilepath = Server.MapPath("~/sitecore/admin/MeterReading/temp/Import/" + DateTime.Now.ToString("yyyyMMddHHmmss") + fuExcelselection.FileName);
                    fuExcelselection.SaveAs(savefilepath);
                    using (XLWorkbook workBook = new XLWorkbook(savefilepath))
                    {
                        IXLWorksheet workSheet = workBook.Worksheet(1);

                        //Create a new DataTable.
                        DataTable dt = new DataTable();

                        //Loop through the Worksheet rows.
                        bool firstRow = true;
                        foreach (IXLRow row in workSheet.Rows())
                        {
                            if (!row.IsEmpty())
                            {
                                if (firstRow)
                                {
                                    foreach (IXLCell cell in row.Cells())
                                    {
                                        dt.Columns.Add(cell.Value.ToString());
                                    }
                                    firstRow = false;
                                }
                                else
                                {
                                    //Add rows to DataTable.
                                    dt.Rows.Add();
                                    int i = 0;
                                    foreach (IXLCell cell in row.Cells())
                                    {
                                        dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                        i++;
                                    }
                                }
                            }
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            Importdata(dt);
                            lblSuccessMsg.Visible = true;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblErroMsg.Visible = true;
                lblErroMsg.Text = ex.Message;
                Log.Error(ex.Message, ex, this);
            }

        }

        private void Importdata(DataTable dt)
        {
            try
            {
                //Note : Get Available List of Items from 	EBILL MRDATE
                Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("master");
                var itemInfo = master.GetItem(new Data.ID(Templates.ItemList.MeterReadingItemList.ToString()));
                var AvailableItemList = itemInfo.GetChildren();

                //Note : remove Duplicate value if any from xls copy
                string[] dtcolumns = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
                DataTable distinctTable = dt.DefaultView.ToTable(true, dtcolumns);
                foreach (DataRow row in distinctTable.Rows)
                {
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {

                        // ADD THE ITEM 
                        
                        if (!AvailableItemList.Any(w => w.Fields[Templates.MeterReadingProperties.CYCLE.ToString()].Value == row["CYCLE"].ToString() && w.Fields[Templates.MeterReadingProperties.BILLMONTH.ToString()].Value == row["BILLMONTH"].ToString()))
                        {
                            string ItemVal = string.Concat((row["CYCLE"].ToString() ?? string.Empty) + "_" + (row["BILLMONTH"].ToString() ?? string.Empty));
                            // Get the template for which you need to create item
                            TemplateItem template = master.GetItem("/sitecore/templates/Project/Electricity/Content Types/MeterReading");

                            // Get the place in the site tree where the new item must be inserted
                            Sitecore.Data.Items.Item parentItem = master.GetItem("/sitecore/content/Electricity/Global/EBILL MRDATE");

                            Sitecore.Data.Items.Item newItem = parentItem.Add(ItemVal, template);

                            // Set the new item in editing mode
                            // Fields can only be updated when in editing mode
                            // (It's like the begin transaction on a database)
                            newItem.Editing.BeginEdit();
                            try
                            {
                                // Assign values to the fields of the new item

                                newItem.Fields[Templates.MeterReadingProperties.CYCLE.ToString()].Value = row["CYCLE"].ToString() ?? string.Empty;
                                newItem.Fields[Templates.MeterReadingProperties.BILLMONTH.ToString()].Value = row["BILLMONTH"].ToString() ?? string.Empty;
                                newItem.Fields[Templates.MeterReadingProperties.METERREADINGDATE.ToString()].Value = row["METERREADINGDATE"].ToString() ?? string.Empty;
                                newItem.Fields[Templates.MeterReadingProperties.FULLMETERREADINGDATE.ToString()].Value = row["FULLMETERREADINGDATE"].ToString() ?? string.Empty;
                                newItem.Fields[Templates.MeterReadingProperties.PROPOSEDDELIVERYDATE.ToString()].Value = row["PROPOSEDDELIVERYDATE"].ToString() ?? string.Empty;
                                newItem.Fields[Templates.MeterReadingProperties.PROPOSEDDUEDATE_RESI.ToString()].Value = row["PROPOSEDDUEDATE_RESI"].ToString() ?? string.Empty;
                                newItem.Fields[Templates.MeterReadingProperties.PROPOSEDDUEDATE_COM.ToString()].Value = row["PROPOSEDDUEDATE_COM"].ToString() ?? string.Empty;

                                // End editing will write the new values back to the Sitecore
                                // database (It's like commit transaction of a database)
                                newItem.Editing.EndEdit();
                            }
                            catch (System.Exception ex)
                            {
                                // Log the message on any failure to sitecore log
                                Sitecore.Diagnostics.Log.Error("Could not update item " + newItem.Paths.FullPath + ": " + ex.Message, this);

                                // Cancel the edit (not really needed, as Sitecore automatically aborts
                                // the transaction on exceptions, but it wont hurt your code)
                                newItem.Editing.CancelEdit();
                                continue;
                            }
                        }
                        // UPDATE THE ITEM 
                        else
                        {
                            var updateitem = AvailableItemList.Where(w => w.Fields["CYCLE"].Value == row["CYCLE"].ToString() && w.Fields["BILLMONTH"].Value == row["BILLMONTH"].ToString()).FirstOrDefault();
                            if (updateitem != null)
                            {
                                updateitem.Editing.BeginEdit();
                                try
                                {
                                    //perform the editing
                                    
                                    updateitem.Fields[Templates.MeterReadingProperties.CYCLE.ToString()].Value = row["CYCLE"].ToString() ?? string.Empty;
                                    updateitem.Fields[Templates.MeterReadingProperties.BILLMONTH.ToString()].Value = row["BILLMONTH"].ToString() ?? string.Empty;
                                    updateitem.Fields[Templates.MeterReadingProperties.METERREADINGDATE.ToString()].Value = row["METERREADINGDATE"].ToString() ?? string.Empty;
                                    updateitem.Fields[Templates.MeterReadingProperties.FULLMETERREADINGDATE.ToString()].Value = row["FULLMETERREADINGDATE"].ToString() ?? string.Empty;
                                    updateitem.Fields[Templates.MeterReadingProperties.PROPOSEDDELIVERYDATE.ToString()].Value = row["PROPOSEDDELIVERYDATE"].ToString() ?? string.Empty;
                                    updateitem.Fields[Templates.MeterReadingProperties.PROPOSEDDUEDATE_RESI.ToString()].Value = row["PROPOSEDDUEDATE_RESI"].ToString() ?? string.Empty;
                                    updateitem.Fields[Templates.MeterReadingProperties.PROPOSEDDUEDATE_COM.ToString()].Value = row["PROPOSEDDUEDATE_COM"].ToString() ?? string.Empty;

                                    updateitem.Editing.EndEdit();
                                }
                                catch (Exception ex)
                                {
                                    Sitecore.Diagnostics.Log.Error("Could not update item " + updateitem.Paths.FullPath + ": " + ex.Message, this);
                                    updateitem.Editing.CancelEdit();
                                    continue;
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the message on any failure to sitecore log
                Sitecore.Diagnostics.Log.Error("Error Description : " + ex.Message, this);
            }
        }
    }
}