/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Commands.ImportFileCommands;
using Farmpik.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Farmpik.Domain.Interfaces.Repositories
{
    public interface IProductRepository : IGenericRepository<ProductDetails>
    {
        Task<ImportDetails> ProductImportDetails();

        Task<bool> ImportBulkProducts(List<ProductDetails> products,
            Guid createdBy);

        Task<Dictionary<string, int>> GetProductCategories();
    }
}