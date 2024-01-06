/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Commands.ImportFileCommands;
using Farmpik.Domain.Entities;
using Farmpik.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Farmpik.Infrastructure.Repository
{
    public class PaymentRepository : GenericRepository<PaymentReport>, IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ImportDetails> PaymentImportDetails()
        {
            var payment = await _context.PaymentReports
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync();
            if (payment == null) return new ImportDetails { TemplateName = "Payment status" };

            return new ImportDetails
            {
                NoOfUploads = await _context.PaymentReports.LongCountAsync(x => x.IsActive && x.HasErrorField != true),
                LastImportedDate = payment.CreatedOn,
                LastImportedUser = (await _context.Employees.FirstOrDefaultAsync(x => x.Id == payment.CreatedBy))?.FirstName,
                TemplateName = "Payment status"
            };
        }

        public async Task<bool> ImportBulkPaymentReports(List<PaymentReport> payments, Guid createdBy)
        {
            return await Task.FromResult(BulkSave("PaymentReport", payments, createdBy));
        }

        public async Task<decimal> GetNetDue(string vendorCode)
        {
            var amount =  await _context.PaymentReports
                .Where(x => x.IsActive && !x.HasErrorField && x.VendorCode == vendorCode)
                .SumAsync(x => (decimal?)x.Amount);
            return  amount.HasValue && amount < 0 ?Math.Abs(amount.Value) : 0;
        }
    }
}