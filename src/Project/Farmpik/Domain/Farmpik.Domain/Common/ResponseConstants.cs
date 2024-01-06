/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using System.Runtime.InteropServices;

namespace Farmpik.Domain.Common
{
    public class ResponseConstants
    {
        public struct ResponseStatusCode
        {
            public const int Success = 200;
            public const int NoContent = 204;
            public const int InternalServerError = 500;
            public const int NotFound = 400;
            public const int Unauthorized = 401;
        }

        public struct ResponseMessage
        {
            public const string Success = "Success";
            public const string InternalServerError = "Internal Server Error";
            public const string Error = "Error";
            public const string No_Record_Found = "No record found";
            public const string Unauthorised_Access = "Unauthorised Access";
            public const string BadRequest = "Bad Request";
            public const string Invalid_Credentials = "Invalid credentials";
            public const string Invalid_VendorCode = "Please enter valid vendor ID";
            public const string Invalid_Mobile_Number = "Please enter valid mobile number";
            public const string OTP_Sent_Successfully = "OTP has been sent successfully";
            public const string Maximum_Limit = "You have exceeded maximum login tries, please try after 10 minutes";
            public const string Invalid_Otp = "Please enter a valid OTP";
            public const string Something_Went_Wrong = "Something Went Wrong";
            public const string OTP_Expired = "OTP has expired";
            public const string Token_Expired = "Token expired";
            public const string Invalid_Token = "Invalid token";
            public const string Default_VendorCode = "987654";
            public const string Default_OTP = "123456";
        }

        public struct AuthenticationSchemes
        {
            public const string COOKIE_MIDDLEWARE = "AppCookieMiddleWare";
            public const string COOKIE_APIMIDDLEWARE = "AuthToken";
        }

        public struct AuthenticationClaimTypes
        {
            public const string Id = "Id";
            public const string Audience = "aud";
            public const string Expired = "exp";
            public const string VendorCode = "VendorCode";
        }
    }
}