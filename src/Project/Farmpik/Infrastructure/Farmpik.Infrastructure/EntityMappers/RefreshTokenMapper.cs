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
    internal class RefreshTokenMapper
    {
        public RefreshTokenMapper(EntityTypeConfiguration<RefreshToken> entity)
        {
            entity.ToTable("RefreshToken");
            entity.HasKey(x => x.Id);

            entity.Property(p => p.IsActive)
                .IsRequired();

            entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime");


            entity.Property(e => e.ChangedOn)
                .HasColumnType("datetime");

            entity.Property(e => e.RefreshTokenExpiryTime)
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(x => x.RefreshTokens)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
