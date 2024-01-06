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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Farmpik.Domain.Interfaces.Repositories
{
    public interface IVendorRepository : IGenericRepository<Vendor>
    {
        Task<ImportDetails> VendorImportDetails();

        Task<bool> ImportBulkVendors(List<Vendor> vendors,
            Guid createdBy);

        Task<VendorOtp> GetVendorOtp(Expression<Func<VendorOtp, bool>> filter);

        Task<VendorOtp> AddVendorOtpAsync(VendorOtp entity);

        Task<bool> UpdateVendorOtpAsync(VendorOtp vendorOtp);

        Task<RefreshToken> GetRefreshToken(Expression<Func<RefreshToken, bool>> filter);

        Task<RefreshToken> AddRefreshTokenAsync(RefreshToken entity);

        Task<bool> UpdateRefreshTokenAsync(RefreshToken token);
    }
}