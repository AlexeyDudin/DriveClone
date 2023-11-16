using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Domain
{
    public class Settings : INotifyPropertyChanged
    {
        private string sourcePath = "";
        private string destPath = "";

        public string SourcePath 
        { 
            get => sourcePath;
            set
            {
                sourcePath = value;
                OnPropertyChanged(nameof(SourcePath));
            }
        }
        public string DestPath 
        {
            get => destPath;
            set
            {
                destPath = value;
                OnPropertyChanged(nameof(DestPath));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public Settings Clone()
        {
            return new Settings()
            {
                SourcePath = SourcePath,
                DestPath = DestPath
            };
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
