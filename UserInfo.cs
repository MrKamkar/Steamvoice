using Steamworks;
using System.Windows.Media;

namespace Steamvoice
{
    public class UserInfo
    {
        public CSteamID SteamID { get; set; }
        public ImageSource? Avatar { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public bool SelfMuted { get; set; } = false;

        public UserInfo(CSteamID steamID, ImageSource? avatar, string nickname)
        {
            SteamID = steamID;
            Avatar = avatar;
            Nickname = nickname;
        }
    }
}
