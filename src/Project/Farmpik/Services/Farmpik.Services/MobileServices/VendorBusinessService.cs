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
using Farmpik.Domain.Interfaces.Repositories;
using Farmpik.Domain.Interfaces.Services;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Farmpik.Domain.Common.ResponseConstants;
using RoleType = Farmpik.Domain.Common.Enum.RoleType;

namespace Farmpik.Services.MobileServices
{
    public class VendorBusinessService : IVendorBusinessService
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IGuestUserRepository _guestUserRepository;
        private readonly IHelperMethod _helperMethod;

        public VendorBusinessService(IVendorRepository vendorRepository, 
            IGuestUserRepository guestUserRepository,
            IHelperMethod helperMethod)
        {
            _vendorRepository = vendorRepository;
            _guestUserRepository = guestUserRepository;
            _helperMethod = helperMethod;
        }

        public async Task<ApiResponse<OtpDto>> Authenticate(GenerateOtpCommand command)
        {
            try {
                command.EncryptedVendorId = _helperMethod.Decrypt<string>(command.EncryptedVendorId);
                
                if(command.UserType == RoleType.Guest
                    && command.EncryptedVendorId.Length != 10 
                    && command.EncryptedVendorId.All(char.IsDigit)) 
                    return ResponseHelper.GetResponse<OtpDto>(default, false, errorMes: ResponseMessage.Invalid_Mobile_Number);

            } catch (Exception)
            {
                return ResponseHelper.GetResponse<OtpDto>(default, false, 
                    errorMes: command.UserType == RoleType.Vendor 
                    ? ResponseMessage.Invalid_VendorCode
                    :ResponseMessage.Invalid_Mobile_Number);
            }
            
            var response = new ApiResponse<OtpDto>();
            OtpDto otpDto = new OtpDto() { Message = ResponseMessage.Success };

            var vendor = command.UserType == RoleType.Vendor 
                ? await _vendorRepository.GetAsync(x => x.VendorCode == command.EncryptedVendorId && x.IsActive && x.HasErrorField != true)
                : await _guestUserRepository.SaveGuestUser(_helperMethod.Encrypt(command.EncryptedVendorId));

            if (vendor is null)
            {
                otpDto.Message = ResponseMessage.Invalid_VendorCode;
                return response.SetResponse(default, otpDto.Message, false);
            }
            var dueDate = DateTime.UtcNow.AddMinutes(-10);
            var vendorOtp = await _vendorRepository.GetVendorOtp(x => x.VendorId == vendor.Id && x.IsActive &&
            x.CreatedOn > dueDate);

            if (vendorOtp != null && vendorOtp.SendCount >= 5)
            {
                otpDto.Message = ResponseMessage.Maximum_Limit;
                return response.SetResponse(default, otpDto.Message, false);
            }

            if (vendorOtp is null || vendorOtp?.CreatedOn < DateTime.UtcNow.AddMinutes(-5))
            {
                vendorOtp = await _vendorRepository.AddVendorOtpAsync(
                    new VendorOtp()
                    {
                        VendorId = vendor.Id,
                        Otp = command.EncryptedVendorId == ResponseMessage.Default_VendorCode ? ResponseMessage.Default_OTP : _helperMethod.GenerateOtp(),
                        OtpType = (int)command.UserType
                    }
                );
            }
            else if (vendorOtp != null)
            {
                vendorOtp.Otp = command.EncryptedVendorId == ResponseMessage.Default_VendorCode ? ResponseMessage.Default_OTP : _helperMethod.GenerateOtp();
                await _vendorRepository.UpdateVendorOtpAsync(vendorOtp);
            }
            vendor.Telephone = _helperMethod.Decrypt<string>(vendor.Telephone);
            if(command.EncryptedVendorId != ResponseMessage.Default_VendorCode)
            {
                await _helperMethod.SendOtp(vendor, vendorOtp?.Otp ?? string.Empty, command.Platform);
            }
            otpDto.Id = vendorOtp?.Id ?? default;
            otpDto.Telephone = Regex.Replace(vendor.Telephone, @"\d(?=.{4}\d{0,3}$)", "X");

            return otpDto?.Id != null
                ? response.SetResponse(otpDto)
                : response.SetResponse(default, otpDto?.Message ?? string.Empty, false);
        }

