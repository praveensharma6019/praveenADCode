/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Commands.ProductCommands;
using Farmpik.Domain.Common;
using Farmpik.Domain.Common.Enum;
using Farmpik.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Farmpik.Domain.Interfaces.Services
{
    public interface IProductBusinessService
    {
        Task<PaginationApiResponse<List<ProductDetailsDto>>> GetProducts(QueryProductsCommand command);

        Task<ApiResponse<SimilarProductDto>> GetProduct(Guid id);

        Task<PaginationApiResponse<List<ProductCategoryDto>>> GetProductCategories();
        Task<PaginationApiResponse<List<ProductStockKeepingUnitDto>>> GetProductPrices(QueryProductPricesCommand command);
        Task<ApiResponse<ProductPriceDto>> GetProductPrices(LocationType locationId);
    }
}