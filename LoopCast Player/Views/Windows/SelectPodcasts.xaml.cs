using System.Collections.ObjectModel;
using System.Windows;
using LoopCast_Player.Model;
using LoopCast_Player.Model.SQL;

namespace LoopCast_Player.Views.Windows
{
    /// <summary>
    /// Interaction logic for SelectPodcasts.xaml
    /// </summary>
    public partial class SelectPodcasts : Window
    {
        private ObservableCollection<Feed> _feeds;
        private Envelope<Feed> _selectedFeed;
        public SelectPodcasts(ObservableCollection<Feed> feedsLoaded, Envelope<Feed> selectedFeed)
        {
            InitializeComponent();
            _feeds = feedsLoaded;
            Podcasts.ItemsSource = feedsLoaded;
            _selectedFeed = selectedFeed;

            this.Show();
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            if (Podcasts.SelectedItems.Count != 0)
            {
                /* Get actual feed */
                Feed removableFeed = (Feed) Podcasts.SelectedItem;

                /* Change */
                _feeds.RemoveAt(Podcasts.SelectedIndex);
                
                /* Remove from DB */
                DB.Remove(removableFeed);
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            Envelope<string> returner = new Envelope<string>();
            new AddPodcast(returner);
            if (returner.Item != null)
            {
                Feed feed = new Feed(returner.Item);
                _feeds.Add(feed);
                DB.AddFeed(feed);

                Podcasts.SelectedIndex = _feeds.Count - 1;
            }
        }

        private void Select(object sender, RoutedEventArgs e)
        {
            if (Podcasts.SelectedItems.Count != 0)
            {
                _selectedFeed.Item = (Feed)Podcasts.SelectedItem;
            }
            this.Close();
        }
    }
}
