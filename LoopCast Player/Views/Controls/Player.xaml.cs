using System.Windows;
using System.Windows.Controls;
using LoopCast_Player.Model;

namespace LoopCast_Player.Views.Controls
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        private Podcast _currentPodcast;
        public Player()
        {
            InitializeComponent();
        }

        public void SetPodcast(Podcast podcast)
        {
            podcast.ConnectStream();
            podcast.StartPlayer();

            FileName.Content = podcast.Name;
            _currentPodcast = podcast;
        }

        public Player(Podcast podcast) : this()
        {
            SetPodcast(podcast);
        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            /* Comparing true since ?. returns Nullable<bool> */
            if (_currentPodcast?.IsPlaying == true)
            {
                _currentPodcast.Pause();
            }
            else
            {
                _currentPodcast?.Play();
            }
        }
    }
}
