using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Blazor_OpenBMCLAPI.BackEnd
{
    //By ChatGPT
    public class InvalidateSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public InvalidateSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // 如果用户已经通过身份验证
            if (context.User.Identity.IsAuthenticated)
            {
                // 获取用户的 Email
                var userEmail = context.User.FindFirst(ClaimTypes.Email)?.Value;

                // 检查用户是否仍然存在于数据库中
                if (!string.IsNullOrEmpty(userEmail) && !await Shared.Database.AuthUser(userEmail))
                {
                    // 用户不存在，使会话失效
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }

            // 继续处理请求
            await _next(context);
        }
    }
}
