/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using System;

namespace Farmpik.Domain.Dto
{
    public class ProductStockKeepingUnitDto
    {
        public Guid Id { get; set; }

        public string StockKeepingUnit { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string Location { get; set; }

        public decimal Price { get; set; }
    }
}