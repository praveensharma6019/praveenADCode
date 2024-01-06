/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using ClosedXML.Excel;
using Farmpik.Domain.Common.Enum;
using Farmpik.Domain.Entities;
using Farmpik.Domain.Interfaces.Repositories;
using Farmpik.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Farmpik.Infrastructure.Repository
{
    public class ReportRepository : IReportRepository
    {
        public byte[] ExportVendorTemplate(List<Vendor> vendors, string filePath, bool isErrorTemplate)
        {
            using (XLWorkbook workbook = new XLWorkbook(filePath))
            {
                var sheet = workbook.Worksheet(1);
                int row = 2;
                if (isErrorTemplate) sheet.Cell(1, 4).Value = "Error Fields";

                foreach (var vendor in vendors)
                {
                    sheet.Cell(row, 1).Value = vendor.VendorCode;
                    sheet.Cell(row, 2).Value = vendor.VendorName;
                    sheet.Cell(row, 3).Value = vendor.Telephone;
                    if (isErrorTemplate) sheet.Cell(row, 4).Value = vendor.ErrorField;
                    row++;
                }
                return ConvertToExcel(workbook);
            }
        }

        public byte[] ExportPurchaseTemplate(List<PurchaseReport> purchases, string filePath, bool isErrorTemplate)
        {
            using (XLWorkbook workbook = new XLWorkbook(filePath))
            {
                var sheet = workbook.Worksheet(1);
                int row = 2;
                if (isErrorTemplate) sheet.Cell(1, 46).Value = "Error Fields";
                sheet.Range($"H2:AS{purchases.Count+1}").Style.NumberFormat.Format = "#,##0.00";
                sheet.Range($"G2:G{purchases.Count + 1}").Style.NumberFormat.Format = "@";
                foreach (var purchase in purchases)
                {
                    sheet.Cell(row, 1).Value = purchase.PlantName;
                    sheet.Cell(row, 2).Value = purchase.GateNumber;
                    sheet.Cell(row, 3).Value = purchase.VendorCode;
                    sheet.Cell(row, 4).Value = purchase.VendorName;
                    sheet.Cell(row, 5).Value = purchase.PurchaseOrderNumber;
                    sheet.Cell(row, 6).Value = purchase.MatOrderNumber;
                    sheet.Cell(row, 7).Value = purchase.ErrorField.Contains("Invalid Psting Date") ? string.Empty: purchase.PostingDate.ToString("dd-MM-yy");
                    sheet.Cell(row, 8).Value = purchase.TotalQuantity;
                    sheet.Cell(row, 9).Value = purchase.TotalAmount;
                    sheet.Cell(row, 10).Value = purchase.A1PremiumELQuantity;
                    sheet.Cell(row, 11).Value = purchase.A2PremiumLMQuantity;
                    sheet.Cell(row, 12).Value = purchase.A3PremiumP2Quantity;
                    sheet.Cell(row, 13).Value = purchase.A4PremiumSQuantity;
                    sheet.Cell(row, 14).Value = purchase.A5PremiumESQuantity;
                    sheet.Cell(row, 15).Value = purchase.A6PremiumEESQuantity;
                    sheet.Cell(row, 16).Value = purchase.B1SupremeELQuantity;
                    sheet.Cell(row, 17).Value = purchase.B2SupremeLMQuantity;
                    sheet.Cell(row, 18).Value = purchase.B3SupremeP2Quantity;
                    sheet.Cell(row, 19).Value = purchase.B4SupremeSQuantity;
                    sheet.Cell(row, 20).Value = purchase.B5SupremeESQuantity;
                    sheet.Cell(row, 21).Value = purchase.B6SupremeEESQuantity;
                    sheet.Cell(row, 22).Value = purchase.C1UnderSizeQuantity;
                    sheet.Cell(row, 23).Value = purchase.C2SuperEESP2Quantity;
                    sheet.Cell(row, 24).Value = purchase.D1PssEELQuantity;
                    sheet.Cell(row, 25).Value = purchase.D2SuperEL2ESQuantity;
                    sheet.Cell(row, 26).Value = purchase.W1PssROLQuantity;
                    sheet.Cell(row, 27).Value = purchase.A1PremiumELAmount;
                    sheet.Cell(row, 28).Value = purchase.A2PremiumLMAmount;
                    sheet.Cell(row, 29).Value = purchase.A3PremiumP2Amount;
                    sheet.Cell(row, 30).Value = purchase.A4PremiumSAmount;
                    sheet.Cell(row, 31).Value = purchase.A5PremiumESAmount;
                    sheet.Cell(row, 32).Value = purchase.A6PremiumEESAmount;
                    sheet.Cell(row, 33).Value = purchase.B1SupremeELAmount;
                    sheet.Cell(row, 34).Value = purchase.B2SupremeLMAmount;
                    sheet.Cell(row, 35).Value = purchase.B3SupremeP2Amount;
                    sheet.Cell(row, 36).Value = purchase.B4SupremeSAmount; 
                    sheet.Cell(row, 37).Value = purchase.B5SupremeESAmount;
                    sheet.Cell(row, 38).Value = purchase.B6SupremeEESAmount;
                    sheet.Cell(row, 39).Value = purchase.C1UnderSizeAmount;
                    sheet.Cell(row, 40).Value = purchase.C2SuperEESP2Amount;
                    sheet.Cell(row, 41).Value = purchase.D1PssEELAmount;
                    sheet.Cell(row, 42).Value = purchase.D2SuperEL2ESAmount;
                    sheet.Cell(row, 43).Value = purchase.W1PssROLAmount;
                    sheet.Cell(row, 44).Value = purchase.ROAQuantity;
                    sheet.Cell(row, 45).Value = purchase.ROAAmount;

                    if (isErrorTemplate) sheet.Cell(row, 46).Value = purchase.ErrorField;
                    row++;
                }

                return ConvertToExcel(workbook);
            }
        }

        public byte[] ExportProductTemplate(List<ProductDetails> products, string filePath,bool isErrorTemplate)
        {
            using (XLWorkbook workbook = new XLWorkbook(filePath))
            {
                var sheet = workbook.Worksheet(1);
                int row = 2;
                if (isErrorTemplate) sheet.Cell(1, 11).Value = "Error Fields";

                foreach (var product in products)
                {
                    sheet.Cell(row, 1).Value = row-1;
                    sheet.Cell(row, 2).Value = product.Category;
                    sheet.Cell(row, 3).Value = product.Name;
                    sheet.Cell(row, 4).Value = product.Description;
                    sheet.Cell(row, 5).Value = product.Imagelink;
                    sheet.Cell(row, 6).Value = !product.ErrorField.Contains("Availability status in Rampur")? product.IsAvailableInRampur ?"Y":"N":"";
                    sheet.Cell(row, 7).Value = !product.ErrorField.Contains("Availability status in Rohru") ? product.IsAvailableInRohru ? "Y" : "N" : "";
                    sheet.Cell(row, 8).Value = !product.ErrorField.Contains("Availability status in Sainj") ? product.IsAvailableInSainj ? "Y" : "N" : "";
                    sheet.Cell(row, 9).Value = !product.ErrorField.Contains("Availability status in Oddi") ? product.IsAvailableInOddi ? "Y" : "N" : "";
                    sheet.Cell(row, 10).Value = !product.ErrorField.Contains("Invalid Price") ? product.Price.ToString() : "";
                    if (isErrorTemplate) sheet.Cell(row, 11).Value = product.ErrorField;
                    row++;
                }
                workbook.RecalculateAllFormulas();
                return ConvertToExcel(workbook);
            }
        }

        public byte[] ExportPriceTemplate(List<ProductStockKeepingUnit> prices, string filePath, bool isErrorTemplate)
        {
            using (XLWorkbook workbook = new XLWorkbook(filePath))
            {
                var sheet = workbook.Worksheet(1);
                var skus = new List<string>
            {
                "APPLE RD >80% EL",
                "APPLE RD >80% L+M",
                "APPLE RD >80% S",
                "APPLE RD >80% ES",
                "APPLE RD >80% EES",
                "APPLE RD >80% P2",
                "APPLE RD 60-80% EL",
                "APPLE RD 60-80% L+M",
                "APPLE RD 60-80% S",
                "APPLE RD 60-80% ES",
                "APPLE RD 60-80% EES",
                "APPLE RD 60-80% P2",
                "APPLE RD <60% EES+P2",
                "APPLE RD <60% EL2ES",
                "APPLE RD 0-100% EEL",
                "APPLE RD 0-100% US",
                "APPLE RD 0-100% ROL"
            };
                if (isErrorTemplate) sheet.Cell(6, 10).Value = "Error Fields";

                int col = 2;
                foreach(var location in new List<string> { LocationType.Rampur.ToString(),
                    LocationType.Rohru.ToString(),
                    LocationType.Sainj.ToString(),
                    LocationType.Oddi.ToString() })
                {
                    var price = prices.FirstOrDefault(x=> x.Location == location);
                    sheet.Cell(4,col).Value = isErrorTemplate && price.ErrorField.Contains("ShimlaEffectiveDate") ? "" : price.ShimlaEffectiveDate.Value.ToString("M/dd/yyyy");
                    sheet.Cell(5,col).Value = isErrorTemplate && price.ErrorField.Contains("ShimlaExpiryDate") ? "" : price.ShimlaExpiryDate.Value.ToString("M/dd/yyyy");

                    if (location == LocationType.Rampur.ToString() || location == LocationType.Oddi.ToString())
                    {
                        sheet.Cell(4, col + 1).Value = isErrorTemplate && price.ErrorField.Contains("KinnaurEffectiveDate") ? "" : price.KinnaurEffectiveDate.Value.ToString("M/dd/yyyy");
                        sheet.Cell(5, col + 1).Value = isErrorTemplate && price.ErrorField.Contains("KinnaurExpiryDate") ? "" : price.KinnaurExpiryDate.Value.ToString("M/dd/yyyy");
                    }
                    if (isErrorTemplate)
                    {
                        sheet.Cell(4, col).Value = price.ErrorField.Contains("ShimlaEffectiveDate") ?"Invalid Effective Date" : price.ShimlaEffectiveDate.Value.ToString("M/dd/yyyy");
                        sheet.Cell(5, col).Value = price.ErrorField.Contains("ShimlaExpiryDate") ? "Invalid Expiry Date" : price.ShimlaExpiryDate.Value.ToString("M/dd/yyyy");

                        if (location == LocationType.Rampur.ToString() || location == LocationType.Oddi.ToString())
                        {
                            sheet.Cell(4, col + 1).Value = price.ErrorField.Contains("KinnaurEffectiveDate") ? "Invalid Effective Date" : price.KinnaurEffectiveDate.Value.ToString("M/dd/yyyy");
                            sheet.Cell(5, col+1).Value = price.ErrorField.Contains("KinnaurExpiryDate") ? "Invalid Expiry Date" : price.KinnaurExpiryDate.Value.ToString("M/dd/yyyy");
                        }
                    }

                    int row = 7;

                    foreach (var sku in skus)
                    {
                        var productStock = prices.FirstOrDefault(x => x.Location == location && x.StockKeepingUnit == sku);
                        if (productStock != null)
                        {
                            if (isErrorTemplate && (productStock.ErrorField.Contains("Price")
                                || productStock.ErrorField.Contains("Invalid SKU Name")))
                            {
                                sheet.Cell(row, 10).Value = productStock.ErrorField.Contains("Invalid SKU Name") && productStock.ErrorField.Contains("Price") ? "Invalid SKU Name, Invalid Price" :
                                    productStock.ErrorField.Contains("Invalid SKU Name")? "Invalid SKU Name" : "Invalid Price";

                            }

                            if(!productStock.ErrorField.Contains("Invalid Shimla Price"))
                                sheet.Cell(row, col).Value =  productStock.ShimlaPrice;

                            if (!productStock.ErrorField.Contains("Invalid Kinnaur Price") && (location == LocationType.Rampur.ToString() || location == LocationType.Oddi.ToString()))
                                sheet.Cell(row, col + 1).Value = productStock.KinnaurPrice;
                        }
                        row++;
                    }

                    col += (location == LocationType.Rampur.ToString() || location == LocationType.Oddi.ToString()?2:1);
                }

                return ConvertToExcel(workbook);
            }
        }

        public byte[] ExportPaymentTemplate(List<PaymentReport> paymentReports, string filePath, bool isErrorTemplate)
        {
            using (XLWorkbook workbook = new XLWorkbook(filePath))
            {
                var sheet = workbook.Worksheet(1);
                int row = 2;
                if (isErrorTemplate) { 
                    sheet.Cell(1, 3).Value = "Error Fields"; 
                    sheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.FromArgb(192, 192, 192);
                }

                foreach (var payment in paymentReports)
                {
                    sheet.Cell(row, 1).Value = payment.VendorCode;
                    sheet.Cell(row, 2).Value = payment.Amount;
                    sheet.Cell(row, 2).Style.NumberFormat.Format = "#,##0.00";
                    if (isErrorTemplate) sheet.Cell(row, 3).Value = payment.ErrorField;
                    row++;
                }
                return ConvertToExcel(workbook);
            }
        }

        public byte[] ExportToExcel<T>(List<T> items, bool isCSVFile = false)
        {
            DataTable dataTable = GetDataTable(items);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var sheet = wb.Worksheets.Add(dataTable, ApplicationConstants.WorkSheetName);
                dataTable.Dispose();

                sheet.ColumnWidth = 15;
                sheet.SheetView.FreezeRows(1);
                sheet.Table(0).ShowAutoFilter = false;
                sheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                sheet.Style.Alignment.WrapText = true;
                sheet.Table(0).Theme = XLTableTheme.TableStyleLight9;
                sheet.Table(0).Style.NumberFormat.Format = "@";

                return isCSVFile ? ConvertToCSV(sheet) : ConvertToExcel(wb);
            }
        }

        private static DataTable GetDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            dataTable.Columns.Add("S. No.");

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(Regex.Replace(prop.Name, @"([A-Z]+?(?=[A-Z][^A-Z])|[A-Z]+?(?=[^A-Z]))", " $1"));
            }

            int serialNumber = 1;
            foreach (T item in items)
            {
                var values = new object[(Props.Length + 1)];
                values[0] = serialNumber;
                serialNumber++;

                for (int i = 1; i <= Props.Length; i++)
                {
#pragma warning disable
                    values[i] = Props[i - 1].GetValue(item, null);
#pragma warning restore
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        private static byte[] ConvertToExcel(XLWorkbook workbook)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }

        private static byte[] ConvertToCSV(IXLWorksheet sheet)
        {
            StringBuilder sb = new StringBuilder();
            var lastCellAddress = sheet.RangeUsed().LastCell().Address;
            var rows = sheet.Rows(1, lastCellAddress.RowNumber)
                .Select(r => string.Join(",", r.Cells(1, lastCellAddress.ColumnNumber)
                        .Select(cell =>
                        {
                            var cellValue = cell.GetValue<string>();
                            return cellValue.Contains(',') ? $"\"{cellValue}\"" : cellValue;
                        })));
            foreach (var row in rows)
            {
                sb.AppendLine(row.ToString());
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public DateTime? ConvertIstFromUtc(DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.AddHours(5).AddMinutes(30) : dateTime;
        }
    }
}