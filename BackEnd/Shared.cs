
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
        /// SQL管理器的接口
        /// </summary>
        public static IDatabase Database { get; set; }
    }
}
