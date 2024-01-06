/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;
using static Farmpik.Domain.Common.ResponseConstants;

namespace Sitecore.Farmpik.Api.Website.Controllers
{
    [ExcludeFromCodeCoverage]
    [EnableCors(origins: "https://www.farmpik.com", headers: "*", methods: "*")]
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Get current user id from claims
        /// </summary>
        public string CurrentVendorCode
        {
            get
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                return claimsIdentity.FindFirst(AuthenticationClaimTypes.Id).Value;
            }
        }

        protected ApiResponse<T> GetServerError<T>()
        {
            return new ApiResponse<T>
            {
                Message = ResponseConstants.ResponseMessage.InternalServerError,
                Status = false,
                StatusCode = ResponseStatusCode.InternalServerError,
                Payload = default
            };
        }

        protected PaginationApiResponse<List<T>> GetPaginationServerError<T>()
        {
            return new PaginationApiResponse<List<T>>
            {
                Message = ResponseConstants.ResponseMessage.InternalServerError,
                Status = false,
                StatusCode = ResponseStatusCode.InternalServerError,
                Payload = default
            };
        }
    }
}