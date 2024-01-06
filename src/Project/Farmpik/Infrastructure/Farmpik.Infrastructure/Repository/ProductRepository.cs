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
    public class ProductRepository : GenericRepository<ProductDetails>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ImportDetails> ProductImportDetails()
        {
            var product = await _context.Products
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync();

            if (product == null) return new ImportDetails { TemplateName = "Product details" };

            return new ImportDetails
            {
                NoOfUploads = await _context.Products.LongCountAsync(x => x.IsActive && x.HasErrorField != true),
                LastImportedDate = product.CreatedOn,
                LastImportedUser = (await _context.Employees.FirstOrDefaultAsync(x => x.Id == product.CreatedBy))?.FirstName,
                TemplateName = "Product details"
            };
        }

        public async Task<bool> ImportBulkProducts(List<ProductDetails> products,
            Guid createdBy)
        {
            return await Task.FromResult(BulkSave("Product", products, createdBy));
        }

        public async Task<Dictionary<string,int>> GetProductCategories()
        {
            return await _context.Products
                .Where(x => x.IsActive && !x.HasErrorField && (x.IsAvailableInRampur || x.IsAvailableInRohru || x.IsAvailableInSainj || x.IsAvailableInOddi))
                .GroupBy(x => x.Category)
                .ToDictionaryAsync(g => g.Key,g => g.Count());
        }
    }
}