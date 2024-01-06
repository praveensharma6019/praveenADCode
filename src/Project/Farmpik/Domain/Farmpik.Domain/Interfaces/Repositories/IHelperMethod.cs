/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Entities;
using System.Threading.Tasks;

namespace Farmpik.Domain.Interfaces.Repositories
{
    public interface IHelperMethod
    {
        string GenerateOtp();

        Task<bool> SendEmail(string url, string body);

        Task<bool> SendOtp(Vendor vendor, string otp, string platform);

        string Encrypt<T>(T model);

        T Decrypt<T>(string cipherText);
    }
}