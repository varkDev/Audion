using Audion.Models;
using Audion.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace Audion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void AddPlaylistButton_Click(object button, RoutedEventArgs metaInformation)
        {
            var folderDialog = new FolderBrowserDialog
            {
                Description = "Add playlist folder",
                ShowNewFolderButton = false,
            };

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var folderPath = folderDialog.SelectedPath;

                if (DataContext is MainViewModel vm)
                {
                    if (!vm.Playlists.Any(p => p.Name == System.IO.Path.GetFileName(folderPath)))
                    {
                        var newPlaylist = new Playlist(folderPath);
                        vm.Playlists.Add(newPlaylist);
                        vm.SelectedPlaylist = newPlaylist;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("This playlist already exists.", "Duplicate playlist", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void DeletePlaylistButton_Click(object button, RoutedEventArgs metaInformation)
        {
            if (DataContext is MainViewModel vm)
            {
                var selectedPlaylist = vm.SelectedPlaylist;
                if (selectedPlaylist != null) 
                {
                    vm.Playlists.Remove(selectedPlaylist);
                    vm.SelectedPlaylist = null;
                }
                else
                {
                    System.Windows.MessageBox.Show("No playlist selected.", "Delete Playlist", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void RefreshButton_Click(object button, RoutedEventArgs metaInformation)
        {
            if (DataContext is MainViewModel vm)
            {
                if (vm.Playlists.Count == 0)
                {
                    System.Windows.MessageBox.Show("You have no playlists added.", "Unsuccessful", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    foreach (var playlist in vm.Playlists)
                    {
                        playlist.Refresh();
                    }

                    System.Windows.MessageBox.Show("Playlists have been refreshed!", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void FTraverse(object button, RoutedEventArgs metaInformation)
        {
            if (DataContext is MainViewModel vm && vm.SelectedPlaylist != null)
            {
                vm.NextTrack();
                vm.IsLooping = false;
            }
        }

        private void BTraverse(object button, RoutedEventArgs metaInformation)
        {
            if (DataContext is MainViewModel vm && vm.SelectedPlaylist != null)
            {
                vm.PreviousTrack();
                vm.IsLooping = false;
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                if (vm.IsPlaying)
                {
                    vm.Pause();
                }
                else
                {
                    vm.PlayCurrentTrack();
                }
            }
        }

        private void VolumeSlider_ValueChanged(object slider, RoutedPropertyChangedEventArgs<double> e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.SetVolume(e.NewValue);
            }
        }

        private void AudioBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.InitiateSlide();
            }
        }

        private void AudioBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Slider slider && DataContext is MainViewModel vm)
            {
                vm.AudioPosition(slider.Value);
                vm.CeaseSlide();
            }
        }

        private void Looping(object button, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.ToggleLoop();
            }
        }

    }
}
