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
                switch (_status)
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
                if (_status != Status.Syncing)
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
                if (_status == Status.Syncing)
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
                switch (_status)
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
        private Status? _status;
        public Status? status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                switch (value)
                {
                    case Status.Online:
                        _icon_type = "check-circle";
                        break;
                    case Status.Offline:
                        _icon_type = "close-circle";
                        break;
                    case Status.Syncing:
                        _icon_type = "sync";
                        break;
                    default:
                        _icon_type = "question-circle";
                        break;
                }
            }
        }
        private string? _icon_type;
        public string? iconType
        {
            get
            {
                return _icon_type;
            }
            set
            {
                _icon_type = value;
                switch (value)
                {
                    case "check-circle":
                        _status = Status.Online;
                        break;
                    case "close-circle":
                        _status = Status.Offline;
                        break;
                    case "sync":
                        _status = Status.Syncing;
                        break;
                    default:
                        _status = Status.Unknown;
                        break;
                }
            }
        }
    }
}
