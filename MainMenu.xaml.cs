using Steamworks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Steamvoice
{
    /// <summary>
    /// Logika interakcji dla klasy MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        MainWindow mainWindow;
        private UserInfo userInfo;
        private FileHandler fileHandler = new FileHandler("Steamvoice.cfg");
        private Brush[] buttonColors = { new SolidColorBrush((Color)ColorConverter.ConvertFromString("#66c0f4")), new SolidColorBrush((Color)ColorConverter.ConvertFromString("#171a21")) };
        private bool keyDownHandlerAttached = false;

        SteamVoiceChat steamVoiceChat = new SteamVoiceChat();
        private System.Timers.Timer updateTimer;

        public MainMenu()
        {
            InitializeComponent();

            mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;

            CSteamID steamID = SteamUser.GetSteamID();

            userInfo = new UserInfo(steamID, SteamAvatar.GetImageFromAvatar(steamID), SteamFriends.GetPersonaName());

            Avatar.Source = userInfo.Avatar;
            Nickname.Content = userInfo.Nickname;

            fileHandler.ReadSave();

            SwitchButtons(fileHandler.savedData.InputMode);
            steamVoiceChat.Start();
            Callback.IsChecked = fileHandler.savedData.Callback;
            KeyToTalk.Content = fileHandler.savedData.KeyToTalk.ToString();
            KeyToMute.Content = fileHandler.savedData.KeyToMute.ToString();

            this.KeyDown += MainWindow_KeyHandler;
            this.KeyUp += MainWindow_KeyHandler;
        }

        private void VoiceActivity_Click(object sender, RoutedEventArgs e)
        {
            steamVoiceChat.StartRecording(userInfo.SteamID);
            SwitchButtons(0);
            fileHandler.savedData.InputMode = 0;
            fileHandler.WriteSave();
        }

        private void PushToTalk_Click(object sender, RoutedEventArgs e)
        {
            steamVoiceChat.StopRecording(userInfo.SteamID);
            SwitchButtons(1);
            fileHandler.savedData.InputMode = 1;
            fileHandler.WriteSave();
        }

        private void SwitchButtons(int option)
        {
            switch (option)
            {
                case 0:
                    VoiceActivity.Background = buttonColors[0];
                    PushToTalk.Background = buttonColors[1];
                    KeyToTalkAvailability(false);
                    break;
                case 1:
                    VoiceActivity.Background = buttonColors[1];
                    PushToTalk.Background = buttonColors[0];
                    KeyToTalkAvailability(true);
                    break;
            }
        }

        private void KeyToTalkAvailability(bool usable)
        {
            if (usable)
            {
                KeyToTalk.IsEnabled = true;
                KeyToTalkLabel.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                KeyToTalk.IsEnabled = false;
                KeyToTalkLabel.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }
        void KeyToTalk_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            KeyToTalk.Content = e.Key.ToString();
            this.KeyDown -= KeyToTalk_KeyDown;
            keyDownHandlerAttached = false;
            fileHandler.savedData.KeyToTalk = e.Key;
            fileHandler.WriteSave();
        }

        void KeyToMute_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            KeyToMute.Content = e.Key.ToString();
            this.KeyDown -= KeyToMute_KeyDown;
            keyDownHandlerAttached = false;
            fileHandler.savedData.KeyToMute = e.Key;
            fileHandler.WriteSave();
        }

        void MainWindow_KeyHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == fileHandler.savedData.KeyToTalk && e.IsDown && fileHandler.savedData.InputMode == 1)
            {
                if (!userInfo.SelfMuted) steamVoiceChat.StartRecording(userInfo.SteamID);
            }
            else if (e.Key == fileHandler.savedData.KeyToTalk && e.IsUp && fileHandler.savedData.InputMode == 1)
            {
                steamVoiceChat.StopRecording(userInfo.SteamID);
            }
            else if (e.Key == fileHandler.savedData.KeyToMute && e.IsDown)
            {
                userInfo.SelfMuted = !userInfo.SelfMuted;
                if (userInfo.SelfMuted) steamVoiceChat.StopRecording(userInfo.SteamID);
                else if (fileHandler.savedData.InputMode == 0) steamVoiceChat.StartRecording(userInfo.SteamID);
            }
        }

        private void KeyToTalk_Click(object sender, RoutedEventArgs e)
        {
            if (!keyDownHandlerAttached)
            {
                KeyToTalk.Content = "...";
                this.PreviewKeyDown += KeyToTalk_KeyDown;
                keyDownHandlerAttached = true;
            }
        }
        private void KeyToMute_Click(object sender, RoutedEventArgs e)
        {
            if (!keyDownHandlerAttached)
            {
                KeyToMute.Content = "...";
                this.KeyDown += KeyToMute_KeyDown;
                keyDownHandlerAttached = true;
            }
        }

        private void Callback_Click(object sender, RoutedEventArgs e)
        {
            fileHandler.savedData.Callback = Callback.IsChecked ?? false;
            fileHandler.WriteSave();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.NavigateTo("Browser");
        }
    }
}
