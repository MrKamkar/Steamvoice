using Steamworks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace Steamvoice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<UserInfo> userInfos = new ObservableCollection<UserInfo>();

        SteamVoiceChat steamVoiceChat = new SteamVoiceChat();
        private CancellationTokenSource cancellationTokenSource;
        public MainWindow()
        {
            InitializeComponent();
            if (!SteamAPI.Init()) MessageBox.Show("Steam must be running to use voicechat (SteamAPI_Init() failed).");
            SteamAPI.RestartAppIfNecessary((AppId_t)480);

            CSteamID steamID = SteamUser.GetSteamID();

            steamVoiceChat.steamID = steamID;
            steamVoiceChat.Start();
            SteamFriends.SetInGameVoiceSpeaking(steamID, true);

            UserInfo userInfo = new UserInfo(steamID, SteamAvatar.GetImageFromAvatar(steamID), SteamFriends.GetPersonaName());
            userInfos.Add(userInfo);

            //LobbyListView.ItemsSource = userInfos;

            DispatcherTimer timer = new DispatcherTimer();
            cancellationTokenSource = new CancellationTokenSource();

            // Start the asynchronous thread
            Task.Run(() => YourWhileLoopMethod(cancellationTokenSource.Token), cancellationTokenSource.Token);
        }

        private void YourWhileLoopMethod(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                steamVoiceChat.Update();
            }
        }

        private async Task TimerTickAsync()
        {
            await Task.Run(() =>
            {
                steamVoiceChat.Update();
                SteamAPI.RunCallbacks();
            });
        }
    }
}