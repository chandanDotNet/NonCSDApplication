using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace POS.API.Helpers
{
    public class ClaimCheckAttribute : Attribute, IActionFilter
    {
        private readonly string _claimName;
        private StringValues auth;

        public ClaimCheckAttribute(string claimName)
        {
            _claimName = claimName;

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out auth);

            var tokenValue = auth[0].Replace("Bearer ", "");
            string[] claimArray = new string[] { };
            JwtSecurityToken token;
            Claim claim = null;

            if (_claimName.Contains(","))
            {
                claimArray = _claimName.Split(",");
            }
            token = new JwtSecurityTokenHandler().ReadJwtToken(tokenValue);
            if (claimArray.Count() > 0)
            {
                for (int i = 0; i < claimArray.Length; i++)
                {
                    claim = token.Claims.Where(c => c.Type.Trim() == claimArray[i].Trim() && c.Value == "true").FirstOrDefault();

                    if (claim != null)
                    {
                        break;
                    }
                }
            }
            else
            {
                claim = token.Claims.Where(c => c.Type == _claimName && c.Value == "true").FirstOrDefault();
            }

            if (claim == null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);

            }
        }
    }
}