/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Dto;
using System;
using System.Collections.Generic;

namespace Sitecore.Farmpik.Website.ViewModel
{
    public class PriceViewModel
    {
        public DateTime? ShimlaEffectiveDate { get; set; }

        public DateTime? KinnaurEffectiveDate { get; set; }

        public List<ProductStockKeepingUnitDto> Shimla { get; set; }

        public List<ProductStockKeepingUnitDto> Kinnaur { get; set; }
    }
}