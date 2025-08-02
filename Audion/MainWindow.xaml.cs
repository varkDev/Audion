using System.Windows;
using Audion.ViewModel;
using System.Windows.Forms;
using Audion.Models;

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

    }
}