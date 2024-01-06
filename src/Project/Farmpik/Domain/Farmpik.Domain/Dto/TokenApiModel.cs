/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using System;
using System.ComponentModel;

namespace Farmpik.Domain.Dto
{
    public class TokenApiModel
    {
        public string AuthTokenValue { get; set; }

        public string RefreshToken { get; set; }

        [DefaultValue(null)]
        public DateTime? Expiration { get; set; }
    }
}