using Audion.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Media;

namespace Audion.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged //the purpose of this model is to be the middleman between the view and the model
    {
        private MediaPlayer mediaPlayer;

        public MainViewModel()
        {
            mediaPlayer = new MediaPlayer();
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            timer.Tick += Timer_Tick;
        }


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

        private bool isPlaying;
        public bool IsPlaying
        {
            get => isPlaying;
            set
            {
                if (isPlaying != value)
                {
                    isPlaying = value;
                    OnPropertyChanged(nameof(IsPlaying));
                    OnPropertyChanged(nameof(PlayPauseIcon));
                }
            }
        }

        private double volume = 0.5;
        public double Volume
        {
            get => volume;
            set
            {
                if (volume != value)
                {
                    volume = value;
                    mediaPlayer.Volume = volume;
                    OnPropertyChanged(nameof(Volume));
                }
            }
        }

        public string PlayPauseIcon => IsPlaying ? "\uE769" : "\uE768"; /* pause : play icon */

        public event PropertyChangedEventHandler? PropertyChanged; //defining the event for the property change

        protected void OnPropertyChanged(string propertyName) //protected means that it can only be accessed within this class
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); //if PropertyChanged is not null, invoke it with the current instance and the property name that has changed
        }

        private Uri? currentUri = null;

        public void PlayCurrentTrack()
        {
            if (SelectedPlaylist?.SelectedTrack != null)
            {

                var newUri = new Uri(SelectedPlaylist.SelectedTrack.FilePath);

                if (currentUri == null || currentUri != newUri)
                {
                    mediaPlayer.Stop(); // Stop any current playback
                    currentUri = newUri;
                    mediaPlayer.Volume = volume; // Ensure volume is up
                    mediaPlayer.MediaOpened += MediaPlayer_MediaOpened; // Subscribe event
                    mediaPlayer.Open(newUri);
                }
                else
                {
                    mediaPlayer.Play();
                    isPlaying = true;
                    timer.Start();
                }
            }
            else
            {
                return;
            }
        }

        private void MediaPlayer_MediaOpened(object? sender, EventArgs e)
        {
            mediaPlayer.MediaOpened -= MediaPlayer_MediaOpened; //unsubscribe event
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                Duration = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            }
            mediaPlayer.Play();
            IsPlaying = true;
            timer.Start();
        }

        private void Timer_Tick(object? slider, EventArgs e)
        {
            if (!isDraggingAudioBar && IsPlaying && mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                Position = mediaPlayer.Position.TotalSeconds;
            }
        }

        public void Pause()
        {
            mediaPlayer.Pause();
            IsPlaying = false;
            timer.Stop();
        }

        public void SetVolume(double volume)
        {
            mediaPlayer.Volume = volume;
        }

        private DispatcherTimer timer;
        private bool isDraggingAudioBar = false;

        private double position;
        public double Position
        {
            get => position;
            set
            {
                if (position != value)
                {
                    position = value;
                    OnPropertyChanged(nameof(Position));
                    OnPropertyChanged(nameof(PositionFormatted));
                }
            }
        }

        private double duration;
        public double Duration
        {
            get => duration;
            set
            {
                if (duration != value)
                {
                    duration = value;
                    OnPropertyChanged(nameof(Duration));
                    OnPropertyChanged(nameof(DurationFormatted));
                }
            }
        }

        public void AudioPosition(double seconds)
        {
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                mediaPlayer.Position = TimeSpan.FromSeconds(seconds); //converts from seconds to timespan
                Position = seconds;  // update for UI
            }
        }

        public void InitiateSlide() //when they left click hold on the audio bar to move it
        {
            isDraggingAudioBar = true;
            timer.Stop();
        }

        public void CeaseSlide() //when they let go
        {
            isDraggingAudioBar = false;
            timer.Start();
        }
        public string PositionFormatted => TimeSpan.FromSeconds(Position).ToString(@"mm\:ss");
        public string DurationFormatted => TimeSpan.FromSeconds(Duration).ToString(@"mm\:ss");

    }
}
