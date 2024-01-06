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
    public class PaymentDto
    {
        public Guid Id { get; set; }

        public int CompanyCode { get; set; }

        public long PurchaseOrderNumber { get; set; }

        public long DocumentNumber { get; set; }

        public long InvoiceNumber { get; set; }

        public DateTime? PostingDate { get; set; }

        public long VendorCode { get; set; }

        public string VendorName { get; set; }

        public string DocumentType { get; set; }

        public decimal AmountInLC { get; set; }

        public decimal WithholdingTaxAmount { get; set; }

        public decimal Total { get; set; }
    }
}
