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
    internal class RoleTypeMapper
    {
        public RoleTypeMapper(EntityTypeConfiguration<RoleType> entity)
        {
            entity.ToTable("RoleType");
            entity.HasKey(x => x.Id);

            entity.Property(p => p.IsActive)
                .IsRequired();

            entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime");

            entity.Property(e => e.ChangedOn)
                .HasColumnType("datetime");

            entity.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
