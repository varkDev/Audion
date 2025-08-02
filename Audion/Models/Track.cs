using System;
using System.ComponentModel;
using System.IO;

namespace Audion.Models
{
    public class Track : INotifyPropertyChanged
    {
        private string filePath;
        private string title;

        public string FilePath
        {
            get => filePath; //fetch current value of filepath
            set
            {
                if (filePath != value) //if current set value is not equal to the new value
                {
                    filePath = value; //change the already set value to the new value
                    OnPropertyChanged(nameof(FilePath)); //notify the UI that the FilePath property has changed
                }
            }
        }

        public string Title
        {
            get => title; //fetch current value of title
            set
            {
                if (title != value) //if current set value is not equal to the new value
                {
                    title = value; //change the already set value to the new value
                    OnPropertyChanged(nameof(Title)); //notify the UI that the Title property has changed
                }
            }
        }

        public Track(string filePath) //on initialization
        {
            FilePath = filePath; //setting the dynamic value to the fixed property
            Title = Path.GetFileNameWithoutExtension(filePath); //retrieving the file name without the extension
        }

        public event PropertyChangedEventHandler? PropertyChanged; //defining the event for the property change

        protected void OnPropertyChanged(string propertyName) //protected means that it can only be accessed within this class
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); //if PropertyChanged is not null, invoke it with the current instance and the property name that has changed
        }

    }
}