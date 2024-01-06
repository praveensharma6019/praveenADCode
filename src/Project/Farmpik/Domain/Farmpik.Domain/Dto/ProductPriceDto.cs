/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using System;
using System.Collections.Generic;

namespace Farmpik.Domain.Dto
{
    public class ProductPriceDto
    {
        public string LocationName { get; set; }

        public PriceVeiwModel Shimla { get; set; }

        public PriceVeiwModel Kinnaur { get; set; }
    }

    public class PriceVeiwModel
    {
        public string EffectiveDate { get; set; }
        public string ExpiryDate { get; set; }

        public List<PriceDto> Prices { get; set; }
    }

    public class PriceDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal? Price { get; set; }
    }
}
