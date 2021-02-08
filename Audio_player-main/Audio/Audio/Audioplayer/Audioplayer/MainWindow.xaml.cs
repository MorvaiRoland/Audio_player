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
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.IO;

namespace Audioplayer
{

    public partial class MainWindow : Window
    {

        readonly MediaPlayer mediaPlayer = new MediaPlayer();
        readonly DispatcherTimer timer = new DispatcherTimer();
        string fajlnev;
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;

        int positionSliderIsMoving = 0;
        int isPlaying = 0;
        int playing = -1;
        int repeatType = 0;

        public MainWindow()
        {
            InitializeComponent();
            MessageBox.Show(" Üdvözöljük a legjobb zenelejátszóban!\n Először kattintson a betöltés gombra, utánna mehet a buli! ");

        }


        private void lejatszas_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
            sliProgress.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            sliProgress.Value = mediaPlayer.Position.TotalSeconds;
            if (sliProgress.Value == mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds)
            {
                timer.Stop();
                sliProgress.Value = 0;
            }
        }

        private void szunet_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
        }




        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)volumeSlider.Value;
        }



        private void VolumeSlider_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            mediaPlayer.Volume = volumeSlider.Value;
            VolumeLabel.Content = Math.Round((volumeSlider.Value * 100), 0) + "%";
        }
        private void AddSongsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "MP3 files (*.mp3)|*.mp3",
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    if (!SongsListBox.Items.Contains(file))
                    {
                        SongsListBox.Items.Add(file);
                    }
                }
            }
        }

        private void SongsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
            mediaPlayer.Open(new Uri(file));
            mediaPlayer.Play();
            isPlaying = 2;
            playing = Convert.ToInt32(SongsListBox.SelectedIndex);
            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Start();
            sliProgress.IsEnabled = true;

        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                if (isPlaying == 0)
                {
                    if (SongsListBox.SelectedIndex != -1)
                    {
                        string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                        mediaPlayer.Open(new Uri(file));
                        mediaPlayer.Play();
                        isPlaying = 2;
                        playing = Convert.ToInt32(SongsListBox.SelectedIndex);
                        timer.Interval = TimeSpan.FromMilliseconds(200);
                        timer.Tick += timer_Tick;
                        timer.Start();
                        sliProgress.IsEnabled = true;

                    }
                    else
                    {
                        SongsListBox.SelectedIndex = 0;
                        string file = SongsListBox.Items[0].ToString();
                        mediaPlayer.Open(new Uri(file));
                        mediaPlayer.Play();
                        isPlaying = 2;
                        playing = 0;
                        timer.Interval = TimeSpan.FromSeconds(1);
                        timer.Tick += timer_Tick;
                        timer.Start();
                        sliProgress.IsEnabled = true;



                    }
                }
            }
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {

            playing++;
            SongsListBox.SelectedIndex = playing;
            string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
            mediaPlayer.Open(new Uri(file));
            mediaPlayer.Play();
            isPlaying = 2;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            sliProgress.IsEnabled = true;

        }

        private void elozo(object sender, RoutedEventArgs e)
        {

            playing=playing - 1;
            SongsListBox.SelectedIndex = playing;
            string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
            mediaPlayer.Open(new Uri(file));
            mediaPlayer.Play();
            isPlaying = 2;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            sliProgress.IsEnabled = true;

        }

        private void LoadPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt",
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    if (!SongsListBox.Items.Contains(file))
                    {
                        SongsListBox.Items.Add(file);
                    }
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                MessageBoxResult mbr = MessageBox.Show(" Biztos hogy törlöd a listád tartalmát?", "Lista törlése", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (mbr == MessageBoxResult.Yes)
                {
                    SongsListBox.Items.Clear();
                    SongsListBox.SelectedIndex = -1;
                    mediaPlayer.Close();
                    timer.Stop();
                    isPlaying = 0;
                    playing = -1;
                    sliProgress.Value = 0;
                    sliProgress.Maximum = 1;
                    sliProgress.IsEnabled = false;
                    PositionLabel.Content = "00:00/00:00";
                }
            }
            else
            {
                MessageBox.Show("Listád üres.", "Üres a lista", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RemoveSongButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count == 0)
            {
                MessageBox.Show("Nincs mit törölni.", "Üres", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (SongsListBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Válassz ki egy zenét.", "Zene választás", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (SongsListBox.SelectedIndex == playing)
                    {
                        SongsListBox.Items.Remove(SongsListBox.SelectedItem);
                        SongsListBox.SelectedIndex = -1;
                        mediaPlayer.Close();
                        timer.Stop();
                        isPlaying = 0;
                        playing = -1;
                        sliProgress.Value = 0;
                        sliProgress.Maximum = 1;
                        PositionLabel.Content = "00:00/00:00";
                        sliProgress.IsEnabled = false;
                    }
                    else
                    {
                        if (SongsListBox.SelectedIndex > playing)
                        {
                            SongsListBox.Items.Remove(SongsListBox.SelectedItem);
                            SongsListBox.SelectedIndex = playing;
                        }
                        else
                        {
                            playing--;
                            SongsListBox.Items.Remove(SongsListBox.SelectedItem);
                            SongsListBox.SelectedIndex = playing;
                        }
                    }
                }
            }
        }

        private void SavePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count == 0)
            {
                MessageBox.Show("Nincs mit menteni.", "Üres", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
               
                    SaveFileDialog sf = new SaveFileDialog
                    {
                        Filter = "Text files (*.txt)|*.txt"
                    };
                    sf.ShowDialog();
                    StreamWriter sw = new StreamWriter(sf.FileName);
                    sw.WriteLine("LEGJOBB LEJÁTSZÓ.");
                    for (int i = 0; i < SongsListBox.Items.Count; i++)
                    {
                        sw.WriteLine(SongsListBox.Items[i].ToString());
                    }
                    sw.Close();
                    MessageBox.Show("Sikeresen mentetted a listádat.", "ELMENTVE", MessageBoxButton.OK, MessageBoxImage.Information);
                
              

            }
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (repeatType == 0)
            {
                repeatType = 1;
            }
            else if (repeatType == 1)
            {
                repeatType = 2;
                Repeat.Content = "BE";
            }
            else
            {
                repeatType = 0;
                Repeat.Content = "KI";

            }
        }
    }
}

  
    

