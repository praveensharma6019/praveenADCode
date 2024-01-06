/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Commands.ImportFileCommands;
using Farmpik.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Farmpik.Domain.Interfaces.Repositories
{
    public interface IPaymentRepository : IGenericRepository<PaymentReport>
    {
        Task<ImportDetails> PaymentImportDetails();

        Task<bool> ImportBulkPaymentReports(List<PaymentReport> payments, System.Guid createdBy);

        Task<decimal> GetNetDue(string vendorCode);
    }
}