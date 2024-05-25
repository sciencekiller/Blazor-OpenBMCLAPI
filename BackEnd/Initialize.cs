using Blazor_OpenBMCLAPI.BackEnd.Database;
using System.Text;
using static Blazor_OpenBMCLAPI.BackEnd.Enums;

namespace Blazor_OpenBMCLAPI.BackEnd
{
    public static class Initialize
    {
        public static async Task Run()
        {
            Shared.rootDirectory = Directory.GetCurrentDirectory();
            //启动的时候应该不会跑着吧
            Statistics.status = new DisplayStatus(Status.Offline);
            //寻找db.json文件
            if (!File.Exists(Path.Combine(Shared.rootDirectory, "db.json")))
            {
                Shared.Database = new SQLiteDatabase();
            }
            //初始化db
            var sql = Shared.Database;
            await sql.Init();

        }
    }
}
