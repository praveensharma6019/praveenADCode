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
    public class EncryptionsOptions
    {
        public string Key { get; set; } = ConfigurationManager.AppSettings["Key"];

        public string IV { get; set; } = ConfigurationManager.AppSettings["IV"];
    }
}