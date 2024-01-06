/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Common;
using Farmpik.Domain.Common.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Sitecore.Farmpik.Api.Website.Jwt
{
    public static class JwtProvider
    {
        public static string CreateToken(string id, RoleType roleType)
        {
            var handler = new System.IdentityModel.Tokens.JwtSecurityTokenHandler();
            var securityToken = CreateSecurityToken(handler, id, roleType);

            return handler.WriteToken(securityToken);
        }

        private static SecurityToken CreateSecurityToken(JwtSecurityTokenHandler handler,
            string id, RoleType roleType)
        {
            var identity = CreateClaimsIdentity(id, roleType);
            var credentials = new SigningCredentials(
                new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JwtSecurityKey"])),
                "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                "http://www.w3.org/2001/04/xmlenc#sha256");

            return handler.CreateToken(new System.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = identity,
                TokenIssuerName = ConfigurationManager.AppSettings["Issuer"],
                SigningCredentials = credentials,
                Lifetime = new Lifetime(DateTime.Now,DateTime.Now.AddDays(double.Parse(ConfigurationManager.AppSettings["TokenValidityInDays"])))
            });
        }

        private static ClaimsIdentity CreateClaimsIdentity(string id, RoleType roleType)
        {
            var claims = new List<Claim>(2)
                    {
                        new Claim(ResponseConstants.AuthenticationClaimTypes.Id, id),
                        new Claim(ClaimTypes.Role, roleType.ToString()),
                    };
            return new ClaimsIdentity(claims, ResponseConstants.AuthenticationSchemes.COOKIE_MIDDLEWARE);
        }

        public static ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifetime = false)
        {
            var credentials = new SigningCredentials(
                new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JwtSecurityKey"])),
                "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                "http://www.w3.org/2001/04/xmlenc#sha256");
            var tokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuer = ConfigurationManager.AppSettings["Issuer"],
                IssuerSigningKey = credentials.SigningKey,
                ValidateLifetime = validateLifetime
            };
            try
            {
                var tokenHandler = new System.IdentityModel.Tokens.JwtSecurityTokenHandler();
                return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
