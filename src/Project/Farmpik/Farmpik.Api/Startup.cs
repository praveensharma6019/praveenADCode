using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Text;

namespace Sitecore.Farmpik.Api.Website
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var credentials = new SigningCredentials(
                 new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JwtSecurityKey"])),
                 "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                 "http://www.w3.org/2001/04/xmlenc#sha256");

            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidIssuer = ConfigurationManager.AppSettings["Issuer"],
                        IssuerSigningKey = credentials.SigningKey,
                        ClockSkew = System.TimeSpan.Zero
                    }
                });
        }
    }
}