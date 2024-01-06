/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

namespace Farmpik.Domain.Dto
{
    public class EncryptedResponseApiModel : TokenApiModel
    {
        public string Encrypted { get; set; }
    }
}