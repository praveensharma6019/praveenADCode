using Farmpik.Domain.Common;
using Farmpik.Infrastructure.Utilities;
using Sitecore.Farmpik.Api.Website.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using static Farmpik.Domain.Common.ResponseConstants;

namespace Sitecore.Farmpik.Api.Website.Filters
{
    public class CustomAuthorized : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext context)
        {
            base.OnAuthorization(context);
            var authorization = context.Request.Headers.Authorization;
            bool isTokenExpired = false;
            if (authorization != null)
            {
                var token = authorization.ToString();
                token = token.Split(' ')[token.Split(' ').Length - 1];
                var claims = JwtProvider.GetPrincipalFromToken(token, false);
                if (claims != null)
                {
                    var date = System.DateTimeOffset.FromUnixTimeSeconds(long.Parse(claims.FindFirst(x => x.Type == AuthenticationClaimTypes.Expired).Value)).DateTime;
                    if (date > System.DateTime.Now)
                    {
                        string userId = claims.FindFirst(x => x.Type == AuthenticationClaimTypes.Id).Value;
                        var claimsIdentity = new ClaimsIdentity();
                        claimsIdentity.AddClaim(new Claim(UserClaimTypes.Id, userId));
                        context.RequestContext.Principal = new ClaimsPrincipal(claimsIdentity);
                        return;
                    }
                    isTokenExpired = true;
                }
            }

            context.Response = context.ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new ApiResponse<object>()
            {
                Status = false,
                StatusCode = isTokenExpired ? (int)HttpStatusCode.Forbidden : (int)HttpStatusCode.Unauthorized,
                Message = isTokenExpired ? "Token expired" : "Invalid token"
            });
        }
    }
}