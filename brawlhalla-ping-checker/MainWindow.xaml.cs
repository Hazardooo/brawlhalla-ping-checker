using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            // Получаем текст кнопки, чтобы определить, какой сервер нужно пинговать
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

        private void PingServer(string serverAddress)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(serverAddress);
                    if (reply.Status == IPStatus.Success)
                    {
                        MessageBox.Show($"Ping to {serverAddress} successful.\nTime: {reply.RoundtripTime} ms", "Ping Result", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Ping to {serverAddress} failed.", "Ping Result", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error pinging server: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
