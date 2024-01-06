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
    public class VendorDto
    {
        public Guid Id { get; set; }

        public string VendorCode { get; set; }

        public string VendorName { get; set; }

        public string Telephone { get; set; }

        public string DeviceToken { get; set; }
    }
}
