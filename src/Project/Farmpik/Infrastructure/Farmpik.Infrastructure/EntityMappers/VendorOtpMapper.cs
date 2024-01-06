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
    internal class VendorOtpMapper
    {
        public VendorOtpMapper(EntityTypeConfiguration<VendorOtp> entity)
        {
            entity.ToTable("VendorOtp");
            entity.HasKey(x => x.Id);

            entity.Property(p => p.IsActive)
                .IsRequired();

            entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime");


            entity.Property(e => e.ChangedOn)
                .HasColumnType("datetime");

            entity.Property(x => x.VendorId)
                .IsRequired();

            entity.Property(x => x.Otp)
                .HasMaxLength(6)
                .IsRequired();
        }
    }
}
