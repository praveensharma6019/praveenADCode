/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */
using System.Collections.Generic;

namespace Farmpik.Domain.Dto
{
    public class SimilarProductDto: ProductDetailsDto
    {
        public List<ProductDetailsDto> SimilarProducts { get; set; }
    }
}
