/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */


using Farmpik.Domain.Commands.ImportFileCommands;
using Farmpik.Domain.Commands.ReceiptCommands;
using Farmpik.Domain.Common;
using Farmpik.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Farmpik.Domain.Interfaces.Repositories
{
    public interface IPurchaseRepository:IGenericRepository<PurchaseReport>
    {
        Task<ImportDetails> PurchaseImportDetails();

        Task<PaginatedList<PurchaseReport>> GetGoodsReceipts(QueryReceiptsCommand command);

        Task<bool> ImportBulkPurchaseReports(List<PurchaseReport> purchases, System.Guid createdBy);

        Task<decimal> GetTotalEarning(string vendorCode);
    }
}
