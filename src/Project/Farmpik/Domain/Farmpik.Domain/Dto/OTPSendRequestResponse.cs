/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using System.Collections.Generic;

namespace Farmpik.Domain.Dto
{
    public class OtpSendRequest
    {
        public string MobileNo { get; set; }
        public string CountryCode { get; set; }
        public string compaignID { get; set; }
        public List<OtpKey> Data { get; set; }
    }

    public class OtpKey
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}