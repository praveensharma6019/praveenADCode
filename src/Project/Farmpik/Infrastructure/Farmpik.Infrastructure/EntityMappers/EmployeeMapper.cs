/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Entities;
using System;
using System.Data.Entity.ModelConfiguration;

namespace Farmpik.Infrastructure.EntityMappers
{
    internal class EmployeeMapper
    {
        public EmployeeMapper(EntityTypeConfiguration<Employee> entity)
        {
            entity.ToTable("Employee");
            entity.HasKey(x => x.Id);

            entity.Property(p => p.IsActive)
                .IsRequired();

            entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime");


            entity.Property(e => e.ChangedOn)
                .HasColumnType("datetime");

            entity.Property(x=> x.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.LastName)
                .HasMaxLength(50);

            entity.Property(x => x.EmailId)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Password)
                .HasMaxLength(100);

            entity.Property(e => e.AttemptedOn)
                .HasColumnType("datetime");
        }
    }
}
