using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Blazor_OpenBMCLAPI.BackEnd;
using Blazor_OpenBMCLAPI.BackEnd.Database;

namespace Blazor_OpenBMCLAPI.Middleware
{
    //By ChatGPT
    public class InvalidateSessionMiddleware
    {
        private readonly RequestDelegate _next;
        private IDatabase db;

        public InvalidateSessionMiddleware(RequestDelegate next,IDatabase db)
        {
            _next = next;
            this.db = db;
        }

        public async Task Invoke(HttpContext context)
        {
            // 如果用户已经通过身份验证
            if (context.User.Identity.IsAuthenticated)
            {
                // 获取用户的 Email
                var userEmail = context.User.FindFirst(ClaimTypes.Email)?.Value;

                // 检查用户是否仍然存在于数据库中
                if (!string.IsNullOrEmpty(userEmail) && !await db.AuthUser(userEmail))
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
