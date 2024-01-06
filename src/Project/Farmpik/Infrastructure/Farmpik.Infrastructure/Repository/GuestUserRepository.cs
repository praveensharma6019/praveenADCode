/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Entities;
using Farmpik.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace Farmpik.Infrastructure.Repository
{
    public class GuestUserRepository : GenericRepository<GuestUser>, IGuestUserRepository
    {
        private readonly AppDbContext _context;
        public GuestUserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Vendor> SaveGuestUser(string mobileNumber)
        {
            var user = await GetAsync(x=> x.Telephone == mobileNumber && x.IsActive);
            if (user == null)
            {
                user = await AddAsync(new GuestUser
                {
                    Telephone = mobileNumber,
                    CreatedBy = System.Guid.Empty,
                    IsVerified = false
                });
            }
            return new Vendor
            {
                Id = user.Id,
                Telephone = user.Telephone,
                IsActive = user.IsActive,
                VendorName = user.VendorName
            };
        }

        public async Task<Vendor> UpdateGuestUser(Guid id, string deviceToken)
        {
            var user = await GetAsync(x => x.Id == id && x.IsActive);
            if (user == null) return null;

            user.IsVerified = true;
            user.DeviceToken = deviceToken;
            user.LastLogedInOn = DateTime.UtcNow;
            await UpdateAsync(user);
            return new Vendor
            {
                Id = user.Id,
                Telephone = user.Telephone,
                IsActive = user.IsActive,
                VendorName = user.VendorName,
                VendorCode = user.Id.ToString()
            };
        }
    }
}
