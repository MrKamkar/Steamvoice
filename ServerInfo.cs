using Steamworks;
using System.Windows.Media;
using System.Xml.Linq;

namespace Steamvoice
{
    public class ServerInfo
    {
        public CSteamID LobbyID { get; set; }
        public string Name { get; set; } = string.Empty;
        public ImageSource? Avatar { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public int Players { get; set; } = 1;
        public int MaxPlayers { get; set; } = 0;
        public bool Secured { get; set; } = false;
        public string Password { get; set; } = string.Empty;

        public ServerInfo(CSteamID lobbyID, string name, ImageSource? avatar, string nickname)
        {
            LobbyID = lobbyID;
            Name = name;
            Avatar = avatar;
            Nickname = nickname;
        }

        public ServerInfo(CSteamID lobbyID, string name, ImageSource? avatar, string nickname, string password)
        {
            LobbyID = lobbyID;
            Name = name;
            Avatar = avatar;
            Nickname = nickname;
            Secured = true;
            Password = password;
        }
    }
}
