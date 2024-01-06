/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using System;

namespace Farmpik.Domain.Entities
{
    public class AuditEntity : BaseEntity
    {
        public DateTime? CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public Guid? ChangedBy { get; set; }
    }
}