        public async Task<ApiResponse<EncryptedResponseApiModel>> VerifyOtp(VerifyOtpCommand comamnd)
        {
            ApiResponse<EncryptedResponseApiModel> response = new ApiResponse<EncryptedResponseApiModel>();
            var vendorOtp = await _vendorRepository.GetVendorOtp(x=> x.Id == comamnd.Id);

            if (vendorOtp == null) return response.SetResponse(default, ResponseMessage.Invalid_Otp,false);

            if(vendorOtp.Attempted >=5 && vendorOtp.CreatedOn > DateTime.UtcNow.AddMinutes(-10)) return response.SetResponse(default, ResponseMessage.Maximum_Limit, false);

            if (vendorOtp.Otp == comamnd.Otp && vendorOtp.CreatedOn < DateTime.UtcNow.AddMinutes(-5)) return response.SetResponse(default, ResponseMessage.OTP_Expired, false);
            var encrypted = new EncryptedResponseApiModel
            {
                RefreshToken = GenerateRefreshToken(),
                Expiration = DateTime.Now.AddDays(double.Parse(ConfigurationManager.AppSettings["RefreshTokenValidityInDays"]))
            };

            if (vendorOtp.Otp == comamnd.Otp)
            {
                vendorOtp.IsActive = false;
                var vendor = vendorOtp.OtpType == (int)RoleType.Vendor 
                    ? await  _vendorRepository.GetAsync(x=> x.Id == vendorOtp.VendorId && x.IsActive)
                    : await _guestUserRepository.UpdateGuestUser(vendorOtp.VendorId, comamnd.DeviceToken);

                if(vendorOtp.OtpType == (int)RoleType.Vendor)
                {
                    vendor.DeviceToken = comamnd.DeviceToken;
                    await _vendorRepository.UpdateAsync(vendor);
                }

                await _vendorRepository.AddRefreshTokenAsync(new Domain.Entities.RefreshToken {
                    RefreshTokenExpiryTime = encrypted.Expiration.Value,
                    RefreshTokens = encrypted.RefreshToken,
                    VendorId = vendor.Id
                });

                encrypted.AuthTokenValue = vendor.VendorCode.ToString();
                encrypted.Encrypted = _helperMethod.Encrypt(new { 
                    id = vendor.Id,
                    vendorCode = _helperMethod.Encrypt(vendor.VendorCode),
                    vendorName = _helperMethod.Encrypt(vendor.VendorName),
                    telephone = vendor.Telephone,
                    userType = vendorOtp.OtpType
                });
            }

            vendorOtp.Attempted++;
            await _vendorRepository.UpdateVendorOtpAsync(vendorOtp);

            return vendorOtp.Otp == comamnd.Otp
                ? response.SetResponse(encrypted)
                : response.SetResponse(default, ResponseMessage.Invalid_Otp, false);
        }

        public async Task<TokenApiModel> RefreshToken(Guid vendorId, string refreshToken)
        {
           var refresh = await _vendorRepository.GetRefreshToken(x => x.RefreshTokens == refreshToken
                        && x.IsActive
                        && x.VendorId == vendorId
                        && x.RefreshTokenExpiryTime >= DateTime.Now);

            if (refresh == null) return null;

            refresh.RefreshTokens = GenerateRefreshToken();
            refresh.RefreshTokenExpiryTime = DateTime.Now.AddDays(double.Parse(ConfigurationManager.AppSettings["RefreshTokenValidityInDays"]));
            await _vendorRepository.UpdateRefreshTokenAsync(refresh);

            return new TokenApiModel { Expiration = refresh.RefreshTokenExpiryTime, RefreshToken = refresh.RefreshTokens };
        }

        public async Task<ApiResponse<bool>> SignOut(string vendorCode)
        {
           var vendor = await _vendorRepository.GetAsync(x=> x.IsActive && x.VendorCode == vendorCode);
            if(vendor != null)
            {
                vendor.DeviceToken = null;
                await _vendorRepository.UpdateAsync(vendor);
            }
            return new ApiResponse<bool>().SetResponse(true);
        }

        public async Task<Vendor> GetVendorByCode(string vendorCode)
        {
            return await _vendorRepository.GetAsync( x=> x.IsActive && x.VendorCode == vendorCode);
            
        }

        private  static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }        
        }
    }
}
