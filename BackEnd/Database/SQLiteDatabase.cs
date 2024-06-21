#region unuse
//using Blazor_OpenBMCLAPI.BackEnd.Cipher;
//using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.Data.SQLite;
//using System.Diagnostics;
//using System.Security.Cryptography;
//using System.Text;

//namespace Blazor_OpenBMCLAPI.BackEnd.Database
//{
//    public class SQLiteDatabase : IDatabase
//    {
//        private SQLiteConnection? connection;
//        public async Task Init()
//        {

//            connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", Path.Combine(Shared.rootDirectory, "statistics.db")));
//            await connection.OpenAsync();
//            //创建表
//            await ExecuteNonQuery("create table if not exists users(name text not null, password text not null, salt text not null)");
//        }
//        #region Execute commands
//        private async Task ExecuteNonQuery(string sql)
//        {
//            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
//            {
//                await command.ExecuteNonQueryAsync();
//            }
//        }
//        private async Task ExecuteNonQuery(string sql, SQLiteParameter[] parameter)
//        {
//            using (SQLiteCommand command = new(sql, connection))
//            {
//                command.Parameters.AddRange(parameter);
//                await command.ExecuteNonQueryAsync();
//            }
//        }
//        private async Task<DbDataReader> ExecuteQuery(string sql)
//        {
//            using (SQLiteCommand command = new(sql, connection))
//            {
//                return await command.ExecuteReaderAsync();
//            }
//        }
//        private async Task<DbDataReader> ExecuteQuery(string sql, SQLiteParameter[] parameters)
//        {
//            using (SQLiteCommand command = new(sql, connection))
//            {
//                command.Parameters.AddRange(parameters);
//                return await command.ExecuteReaderAsync();
//            }
//        }
//        #endregion
//        public async Task<List<ClusterInfo>> GetClusters(string userName)
//        {
//            List<ClusterInfo> clusterInfoList = new();
//            DbDataReader reader = await ExecuteQuery(string.Format("select * from {0}_clusters", userName));
//            while (reader.Read())
//            {
//                ClusterInfo clusterInfo = new ClusterInfo();
//                clusterInfo.cluster_id = reader["id"].ToString();
//                clusterInfo.cluster_secret = reader["secret"].ToString();
//                clusterInfo.cluster_id_hash = clusterInfo.cluster_id;
//                //AES解密
//                string userPasswordCipher = await QueryUserPasswordCipher(userName);
//                clusterInfo.cluster_id = AESCipher.Decrypt(clusterInfo.cluster_id, userPasswordCipher);
//                clusterInfo.cluster_secret = AESCipher.Decrypt(clusterInfo.cluster_secret, userPasswordCipher);
//                clusterInfoList.Add(clusterInfo);
//            }
//            return clusterInfoList;
//        }
//        public async Task<bool> IsUserExist()
//        {
//            DbDataReader reader = await ExecuteQuery("select * from users");
//            if (!reader.Read()) return false;
//            return true;
//        }
//        public async Task CreateUser(string userName, string password)
//        {
//            await ExecuteNonQuery(string.Format("create table if not exists {0}_clusters(id text not null, secret text not null)", userName));
//            await ExecuteNonQuery(string.Format("create table if not exists {0}_storages(name text not null, type text not null, endpoint text, path text not null, username text, password not null)", userName));
//            var (hashUserName, hashPassword, salt) = SHA256Cipher.CreatePasswordHash(userName, password);

//            string sql = "insert into users (name, password, salt) values (@name, @password, @salt)";
//            SQLiteParameter[] parameters =
//            {
//                new SQLiteParameter("@name",hashUserName),
//                new SQLiteParameter("@password",hashPassword),
//                new SQLiteParameter("@salt",salt)
//            };
//            await ExecuteNonQuery(sql, parameters);
//            //创建该用户的表

