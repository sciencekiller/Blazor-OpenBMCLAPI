using Microsoft.AspNetCore.Mvc.Formatters;

namespace Blazor_OpenBMCLAPI.BackEnd.Database
{
    public interface IDatabase
    {
        public Task<List<ClusterInfo>> GetClusters(string userName);
        public Task<bool> IsUserExist();
        public Task<bool> AuthUser(string userName,string password);
        public Task<bool> AuthUser(string userName);
        public Task CreateUser(string userName,string password);
        public Task AddCluster(string userName,string clusterName,string clusterSecret);
        public Task<string> QueryUserPasswordCipher(string userName);
        public Task<bool> CheckCluster(string userName,string cluster_id);
        public Task<bool> DeleteCluster(string userName,string cluster_id);
        public Task AddProfile(string userName, string name, string type, string endpoint);
        public Task<bool> DeleteProfile(string userName,string name);
        public Task<bool> CheckProfile(string userName,string name);
        public Task<List<ProfileInfo>> GetProfiles(string userName);
    }
}
