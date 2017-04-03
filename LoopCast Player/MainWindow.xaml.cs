using System.ComponentModel;
using System.Windows;
using LoopCast_Player.Model.SQL;

namespace LoopCast_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DB.Load();
            InitializeComponent();
        }

        private void ClosingWindow(object sender, CancelEventArgs e)
        {
            DB.Disconnect();
        }
    }
}
