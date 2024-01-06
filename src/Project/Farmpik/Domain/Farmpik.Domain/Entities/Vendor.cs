/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

namespace Farmpik.Domain.Entities
{
    public partial class Vendor : AuditEntity
    {
        public string VendorCode { get; set; }

        public string VendorName { get; set; }

        public string Telephone { get; set; }

        public bool HasErrorField { get; set; }

        public string ErrorField { get; set; }

        public string DeviceToken { get; set; }
    }
}