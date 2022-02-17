using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Assignment.Entity;
using UWP_Assignment.Service;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MySong : Page
    {
        private SongService songService;

        public MySong()
        {
            this.InitializeComponent();
            this.songService = new SongService();
            Loaded += MySong_Loaded;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            MediaPlayer.MediaPlayer.Pause();
            base.OnNavigatedFrom(e);
        }

        private async void MySong_Loaded(object sender, RoutedEventArgs e)
        {
            List<Song> list = await this.songService.GetMySongAsync();
            ObservableCollection<Song> songs = new ObservableCollection<Song>(list);
            MyListSong.ItemsSource = songs;
        }

        private void MyListSong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSong = MyListSong.SelectedItem as Song;
            MediaPlayer.MediaPlayer.Source = MediaSource.CreateFromUri(new Uri(currentSong.link));
            MediaPlayer.MediaPlayer.Play();
        }
    }
}