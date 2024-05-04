using Blazor_OpenBMCLAPI.Core.Profile;

namespace Blazor_OpenBMCLAPI.Core
{
    public static class Shared
    {
        /// <summary>
        /// 配置文件实例
        /// </summary>
        public static ProfileInstance? profile { get; set; }

        /// <summary>
        /// 程序运行根目录
        /// </summary>
        public static string? rootDirectory { get; set; }
        /// <summary>
        /// 配置文件管理器的实例
        /// </summary>
        public static ProfileManager? profileManager { get; set; }
        /// <summary>
        /// 用于代码获取依赖注入（虽然我也不知道为什么要这个，但是如果直接注册服务的话会有一些意想不到的东西被执行，干脆用一些奇怪的手段）
        /// </summary>
        public static IServiceProvider? serviceProvider { get; set; }
    }
}
