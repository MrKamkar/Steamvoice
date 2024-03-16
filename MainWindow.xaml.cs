using Steamworks;
using System.Windows;

namespace Steamvoice
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            if (!SteamAPI.Init()) MessageBox.Show("Steam must be running to use voicechat (SteamAPI_Init() failed).");
            SteamAPI.RestartAppIfNecessary((AppId_t)480);
            NavigateTo("MainMenu");
        }

        public void NavigateTo(string name)
        {
            mainFrame.Navigate(new Uri(name + ".xaml", UriKind.Relative));
        }
    }
}
