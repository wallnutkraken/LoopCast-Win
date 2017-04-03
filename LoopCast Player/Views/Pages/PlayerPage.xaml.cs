using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LoopCast_Player.Model;
using LoopCast_Player.Model.SQL;
using LoopCast_Player.Views.Windows;

namespace LoopCast_Player.Views.Pages
{
    /// <summary>
    /// Interaction logic for PlayerPage.xaml
    /// </summary>
    public partial class PlayerPage : Page
    {
        private ObservableCollection<Feed> feeds;
        private ObservableCollection<PodcastInfo> _podcasts = new ObservableCollection<PodcastInfo>();

        public PlayerPage()
        {
            InitializeComponent();
            feeds = new ObservableCollection<Feed>(DB.GetFeeds());

            EpisodeSelection.ItemsSource = _podcasts;
        }

        private void ChangePodcast_Click(object sender, RoutedEventArgs e)
        {
            Envelope<Feed> selected = new Envelope<Feed>();
            SelectPodcasts select = new SelectPodcasts(feeds, selected);
            select.Closed += (s, a) =>
            {
                if (selected.Item != null)
                {
                    _podcasts.Clear();
                    selected.Item.Podcasts.ForEach(p => _podcasts.Add(p));
                }
            };
        }

        private void EpisodeChanged(object sender, SelectionChangedEventArgs e)
        {
            object selected = (sender as ComboBox)?.SelectedItem;
            if (selected != null)
            {
                Player.SetPodcast((PodcastInfo)selected);
            }
        }

        private void PrevPodcast(object sender, RoutedEventArgs args)
        {
            if (EpisodeSelection.SelectedIndex < _podcasts.Count - 1 && EpisodeSelection.SelectedIndex > 0)
            {
                EpisodeSelection.SelectedIndex++;
            }
        }

        private void NextPodcast(object sender, RoutedEventArgs args)
        {
            if (EpisodeSelection.SelectedIndex > 0)
            {
                EpisodeSelection.SelectedIndex--;
            }
        }
    }
}
