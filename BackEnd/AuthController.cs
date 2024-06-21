using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Blazor_OpenBMCLAPI.BackEnd.Database;

namespace Blazor_OpenBMCLAPI.BackEnd
{
    /// <summary>
    /// Web API to login a authenticated user, and store these claims into a local cookie.
    /// </summary>
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IDatabase db;
        public AuthController(IDatabase database)
        {
            this.db = database;
        }

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
            if(!await db.AuthUser(value.Email, value.Password+"saltwood")) {
                return Forbid();
            }
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

            return Ok();
        }

        /// <summary />
        [HttpPost]
        [Route("api/auth/signout")]
        public async Task<ActionResult> SignOutPost()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
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
