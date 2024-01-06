/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Commands.ImportFileCommands;
using Farmpik.Domain.Common;
using Farmpik.Domain.Dto;
using Farmpik.Domain.Entities;
using Farmpik.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using static Farmpik.Domain.Common.ResponseConstants;
using System.Windows;
using Farmpik.Domain.Commands.ReceiptCommands;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Farmpik.Infrastructure.Repository
{
    public class PurchaseRepository : GenericRepository<PurchaseReport>, IPurchaseRepository
    {
        public readonly AppDbContext _context;
        public PurchaseRepository(AppDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<bool> ImportBulkPurchaseReports(List<PurchaseReport> purchases, Guid createdBy)
        {
            return await Task.FromResult(BulkSave("PurchaseReport", purchases, createdBy));
        }

        public async Task<PaginatedList<PurchaseReport>> GetGoodsReceipts(QueryReceiptsCommand command) 
        {
            if (command.CurrentVendorCode == ResponseMessage.Default_VendorCode)
            {
               return new PaginatedList<PurchaseReport>
                {
                    Value = new List<PurchaseReport> { new PurchaseReport {
                        VendorCode = ResponseMessage.Default_VendorCode,
                        GateNumber = "GNR9876543",
                        PostingDate = System.DateTime.Now,
                        TotalQuantity = 10,
                        TotalAmount = 1000,
                        A1PremiumELQuantity =10,
                        A1PremiumELAmount = 100,
                        A2PremiumLMQuantity = 10,
                        A2PremiumLMAmount = 100,
                        A3PremiumP2Quantity =10,
                        A3PremiumP2Amount = 100,
                        A4PremiumSAmount = 100,
                        A4PremiumSQuantity = 10}
                    },
                    TotalCount = 1
                };
            }

            var query = _context.PurchaseReports.Where(w => w.IsActive && w.HasErrorField != true && w.VendorCode == command.CurrentVendorCode
            && w.GateNumber.Contains(command.Search))
                .GroupBy(g => g.GateNumber);

            var dbpurchases = await query.OrderBy(g => g.Max(m => m.CreatedOn))
                .Skip((command.PageNumber - 1) * command.PageSize)
                .Take(command.PageSize)
                .ToListAsync();

            List<PurchaseReport> purchases = new List<PurchaseReport>();
            dbpurchases.ForEach(x => purchases.Add(new PurchaseReport
            {
                GateNumber = x.Key,
                VendorCode = x.OrderByDescending(o => o.CreatedOn).FirstOrDefault().VendorCode,
                PostingDate = x.OrderByDescending(o=> o.CreatedOn).FirstOrDefault().PostingDate,
                TotalQuantity = x.Sum(s => s.TotalQuantity),
                TotalAmount = x.Sum(s => s.TotalAmount),
                A1PremiumELAmount = x.Sum(s => s.A1PremiumELAmount),
                A1PremiumELQuantity = x.Sum(s => s.A1PremiumELQuantity),
                A2PremiumLMAmount = x.Sum(s => s.A2PremiumLMAmount),
                A2PremiumLMQuantity = x.Sum(s => s.A2PremiumLMQuantity),
                A3PremiumP2Amount = x.Sum(s => s.A3PremiumP2Amount),
                A3PremiumP2Quantity = x.Sum(s => s.A3PremiumP2Quantity),
                A4PremiumSAmount = x.Sum(s => s.A4PremiumSAmount),
                A4PremiumSQuantity = x.Sum(s => s.A4PremiumSQuantity),
                A5PremiumESAmount = x.Sum(s => s.A5PremiumESAmount),
                A5PremiumESQuantity = x.Sum(s => s.A5PremiumESQuantity),
                A6PremiumEESAmount = x.Sum(s => s.A6PremiumEESAmount),
                A6PremiumEESQuantity = x.Sum(s => s.A6PremiumEESQuantity),
                B1SupremeELAmount = x.Sum(s => s.B1SupremeELAmount),
                B1SupremeELQuantity = x.Sum(s => s.B1SupremeELQuantity),
                B2SupremeLMAmount = x.Sum(s => s.B2SupremeLMAmount),
                B2SupremeLMQuantity = x.Sum(s => s.B2SupremeLMQuantity),
                B3SupremeP2Amount = x.Sum(s => s.B3SupremeP2Amount),
                B3SupremeP2Quantity = x.Sum(s => s.B3SupremeP2Quantity),
                B4SupremeSAmount = x.Sum(s => s.B4SupremeSAmount),
                B4SupremeSQuantity = x.Sum(s => s.B4SupremeSQuantity),
                B5SupremeESAmount = x.Sum(s => s.B5SupremeESAmount),
                B5SupremeESQuantity = x.Sum(s => s.B5SupremeESQuantity),
                B6SupremeEESAmount = x.Sum(s => s.B6SupremeEESAmount),
                B6SupremeEESQuantity = x.Sum(s => s.B6SupremeEESQuantity),
                C1UnderSizeAmount = x.Sum(s => s.C1UnderSizeAmount),
                C1UnderSizeQuantity = x.Sum(s => s.C1UnderSizeQuantity),
                C2SuperEESP2Amount = x.Sum(s => s.C2SuperEESP2Amount),
                C2SuperEESP2Quantity = x.Sum(s => s.C2SuperEESP2Quantity),
                D1PssEELAmount = x.Sum(s => s.D1PssEELAmount),
                D1PssEELQuantity = x.Sum(s => s.D1PssEELQuantity),
                D2SuperEL2ESAmount = x.Sum(s => s.D2SuperEL2ESAmount),
                D2SuperEL2ESQuantity = x.Sum(s => s.D2SuperEL2ESQuantity),
                W1PssROLAmount = x.Sum(s => s.W1PssROLAmount),
                W1PssROLQuantity = x.Sum(s => s.W1PssROLQuantity),
                ROAAmount = x.Sum(s=> s.ROAAmount),
                ROAQuantity = x.Sum(s => s.ROAQuantity),
            }));

            return new PaginatedList<PurchaseReport>
            {
                TotalCount = await query.CountAsync(),
                Value = purchases
            };
        }
        
        public async Task<ImportDetails> PurchaseImportDetails()
        {
            var purchase = await _context.PurchaseReports
                 .OrderByDescending(x => x.CreatedOn)
                 .FirstOrDefaultAsync();
            if (purchase == null) return new ImportDetails { TemplateName = "PRN Master" };

            return new ImportDetails
            {
                NoOfUploads = await _context.PurchaseReports.LongCountAsync(x => x.IsActive && x.HasErrorField != true),
                LastImportedDate = purchase.CreatedOn,
                LastImportedUser = (await _context.Employees.FirstOrDefaultAsync(x => x.Id == purchase.CreatedBy))?.FirstName,
                TemplateName ="PRN Master"
            };
        }

        public async Task<decimal> GetTotalEarning(string vendorCode)
        {
            return (await _context.PurchaseReports
                .Where(x => x.IsActive && !x.HasErrorField && x.VendorCode == vendorCode)
                .SumAsync(x => (decimal?)x.TotalAmount))?? 0;
        }
    }
}
