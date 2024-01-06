/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using AutoMapper;
using Farmpik.Domain.Commands.ProductCommands;
using Farmpik.Domain.Common;
using Farmpik.Domain.Common.Enum;
using Farmpik.Domain.Dto;
using Farmpik.Domain.Entities;
using Farmpik.Domain.Interfaces.Repositories;
using Farmpik.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmpik.Services.MobileServices
{
    public class ProductBusinessService : IProductBusinessService
    {
        private readonly IMapper _mapper;
        private readonly IProductStockKeepingUnitRepository _stockKeepingUnitRepository;
        private readonly IProductRepository _productRepository;

        public ProductBusinessService(IMapper mapper,
            IProductStockKeepingUnitRepository stockKeepingUnitRepository,
            IProductRepository productRepository)
        {
            _mapper = mapper;
            _stockKeepingUnitRepository = stockKeepingUnitRepository;
            _productRepository = productRepository;
        }

        public async Task<PaginationApiResponse<List<ProductDetailsDto>>> GetProducts(QueryProductsCommand command)
        {
            var result = await _productRepository.GetAllAsync(x => x.IsActive && x.HasErrorField != true && (x.IsAvailableInRampur || x.IsAvailableInRohru || x.IsAvailableInSainj || x.IsAvailableInOddi)
            && !string.IsNullOrEmpty(x.Category) && (int)command.Category != 0 && x.Category == command.Category.ToString(),
            pageNumber: command.PageNumber, pageSize: command.PageSize, orderBy: x=> new { x.Price }, sortbyAsce: false);

            return ResponseHelper.GetPaginationResponse(
                _mapper.Map<List<ProductDetailsDto>>(result.Value),
                result.TotalCount > 0,
                totalRecord: result.TotalCount);
        }

        public async Task<ApiResponse<SimilarProductDto>> GetProduct(Guid id)
        {
            var result = await _productRepository.GetAsync(id);
            var product = _mapper.Map<SimilarProductDto>(result);
            if (product != null)
            {
                var similarProducts = await _productRepository.GetAllAsync(x => x.IsActive && (x.IsAvailableInRampur || x.IsAvailableInRohru || x.IsAvailableInSainj || x.IsAvailableInOddi)
                && x.Id != product.Id && x.Category == product.Category,pageNumber:1, pageSize:5, orderBy: x => new { x.Price },
                sortbyAsce:false);
                if (similarProducts.TotalCount >= 3)
                {
                    product.SimilarProducts = _mapper.Map<List<ProductDetailsDto>>(similarProducts.Value);
                }
            }
            return ResponseHelper.GetResponse(product, result != null);
        }

        public async Task<PaginationApiResponse<List<ProductCategoryDto>>> GetProductCategories()
        {
            var dbcategories = await _productRepository.GetProductCategories();
            var categories = new List<CategoryType> { CategoryType.Fertilizers, CategoryType.Gloves, CategoryType.Tools,CategoryType.Seeds,CategoryType.Crate,CategoryType.Masks,CategoryType.Spray,CategoryType.Others};
            var result = new  List<ProductCategoryDto>();
            categories.ForEach(x => {
                var category = dbcategories.FirstOrDefault(y => y.Key == x.ToString());
                result.Add(new ProductCategoryDto
                {
                    CategoryId = (int)x,
                    CategoryName = x.ToString(),
                    ProductCount = category.Value
                }); });

            return ResponseHelper.GetPaginationResponse(result, true,
                totalRecord: categories.Count);
        }

        public async Task<PaginationApiResponse<List<ProductStockKeepingUnitDto>>> GetProductPrices(QueryProductPricesCommand command)
        {
            var result = await _stockKeepingUnitRepository.GetAllAsync(x => x.IsActive && x.Location == command.Location,
            pageNumber: command.PageNumber, pageSize: command.PageSize, orderBy: x => new { x.CreatedOn });

            return ResponseHelper.GetPaginationResponse(
                _mapper.Map<List<ProductStockKeepingUnitDto>>(result.Value),
                result.TotalCount > 0,
                totalRecord: result.TotalCount);
        }

        public async Task<ApiResponse<ProductPriceDto>> GetProductPrices(LocationType locationId)
        {
            var result = await _stockKeepingUnitRepository.GetAllAsync(x => x.IsActive && (locationId != 0 && x.Location == locationId.ToString()), orderBy: x => new { x.CreatedOn });
            var first = result.Value.FirstOrDefault();
            ProductPriceDto priceDto = new ProductPriceDto
            {
                LocationName = first != null ? first.Location:string.Empty,
                Shimla = GetPriceDetails(true,first, result.Value)
            };

            if(locationId == LocationType.Rampur|| locationId == LocationType.Oddi)
            {
                priceDto.Kinnaur = GetPriceDetails(false, first, result.Value);
            }
            return ResponseHelper.GetResponse(priceDto, result.Value != null);
        } 

        private static PriceVeiwModel GetPriceDetails(bool isShimla, ProductStockKeepingUnit first, List<ProductStockKeepingUnit> skus)
        {
            bool hasInvalidDate = isShimla ? (first.ErrorField.Contains("ShimlaEffectiveDate") || first.ErrorField.Contains("ShimlaExpiryDate"))
                : (first.ErrorField.Contains("KinnaurEffectiveDate") || first.ErrorField.Contains("KinnaurExpiryDate"));

            if (first == null || hasInvalidDate) { return new PriceVeiwModel { }; };

            var query = isShimla? skus.Where(x => !x.ErrorField.Contains("Invalid SKU Name") && !x.ErrorField.Contains("Invalid Shimla Price"))
                : skus.Where(x => !x.ErrorField.Contains("Invalid SKU Name") && !x.ErrorField.Contains("Invalid Kinnaur Price"));
            return new PriceVeiwModel
            {
                EffectiveDate = isShimla? first.ShimlaEffectiveDate.Value.ToString("d MMM yyyy"): first.KinnaurEffectiveDate.Value.ToString("d MMM yyyy"),
                ExpiryDate = isShimla ? first.ShimlaExpiryDate.Value.ToString("d MMM yyyy") : first.KinnaurExpiryDate.Value.ToString("d MMM yyyy"),
                Prices = query.Select(x => new PriceDto { Id = x.Id, Price = isShimla ? x.ShimlaPrice : x.KinnaurPrice, Name = (isShimla ? "SHI " : "KIN ") + x.StockKeepingUnit }).ToList(),
            };
        }
    }
}