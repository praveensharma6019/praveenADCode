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
    public class ProductStockKeepingUnitRepository : GenericRepository<ProductStockKeepingUnit>, IProductStockKeepingUnitRepository
    {
        public readonly AppDbContext _context;

        public ProductStockKeepingUnitRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ImportBulkPrices(List<ProductStockKeepingUnit> productStockKeepingUnits,
            Guid createdBy)
        {
            var haserror = productStockKeepingUnits.FirstOrDefault(x => x.HasErrorField == true) != null;
            if(haserror) productStockKeepingUnits.ForEach(x=> x.HasErrorField = true);

            return await Task.FromResult(BulkSave("ProductStockKeepingUnit", productStockKeepingUnits, createdBy, haserror ? "HasErrorField= 1;":string.Empty));
        }

        public async Task<ImportDetails> PriceImportDetails()
        {
            var productStocks = await _context.ProductStockKeepingUnits
                .Where(x => x.IsActive && x.HasErrorField == false)
                .ToListAsync();

            if (productStocks == null || productStocks.Count == 0) return new ImportDetails { TemplateName = "Price Master" };
            var lastStock = productStocks.LastOrDefault();
            int totalRecord = productStocks.Count(x => x.Location == "Rohru" || x.Location == "Sainj")
                + 2 * productStocks.Count(x => x.Location == "Rampur" || x.Location == "Oddi");
            return new ImportDetails
            {
                NoOfUploads = totalRecord,
                LastImportedDate = lastStock.CreatedOn,
                LastImportedUser = (await _context.Employees.FirstOrDefaultAsync(x => x.Id == lastStock.CreatedBy))?.FirstName,
                TemplateName = "Price Master"
            };
        }
    }
}