using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_Assignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("latestSong", typeof(Pages.LatestSong)),
            ("yourSong", typeof(Pages.MySong)),
            ("uploadSong", typeof(Pages.UploadSong)),
            ("register", typeof(Pages.RegisterPage)),
            ("login", typeof(Pages.LoginPage)),
            ("uploadMySong", typeof(Pages.UploadMySong))
        };

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(typeof(Pages.LatestSong));
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                MainContent.Navigate(typeof(Pages.SettingPage));
            }
            else
            {
                var selectedItem = sender.SelectedItem as NavigationViewItem;
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(selectedItem.Tag));
                MainContent.Navigate(item.Page);
            }
        }
    }
}