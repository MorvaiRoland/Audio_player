using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Audioplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        MediaPlayer mediaPlayer = new MediaPlayer();
        string fajlnev;
        private void betoltes_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog filedialog = new OpenFileDialog
            {
                Multiselect = false,
                DefaultExt = ".mp3"

            };
            bool? dialogOk = filedialog.ShowDialog();
            if(dialogOk==true)
            {

                fajlnev = filedialog.FileName;
                Box.Text = filedialog.SafeFileName;
                mediaPlayer.Open(new Uri(fajlnev));
            }
        }

        private void lejatszas_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }

        private void szunet_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }
    }
}
