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
    public class GoodsReceiptDto
    {
        public decimal? NetDue { get; set; }

        public decimal? TotalEarning { get; set; }

        public long TotalCount { get; set; }

        public List<ReceipDetails> Receipts { get; set; }
    }

    public class ReceipDetails
    {
        public string VendorCode { get; set; }

        public string GateNumber { get; set; }

        public DateTime PostingDate { get; set; }

        public decimal TotalQuantity { get; set; }

        public decimal TotalAmount { get; set; }

        public int TotalSkus { get { return SKUs.Count; } }

        public List<SKU> SKUs { get; set; }
	}

    public class SKU {

        public string Name { get; set; }

        public decimal Quantity { get; set; }

        public decimal Amount { get; set; }
    }
}
