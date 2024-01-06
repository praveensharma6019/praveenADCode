/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using AutoMapper;
using Farmpik.Domain.Dto;
using Farmpik.Domain.Entities;

namespace Farmpik.Services
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<ProductDetails, ProductDetailsDto>().ReverseMap();
            CreateMap<ProductDetails, SimilarProductDto>().ReverseMap();
            CreateMap<ProductStockKeepingUnit, ProductStockKeepingUnitDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Vendor, VendorDto>().ReverseMap();
            CreateMap<PaymentReport, PaymentDto>().ReverseMap();
            CreateMap<PurchaseReport, PurchaseDto>().ReverseMap();
        }
    }
}