using AntDesign;
using System.ComponentModel;
using System.Diagnostics;

namespace Blazor_OpenBMCLAPI.BackEnd
{
    public interface ICluster
    {
        public string cluster_id { get; set; }
        public string cluster_secret { get; set; }
    }
    public class ClusterInfo : ICluster
    {
        [DisplayName("Cluster ID")]
        public string cluster_id { get; set; }
        [DisplayName("Cluster Secret")]
        public string cluster_secret { get; set; }
        public string cluster_id_hash { get; set; }
        //[DisplayName("Is Full")]
        //public bool is_full { get; set;}
    }
    public class UserInfo
    {
        public string userName { get; set; }
        public string password { get; set; }
        //public bool is_full { get; set;}
    }
    public interface IProfile
    {
        [DisplayName("Name")]
        public string name { get; set; }
        [DisplayName("Type")]
        public string type { get; set; }
        [DisplayName("Endpoint")]
        public string endpoint { get; set; }
    }
    public class ProfileInfo : IProfile
    {
        [DisplayName("Name")]
        public string name { get; set; }
        [DisplayName("Type")]
        public string type { get; set; }
        [DisplayName("Endpoint")]
        public string endpoint { get; set; }

    }
    public class ProfileType
    {
        public string TypeCode { get; set; }
        public string TypeDisplay { get; set; }
        public ProfileType(string typeCode, string typeDisplay)
        {
            TypeCode = typeCode;
            TypeDisplay = typeDisplay;
        }
    }
}
