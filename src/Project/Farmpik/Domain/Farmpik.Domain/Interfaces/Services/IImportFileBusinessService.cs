/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Commands.ImportFileCommands;
using Farmpik.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Farmpik.Domain.Interfaces.Services
{
    public interface IImportFileBusinessService
    {
        Task<TemplateDto<string>> ImportVendorTemplate(HttpPostedFileBase formFile, Guid createdBy);
        Task<TemplateDto<string>> ImportPurchaseTemplate(HttpPostedFileBase formFile, Guid createdBy);
        Task<TemplateDto<string>> ImportPriceTemplate(HttpPostedFileBase formFile, Guid createdBy);
        Task<TemplateDto<string>> ImportProductTemplate(HttpPostedFileBase formFile, Guid createdBy);
        Task<TemplateDto<string>> ImportPaymentTemplate(HttpPostedFileBase formFile, Guid createdBy);

        Task<List<ImportDetails>> GetImportDetails();
        Task<ImportDetails> GetImportDetailsByTemplateName(string templateName);
    }
}