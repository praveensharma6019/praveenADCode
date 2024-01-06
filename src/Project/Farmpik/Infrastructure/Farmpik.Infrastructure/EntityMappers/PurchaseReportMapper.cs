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
    internal class PurchaseReportMapper
    {
        public PurchaseReportMapper(EntityTypeConfiguration<PurchaseReport> entity)
        {
            entity.ToTable("PurchaseReport");
            entity.HasKey(x => x.Id);

            entity.Property(p => p.IsActive)
                .IsRequired();

            entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime");


            entity.Property(e => e.ChangedOn)
                .HasColumnType("datetime");

            entity.Property(x => x.VendorName)
                .HasMaxLength(50);

            entity.Property(x => x.PlantName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.MatOrderNumber)
                .HasMaxLength(10);

            entity.Property(x => x.GateNumber)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.PostingDate)
                    .HasColumnType("datetime");
        }
    }
}
