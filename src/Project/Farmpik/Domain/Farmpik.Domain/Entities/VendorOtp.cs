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
    public class VendorOtp : AuditEntity
    {
        public Guid VendorId { get; set; }

        public int OtpType { get; set; }

        public int Attempted { get; set; }

        public int SendCount { get; set; }

        public string Otp { get; set; }
    }
}