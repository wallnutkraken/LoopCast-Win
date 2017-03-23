using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
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
        private string _length;
        private DispatcherTimer _elapsedTimer;

        public Player()
        {
            InitializeComponent();
        }

        public void SetPodcast(Podcast podcast)
        {
            _elapsedTimer?.Stop();
            try
            {
                podcast?.Stop();
            }
            catch { }
            FileName.Content = "Loading stream...";

            Task t = new Task(() =>
            {
                podcast.ConnectStream();
                podcast.StartPlayer();
            });

            t.GetAwaiter().OnCompleted(() =>
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
            Time.Content = $"{_currentPodcast?.CurrentTime}/{_length}";
        }
    }
}
