using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

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

    public event PropertyChangedEventHandler? PropertyChanged; //defining the event for the property change

    protected void OnPropertyChanged(string propertyName) //protected means that it can only be accessed within this class
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); //if PropertyChanged is not null, invoke it with the current instance and the property name that has changed
    }

}