/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

namespace Farmpik.Domain.Common.ConfigurationModel
{
    public class CaptchaSettings
    {
        public string CredentialFile { get; set; }
        public string SiteKey { get; set; }
        public string ProjectId { get; set; }
        public string GoogleVerificationUrl { get; set; }
        public string SecretKey { get; set; }
        public bool IsCaptchaRequired { get; set; }
    }
}