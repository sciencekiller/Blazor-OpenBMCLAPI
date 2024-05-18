using System.Data.SQLite;
namespace Blazor_OpenBMCLAPI.BackEnd.SQL
{
    public class SQLite:ISQLManager
    {
        public async Task<SQLiteConnection> GetConnection()
        {
            while (Shared.SQLLock == true) await Task.Delay(100);
            string dbPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "statistics.db");
            string con = "Data Source=" + dbPath + ";Version=3;";
            var connection=new SQLiteConnection(con);
            connection.Open();
            Shared.SQLLock = true;
            return connection;

        }

        public async Task CreateTable(string tableName)
        {
            var _connection=await GetConnection();
            string createTableSql = string.Format("CREATE TABLE IF NOT EXISTS {0} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Key TEXT, Value TEXT)",tableName);
            using (SQLiteCommand command = new SQLiteCommand(createTableSql, _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }

        public async Task UpdateValue(string tableName, string key, string value)
        {
            if (!await Exists(tableName, key))
            {
                await InsertValue(tableName, key, value);
            }
            else
            {
                var _connection = await GetConnection();
                string updateValueSql = string.Format("UPDATE {0} SET Value = {2} WHERE Key = {1}",tableName,key,value);
                using (SQLiteCommand command = new SQLiteCommand(updateValueSql, _connection))
                {
                    command.ExecuteNonQuery();
                }
                _connection.Close();
            }
        }
        public async Task InsertValue(string tableName, string key, string value)
        {
            var _connection = await GetConnection();
            string updateValueSql = string.Format("INSERT INTO {0} (Key, Value) VALUES ({1}, {2})",tableName,key,value);
            using (SQLiteCommand command = new SQLiteCommand(updateValueSql, _connection))
            {
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }
        public async Task<string> QueryValue(string tableName, string key)
        {
            var _connection = await GetConnection();
            string updateValueSql = string.Format("SELECT * FROM {0} WHERE Key = {1}",tableName,key);
            using (SQLiteCommand command = new SQLiteCommand(updateValueSql, _connection))
            {
                SQLiteDataReader reader= command.ExecuteReader();
                reader.Read();
                _connection.Close();
                return reader["Value"].ToString();
            }
        }
        public async Task<bool> Exists(string tableName, string key)
        {
            var _connection = await GetConnection();
            string updateValueSql = string.Format("SELECT COUNT(*) FROM {0} WHERE Key = {1}",tableName,key);
            using (SQLiteCommand command = new SQLiteCommand(updateValueSql, _connection))
            {
                if ((long)command.ExecuteScalar() > 0) return true;
                _connection.Close();
                return false;
            }
        }
    }
}
