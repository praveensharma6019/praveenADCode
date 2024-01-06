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
    internal class GuestUserMapper
    {
        public GuestUserMapper(EntityTypeConfiguration<GuestUser> entity)
        {
            entity.ToTable("GuestUser");
            entity.HasKey(x => x.Id);

            entity.Property(p => p.IsActive)
                .IsRequired();

            entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime");


            entity.Property(e => e.ChangedOn)
                .HasColumnType("datetime");

            entity.Property(x => x.VendorName)
                .HasMaxLength(60)
                .IsRequired();

            entity.Property(x => x.Telephone)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.DeviceToken)
                .HasMaxLength(200);

            entity.Property(e => e.LastLogedInOn)
                .HasColumnType("datetime");
        }
    }
}
