using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LoopCast_Player.Model;
using LoopCast_Player.Views.Windows;

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
            try
            {
                podcast?.Stop();
            }
            catch { }
            Notification notification = new Notification("Downloading episode");

            Task t = new Task(() =>
            {
                podcast.ConnectStream();
                podcast.StartPlayer();
            });

            t.GetAwaiter().OnCompleted(() =>
            {
                FileName.Content = podcast.Name;
                _currentPodcast = podcast;

                if (!notification.Closed)
                    notification.Close();
                _currentPodcast.Play();
            });
            t.Start();
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
