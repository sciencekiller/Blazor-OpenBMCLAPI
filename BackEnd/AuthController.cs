﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blazor_OpenBMCLAPI.BackEnd
{
    /// <summary>
    /// Web API to login a authenticated user, and store these claims into a local cookie.
    /// </summary>
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static readonly AuthenticationProperties COOKIE_EXPIRES = new AuthenticationProperties()
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24),
            IsPersistent = true,
        };

        /// <summary />
        [HttpPost]
        [Route("api/auth/signin")]
        public async Task<ActionResult> SignInPost(SigninData value)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, value.Email),
                new Claim(ClaimTypes.Name, value.Email),
                new Claim(ClaimTypes.Role, "Administrator"),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = COOKIE_EXPIRES;

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          new ClaimsPrincipal(claimsIdentity),
                                          authProperties);

            return this.Ok();
        }

        /// <summary />
        [HttpPost]
        [Route("api/auth/signout")]
        public async Task<ActionResult> SignOutPost()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return this.Ok();
        }
    }

    /// <summary />
    public class SigninData
    {
        /// <summary />
        public string Email { get; set; }
        /// <summary />
        public string Password { get; set; }
    }
}
