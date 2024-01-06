/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Commands.ProductCommands;
using Farmpik.Domain.Common;
using Farmpik.Domain.Dto;
using Farmpik.Domain.Interfaces.Services;
using log4net;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Farmpik.Domain.Common.Enum;

namespace Sitecore.Farmpik.Api.Website.Controllers
{
    [RoutePrefix("v1/Product")]
    public class ProductController : BaseApiController
    {
        private readonly IProductBusinessService _productBusinessService;
        private readonly ILog _logger;

        public ProductController(IProductBusinessService productBusinessService)
        {
            _logger = LogManager.GetLogger(typeof(ProductController));
            _productBusinessService = productBusinessService;
        }

        [HttpGet]
        [Route("")]
        public async Task<PaginationApiResponse<List<ProductDetailsDto>>> Products([FromUri] CategoryType categoryId, [FromUri] int pageNumber = 1, [FromUri] int pageSize = 10)
        {
            try
            {
                 return await _productBusinessService.GetProducts(new QueryProductsCommand { Category = categoryId, PageNumber = pageNumber, PageSize = pageSize });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return GetPaginationServerError<ProductDetailsDto>();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ApiResponse<SimilarProductDto>> Product(Guid id)
        {
            try
            {
                return await _productBusinessService.GetProduct(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return GetServerError<SimilarProductDto>();
            }
        }

        [HttpGet]
        [Route("Categories")]
        public async Task<PaginationApiResponse<List<ProductCategoryDto>>> Categories()
        {
            try
            {
                return await _productBusinessService.GetProductCategories();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return GetPaginationServerError<ProductCategoryDto>();
            }
        }

        [HttpGet]
        [Route("Prices/{locationId}")]
        public async Task<ApiResponse<ProductPriceDto>> Prices(LocationType locationId)
        {
            try
            {
                return await _productBusinessService.GetProductPrices(locationId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return GetServerError<ProductPriceDto>();
            }
            
        }

  
    }
}