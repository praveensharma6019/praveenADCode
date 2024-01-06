/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Dto;
using System.Collections.Generic;

namespace Sitecore.Farmpik.Website.ViewModel
{
    public class PurchaseViewModel
    {
        public List<PurchaseDto> Purchases { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public int PageCount { get { return TotalCount % PageSize == 0 ? (int)(TotalCount / PageSize) : (int)(TotalCount / PageSize)+1; } }
    }
}