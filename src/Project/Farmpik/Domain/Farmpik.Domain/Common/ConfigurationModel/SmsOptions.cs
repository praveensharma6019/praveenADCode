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
    public class SmsOptions
    {
        public string SmsUrl { get; set; }= ConfigurationManager.AppSettings["SmsUrl"];
        public string SmsApiKey { get; set; }= ConfigurationManager.AppSettings["SmsApiKey"];
        public string compaignID { get; set; }
    }
}