//        }
//        public async Task<bool> AuthUser(string userName, string password)
//        {
//            //获取盐和密钥
//            string userNameHash = SHA256Cipher.GetUserNameHash(userName);
//            Trace.WriteLine(userNameHash);
//            string sql = "select * from users where name=@name";
//            SQLiteParameter[] parameters =
//            {
//                new SQLiteParameter("@name",userNameHash)
//            };
//            DbDataReader reader = await ExecuteQuery(sql, parameters);
//            if (!reader.Read())
//            {
//                Trace.WriteLine("User not found");
//                return false;
//            }
//            string passwordHash = reader["password"].ToString();
//            string salt = reader["salt"].ToString();
//            Trace.WriteLine(userNameHash + "\t" + passwordHash + "\t" + salt);
//            return SHA256Cipher.VerifyPassword(userName, password, userNameHash, passwordHash, salt);
//        }
//        public async Task<bool> AuthUser(string userName)
//        {
//            userName = SHA256Cipher.GetUserNameHash(userName);
//            string sql = "select * from users where name=@name";
//            SQLiteParameter[] parameters =
//            {
//                new SQLiteParameter("@name",userName)
//            };
//            DbDataReader reader = await ExecuteQuery(sql, parameters);
//            if (!reader.Read()) return false;
//            return true;
//        }
//        public async Task AddCluster(string userName, string clusterName, string clusterSecret)
//        {
//            string userPasswordCipher = await QueryUserPasswordCipher(userName);
//            string clusterSecretHash = AESCipher.Encrypt(clusterSecret, userPasswordCipher);
//            string clusterIDHash = AESCipher.Encrypt(clusterName, userPasswordCipher);
//            Trace.WriteLine(clusterIDHash + "\t" + clusterSecretHash);
//            string sql = string.Format("insert into {0}_clusters (id, secret) values (@id, @secret)", userName);
//            SQLiteParameter[] parameters =
//            {
//                new SQLiteParameter("@id",clusterIDHash),
//                new SQLiteParameter("@secret",clusterSecretHash)
//            };
//            await ExecuteNonQuery(sql, parameters);
//        }
//        public async Task<string> QueryUserPasswordCipher(string userName)
//        {
//            userName = SHA256Cipher.GetUserNameHash(userName);
//            string sql = "select * from users where name=@name";
//            SQLiteParameter[] parameters =
//            {
//                new SQLiteParameter("@name",userName)
//            };
//            DbDataReader reader = await ExecuteQuery(sql, parameters);
//            if (!reader.Read()) return null;
//            return reader["password"].ToString();
//        }

