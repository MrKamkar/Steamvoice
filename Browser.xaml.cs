using Steamworks;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Steamvoice
{
    public partial class Browser : Page
    {
        private ObservableCollection<ServerInfo> servers = new();

        public Browser()
        {
            InitializeComponent();
            servers.Add(new ServerInfo(new CSteamID(), "Mój serwer", SteamAvatar.GetImageFromAvatar(SteamUser.GetSteamID()), SteamFriends.GetPersonaName()));
            serverListView.ItemsSource = servers;
        }

        class CLobbyListManager
        {
            private CallResult<LobbyMatchList_t> m_CallResultLobbyMatchList;
            void FindLobbies()
            {
                // SteamMatchmaking()->AddRequestLobbyListFilter*() functions would be called here, before RequestLobbyList()
                SteamAPICall_t hSteamAPICall = SteamMatchmaking.RequestLobbyList();
                m_CallResultLobbyMatchList.Set(OnLobbyMatchList);
            }

            void OnLobbyMatchList(LobbyMatchList_t* pLobbyMatchList, bool bIOFailure)
            {
                // lobby list has been retrieved from Steam back-end, use results
            }
        }
    }
}
