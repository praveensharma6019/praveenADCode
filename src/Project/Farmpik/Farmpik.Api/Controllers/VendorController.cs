/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Sitecore.Farmpik.Api.Website.Filters;
using Sitecore.Farmpik.Api.Website.Jwt;
using Farmpik.Domain.Commands.VendorCommands;
using Farmpik.Domain.Common;
using Farmpik.Domain.Dto;
using Farmpik.Domain.Interfaces.Services;
using log4net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using static Farmpik.Domain.Common.ResponseConstants;
using Farmpik.Domain.Common.Enum;
using System.Security.Claims;

namespace Sitecore.Farmpik.Api.Website.Controllers
{
    [ExcludeFromCodeCoverage]
    [RoutePrefix("v1/Vendor")]
    public class VendorController : BaseApiController
    {
        private readonly ILog _logger;
        private IVendorBusinessService _vendorBusinessService;

        public VendorController(IVendorBusinessService vendorBusinessService)
        {
            _logger = LogManager.GetLogger(typeof(VendorController));
            _vendorBusinessService = vendorBusinessService;
        }

        [HttpPost]
        [Route("SignIn")]
        [ValidateModel]
        public async Task<ApiResponse<OtpDto>> SignIn([FromBody] GenerateOtpCommand command)
        {
            try
            {
                return await _vendorBusinessService.Authenticate(command);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return GetServerError<OtpDto>();
            }
        }

        [HttpPost]
        [Route("VerifyOtp")]
        [ValidateModel]
        public async Task<ApiResponse<EncryptedResponseApiModel>> VerifyOtp([FromBody] VerifyOtpCommand comamnd)
        {
            try
            {
                var response = await _vendorBusinessService.VerifyOtp(comamnd);
                if (response.Status ?? false)
                {
                    response.Payload.AuthTokenValue = JwtProvider.CreateToken(response.Payload.AuthTokenValue,
                        response.Payload.AuthTokenValue.All(char.IsDigit) ? RoleType.Vendor : RoleType.Guest);
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return GetServerError<EncryptedResponseApiModel>();
            }
        }

        [HttpPost]
        [Route("RefreshToken")]
        [ValidateModel]
        public async Task<ApiResponse<TokenApiModel>> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var apiResponse = new ApiResponse<TokenApiModel>();
            apiResponse.SetResponse(
                default,
                ResponseConstants.ResponseMessage.BadRequest,
                false,
                ResponseStatusCode.Unauthorized
            );

            if (
                string.IsNullOrEmpty(command.AuthTokenValue)
                || string.IsNullOrEmpty(command.RefreshToken)
            )
            {
                return apiResponse;
            }

            try
            {
                var claims = JwtProvider
                    .GetPrincipalFromToken(command.AuthTokenValue)
                    ?.Claims;
                if (claims == null || claims.Count() == 0)
                {
                    return apiResponse;
                }
                string userId =
                    claims.FirstOrDefault(x => x.Type == AuthenticationClaimTypes.Id)?.Value;
                string audience =
                    claims.FirstOrDefault(x => x.Type == AuthenticationClaimTypes.Audience)?.Value;

                Enum.TryParse(claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value, out RoleType userType);

                var id = userType == RoleType.Vendor ?
                    (await _vendorBusinessService.GetVendorByCode(userId)).Id
                    : Guid.Parse(userId);

                var refreshToken = await _vendorBusinessService.RefreshToken(id, command.RefreshToken);

                if (refreshToken != null)
                {
                    refreshToken.AuthTokenValue = JwtProvider.CreateToken(userId, userType);
                    return apiResponse.SetResponse(refreshToken);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return apiResponse;
        }

        [HttpPost]
        [Route("SignOut")]
        [CustomAuthorized]
        public async Task<ApiResponse<bool>> SignOut()
        {
            try
            {
                return await _vendorBusinessService.SignOut(CurrentVendorCode);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return GetServerError<bool>();
            }
        }
    }
}