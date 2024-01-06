/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using System.Configuration;

namespace Farmpik.Domain.Common.ConfigurationModel
{
    public class AdaniEmailOptions
    {
        public string EmailUrl { get; set; } = ConfigurationManager.AppSettings["EmailUrl"];
        public string OtpType { get; set; } = ConfigurationManager.AppSettings["OtpType"];
    }
}