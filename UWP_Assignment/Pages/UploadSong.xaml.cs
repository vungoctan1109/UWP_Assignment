using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using UWP_Assignment.Entity;
using UWP_Assignment.Service;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class UploadSong : Page
    {
        private SongService songService = new SongService();

        public UploadSong()
        {
            this.InitializeComponent();
        }

        private async void Handle_Submit(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SongUpload songUpload = new SongUpload
            {
                name = txtName.Text,
                singer = txtSinger.Text,
                author = txtAuthor.Text,
                description = txtDescription.Text,
                thumbnail = txtThumbnail.Text,
                link = txtLink.Text
            };
            SortedList<string, string> errorMessage = Validate_Song(songUpload);
            msgName.Text = errorMessage.ContainsKey("nameError") ? errorMessage["nameError"] : "";
            msgSinger.Text = errorMessage.ContainsKey("singerError") ? errorMessage["singerError"] : "";
            msgAuthor.Text = errorMessage.ContainsKey("authorError") ? errorMessage["authorError"] : "";
            msgDescription.Text = errorMessage.ContainsKey("descriptionError") ? errorMessage["descriptionError"] : "";
            msgThumbnail.Text = errorMessage.ContainsKey("thumbnailError") ? errorMessage["thumbnailError"] : "";
            msgLink.Text = errorMessage.ContainsKey("linkError") ? errorMessage["linkError"] : "";
            if (errorMessage.Count == 0)
            {
                var result = await songService.CreatSongAsync(songUpload);
                ContentDialog contentDialog = new ContentDialog();
                if (result)
                {
                    contentDialog.Title = "Action success.";
                    contentDialog.Content = "Your song has been upload";
                    Frame.Navigate(typeof(LatestSong));
                }
                else
                {
                    contentDialog.Title = "Action failed.";
                    contentDialog.Content = "Please try again.";
                }
                contentDialog.CloseButtonText = "Ok";
                await contentDialog.ShowAsync();
            }
        }

        private SortedList<string, string> Validate_Song(SongUpload songUpload)
        {
            SortedList<string, string> errorMessage = new SortedList<string, string>();
            string linkPattern = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";
            if (string.IsNullOrEmpty(songUpload.name))
            {
                errorMessage.Add("nameError", "Song's name is required.");
            }
            if (string.IsNullOrEmpty(songUpload.singer))
            {
                errorMessage.Add("singerError", "Singer is required.");
            }
            if (string.IsNullOrEmpty(songUpload.author))
            {
                errorMessage.Add("authorError", "Author is required.");
            }
            if (string.IsNullOrEmpty(songUpload.description))
            {
                errorMessage.Add("descriptionError", "Description is required.");
            }
            if (!Regex.IsMatch(songUpload.thumbnail, linkPattern))
            {
                errorMessage.Add("thumbnailError", "Invalid thumbnail.");
            }
            if (!Regex.IsMatch(songUpload.link, linkPattern))
            {
                errorMessage.Add("linkError", "Invalid link.");
            }

            return errorMessage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txtName.Text = "";
            txtSinger.Text = "";
            txtAuthor.Text = "";
            txtLink.Text = "";
            txtThumbnail.Text = "";
            txtDescription.Text = "";
        }
    }
}