namespace Blazor_OpenBMCLAPI.BackEnd.Database
{
    public interface IDatabase
    {
        public Task Init();
        public Task<List<ClusterInfo>> GetClusters();
        public Task<bool> CheckUser();
    }
}
