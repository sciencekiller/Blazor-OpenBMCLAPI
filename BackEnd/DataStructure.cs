using System.Diagnostics;

namespace Blazor_OpenBMCLAPI.BackEnd
{
    public class ClusterInfo
    {
        public string cluster_id { get; set; }
        public string cluster_secret { get; set; }
        public async Task WriteToSQL()
        {
            var sql = Shared.SQLManager;
            Trace.WriteLine("cluster ID:"+cluster_id+",cluster secret"+cluster_secret);
            await sql.CreateTable("Cluster");
            await sql.UpdateValue("Cluster", "ID", cluster_id);
            await sql.UpdateValue("Cluster", "Secret", cluster_secret);
        }
    }
}
