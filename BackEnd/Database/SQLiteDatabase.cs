using Blazor_OpenBMCLAPI.BackEnd.Cipher;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Blazor_OpenBMCLAPI.BackEnd.Database
{
    public class SQLiteDatabase:IDatabase
    {
        private SQLiteConnection? connection;
        public async Task Init()
        {

            connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;",Path.Combine(Shared.rootDirectory,"statistics.db")));
            await connection.OpenAsync();
            //创建表
            await ExecuteNonQuery("create table if not exists users(name text not null, password text not null, salt text not null)");
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
        public async Task<List<ClusterInfo>> GetClusters(string userName)
        {
            List<ClusterInfo> clusterInfoList = new();
            DbDataReader reader = await ExecuteQuery(string.Format("select * from {0}_clusters",userName));
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
            await ExecuteNonQuery(string.Format("create table if not exists {0}_clusters(id text not null, secret text not null)", userName));
            var (hashUserName, hashPassword, salt)=SHA256Cipher.CreatePasswordHash(userName,password);

            string sql = "insert into users (name, password, salt) values (@name, @password, @salt)";
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@name",hashUserName),
                new SQLiteParameter("@password",hashPassword),
                new SQLiteParameter("@salt",salt)
            };
            await ExecuteNonQuery(sql, parameters);
            //创建该用户的表
            
        }
        public async Task<bool> AuthUser(string userName,string password)
        {
            //获取盐和密钥
            string userNameHash=SHA256Cipher.GetUserNameHash(userName);
            Trace.WriteLine(userNameHash);
            string sql = "select * from users where name=@name";
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@name",userNameHash)
            };
            DbDataReader reader=await ExecuteQuery(sql, parameters);
            if(!reader.Read())
            {
                Trace.WriteLine("User not found");
                return false;
            }
            string passwordHash = reader["password"].ToString();
            string salt = reader["salt"].ToString();
            Trace.WriteLine(userNameHash + "\t" + passwordHash + "\t" + salt);
            return SHA256Cipher.VerifyPassword(userName,password,userNameHash,passwordHash,salt);
        }
        public async Task<bool> AuthUser(string userName)
        {
            userName = SHA256Cipher.GetUserNameHash(userName);
            string sql = "select * from users where name=@name";
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@name",userName)
            };
            DbDataReader reader = await ExecuteQuery(sql, parameters);
            if (!reader.Read()) return false;
            return true;
        }

        public async Task AddCluster(string userName, string clusterName, string clusterSecret)
        {
            
        }
        private async Task<string> QueryUserPasswordCipher(string userName)
        {
            userName = SHA256Cipher.GetUserNameHash(userName);
            string sql = "select * from users where name=@name";
            SQLiteParameter[] parameters =
            {
                new SQLiteParameter("@name",userName)
            };
            DbDataReader reader = await ExecuteQuery(sql, parameters);
            if (!reader.Read()) return null;
            return reader["password"].ToString();
        }
    }
}
