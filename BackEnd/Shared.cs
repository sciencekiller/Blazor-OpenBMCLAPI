﻿
using Blazor_OpenBMCLAPI.BackEnd.Database;

namespace Blazor_OpenBMCLAPI.BackEnd
{
    public static class Shared
    {


        /// <summary>
        /// 程序运行根目录
        /// </summary>
        public static string? rootDirectory { get; set; }
        /// <summary>
        /// 用于代码获取依赖注入（虽然我也不知道为什么要这个，但是如果直接注册服务的话会有一些意想不到的东西被执行，干脆用一些奇怪的手段）
        /// </summary>
        public static IServiceProvider? serviceProvider { get; set; }
        /// <summary>
        /// SQL管理器的接口
        /// </summary>
        public static IDatabase Database { get; set; }
    }
}