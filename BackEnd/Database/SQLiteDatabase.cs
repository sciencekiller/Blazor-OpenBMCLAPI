using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Data.Common;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace Blazor_OpenBMCLAPI.BackEnd.Database
{
    public class SQLiteDatabase:IDatabase
    {
        private SQLiteConnection connection;
        public async Task Init()
        {

            connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;",Path.Combine(Shared.rootDirectory,"statistics.db")));
            await connection.OpenAsync();
            //创建表
            await ExecuteNonQuery("create table if not exists clusters(id text not null, secret text not null)");
            await ExecuteNonQuery("create table if not exists users(name text not null, password text not null)");
        }
        #region Execute commands
        private async Task ExecuteNonQuery(string sql)
        {
            using(SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
        private async Task ExecuteNonQuery(string sql,SQLiteParameter[] parameter)
        {
            using (SQLiteCommand command = new(sql,connection))
            {
                command.Parameters.AddRange(parameter);
                await command.ExecuteNonQueryAsync();
            }
        }
        private async Task<DbDataReader> ExecuteQuery(string sql)
        {
            using(SQLiteCommand command= new(sql, connection))
            {
                return await command.ExecuteReaderAsync();
            }
        }
        private async Task<DbDataReader> ExecuteQuery(string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteCommand command = new(sql, connection))
            {
                command.Parameters.AddRange(parameters);
                return await command.ExecuteReaderAsync();
            }
        }
        #endregion
        public async Task<List<ClusterInfo>> GetClusters()
        {
            List<ClusterInfo> clusterInfoList = new();
            DbDataReader reader = await ExecuteQuery("select * from clusters");
            while (reader.Read())
            {
                ClusterInfo clusterInfo = new ClusterInfo();
                clusterInfo.cluster_id = reader["id"].ToString();
                clusterInfo.cluster_secret = reader["secret"].ToString();
                clusterInfoList.Add(clusterInfo);
            }
            return clusterInfoList;
        }
        public async Task<bool> IsUserExist()
        {
            DbDataReader reader = await ExecuteQuery("select * from users");
            if (!reader.Read()) return false;
            return true;
        }
        public async Task CreateUser(string userName,string password)
        {
            MD5 md5 = MD5.Create();
            byte[] passwordbyte=Encoding.UTF8.GetBytes(password);
            byte[] result= md5.ComputeHash(passwordbyte);
            password = BitConverter.ToString(result).Replace("-", "");
            byte[] namebyte=Encoding.UTF8.GetBytes(userName);
            result=md5.ComputeHash(namebyte);
            userName = BitConverter.ToString(result).Replace("-", "");
            string sql = "insert into users (name, password) values (@name, @password)";
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@name",userName),
                new SQLiteParameter("@password",password)
            };
            await ExecuteNonQuery(sql, parameters);
        }
        public async Task<bool> AuthUser(string userName,string password)
        {
            MD5 md5 = MD5.Create();
            byte[] passwordbyte = Encoding.UTF8.GetBytes(password);
            byte[] result = md5.ComputeHash(passwordbyte);
            password = BitConverter.ToString(result).Replace("-", "");
            byte[] namebyte = Encoding.UTF8.GetBytes(userName);
            result = md5.ComputeHash(namebyte);
            userName = BitConverter.ToString(result).Replace("-", "");
            string sql = "select * from users where name=@name and password=@password";
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@name",userName),
                new SQLiteParameter("@password",password)
            };
            DbDataReader reader=await ExecuteQuery(sql, parameters);
            if(!reader.Read()) return false;
            return true;
        }
    }
}
