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
    public partial class RefreshToken : AuditEntity
    {
        public string RefreshTokens { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public Guid VendorId { get; set; }
    }
}