/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Farmpik.Domain.Interfaces.Repositories
{
    public interface IReportRepository
    {
        DateTime? ConvertIstFromUtc(DateTime? dateTime);

        byte[] ExportToExcel<T>(List<T> items, bool isCSVFile = false);

        byte[] ExportVendorTemplate(List<Vendor> vendors, string filePath, bool isErrorTemplate);

        byte[] ExportPurchaseTemplate(List<PurchaseReport> purchases, string filePath, bool isErrorTemplate);

        byte[]  ExportPriceTemplate(List<ProductStockKeepingUnit> prices, string filePath, bool isErrorTemplate);

        byte[] ExportProductTemplate(List<ProductDetails> products, string filePath, bool isErrorTemplate);

        byte[] ExportPaymentTemplate(List<PaymentReport> products, string filePath, bool isErrorTemplate);
    }
}