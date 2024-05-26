using System.ComponentModel;
using System.Diagnostics;

namespace Blazor_OpenBMCLAPI.BackEnd
{
    public class ClusterInfo
    {
        public string cluster_id { get; set; }
        public string cluster_secret { get; set; }
    }
    public class UserInfo
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
}