//        public async Task<bool> CheckCluster(string userName, string cluster_id)
//        {
//            var clusterList = await GetClusters(userName);
//            foreach (var cluster in clusterList)
//            {
//                if (cluster.cluster_id == cluster_id) return true;
//            }
//            return false;
//        }
//        public async Task<bool> DeleteCluster(string userName, string cluster_id)
//        {
//            if(!await CheckCluster(userName, cluster_id)) return false;
//            var clusterList = await GetClusters(userName);
//            List<string> cluster_id_hash = new();
//            foreach (var cluster in clusterList)
//            {
//                if (cluster_id == cluster.cluster_id)
//                {
//                    cluster_id_hash.Add(cluster.cluster_id_hash);
//                    break;
//                }
//            }
//            foreach (var cluster in cluster_id_hash)
//            {
//                string sql = string.Format("delete from {0}_clusters where id=@id", userName);
//                SQLiteParameter[] parameters =
//                {
//                new SQLiteParameter("@id",cluster)
//                };
//                await ExecuteNonQuery(sql, parameters);
//            }
//            return true;
//        }
//    }
//}
#endregion
using Blazor_OpenBMCLAPI.BackEnd.Cipher;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Blazor_OpenBMCLAPI.BackEnd.Database
{
    public class SQLiteDatabase : IDatabase
    {
        private SqliteConnection? connection;
        public SQLiteDatabase()
        {
            connection = new SqliteConnection($"Data Source={Path.Combine(Directory.GetCurrentDirectory(), "statistics.db")}");
            connection.Open();
            // 创建表
            ExecuteNonQuery("create table if not exists users(name text not null, password text not null, salt text not null)").GetAwaiter();
        }


        #region Execute commands
        private async Task ExecuteNonQuery(string sql)
        {
            EnsureConnectionOpen();
            using (SqliteCommand command = new SqliteCommand(sql, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task ExecuteNonQuery(string sql, SqliteParameter[] parameters)
        {
            EnsureConnectionOpen();
            using (SqliteCommand command = new(sql, connection))
            {
                command.Parameters.AddRange(parameters);
                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task<DbDataReader> ExecuteQuery(string sql)
        {
            EnsureConnectionOpen();
            SqliteCommand command = new(sql, connection);
            return await command.ExecuteReaderAsync();
        }

        private async Task<DbDataReader> ExecuteQuery(string sql, SqliteParameter[] parameters)
        {
            EnsureConnectionOpen();
            SqliteCommand command = new(sql, connection);
            command.Parameters.AddRange(parameters);
            return await command.ExecuteReaderAsync();
        }

        private void EnsureConnectionOpen()
        {
            if (connection?.State != System.Data.ConnectionState.Open)
            {
                connection?.Open();
            }
        }
        #endregion

        

        public async Task<bool> IsUserExist()
        {
            using (DbDataReader reader = await ExecuteQuery("select * from users"))
            {
                return await reader.ReadAsync();
            }
        }

        public async Task CreateUser(string userName, string password)
        {
            await ExecuteNonQuery($"create table if not exists {userName}_clusters(id text not null, secret text not null)");
            await ExecuteNonQuery($"create table if not exists {userName}_profiles(name text not null, type text not null, endpoint text not null)");

            var (hashUserName, hashPassword, salt) = SHA256Cipher.CreatePasswordHash(userName, password);
            string sql = "insert into users (name, password, salt) values (@name, @password, @salt)";
            SqliteParameter[] parameters =
            {
                new SqliteParameter("@name", hashUserName),
                new SqliteParameter("@password", hashPassword),
                new SqliteParameter("@salt", salt)
            };
            await ExecuteNonQuery(sql, parameters);
            // 创建该用户的表
        }

        public async Task<bool> AuthUser(string userName, string password)
        {
            // 获取盐和密钥
            string userNameHash = SHA256Cipher.GetUserNameHash(userName);
            Trace.WriteLine(userNameHash);
            string sql = "select * from users where name=@name";
            SqliteParameter[] parameters =
            {
                new SqliteParameter("@name", userNameHash)
            };
            using (DbDataReader reader = await ExecuteQuery(sql, parameters))
            {
                if (!await reader.ReadAsync())
                {
                    Trace.WriteLine("User not found");
                    return false;
                }
                string passwordHash = reader["password"].ToString();
                string salt = reader["salt"].ToString();
                Trace.WriteLine($"{userNameHash}\t{passwordHash}\t{salt}");
                return SHA256Cipher.VerifyPassword(userName, password, userNameHash, passwordHash, salt);
            }
        }

        public async Task<bool> AuthUser(string userName)
        {
            userName = SHA256Cipher.GetUserNameHash(userName);
            string sql = "select * from users where name=@name";
            SqliteParameter[] parameters =
            {
                new SqliteParameter("@name", userName)
            };
            using (DbDataReader reader = await ExecuteQuery(sql, parameters))
            {
                return await reader.ReadAsync();
            }
        }

        public async Task AddCluster(string userName, string clusterName, string clusterSecret)
        {
            string userPasswordCipher = await QueryUserPasswordCipher(userName);
            string clusterSecretHash = AESCipher.Encrypt(clusterSecret, userPasswordCipher);
            string clusterIDHash = AESCipher.Encrypt(clusterName, userPasswordCipher);
            Trace.WriteLine($"{clusterIDHash}\t{clusterSecretHash}");
            string sql = $"insert into {userName}_clusters (id, secret) values (@id, @secret)";
            SqliteParameter[] parameters =
            {
                new SqliteParameter("@id", clusterIDHash),
                new SqliteParameter("@secret", clusterSecretHash)
            };
            await ExecuteNonQuery(sql, parameters);
        }

        public async Task<string> QueryUserPasswordCipher(string userName)
        {
            userName = SHA256Cipher.GetUserNameHash(userName);
            string sql = "select * from users where name=@name";
            SqliteParameter[] parameters =
            {
                new SqliteParameter("@name", userName)
            };
            using (DbDataReader reader = await ExecuteQuery(sql, parameters))
            {
                return await reader.ReadAsync() ? reader["password"].ToString() : null;
            }
        }

        public async Task<bool> CheckCluster(string userName, string cluster_id)
        {
            var clusterList = await GetClusters(userName);
            foreach (var cluster in clusterList)
            {
                if (cluster.cluster_id == cluster_id) return true;
            }
            return false;
        }

        public async Task<bool> DeleteCluster(string userName, string cluster_id)
        {
            if (!await CheckCluster(userName, cluster_id)) return false;
            var clusterList = await GetClusters(userName);
            List<string> cluster_id_hash = new();
            foreach (var cluster in clusterList)
            {
                if (cluster_id == cluster.cluster_id)
                {
                    cluster_id_hash.Add(cluster.cluster_id_hash);
                    break;
                }
            }
            foreach (var cluster in cluster_id_hash)
            {
                string sql = $"delete from {userName}_clusters where id=@id";
                SqliteParameter[] parameters =
                {
                    new SqliteParameter("@id", cluster)
                };
                await ExecuteNonQuery(sql, parameters);
            }
            return true;
        }
        public async Task<List<ClusterInfo>> GetClusters(string userName)
        {
            List<ClusterInfo> clusterInfoList = new();
            using (DbDataReader reader = await ExecuteQuery($"select * from {userName}_clusters"))
            {
                while (await reader.ReadAsync())
                {
                    ClusterInfo clusterInfo = new ClusterInfo
                    {
                        cluster_id = reader["id"].ToString(),
                        cluster_secret = reader["secret"].ToString(),
                        cluster_id_hash = reader["id"].ToString()
                    };
                    // AES解密
                    string userPasswordCipher = await QueryUserPasswordCipher(userName);
                    clusterInfo.cluster_id = AESCipher.Decrypt(clusterInfo.cluster_id, userPasswordCipher);
                    clusterInfo.cluster_secret = AESCipher.Decrypt(clusterInfo.cluster_secret, userPasswordCipher);
                    clusterInfoList.Add(clusterInfo);
                }
            }
            return clusterInfoList;
        }
        public async Task<bool> CheckProfile(string userName, string name)
        {
            string sql = $"select * from {userName}_profiles where name=@name";
            SqliteParameter[] parameters =
            {
                new SqliteParameter("@name",name)
            };
            var reader=await ExecuteQuery(sql, parameters);
            if(!reader.Read()) return false;
            return true;

        }
        public async Task AddProfile(string userName, string name, string type, string endpoint)
        {
            if (await CheckProfile(userName, name)) return;
            string sql = $"insert into {userName}_profiles (name, type, endpoint) values (@name, @type, @endpoint)";
            SqliteParameter[] parameters =
            {
                new SqliteParameter("@name",name),
                new SqliteParameter("@type",type),
                new SqliteParameter("@endpoint",endpoint)
            };
            await ExecuteNonQuery(sql, parameters);
        }
        public async Task<bool> DeleteProfile(string userName,string name)
        {
            if(!await CheckProfile(userName,name)) return false;
            string sql = $"delete from {userName}_profiles where name=@name";
            SqliteParameter[] parameters =
            {
                new SqliteParameter("@name",name)
            };
            await ExecuteNonQuery(sql, parameters);
            return true;
        }
        public async Task<List<ProfileInfo>> GetProfiles(string userName)
        {
            List<ProfileInfo> profiles = new List<ProfileInfo>();
            using (DbDataReader reader = await ExecuteQuery($"select * from {userName}_profiles"))
            {
                while(await reader.ReadAsync())
                {
                    ProfileInfo profile = new ProfileInfo
                    {
                        name = reader["name"].ToString(),
                        type = reader["type"].ToString(),
                        endpoint = reader["endpoint"].ToString()
                    };
                    profiles.Add(profile);
                }
            }
            return profiles;
        }
    }
}

