/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Farmpik.Domain.Interfaces.Repositories
{
    public interface IGuestUserRepository: IGenericRepository<GuestUser>
    {
        Task<Vendor> SaveGuestUser(string mobileNumber);

        Task<Vendor> UpdateGuestUser(Guid id, string deviceToken);
    }
}
