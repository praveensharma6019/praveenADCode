/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using System.Collections.Generic;

namespace Farmpik.Domain.Common
{
    public class PaginatedList<T>
    {
        public long TotalCount { get; set; }

        public List<T> Value { get; set; }
    }
}