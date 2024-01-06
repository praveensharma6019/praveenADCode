/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Commands.VendorCommands;
using Farmpik.Domain.Common;
using Farmpik.Domain.Dto;
using Farmpik.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Farmpik.Domain.Interfaces.Services
{
    public interface IVendorBusinessService
    {
        Task<ApiResponse<OtpDto>> Authenticate(GenerateOtpCommand command);

        Task<ApiResponse<EncryptedResponseApiModel>> VerifyOtp(VerifyOtpCommand comamnd);

        Task<TokenApiModel> RefreshToken(Guid vendorId, string refreshToken);

        Task<ApiResponse<bool>> SignOut(string vendorCode);
        Task<Vendor> GetVendorByCode(string vendorCode);
    }
}
