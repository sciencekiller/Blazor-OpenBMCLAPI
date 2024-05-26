namespace Blazor_OpenBMCLAPI.BackEnd.Database
{
    public interface IDatabase
    {
        public Task Init();
        public Task<List<ClusterInfo>> GetClusters();
        public Task<bool> IsUserExist();
        public Task<bool> AuthUser(string userName,string password);
        public Task CreateUser(string userName,string password);
    }
}
