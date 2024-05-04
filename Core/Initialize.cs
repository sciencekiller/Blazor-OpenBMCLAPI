using System.Text;
using static Blazor_OpenBMCLAPI.Core.Enums;

namespace Blazor_OpenBMCLAPI.Core
{
    public static class Initialize
    {
        public static async Task Run()
        {
            Shared.rootDirectory = Directory.GetCurrentDirectory();
            Shared.profileManager = new ProfileManager();
            Statistics.status = new DisplayStatus(Status.Syncing);
        }
    }
}
