﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        public string CurrentSong { get { return currentSong; } set { SetProperty(ref currentSong, value); } }
        private string currentSong;

        private readonly string HistoryUrl = "https://nolife-radio.com/history.txt";
        private DispatcherTimer SongUpdateTimer;
        private HttpClient HistoryClient;

        public MainViewModel()
        {
            HistoryList = new ObservableCollection<string>();
            CurrentSong = string.Empty;

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

            if (songs.Length > 1)
                CurrentSong = songs[1];

            HistoryList.Clear();
            foreach(string song in songs)
            {
                if (!string.IsNullOrEmpty(song))
                    HistoryList.Add(song);
            }
            
        }
    }
}
