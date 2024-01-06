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
   internal class PaymentReportMapper
    {
        public PaymentReportMapper(EntityTypeConfiguration<PaymentReport> entity)
        {
            entity.ToTable("PaymentReport");
            entity.HasKey(x => x.Id);

            entity.Property(p => p.IsActive)
                .IsRequired();

            entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime");


            entity.Property(e => e.ChangedOn)
                .HasColumnType("datetime");

            entity.Property(x => x.VendorCode)
                .HasMaxLength(60)
                .IsRequired();
        }
    }
}
