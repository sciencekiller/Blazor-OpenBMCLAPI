using System.Data.SQLite;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Blazor_OpenBMCLAPI.BackEnd
{
    public class SQLManager
    {
        private SQLiteConnection _connection;
        public SQLManager()
        {
            if (Shared.SQLLock == true)
            {
                throw new IOException("Cannot connect to the database: already in use");
            }
            string dbPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "statistics.db");
            if(!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }
            _connection=new SQLiteConnection(dbPath);
            Shared.SQLLock = true;
        }

    }
}
