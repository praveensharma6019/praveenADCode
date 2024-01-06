/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using System.Runtime.InteropServices;

namespace Farmpik.Domain.Dto
{
    public class OtpDto
    {
        public string Message { get; set; }

        public System.Guid Id { get; set; }

        public string Telephone { get; set; }


    }
}