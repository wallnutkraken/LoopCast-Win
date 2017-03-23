using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LoopCast_Player.Model;
using LoopCast_Player.Views.Windows;

namespace LoopCast_Player.Views.Pages
{
    /// <summary>
    /// Interaction logic for PlayerPage.xaml
    /// </summary>
    public partial class PlayerPage : Page
    {
        private ObservableCollection<Feed> feeds;
        private ObservableCollection<Podcast> _podcasts = new ObservableCollection<Podcast>();

        public PlayerPage()
        {
            InitializeComponent();
            feeds = new ObservableCollection<Feed>();

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
    }
}
