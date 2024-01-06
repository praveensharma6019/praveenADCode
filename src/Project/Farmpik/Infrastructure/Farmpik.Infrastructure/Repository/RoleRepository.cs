/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Entities;
using Farmpik.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Farmpik.Infrastructure.Repository
{
    [ExcludeFromCodeCoverage]
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<RoleType>> GetAllAsync(bool onlyActive = true)
        //{
        //    return await _context.Roles.Where(x => !onlyActive || x.IsActive).ToListAsync();
        //}
    }
}