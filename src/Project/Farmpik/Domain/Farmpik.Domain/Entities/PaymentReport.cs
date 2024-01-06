/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

namespace Farmpik.Domain.Entities
{
    public class PaymentReport : AuditEntity
    {
        public string VendorCode { get; set; }

        public decimal Amount { get; set; }

        public bool HasErrorField { get; set; }

        public string ErrorField { get; set; }
    }
}