/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Common.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Farmpik.Domain.Commands.VendorCommands
{
    public class GenerateOtpCommand
    {
        [Required]
        public string EncryptedVendorId { get; set; }

        [Required]
        public RoleType UserType { get; set; }

        public string Platform { get; set; }
    };

    public class VerifyOtpCommand
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(6)]
        [MinLength(6)]
        public string Otp { get; set; }
        public string DeviceToken { get; set; }
    }

    public class RefreshTokenCommand
    {
        [Required]
        public string AuthTokenValue { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }

    public class CreateRefreshTokenCommand
    {
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public string RefreshTokenToSave { get; set; }

        public Guid UserId { get; set; }
    }
}