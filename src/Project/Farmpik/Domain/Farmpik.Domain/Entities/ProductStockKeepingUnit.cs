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
    public class ProductStockKeepingUnit : AuditEntity
    {
        public string StockKeepingUnit { get; set; }

        public string Location { get; set; }

        public DateTime? ShimlaEffectiveDate { get; set; }

        public DateTime? KinnaurEffectiveDate { get; set; }

        public DateTime? ShimlaExpiryDate { get; set; }

        public DateTime? KinnaurExpiryDate { get; set; }

        public decimal ShimlaPrice { get; set; }

        public decimal KinnaurPrice { get; set; }

        public bool HasErrorField { get; set; }

        public string ErrorField { get; set; }
    }
}