using Audion.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Audion.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged //the purpose of this model is to be the middleman between the view and the model
    {
        public ObservableCollection<Playlist> Playlists { get; set; } = new ObservableCollection<Playlist>(); //collection of playlists that will be displayed in the UI    

        //basically if the playlist they clicked on it not equal to the one that is currently selected, then it will change the selected playlist to the one they clicked on 

        private Playlist? selectedPlaylist; //the currently selected playlist in the UI
        public Playlist? SelectedPlaylist
        {
            get => selectedPlaylist; //fetch current value of selected playlist
            set
            {
                if (selectedPlaylist != value) //if current set value is not equal to the new value
                {
                    selectedPlaylist = value; //change the already set value to the new value
                    OnPropertyChanged(nameof(SelectedPlaylist)); //notify the UI that the SelectedPlaylist property has changed
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged; //defining the event for the property change

        protected void OnPropertyChanged(string propertyName) //protected means that it can only be accessed within this class
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); //if PropertyChanged is not null, invoke it with the current instance and the property name that has changed
        }

    }
}
