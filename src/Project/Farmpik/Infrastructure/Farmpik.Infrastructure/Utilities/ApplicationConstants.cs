/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

namespace Farmpik.Infrastructure.Utilities
{
    public struct ApplicationConstants
    {
        public const string OTP = "OTP";
        public const string Passenger = "PassengerName";
        public const string GeneralPassenger = "Customer";
        public const string API_Key = "ApiKey";
        public const string Captcha_Token = "CaptchaToken";
        public const string Recaptcha_Action = "RecaptchaAction";
        public const string SMS_URL = "SendSMSURL";
        public const string compaignID = "9feff9";
        public const string OTP_Template = "LoginOtpTemplateID";
        public const string Application_JSON = "application/json";
        public const string SMS_Key = "SendSMSAPIKEY";
        public const string Email_Admin = "Admin";
        public const string TokenExpired = "Token-Expired";
        public const string TrueValue = "true";
        public const string WorkSheetName = "Sheet";
        public const string DefaultExcelFileName = "download.xlsx";
        public const string DefaultCSVFileName = "download.csv";
        public const string ExcelFileContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string CSVFileContentType = "text/csv";
        public const string DateFormat_DDMMYYY = "dd/MM/yyyy";
        public const string FirebaseNotificationUrl = "/fcm/send";
        public const string Authorization = "Authorization";
        public const string Sender = "Sender";
    }

    public struct ConfigurationConstants
    {
        public const string KestrelOptions = "KestrelOptions";
        public const string CorsOptions = "CorsOptions";
    }

    public struct JwtSecurityOptions
    {
        public const string PublicKey = "JwtSecurityOptions:Asymmetric:PublicKey";

        public const string PrivateKey = "JwtSecurityOptions:Asymmetric:PrivateKey";

        public const string Audiences = "JwtSecurityOptions:Audiences";

        public const string Issuer = "JwtSecurityOptions:Issuer";
    }

    public struct AuthorizationPolicies
    {
        public const string Farmpik_Authenticated = "Farmpik_Authenticated";
        public const string Farmpik_Cookie_Authenticated = "Farmpik_Cookie_Authenticated";
    }

    public struct ExceptionType
    {
        public const string ValidationException = "ValidationException";
        public const string KeyNotFoundException = "KeyNotFoundException";
    }

    public struct SwaggerDocOptions
    {
        public const string Version = "SwaggerDocOptions:Version";
        public const string Title = "SwaggerDocOptions:Title";
        public const string Description = "SwaggerDocOptions:Description";
        public const string Name = "SwaggerDocOptions:Authorization";
    }

    public struct UserClaimTypes
    {
        public const string Name = "Name";
        public const string Id = "Id";
        public const string Audience = "aud";
    }

    public struct GoogleCloudOptions
    {
        public const string CredentialFile = "GoogleCloudPlatform:CredentialFile";
        public const string GOOGLE_APPLICATION_CREDENTIALS = "GOOGLE_APPLICATION_CREDENTIALS";
    }
}