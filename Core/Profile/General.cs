namespace Blazor_OpenBMCLAPI.Core.Profile
{
    public class General
    {
        /// <summary>
        /// cluster ID
        /// </summary>
        public string ClusterID { get; set; }
        /// <summary>
        /// cluster secret
        /// </summary>
        public string ClusterSecret { get; set; }
        /// <summary>
        /// 下载线程,不要瞎设置(包括2147483647和-1这种，取值区间1-64)
        /// </summary>
        public string DownloadThreads { get; set; }
    }
}
