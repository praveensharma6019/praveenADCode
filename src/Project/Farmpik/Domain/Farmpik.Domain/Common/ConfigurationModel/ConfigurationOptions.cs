/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

namespace Farmpik.Domain.Common.ConfigurationModel
{
    public class ConfigurationOptions
    {
        public SmsOptions SmsOptions { get; set; } = new SmsOptions();

        public AdaniEmailOptions AdaniEmailOptions { get; set; } = new AdaniEmailOptions();

        public EncryptionsOptions EncryptionsOptions { get; set; } = new EncryptionsOptions();

        public CaptchaSettings Captcha { get; set; }
    }
}