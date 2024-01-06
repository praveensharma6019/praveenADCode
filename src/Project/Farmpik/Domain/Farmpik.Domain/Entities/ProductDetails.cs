/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

namespace Farmpik.Domain.Entities
{
    public class ProductDetails : AuditEntity
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string Imagelink { get; set; }

        public bool IsAvailableInRampur { get; set; }

        public bool IsAvailableInRohru { get; set; }

        public bool IsAvailableInSainj { get; set; }

        public bool IsAvailableInOddi { get; set; }

        public decimal Price { get; set; }

        public bool HasErrorField { get; set; }

        public string ErrorField { get; set; }
    }
}