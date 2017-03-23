using System.ComponentModel;
using System.Windows;

namespace LoopCast_Player.Views.Windows
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : Window
    {
        private bool isClosed = false;
        public bool Closed => isClosed;
        public Notification(string text)
        {
            InitializeComponent();
            Body.Text = text;
            
            this.Show();
            Closing += (sender, args) => isClosed = true;
        }

        public Notification(string text, CancelEventHandler onClosing) : this(text)
        {
            Closing += onClosing;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
