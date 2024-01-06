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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Farmpik.Infrastructure.Repository
{
    public class VendorRepository : GenericRepository<Vendor>, IVendorRepository
    {
        private readonly DbSet<VendorOtp> _otpEntities;
        private readonly DbSet<RefreshToken> _refreshEntities;
        private readonly AppDbContext _context;

        public VendorRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _otpEntities = context.Set<VendorOtp>();
            _refreshEntities = context.Set<RefreshToken>();
        }

        public async Task<ImportDetails> VendorImportDetails()
        {
            var vendor = await _context.Vendors
                .Where(x => x.IsActive && !x.HasErrorField)
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync();
            if (vendor == null) return new ImportDetails { TemplateName = "Vendor Master" };

            return new ImportDetails
            {
                NoOfUploads = await _context.Vendors.LongCountAsync(x => x.IsActive && x.HasErrorField != true),
                LastImportedDate = vendor.CreatedOn,
                LastImportedUser = (await _context.Employees.FirstOrDefaultAsync(x => x.Id == vendor.CreatedBy))?.FirstName,
                TemplateName = "Vendor Master"
            };
        }

        public async Task<bool> ImportBulkVendors(List<Vendor> vendors,
            Guid createdBy)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Inside ImportBulkVendors Method", this);
                var vendorCodes = vendors.Where(x => !x.HasErrorField).Select(x => x.VendorCode).ToList();
                if (vendorCodes.Count == 0) vendorCodes.Add("0");
                return await Task.FromResult(BulkSave("Vendor", vendors, createdBy, $"IsActive = 1 AND(HasErrorField = 1 OR VendorCode IN({string.Join(",", vendorCodes)})) "));
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Info("Exception in ImportBulkVendors method, exception:" + ex, this);
                return false;
            }
        }

        public async Task<VendorOtp> GetVendorOtp(Expression<Func<VendorOtp, bool>> filter)
        {
            return await _otpEntities.FirstOrDefaultAsync(filter);
        }

        public async Task<VendorOtp> AddVendorOtpAsync(VendorOtp entity)
        {
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;
            entity.CreatedBy = Guid.Empty;
            _otpEntities.Add(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? entity : null;
        }

        public async Task<bool> UpdateVendorOtpAsync(VendorOtp vendorOtp)
        {
            vendorOtp.SendCount++;
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<RefreshToken> GetRefreshToken(Expression<Func<RefreshToken, bool>> filter)
        {
            return await _refreshEntities.FirstOrDefaultAsync(filter);
        }

        public async Task<RefreshToken> AddRefreshTokenAsync(RefreshToken entity)
        {
            entity.CreatedOn = DateTime.UtcNow;
            entity.IsActive = true;
            entity.CreatedBy = Guid.Empty;
            _refreshEntities.Add(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? entity : null;
        }

        public async Task<bool> UpdateRefreshTokenAsync(RefreshToken token)
        {
            token.ChangedOn = DateTime.UtcNow;
            token.CreatedBy = Guid.Empty;
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}