using System;
using System.Windows;

namespace LoopCast_Player.Views.Windows
{
    /// <summary>
    /// Interaction logic for AddPodcast.xaml
    /// </summary>
    public partial class AddPodcast : Window
    {
        private Envelope<string> _returner;
        public AddPodcast(Envelope<string> returner)
        {
            InitializeComponent();
            _returner = returner;
            _returner.Item = null;
            this.ShowDialog();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            string url = URL.Text;
            if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                MessageBox.Show("Invalid URL");
            }
            else
            {
                /* Valid */
                _returner.Item = url;
                this.Close();
            }
        }
    }
}
