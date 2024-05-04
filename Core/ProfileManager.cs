using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Blazor_OpenBMCLAPI.Core.Profile;

namespace Blazor_OpenBMCLAPI.Core
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
            var deserializer = new DeserializerBuilder()
                                   .WithNamingConvention(PascalCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
                                   .Build();
            Shared.profile = deserializer.Deserialize<ProfileInstance>(profile);
        }
    }
}
