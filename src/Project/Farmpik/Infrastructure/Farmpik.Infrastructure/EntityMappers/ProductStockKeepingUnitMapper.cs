/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Farmpik.Infrastructure.EntityMappers
{
    internal class ProductStockKeepingUnitMapper
    {
        public ProductStockKeepingUnitMapper(EntityTypeConfiguration<ProductStockKeepingUnit> entity)
        {
            entity.ToTable("ProductStockKeepingUnit");
            entity.HasKey(x => x.Id);

            entity.Property(p => p.IsActive)
                .IsRequired();

            entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime");


            entity.Property(e => e.ChangedOn)
                .HasColumnType("datetime");

            entity.Property(e => e.ShimlaEffectiveDate)
                .HasColumnType("datetime");

            entity.Property(e => e.KinnaurEffectiveDate)
                .HasColumnType("datetime");

            entity.Property(e => e.ShimlaExpiryDate)
                .HasColumnType("datetime");

            entity.Property(e => e.KinnaurExpiryDate)
                .HasColumnType("datetime");

            entity.Property(x => x.Location)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.StockKeepingUnit)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
