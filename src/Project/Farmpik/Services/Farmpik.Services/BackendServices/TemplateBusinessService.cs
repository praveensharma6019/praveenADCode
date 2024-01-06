/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using AutoMapper;
using Farmpik.Domain.Common;
using Farmpik.Domain.Dto;
using Farmpik.Domain.Interfaces.Repositories;
using Farmpik.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmpik.Services.BackendServices
{
   public class TemplateBusinessService: ITemplateBusinessService
    {
        private readonly IMapper _mapper;
        private readonly IHelperMethod _helperMethod;
        private readonly IVendorRepository _vendorRepository;
        private IGuestUserRepository _guestUserRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IProductStockKeepingUnitRepository _stockKeepingUnitRepository;
        private readonly IProductRepository _productRepository;
        private readonly IReportRepository _reportRepository;

        public TemplateBusinessService(IMapper mapper,
            IHelperMethod helperMethod,
            IProductStockKeepingUnitRepository stockKeepingUnitRepository,
            IProductRepository productRepository,
            IReportRepository reportRepository,
            IVendorRepository vendorRepository,
            IGuestUserRepository guestUserRepository,
            IPaymentRepository paymentRepository,
            IPurchaseRepository purchaseRepository)
        {
            _mapper = mapper;
            _helperMethod = helperMethod;
            _stockKeepingUnitRepository = stockKeepingUnitRepository;
            _productRepository = productRepository;
            _reportRepository = reportRepository;
            _vendorRepository = vendorRepository;
            _guestUserRepository = guestUserRepository;
            _paymentRepository = paymentRepository;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<PaginationApiResponse<List<VendorDto>>> GetVendors(int pageNumber, int pageSize)
        {
            
            var result = await _vendorRepository.GetAllAsync(x => x.IsActive,
            pageNumber: pageNumber, pageSize: pageSize);
            result.Value.ForEach(x => { x.Telephone = _helperMethod.Decrypt<string>(x.Telephone); });
            var vendors = _mapper.Map<List<VendorDto>>(result.Value);
            return ResponseHelper.GetPaginationResponse(vendors,
                result.TotalCount > 0,
                totalRecord: result.TotalCount);
        }

        public async Task<PaginationApiResponse<List<PaymentDto>>> GetPayments(int pageNumber, int pageSize)
        {

            var result = await _paymentRepository.GetAllAsync(x => x.IsActive,
            pageNumber: pageNumber, pageSize: pageSize);

            return ResponseHelper.GetPaginationResponse(
                _mapper.Map<List<PaymentDto>>(result.Value),
                result.TotalCount > 0,
                totalRecord: result.TotalCount);
        }

        public async Task<PaginationApiResponse<List<PurchaseDto>>> GetPurchases(int pageNumber, int pageSize)
        {

            var result = await _purchaseRepository.GetAllAsync(x => x.IsActive,
            pageNumber: pageNumber, pageSize: pageSize);

            return ResponseHelper.GetPaginationResponse(
                _mapper.Map<List<PurchaseDto>>(result.Value),
                result.TotalCount > 0,
                totalRecord: result.TotalCount);
        }

        public async Task<byte[]> ExportVendorTemplate(string filePath, bool isErrorTemplate)
        {
            var result = await _vendorRepository.GetAllAsync(x => x.IsActive && (x.HasErrorField == isErrorTemplate), orderBy: x => new { x.CreatedOn });
            result.Value.ForEach(x => { x.Telephone = _helperMethod.Decrypt<string>(x.Telephone); });

            return _reportRepository.ExportVendorTemplate(result.Value,filePath, isErrorTemplate);
        }

        public async Task<byte[]> ExportPurchaseTemplate(string filePath, bool isErrorTemplate)
        {
            var result = await _purchaseRepository.GetAllAsync(x => x.IsActive &&  x.HasErrorField == isErrorTemplate, orderBy: x => new { x.CreatedOn });

            return _reportRepository.ExportPurchaseTemplate(result.Value, filePath, isErrorTemplate);
        }

        public async Task<byte[]> ExportProductTemplate(string filePath, bool isErrorTemplate)
        {
            var result = await _productRepository.GetAllAsync(x => x.IsActive && x.HasErrorField == isErrorTemplate, orderBy: x => new { x.CreatedOn });

            return _reportRepository.ExportProductTemplate(result.Value, filePath, isErrorTemplate);
        }

        public async Task<byte[]> ExportPriceTemplate(string filePath, bool isErrorTemplate)
        {
            var result = await _stockKeepingUnitRepository.GetAllAsync(x => x.IsActive && x.HasErrorField == isErrorTemplate, orderBy: x => new { x.CreatedOn });

            return _reportRepository.ExportPriceTemplate(result.Value, filePath, isErrorTemplate);
        }

        public async Task<byte[]> ExportPaymentTemplate(string filePath, bool isErrorTemplate)
        {
            var result = await _paymentRepository.GetAllAsync(x => x.IsActive && x.HasErrorField == isErrorTemplate, orderBy: x => new { x.CreatedOn });

            return _reportRepository.ExportPaymentTemplate(result.Value, filePath, isErrorTemplate);
        }

        public async Task<byte[]> ExportGuestUser(DateTime? start, DateTime? end)
        {
            var result = await _guestUserRepository.GetAllAsync(x => x.IsActive,
                orderBy: x => new { x.LastLogedInOn }, sortbyAsce: false);

            return _reportRepository.ExportToExcel(result.Value.Select(x=> new {
                MobileNumber = _helperMethod.Decrypt<string>(x.Telephone),
                IsVerified = x.IsVerified? "Yes":"No",
                CreatedOn = x.CreatedOn.HasValue ? x.CreatedOn.Value.AddHours(5.5).ToString("dddd, dd MMMM yyyy HH:mm:ss") : string.Empty, 
                LastLogedInOn = x.LastLogedInOn.HasValue ? x.LastLogedInOn.Value.AddHours(5.5).ToString("dddd, dd MMMM yyyy HH:mm:ss") : string.Empty
            }).ToList());
        }
    }
}
