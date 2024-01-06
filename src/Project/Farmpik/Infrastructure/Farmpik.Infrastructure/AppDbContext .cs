/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Entities;
using Farmpik.Infrastructure.EntityMappers;
using System.Data.Entity;

namespace Farmpik.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=FarmpikContext")
        {
        }

        public virtual DbSet<Vendor> Vendors { get; set; }

        public virtual DbSet<GuestUser> GuestUsers { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<ProductDetails> Products { get; set; }

        public virtual DbSet<ProductStockKeepingUnit> ProductStockKeepingUnits { get; set; }

        public virtual DbSet<PaymentReport> PaymentReports { get; set; }

        public virtual DbSet<PurchaseReport> PurchaseReports { get; set; }

        public virtual DbSet<RoleType> Roles { get; set; }

        public virtual DbSet<VendorOtp> VendorOtps { get; set; }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            new EmployeeMapper(modelBuilder.Entity<Employee>());
            new PaymentReportMapper(modelBuilder.Entity<PaymentReport>());
            new ProductDetailMapper(modelBuilder.Entity<ProductDetails>());
            new ProductStockKeepingUnitMapper(modelBuilder.Entity<ProductStockKeepingUnit>());
            new RefreshTokenMapper(modelBuilder.Entity<RefreshToken>());
            new RoleTypeMapper(modelBuilder.Entity<RoleType>());
            new GuestUserMapper(modelBuilder.Entity<GuestUser>());
            new VendorMapper(modelBuilder.Entity<Vendor>());
            new VendorOtpMapper(modelBuilder.Entity<VendorOtp>());
            new PurchaseReportMapper(modelBuilder.Entity<PurchaseReport>());
        }
    }
}