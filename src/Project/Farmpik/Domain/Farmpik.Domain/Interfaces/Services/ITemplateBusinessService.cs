/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Common;
using Farmpik.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Farmpik.Domain.Interfaces.Services
{
    public interface ITemplateBusinessService
    {
        Task<PaginationApiResponse<List<VendorDto>>> GetVendors(int pageNumber, int pageSize);

        Task<PaginationApiResponse<List<PaymentDto>>> GetPayments(int pageNumber, int pageSize);

        Task<PaginationApiResponse<List<PurchaseDto>>> GetPurchases(int pageNumber, int pageSize);

        Task<byte[]> ExportVendorTemplate(string filePath, bool isErrorTemplate);

        Task<byte[]> ExportPurchaseTemplate(string filePath, bool isErrorTemplate);

        Task<byte[]> ExportProductTemplate(string filePath, bool isErrorTemplate);

        Task<byte[]> ExportPriceTemplate(string filePath, bool isErrorTemplate);

        Task<byte[]> ExportPaymentTemplate(string filePath, bool isErrorTemplate);
        Task<byte[]> ExportGuestUser(DateTime? start, DateTime? end);
    }
}
