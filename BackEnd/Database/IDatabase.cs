namespace Blazor_OpenBMCLAPI.BackEnd.Database
{
    public interface IDatabase
    {
        public Task Init();
        public Task<List<ClusterInfo>> GetClusters(string userName);
        public Task<bool> IsUserExist();
        public Task<bool> AuthUser(string userName,string password);
        public Task<bool> AuthUser(string userName);
        public Task CreateUser(string userName,string password);
    }
}
