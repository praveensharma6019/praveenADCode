/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Commands.ReceiptCommands;
using Farmpik.Domain.Common;
using Farmpik.Domain.Dto;
using Farmpik.Domain.Entities;
using Farmpik.Domain.Interfaces.Repositories;
using Farmpik.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmpik.Services.MobileServices
{
    public class GoodsReceiptBusinessService : IGoodsReceiptBusinessService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPurchaseRepository _purchaseRepository;

        public GoodsReceiptBusinessService(
            IVendorRepository vendorRepository,
            IPaymentRepository paymentRepository,
            IPurchaseRepository purchaseRepository)
        {
            _vendorRepository = vendorRepository;
            _paymentRepository = paymentRepository;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<ApiResponse<GoodsReceiptDto>> GetGoodsReceipts(QueryReceiptsCommand command)
        {
            var vendor = await _vendorRepository.GetAsync(x=> x.IsActive && x.VendorCode == command.CurrentVendorCode);
            if(vendor == null) { return ResponseHelper.GetResponse<GoodsReceiptDto>(default, false); }

            var receipts = await _purchaseRepository.GetGoodsReceipts(command);

            return new ApiResponse<GoodsReceiptDto>().SetResponse(new GoodsReceiptDto
            {
                TotalCount = receipts.TotalCount,
                Receipts = receipts.Value.Select(x => GetReceipDetails(x)).ToList(),
                NetDue = await _paymentRepository.GetNetDue(vendor.VendorCode),
                TotalEarning = await _purchaseRepository.GetTotalEarning(vendor.VendorCode)
            });
        }

        private static ReceipDetails GetReceipDetails(PurchaseReport purchase)
        {
            var skus = new List<SKU>();
            if (purchase.A1PremiumELAmount != 0) skus.Add(new SKU { Name = "A1-PREMIUM EL", Quantity = purchase.A1PremiumELQuantity, Amount = purchase.A1PremiumELAmount });
            if (purchase.A2PremiumLMAmount != 0) skus.Add(new SKU { Name = "A2-PREMIUM L+M", Quantity = purchase.A2PremiumLMQuantity, Amount = purchase.A2PremiumLMAmount });
            if (purchase.A3PremiumP2Amount != 0) skus.Add(new SKU { Name = "A3-PREMIUM P2", Quantity = purchase.A3PremiumP2Quantity, Amount = purchase.A3PremiumP2Amount });
            if (purchase.A4PremiumSAmount != 0) skus.Add(new SKU { Name = "A4-PREMIUM S", Quantity = purchase.A4PremiumSQuantity, Amount = purchase.A4PremiumSAmount });
            if (purchase.A5PremiumESAmount != 0) skus.Add(new SKU { Name = "A5-PREMIUM ES", Quantity = purchase.A5PremiumESQuantity, Amount = purchase.A5PremiumESAmount });
            if (purchase.A6PremiumEESAmount != 0) skus.Add(new SKU { Name = "A6-PREMIUM EES", Quantity = purchase.A6PremiumEESQuantity, Amount = purchase.A6PremiumEESAmount });
            if (purchase.B1SupremeELAmount != 0) skus.Add(new SKU { Name = "B1-SUPREME EL", Quantity = purchase.B1SupremeELQuantity, Amount = purchase.B1SupremeELAmount });
            if (purchase.B2SupremeLMAmount != 0) skus.Add(new SKU { Name = "B2-SUPREME L+M", Quantity = purchase.B2SupremeLMQuantity, Amount = purchase.B2SupremeLMAmount });
            if (purchase.B3SupremeP2Amount != 0) skus.Add(new SKU { Name = "B3-SUPREME P2", Quantity = purchase.B3SupremeP2Quantity, Amount = purchase.B3SupremeP2Amount });
            if (purchase.B4SupremeSAmount != 0) skus.Add(new SKU { Name = "B4-SUPREME S", Quantity = purchase.B4SupremeSQuantity, Amount = purchase.B4SupremeSAmount });
            if (purchase.B5SupremeESAmount != 0) skus.Add(new SKU { Name = "B5-SUPREME ES", Quantity = purchase.B5SupremeESQuantity, Amount = purchase.B5SupremeESAmount });
            if (purchase.B6SupremeEESAmount != 0) skus.Add(new SKU { Name = "B6-SUPREME EES", Quantity = purchase.B6SupremeEESQuantity, Amount = purchase.B6SupremeEESAmount });
            if (purchase.C1UnderSizeAmount != 0) skus.Add(new SKU { Name = "C1-Under Size", Quantity = purchase.C1UnderSizeQuantity, Amount = purchase.C1UnderSizeAmount });
            if (purchase.C2SuperEESP2Amount != 0) skus.Add(new SKU { Name = "C2-SUPER EES+P2", Quantity = purchase.C2SuperEESP2Quantity, Amount = purchase.C2SuperEESP2Amount });
            if (purchase.D1PssEELAmount != 0) skus.Add(new SKU { Name = "D1-PSS EEL", Quantity = purchase.D1PssEELQuantity, Amount = purchase.D1PssEELAmount });
            if (purchase.D2SuperEL2ESAmount != 0) skus.Add(new SKU { Name = "D2-SUPER EL 2 ES", Quantity = purchase.D2SuperEL2ESQuantity, Amount = purchase.D2SuperEL2ESAmount });
            if (purchase.W1PssROLAmount != 0) skus.Add(new SKU { Name = "W1-PSS ROL", Quantity = purchase.W1PssROLQuantity, Amount = purchase.W1PssROLAmount });
            if (purchase.ROAAmount != 0) skus.Add(new SKU { Name = "ROA", Quantity = purchase.ROAQuantity, Amount = purchase.ROAAmount });

            return new ReceipDetails
            {
                VendorCode = purchase.VendorCode,
                GateNumber = purchase.GateNumber,
                PostingDate = purchase.PostingDate,
                TotalQuantity = purchase.TotalQuantity,
                TotalAmount = purchase.TotalAmount,
                SKUs = skus
            };
        }
    }
}
