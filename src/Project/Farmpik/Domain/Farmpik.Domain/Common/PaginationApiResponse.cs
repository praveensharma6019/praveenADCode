/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

namespace Farmpik.Domain.Common
{
    public class PaginationApiResponse<T> : ApiResponse<T>
    {
        public long? Count { get; set; }
    }
}