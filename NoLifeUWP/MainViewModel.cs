using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NoLifeUWP
{
    public class MainViewModel : ObservableRecipient
    {
        public ObservableCollection<string> HistoryList { get; set; }
        public string Game { get { return game; } set { SetProperty(ref game, value); } }
        private string game;

        public string Composer { get { return composer; } set { SetProperty(ref composer, value); } } 
        private string composer;

        public string System { get { return system; } set { SetProperty(ref system, value); } }
        private string system;

        private readonly string HistoryUrl = "https://nolife-radio.com/history.txt";
        private DispatcherTimer SongUpdateTimer;
        private HttpClient HistoryClient;

        public MainViewModel()
        {
            HistoryList = new ObservableCollection<string>();
            Game = string.Empty;

            SongUpdateTimer = new DispatcherTimer();
            SongUpdateTimer.Tick += SongUpdateTimer_Tick;
            SongUpdateTimer.Interval = new TimeSpan(0, 0, 2);
            SongUpdateTimer.Start();

            HistoryClient = new HttpClient();
        }

        private async void SongUpdateTimer_Tick(object sender, object e)
        {
            HttpResponseMessage response;

            try
            {
                response = await HistoryClient.GetAsync(HistoryUrl);
            }
            catch (HttpRequestException ex)
            {
                return;
            }
           
            
            string content = await response.Content.ReadAsStringAsync();
            content.Trim();
            string[] songs = content.Split("\n");
            Array.Reverse(songs);

            string songPlaying = string.Empty;
            if (songs.Length > 1)
                songPlaying = songs[1];

            var list = songPlaying.Split("---");
            Game = list[0];
            System = list[1];

            HistoryList.Clear();
            foreach(string song in songs)
            {
                if (!string.IsNullOrEmpty(song))
                    HistoryList.Add(song);
            }
            
        }
    }
}
