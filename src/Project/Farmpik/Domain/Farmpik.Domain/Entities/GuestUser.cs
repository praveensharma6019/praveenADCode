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
    public class GuestUser: AuditEntity
    {
        public string VendorName { get; set; } = "Guest";

        public string Telephone { get; set; }

        public string DeviceToken { get; set; }

        public DateTime? LastLogedInOn { get; set; }

        public bool IsVerified { get; set; }
    }
}
