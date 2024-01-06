/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Farmpik.Domain.Common.Enum;
using Farmpik.Domain.Dto;
using Farmpik.Domain.Entities;
using Farmpik.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Farmpik.Infrastructure.Repository
{
    public class ImportFileRepository : IImportFileRepository
    {
        private readonly IHelperMethod _helperMethod;

        public ImportFileRepository(IHelperMethod helperMethod)
        {
            _helperMethod = helperMethod;
        }

        public async Task<TemplateDto<T>> GetDataFromTemplate<T>(HttpPostedFileBase formFile, string[] columnNames, int startRow,
            int startCol)
        {  
            using (XLWorkbook workbook = new XLWorkbook(formFile.InputStream))
            {
                var sheet = workbook.Worksheet(1);
                TemplateDto<T> template = new TemplateDto<T>()
                {
                    IsValidTemplate = typeof(T).Name == nameof(ProductStockKeepingUnit) ? IsValidPricingTemplate(sheet)
                    : IsValidTemplate(sheet, columnNames, startRow, startCol)
                };

                if (template.IsValidTemplate ?? false)
                {
                    int blankrow = 0;
                    int endCol = startCol + columnNames.Length - 1;
                    for (int row = startRow + 1; row < sheet.RowCount(); row++)
                    {
                        if (IsEmptyRow(sheet.Row(row), startCol, endCol))
                        {
                            blankrow++;
                            if (blankrow > 2) { break; }
                            continue;
                        }

                        blankrow = 0;
                        var item = GetModel(typeof(T).Name, sheet.Row(row), columnNames);

                        if (item.GetType().IsGenericType)
                        {
                            template.Records.AddRange((List<T>)Convert.ChangeType(item, typeof(List<T>)));
                        }
                        else
                        {
                            template.Records.Add((T)Convert.ChangeType(item, typeof(T)));
                        }

                        if (!template.HasErrorFields)
                        {
                            bool? hasErrorFields;
                            if (item.GetType().IsGenericType)
                            {
                                var data = (List<ProductStockKeepingUnit>)Convert.ChangeType(item, typeof(List<ProductStockKeepingUnit>));
                                hasErrorFields = data.FirstOrDefault(x => x.HasErrorField) != null;
                            }
                            else
                            {
                                System.Reflection.PropertyInfo pi = item.GetType().GetProperty("HasErrorField");
                                hasErrorFields = (bool)(pi.GetValue(item, null));
                            }
                            template.HasErrorFields = hasErrorFields ?? false;
                        }
                    }
                }
                return await Task.FromResult(template);
            }
        }

        private object GetModel(string name, IXLRow row, string[] columnNames)
        {
            switch (name)
            {
                case nameof(Vendor): return GetVendorFromRow(row,columnNames);
                case nameof(PurchaseReport): return GetPurchaseFromRow(row, columnNames);
                case nameof(ProductStockKeepingUnit): return GetPriceFromRow(row, columnNames);
                case nameof(ProductDetails): return GetProductFromRow(row, columnNames);
                case nameof(PaymentReport): return GetPaymentFromRow(row, columnNames);
                default: return string.Empty;
            }
        }

        private Vendor GetVendorFromRow(IXLRow row, string[] columnNames)
        {
            string error = string.Empty;
            return new Vendor
            {
                VendorCode = ParseNumberOnly(row.Cell(1), ref error, columnNames[0],6),
                VendorName = ParseVendorName(row.Cell(2), ref error, columnNames[1]),
                Telephone = _helperMethod.Encrypt(ParseNumberOnly(row.Cell(3), ref error, columnNames[2],10)),
                HasErrorField = !string.IsNullOrEmpty(error),
                ErrorField = !string.IsNullOrEmpty(error) ? error.Substring(0, error.Length - 1) : error
            };
        }

        private PurchaseReport GetPurchaseFromRow(IXLRow row, string[] columnNames)
        {
            string error = string.Empty;
            return new PurchaseReport
            {
                PlantName = ParseValue(row.Cell(1), ref error, columnNames[0], 60,false),
                GateNumber = ParseNumberOnly(row.Cell(2), ref error, columnNames[1]),
                VendorCode = ParseNumberOnly(row.Cell(3), ref error, columnNames[2],6),
                VendorName = ParseVendorName(row.Cell(4), ref error, columnNames[3]),
                PurchaseOrderNumber = ParseNumberOnly(row.Cell(5), ref error, columnNames[4]),
                MatOrderNumber = ParseAlpaNumeric(row.Cell(6), ref error, columnNames[5]),
                PostingDate = ParseDateTime(row.Cell(7), ref error, "Invalid Psting Date (Format - DD-MM-yy)", "dd-MM-yy"),
                TotalQuantity = ParseValue<decimal>(row.Cell(8), ref error, columnNames[7]),
                TotalAmount = ParseValue<decimal>(row.Cell(9), ref error, columnNames[8]),
                A1PremiumELQuantity = ParseValue<decimal>(row.Cell(10), ref error, columnNames[9]),
                A2PremiumLMQuantity = ParseValue<decimal>(row.Cell(11), ref error, columnNames[10]),
                A3PremiumP2Quantity = ParseValue<decimal>(row.Cell(12), ref error, columnNames[11]),
                A4PremiumSQuantity = ParseValue<decimal>(row.Cell(13), ref error, columnNames[12]),
                A5PremiumESQuantity = ParseValue<decimal>(row.Cell(14), ref error, columnNames[13]),
                A6PremiumEESQuantity = ParseValue<decimal>(row.Cell(15), ref error, columnNames[14]),
                B1SupremeELQuantity = ParseValue<decimal>(row.Cell(16), ref error, columnNames[15]),
                B2SupremeLMQuantity = ParseValue<decimal>(row.Cell(17), ref error, columnNames[16]),
                B3SupremeP2Quantity = ParseValue<decimal>(row.Cell(18), ref error, columnNames[17]),
                B4SupremeSQuantity = ParseValue<decimal>(row.Cell(19), ref error, columnNames[18]),
                B5SupremeESQuantity = ParseValue<decimal>(row.Cell(20), ref error, columnNames[19]),
                B6SupremeEESQuantity = ParseValue<decimal>(row.Cell(21), ref error, columnNames[20]),
                C1UnderSizeQuantity = ParseValue<decimal>(row.Cell(22), ref error, columnNames[21]),
                C2SuperEESP2Quantity = ParseValue<decimal>(row.Cell(23), ref error, columnNames[22]),
                D1PssEELQuantity = ParseValue<decimal>(row.Cell(24), ref error, columnNames[23]),
                D2SuperEL2ESQuantity = ParseValue<decimal>(row.Cell(25), ref error, columnNames[24]),
                W1PssROLQuantity = ParseValue<decimal>(row.Cell(26), ref error, columnNames[25]),

                A1PremiumELAmount = ParseValue<decimal>(row.Cell(27), ref error, columnNames[26]),
                A2PremiumLMAmount = ParseValue<decimal>(row.Cell(28), ref error, columnNames[27]),
                A3PremiumP2Amount = ParseValue<decimal>(row.Cell(29), ref error, columnNames[28]),
                A4PremiumSAmount = ParseValue<decimal>(row.Cell(30), ref error, columnNames[29]),
                A5PremiumESAmount = ParseValue<decimal>(row.Cell(31), ref error, columnNames[30]),
                A6PremiumEESAmount = ParseValue<decimal>(row.Cell(32), ref error, columnNames[31]),
                B1SupremeELAmount = ParseValue<decimal>(row.Cell(33), ref error, columnNames[32]),
                B2SupremeLMAmount = ParseValue<decimal>(row.Cell(34), ref error, columnNames[33]),
                B3SupremeP2Amount = ParseValue<decimal>(row.Cell(35), ref error, columnNames[34]),
                B4SupremeSAmount = ParseValue<decimal>(row.Cell(36), ref error, columnNames[35]),
                B5SupremeESAmount = ParseValue<decimal>(row.Cell(37), ref error, columnNames[36]),
                B6SupremeEESAmount = ParseValue<decimal>(row.Cell(38), ref error, columnNames[37]),
                C1UnderSizeAmount = ParseValue<decimal>(row.Cell(39), ref error, columnNames[38]),
                C2SuperEESP2Amount = ParseValue<decimal>(row.Cell(40), ref error, columnNames[39]),
                D1PssEELAmount = ParseValue<decimal>(row.Cell(41), ref error, columnNames[40]),
                D2SuperEL2ESAmount = ParseValue<decimal>(row.Cell(42), ref error, columnNames[41]),
                W1PssROLAmount = ParseValue<decimal>(row.Cell(43), ref error, columnNames[42]),
                ROAQuantity = ParseValue<decimal>(row.Cell(44), ref error, columnNames[43]),
                ROAAmount = ParseValue<decimal>(row.Cell(45), ref error, columnNames[44]),
                HasErrorField = !string.IsNullOrEmpty(error),
                ErrorField = !string.IsNullOrEmpty(error) ? error.Substring(0, error.Length - 1) : error
            };
        }

        private static List<ProductStockKeepingUnit> GetPriceFromRow(IXLRow row, string[] columnNames)
        {
            var prices = new List<ProductStockKeepingUnit>();
            for (int col = 2; col <= 6; col++)
            {
                var location = row.Worksheet.Cell(3, col).GetValue<string>();
                if (!string.IsNullOrEmpty(location)) {
                    prices.Add(GetPriceFromCol(row, location, col));

                    if(location == LocationType.Rampur.ToString()|| location == LocationType.Oddi.ToString()) col++;
                };
            } 
            return prices;
        }

        private static ProductStockKeepingUnit GetPriceFromCol(IXLRow row,string location, int col)
        {
            string error = string.Empty;
            return new ProductStockKeepingUnit
            {
                Location = location,
                StockKeepingUnit = ParseSku(row.Cell(1), ref error),
                ShimlaPrice = ParsePriceValue(row.Cell(col), ref error, $"Shimla Price for {location} location"),
                KinnaurPrice = location == LocationType.Rohru.ToString() || location == LocationType.Sainj.ToString() ? 0
                : ParsePriceValue(row.Cell(col+1), ref error, $"Kinnaur Price for {location} location"),
                ShimlaEffectiveDate = ParseDateTime(row.Worksheet.Cell(4,col), ref error, "ShimlaEffectiveDate", "M/dd/yyyy"),
                KinnaurEffectiveDate = location == LocationType.Rohru.ToString()
                || location == LocationType.Sainj.ToString() ? new DateTime(2000, 1, 1) 
                : ParseDateTime(row.Worksheet.Cell(4, col+1), 
                ref error, "KinnaurEffectiveDate", "M/dd/yyyy"),
                ShimlaExpiryDate = ParseDateTime(row.Worksheet.Cell(5, col), ref error, "ShimlaExpiryDate", "M/dd/yyyy"),
                KinnaurExpiryDate = location == LocationType.Rohru.ToString()
                || location == LocationType.Sainj.ToString() ? new DateTime(2000, 1, 1)
                : ParseDateTime(row.Worksheet.Cell(5, col + 1),
                ref error, "KinnaurExpiryDate", "M/dd/yyyy"),
                HasErrorField = !string.IsNullOrEmpty(error),
                ErrorField = !string.IsNullOrEmpty(error) ? error.Substring(0, error.Length - 1) : error
            };
        }

        private static ProductDetails GetProductFromRow(IXLRow row, string[] columnNames)
        {
            string error = string.Empty;
            return new ProductDetails
            {
                Category = ParseDropDownValue(row.Cell(2), ref error, columnNames[0], new List<string> {"Fertilizers","Gloves","Tools","Seeds","Crate","Masks","Spray","Others"}),
                Name = ParseValue(row.Cell(3), ref error, columnNames[1],60,true),
                Description = ParseValue(row.Cell(4), ref error, "Product Description", 2000, true),
                Imagelink = ParseImageUrl(row.Cell(5), ref error, "Product Image", 200),
                IsAvailableInRampur = ParseYesNoValue(row.Cell(6), ref error, "Availability status in Rampur"),
                IsAvailableInRohru = ParseYesNoValue(row.Cell(7), ref error, "Availability status in Rohru"),
                IsAvailableInSainj = ParseYesNoValue(row.Cell(8), ref error, "Availability status in Sainj"),
                IsAvailableInOddi = ParseYesNoValue(row.Cell(9), ref error, "Availability status in Oddi"),
                Price = ParsePriceValue(row.Cell(10), ref error, "Price"),
                HasErrorField = !string.IsNullOrEmpty(error),
                ErrorField = !string.IsNullOrEmpty(error) ? error.Substring(0,error.Length-1) : error
            };
        }

        private static PaymentReport GetPaymentFromRow(IXLRow row, string[] columnNames)
        {
            string error = string.Empty;
            return new PaymentReport
            {
                VendorCode = ParseNumberOnly(row.Cell(1), ref error, columnNames[0],6),
                Amount = ParseValue<decimal>(row.Cell(2), ref error, columnNames[1]),
                HasErrorField = !string.IsNullOrEmpty(error),
                ErrorField = !string.IsNullOrEmpty(error) ? error.Substring(0, error.Length - 1) : error
            };
        }

        private static bool IsValidRow(IXLRow row, int[] uniqueColumn)
        {
            foreach (int col in uniqueColumn)
            {
                if (!string.IsNullOrEmpty(row.Cell(col).GetValue<string>())) { return true; }
            }
            return false;
        }

        private static bool IsEmptyRow(IXLRow row, int startCol, int endCol)
        {
            for (int col = startCol; col <= endCol; col++)
            {
                if (!string.IsNullOrEmpty(row.Cell(col).GetValue<string>())) { return false; }
            }
            return true;
        }

        private static T GetCellValue<T>(IXLWorksheet worksheet, string address)
        {
            return worksheet.Cell(address).GetValue<T>();
        }

        private static bool IsValidPricingTemplate(IXLWorksheet worksheet)
        {
            List<string> locations = new List<string>(); 
            for(int col = 2; col <= 6; col++) {
                var location = worksheet.Cell(3, col).GetValue<string>();
                if (!string.IsNullOrEmpty(location)) locations.Add(location);
            }          
            return locations.Contains(LocationType.Rampur.ToString()) && locations.Contains(LocationType.Rohru.ToString())
            && locations.Contains(LocationType.Sainj.ToString()) && locations.Contains(LocationType.Oddi.ToString());
        }

        private static bool IsValidTemplate(IXLWorksheet worksheet, string[] columnNames, int startRow,
            int startCol)
        {
            for (int col = startCol; col < startCol + columnNames.Length; col++)
            {
                if (!GetCellValue<string>(worksheet, GetAddress(col, startRow)).Trim().Contains(columnNames[col - startCol].Trim()))
                {
                    return false;
                }
            }

            return true;
        }

        private static string GetAddress(int col, int row)
        {
            if (col > 26) { return $"A{(char)(col % 26 + 64)}{row}"; }
            return $"{(char)(col + 64)}{row}";
        }

        private static T ParseValue<T>(IXLCell cell, ref string error, string columnName)
        {
            try
            {
                return cell.GetValue<T>();
            }
            catch (Exception)
            {
                error += $"Invalid {columnName},";
                return default;
            }
        }

        private static string ParseValue(IXLCell cell, ref string error, string columnName,
            int maxLength = 100, bool isRequred= true)
        {
            string value = cell.GetValue<string>();
            if(value.Length < maxLength && (!isRequred || !string.IsNullOrEmpty(value)))
            {
                return value;
            }
            else
            {
                error +=  $"Invalid {columnName},";
                return isRequred?string.Empty: default;
            }
        }

        private static string ParseNumberOnly(IXLCell cell, ref string error, string columnName,int length)
        {
            string value = cell.GetValue<string>();
            if (value.All(char.IsDigit) && value.Length == length)
            {
                return value;
            }
            else
            {
                error += $"Invalid {columnName},";
                return string.Empty;
            }
        }

        private static string ParseNumberOnly(IXLCell cell, ref string error, string columnName)
        {
            string value = cell.GetValue<string>();
            if (value.All(char.IsDigit) && value.Length > 0 && value.Length<=10)
            {
                return value;
            }
            else
            {
                error += $"Invalid {columnName},";
                return string.Empty;
            }
        }

        private static string ParseAlpaNumeric(IXLCell cell, ref string error, string columnName)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9\s]");
            string value = cell.GetValue<string>();
            if (!string.IsNullOrEmpty(value) && !regex.IsMatch(value) && value.Length>0 && value.Length <= 60)
            {
                return value;
            }
            else
            {
                error += $"Invalid {columnName},";
                return string.Empty;
            }
        }

        private static string ParseVendorName(IXLCell cell, ref string error, string columnName)
        {
            string value = cell.GetValue<string>();
            if (!string.IsNullOrEmpty(value) && value.Length > 0 && value.Length <= 60)
            {
                return value;
            }
            else
            {
                error += $"Invalid {columnName},";
                return string.Empty;
            }
        }

        private static bool ParseYesNoValue(IXLCell cell, ref string error, string columnName)
        {
            string value = cell.GetValue<string>();
            if (value == "Y" || value == "N")
            {
                return value == "Y";
            }
            else
            {
                error += $"Invalid {columnName},";
                return default;
            }
        }

        private static string ParseImageUrl(IXLCell cell, ref string error, string columnName, int maxLength = 100)
        {
            Regex regex = new Regex(@"(https?:)?//?[^\'""<>]+?\.(jpg|jpeg|gif|png|webp)");
            string value = cell.GetValue<string>();
            if (string.IsNullOrEmpty(value) || (regex.IsMatch(value) && value.Length <= maxLength))
            {
                return value;
            }
            else
            {
                error += $"Invalid {columnName},";
                return string.Empty;
            }
        }

        private static string ParseDropDownValue(IXLCell cell, ref string error, string columnName, List<string> dropdowns)
        {
            string value = cell.GetValue<string>();
            if (dropdowns.Contains(value))
            {
                return value;
            }
            else
            {
                error += $"Invalid {columnName},";
                return string.Empty;
            }
        }
        
        private static decimal ParsePriceValue(IXLCell cell, ref string error, string columnName)
        {
            try
            {
                var price = cell.GetValue<decimal>();
                if(price<=0) error += $"Invalid {columnName},";
                return price;
            }
            catch (Exception)
            {
                error += $"Invalid {columnName},";
                return default;
            }
        }

        private static DateTime ParseDateTime(IXLCell cell, ref string error, string errorMessage, string format)
        {
            try
            {
                var date = cell.GetValue<string>();
                if (date.All(char.IsDigit))
                {
                    return DateTime.FromOADate(double.Parse(date));
                }
                return DateTime.ParseExact(date.Split(' ')[0], format, null);
            }
            catch(Exception) {
                error += $"{errorMessage},";
                return new DateTime(2000,1,1);
            }
        }

        private static DateTime ParseOnlyPastDate(IXLCell cell, ref string error, string errorMessage, string format)
        {
            try
            {
                var date = cell.GetValue<string>();
                DateTime parseDate = date.All(char.IsDigit) ? DateTime.FromOADate(double.Parse(date)) 
                    : DateTime.ParseExact(date.Split(' ')[0], format, null);

                if (parseDate < DateTime.Now) return parseDate;

                error += $"Future dates are not allowed {errorMessage},";
                return new DateTime(2000, 1, 1);
            }
            catch (Exception)
            {
                error += $"{errorMessage},";
                return new DateTime(2000, 1, 1);
            }
        }

        private static string ParseSku(IXLCell cell, ref string error)
        {
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
            if (skus.Contains(cell.GetValue<string>()))
            {
                return cell.GetValue<string>();
            }
            else
            {
                error += $"Invalid SKU Name,";
                return skus[cell.Address.RowNumber -7];
            }
        }
    }
}