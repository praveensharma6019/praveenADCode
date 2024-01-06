/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Common;
using Farmpik.Domain.Common.Enum;


namespace Farmpik.Domain.Commands.ProductCommands
{
    public class QueryProductsCommand : BasePagination
    {
        public CategoryType Category { get; set; }
    }

    public class QueryProductPricesCommand : BasePagination
    {
        public string Location { get; set; }
    }
}