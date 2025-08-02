using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace Audion.Models
{
    public class Playlist : INotifyPropertyChanged
    {
        private string name;
        private ObservableCollection<Track> tracks = new();

        public string Name
        {
            get => name; //fetch current value of name
            set
            {
                if (name != value) //if current set value is not equal to the new value
                {
                    name = value; //change the already set value to the new value
                    OnPropertyChanged(nameof(Name)); //notify the UI that the Name property has changed
                }
            }
        }

        public ObservableCollection<Track> Tracks
        {
            get => tracks; //fetch current value of tracks
            set
            {
                if (tracks != value) //if current set value is not equal to the new value
                {
                    tracks = value; //change the already set value to the new value
                    OnPropertyChanged(nameof(Tracks)); //notify the UI that the Tracks property has changed
                }
            }
        }

        public Playlist(string folderPath) //on initialization
        {
            Name = Path.GetFileName(folderPath); //setting the name of the playlist to the folder name
            LoadTracks(folderPath); //loading the tracks from the specified folder via call
        }

        private void LoadTracks(string folderPath) //on call
        {
            var mp3Files = Directory.GetFiles(folderPath, "*.mp3"); //finding all mp3 files within the given folder directory
            foreach (var file in mp3Files)
            {
                Tracks.Add(new Track(file)); //adding each track to the tracks collection
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged; //defining the event for the property change

        protected void OnPropertyChanged(string propertyName) //protected means that it can only be accessed within this class
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); //if PropertyChanged is not null, invoke it with the current instance and the property name that has changed
        }

    }
}