/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Commands.ImportFileCommands;
using Farmpik.Domain.Dto;
using Farmpik.Domain.Entities;
using Farmpik.Domain.Interfaces.Repositories;
using Farmpik.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Farmpik.Services.BackendServices
{
    public class ImportFileBusinessService : IImportFileBusinessService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IProductStockKeepingUnitRepository _productStockKeepingUnitRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IImportFileRepository _importFileRepository;

        public ImportFileBusinessService(IImportFileRepository importFileRepository,
            IVendorRepository vendorRepository,
            IProductStockKeepingUnitRepository productStockKeepingUnitRepository,
            IProductRepository productRepository,
            IPaymentRepository paymentRepository,
            IPurchaseRepository purchaseRepository)
        {
            _importFileRepository = importFileRepository;
            _vendorRepository = vendorRepository;
            _productStockKeepingUnitRepository = productStockKeepingUnitRepository;
            _productRepository = productRepository;
            _paymentRepository = paymentRepository;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<List<ImportDetails>> GetImportDetails()
        {
            return new List<ImportDetails>
            {   await _vendorRepository.VendorImportDetails(),
                await _purchaseRepository.PurchaseImportDetails(),
                await _productStockKeepingUnitRepository.PriceImportDetails(),
                await  _productRepository.ProductImportDetails(),
                await _paymentRepository.PaymentImportDetails()
            };
        }

        public async Task<ImportDetails> GetImportDetailsByTemplateName(string templateName)
        {
            switch (templateName)
            {
                case "Vendor Master": return await _vendorRepository.VendorImportDetails();
                case "PRN Master": return await _purchaseRepository.PurchaseImportDetails();
                case "Price Master": return await _productStockKeepingUnitRepository.PriceImportDetails();
                case "Product details": return await _productRepository.ProductImportDetails();
                case "Payment status": return await _paymentRepository.PaymentImportDetails();
            }
            return default;
        }

        public async Task<TemplateDto<string>> ImportVendorTemplate(HttpPostedFileBase formFile, Guid createdBy)
        {
            string[] ColumnNames = { "Vendor Code" ,"Vendor Name","Telephone" };
            var template = await _importFileRepository.GetDataFromTemplate<Vendor>(formFile,
                ColumnNames, 1, 1);
            if (template.IsValidTemplate == true && template.Records.Count > 0)
            {
                await _vendorRepository.ImportBulkVendors(template.Records, createdBy);
            }
            return new TemplateDto<string>
            {
                IsValidTemplate = template.IsValidTemplate,
                HasErrorFields = template.HasErrorFields,
                Name = "Vendor Master",
                TotalRecords = template.Records.Count,
                ErrorRecords = template.Records.Count(x=> x.HasErrorField)
            };
        }

        public async Task<TemplateDto<string>> ImportPurchaseTemplate(HttpPostedFileBase formFile, Guid createdBy)
        {
            string[] ColumnNames = { "Plant Name", "GateIn Number", "Vendor", "Vendor Name", "Purch.Doc.","Mat. Doc.",
                        "Pstng Date", "Total Qty", "Total AMT", "A1-PREMIUM EL Qty", "A2-PREMIUM L+M Qty",
                        "A3-PREMIUM P2 Qty", "A4-PREMIUM S Qty", "A5-PREMIUM ES Qty", "A6-PREMIUM EES Qty",
                        "B1-SUPREME EL Qty", "B2-SUPREME L+M Qty", "B3-SUPREME P2 Qty", "B4-SUPREME S Qty",
                        "B5-SUPREME ES Qty", "B6-SUPREME EES Qty", "C1-Under Size Qty", "C2-SUPER EES+P2 Qty",
                        "D1-PSS EEL Qty", "D2-SUPER EL 2 ES Qty", "W1-PSS ROL Qty", "A1-PREMIUM EL AMT",
                        "A2-PREMIUM L+M AMT", "A3-PREMIUM P2 AMT", "A4-PREMIUM S AMT", "A5-PREMIUM ES AMT",
                        "A6-PREMIUM EES AMT", "B1-SUPREME EL AMT", "B2-SUPREME L+M AMT",
                        "B3-SUPREME P2 AMT", "B4-SUPREME S AMT", "B5-SUPREME ES AMT", "B6-SUPREME EES AMT",
                        "C1-Under Size AMT", "C2-SUPER EES+P2 AMT", "D1-PSS EEL AMT", "D2-SUPER EL 2 ES AMT",
                        "W1-PSS ROL AMT", "ROA Qty", "ROA AMT"
                    };
            var template = await _importFileRepository.GetDataFromTemplate<PurchaseReport>(formFile,
                ColumnNames, 1,1);

            if (template.IsValidTemplate == true && template.Records.Count > 0)
            {
                var errorgateNumbers = template.Records.Where(x => x.HasErrorField).Select(x => x.GateNumber).ToList();
                var errorPurchases = template.Records.Where(x => errorgateNumbers.Contains(x.GateNumber)).ToList();
                errorPurchases.ForEach(x => x.HasErrorField = true);
                await _purchaseRepository.ImportBulkPurchaseReports(template.Records, createdBy);
            }
            return new TemplateDto<string>
            {
                IsValidTemplate = template.IsValidTemplate,
                HasErrorFields = template.HasErrorFields,
                Name = "PRN Master",
                TotalRecords = template.Records.Count,
                ErrorRecords = template.Records.Count(x => x.HasErrorField == true)
            };
        }

        public async Task<TemplateDto<string>> ImportPriceTemplate(HttpPostedFileBase formFile, Guid createdBy)
        {
            string[] ColumnNames = { "Apple SKU List", "Shimla", "Kinnaur" };
            var template = await _importFileRepository.GetDataFromTemplate<ProductStockKeepingUnit>(formFile,
            ColumnNames, 6, 1);
            int errorCount = 0;
            int totalRecord = template.Records.Count(x => x.Location == "Rohru" || x.Location == "Sainj")
                + 2 * template.Records.Count(x => x.Location == "Rampur" || x.Location == "Oddi");

            if (template.IsValidTemplate == true && template.Records.Count > 0)
            {
                await _productStockKeepingUnitRepository.ImportBulkPrices(template.Records, createdBy);
                errorCount = template.Records.Count(x => x.HasErrorField == true && !string.IsNullOrEmpty(x.ErrorField) && (x.Location == "Rohru" || x.Location == "Sainj"));
                foreach(var item in template.Records.Where(x => x.HasErrorField == true && (x.Location == "Rampur" || x.Location == "Oddi"))){
                    if (item.ErrorField.Contains("Invalid SKU Name"))
                    {
                        errorCount+=2;
                        continue;
                    }

                    if (item.ErrorField.Contains("Invalid Shimla Price") || item.ErrorField.Contains("ShimlaEffectiveDate") || item.ErrorField.Contains("ShimlaExpiryDate"))
                    {
                        errorCount++;
                    }

                    if (item.ErrorField.Contains("Invalid Kinnaur Price") || item.ErrorField.Contains("KinnaurEffectiveDate") || item.ErrorField.Contains("KinnaurExpiryDate"))
                    {
                        errorCount++;
                    }
                }
            }

            return new TemplateDto<string>
            {
                IsValidTemplate = template.IsValidTemplate,
                HasErrorFields = template.HasErrorFields,
                Name = "Price Master",
                TotalRecords = totalRecord,
                ErrorRecords = errorCount
            };
        }

        public async Task<TemplateDto<string>> ImportProductTemplate(HttpPostedFileBase formFile, Guid createdBy)
        {
            string[] ColumnNames = { "Category Name", "Product Name", "Description", "Product Image link", "Available in Rampur", "Available in Rohru", "Available in Sainj", "Available in Oddi", "Price (in Rs.)" };
            var template = await _importFileRepository.GetDataFromTemplate<ProductDetails>(formFile,
            ColumnNames, 1, 2);

            if (template.IsValidTemplate == true && template.Records.Count>0)
            {
                await _productRepository.ImportBulkProducts(template.Records, createdBy);
            }
            return new TemplateDto<string>
            {
                IsValidTemplate = template.IsValidTemplate,
                HasErrorFields = template.HasErrorFields,
                Name = "Product details",
                TotalRecords = template.Records.Count,
                ErrorRecords = template.Records.Count(x => x.HasErrorField == true)
            };
        }

        public async Task<TemplateDto<string>> ImportPaymentTemplate(HttpPostedFileBase formFile, Guid createdBy)
        {
            string[] ColumnNames = { "Vendor", "Amount in Doc. Curr."};
            var template = await _importFileRepository.GetDataFromTemplate<PaymentReport>(formFile,
            ColumnNames, 1, 1);

            if (template.IsValidTemplate == true && template.Records.Count>0)
            {
                await _paymentRepository.ImportBulkPaymentReports(template.Records, createdBy);
            }
            return new TemplateDto<string>
            {
                IsValidTemplate = template.IsValidTemplate,
                HasErrorFields = template.HasErrorFields,
                Name = "Payment status",
                TotalRecords = template.Records.Count,
                ErrorRecords = template.Records.Count(x => x.HasErrorField == true)
            };
        }
    }
}