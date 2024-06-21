using System.Security.Claims;
//By ChatGPT
namespace Blazor_OpenBMCLAPI.Services
{
    /// <summary>
    /// 提供程序内查询登录用户信息的类
    /// </summary>
    public class UserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUsername()
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                return user.FindFirst(ClaimTypes.Name)?.Value;
            }
            return null;
        }
    }
}
