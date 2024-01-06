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
    public interface IProductStockKeepingUnitRepository : IGenericRepository<ProductStockKeepingUnit>
    {
        Task<bool> ImportBulkPrices(List<ProductStockKeepingUnit> productStockKeepingUnits,
           System.Guid createdBy);

        Task<ImportDetails> PriceImportDetails();
    }
}