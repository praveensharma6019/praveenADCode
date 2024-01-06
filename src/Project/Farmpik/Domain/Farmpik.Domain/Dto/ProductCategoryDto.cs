/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */


namespace Farmpik.Domain.Dto
{
    public class ProductCategoryDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int ProductCount { get; set; } = 0;

        public bool IsEnabled { get { return ProductCount > 0; } }
    }
}
