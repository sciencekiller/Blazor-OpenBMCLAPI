using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Blazor_OpenBMCLAPI.BackEnd.Profile;

namespace Blazor_OpenBMCLAPI.BackEnd
{
    public class ProfileManager
    {
        /// <summary>
        /// 构造函数中获取配置值
        /// </summary>
        public ProfileManager()
        {
            //不存在配置文件就把样例复制(这就是为什么样例不能删) 
            if (!File.Exists(Path.Combine(Shared.rootDirectory, "Config", "config.yml")))
            {
                File.Copy(Path.Combine(Shared.rootDirectory, "Config", "config.sample.yml"), Path.Combine(Shared.rootDirectory, "Config", "config.yml"));
            }
            //读取配置文件
            StreamReader stream = File.OpenText(Path.Combine(Shared.rootDirectory, "Config", "config.yml"));
            string profile = stream.ReadToEnd();
            stream.Close();
            var deserializer = new DeserializerBuilder()
                                   .WithNamingConvention(PascalCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
                                   .Build();
            Shared.profile = deserializer.Deserialize<ProfileInstance>(profile);
        }
        /// <summary>
        /// 删除配置文件（后面估计要验证管理员，现在先把参数留空
        /// </summary>
        public ProfileManager CleanConfig()
        {
            //存在配置文件就删掉
            if (File.Exists(Path.Combine(Shared.rootDirectory, "Config", "config.yml")))
            {
                File.Delete(Path.Combine(Shared.rootDirectory, "Config", "config.yml"));
            }
            return new ProfileManager();
            //我也不知道gc会不会回收这个原来的对象，但是就一个还好吧。。。没人会没事去在一次启动中删除个几十万遍吧。。。
        }
    }
}
