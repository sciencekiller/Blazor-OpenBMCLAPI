using Blazor_OpenBMCLAPI.Pages;
using Microsoft.Extensions.DependencyInjection;
using static Blazor_OpenBMCLAPI.BackEnd.Enums;


namespace Blazor_OpenBMCLAPI.BackEnd
{
    public class DisplayStatus
    {
        public DisplayStatus(Status st)
        {
            status = st;
        }
        public string statusWord
        {
            get
            {
                switch (status)
                {
                    case Status.Offline:
                        return "offline";
                    case Status.Online:
                        return "online";
                    case Status.Syncing:
                        return "syncing";
                    default:
                        return "unknown";
                }
            }
        }
        public string theme
        {
            get
            {
                if (status != Status.Syncing)
                {
                    return "twotone";
                }
                else
                {
                    return "outline";
                }
            }
        }
        public bool spin
        {
            get
            {
                if (status == Status.Syncing)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string? color
        {
            get
            {
                switch (status)
                {
                    case Status.Syncing:
                        return "#ffff00";
                    case Status.Offline:
                        return "#ff0000";
                    case Status.Online:
                        return "#00ff00";
                    default:
                        return "#ffffff";
                }
            }
        }
        public Status? status { get; set; }
        public string? iconType
        {
            get
            {
                switch (status)
                {
                    case Status.Syncing:
                        return "sync";
                    case Status.Offline:
                        return "close-circle";
                    case Status.Online:
                        return "check-circle";
                    default:
                        return "question-circle";
                }
            }
            /*set
            {
                _icon_type = value;
                switch (value)
                {
                    case "check-circle":
                        status = Status.Online;
                        break;
                    case "close-circle":
                        status = Status.Offline;
                        break;
                    case "sync":
                        status = Status.Syncing;
                        break;
                    default:
                        status = Status.Unknown;
                        break;
                }
            }*/
        }
    }
}
