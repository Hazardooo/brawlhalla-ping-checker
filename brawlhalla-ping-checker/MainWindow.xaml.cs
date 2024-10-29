using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;

namespace brawlhalla_ping_checker
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> ServersList;

        public MainWindow()
        {
            InitializeComponent();

            ServersList = new Dictionary<string, string>()
            {
                { "US-E", "pingtest-atl.brawlhalla.com"},
                { "US-W", "pingtest-cal.brawlhalla.com"},
                { "EU", "pingtest-ams.brawlhalla.com"},
                { "SEA", "pingtest-sgp.brawlhalla.com"},
                { "AUS", "pingtest-aus.brawlhalla.com"},
                { "BRAZIL", "pingtest-brs.brawlhalla.com"},
                { "JAPAN", "pingtest-jpn.brawlhalla.com"},
                { "MIDDLE EAST", "pingtest-mde.brawlhalla.com"},
                { "SOUTHERN AFRICA", "pingtest-saf.brawlhalla.com"},
            };
        }

        private void OnServerButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                string serverName = button.Content.ToString();
                if (ServersList.ContainsKey(serverName))
                {
                    string serverAddress = ServersList[serverName];
                    PingServer(serverAddress);
                }
            }
        }

        private async void PingServer(string serverAddress)
        {
            ResultLabel.Content = $"RESULT";
            int successCount = 0;
            long totalPingTime = 0;
            long minPing = long.MaxValue;
            long maxPing = long.MinValue;

            try
            {
                using (Ping ping = new Ping())
                {
                    for (int i = 0; i < 100; i++)
                    {
                        PingReply reply = await ping.SendPingAsync(serverAddress, 1000);
                        PingSent.Content = i+1;
                        if (reply.Status == IPStatus.Success)
                        {
                            long pingTime = reply.RoundtripTime;
                            totalPingTime += pingTime;
                            minPing = Math.Min(minPing, pingTime);
                            maxPing = Math.Max(maxPing, pingTime);
                            successCount++;
                        }
                    }

                    double avgPing = successCount > 0 ? (double)totalPingTime / successCount : 0;

                    PingReceived.Content = successCount;
                    PingLost.Content = (100 - successCount) * 100 / 100;
                    MinMs.Content = minPing;
                    MaxMs.Content = maxPing;
                    AvgMs.Content = avgPing;
                    ResultLabel.Content = $"RESULT: Done!";
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Content = $"RESULT: Error pinging server: {ex.Message}";
            }
        }
    }
}
