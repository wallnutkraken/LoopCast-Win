using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using LoopCast_Player.Model;

namespace LoopCast_Player.Views.Controls
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : UserControl, IDisposable
    {
        private Podcast _currentPodcast;
        private string _length;
        private DispatcherTimer _elapsedTimer;

        private Task _connectPodcastTask;

        public Player()
        {
            InitializeComponent();
        }

        public void SetPodcast(PodcastInfo podcastDefinition)
        {
            /* So we can free memory by disposing of it here later (the only reference) */
            Podcast podcast = new Podcast(podcastDefinition.URL, podcastDefinition.Name);
            _elapsedTimer?.Stop();
            PlayPause.IsEnabled = false;
            try
            {
                _currentPodcast?.Dispose();
                _currentPodcast?.Stop();

                _connectPodcastTask.Dispose();
            }
            catch { }
            _currentPodcast = null;
            PlayPause.IsEnabled = true;

            FileName.Content = "Loading stream...";

            _connectPodcastTask = new Task(() =>
            {
                podcast.ConnectStream();
                podcast.StartPlayer();
            });

            _connectPodcastTask.GetAwaiter().OnCompleted(() =>
            {
                FileName.Content = podcast.Name;
                _currentPodcast = podcast;
                _length = _currentPodcast.Length.ToString(@"hh\:mm\:ss");

                PlayPause_Click(null, null);

                /* Start timer */
                _elapsedTimer = new DispatcherTimer();
                _elapsedTimer.Interval = TimeSpan.FromMilliseconds(500);
                _elapsedTimer.Tick += UpdatePlayTime;
                _elapsedTimer.Start();
            });
            _connectPodcastTask.Start();
        }

        public Player(PodcastInfo podcast) : this()
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
                UpdatePlayTime(null, null);
            }
        }

        private void ForwardTime(object sender, RoutedEventArgs e)
        {
            _currentPodcast?.Forward(TimeSpan.FromSeconds(10));
        }

        private void ReverseTime(object sender, RoutedEventArgs e)
        {
            _currentPodcast?.Reverse(TimeSpan.FromSeconds(10));
        }

        private void UpdatePlayTime(object sender, EventArgs args)
        {
            if (_currentPodcast == null)
                return;
            Time.Content = $"{_currentPodcast.CurrentTime}/{_length}";
            Elapsed.Value = _currentPodcast.PercentElapsed;
        }

        public void Dispose()
        {
            try
            {
                _connectPodcastTask.Dispose();
            }
            catch { }
            _currentPodcast?.Dispose();
        }
    }
}
