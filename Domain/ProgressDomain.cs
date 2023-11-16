
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Domain
{
    public class ProgressDomain: INotifyPropertyChanged
    {
        private ulong maxElements = 0;
        private ulong currentElementVolume = 0;
        private ulong positionInElements = 0;
        private ulong positionInCurrentElement = 0;
        private string label = string.Empty;

        public ulong MaxElements 
        {
            get => maxElements;
            set
            {
                maxElements = value;
                OnPropertyChanged(nameof(MaxElements));
            }
        }
        public ulong CurrentElementVolume 
        { 
            get => currentElementVolume;
            set
            {
                currentElementVolume = value;
                OnPropertyChanged(nameof(CurrentElementVolume));
            }
        }
        public ulong PositionInElements
        {
            get => positionInElements;
            set
            {
                positionInElements = value;
                OnPropertyChanged(nameof(PositionInElements));
            }
        }
        public ulong PositionInCurrentElement
        {
            get => positionInCurrentElement;
            set
            {
                positionInCurrentElement = value;
                OnPropertyChanged(nameof(PositionInCurrentElement));
            }
        }

        public string Label
        {
            get => label;
            set
            {
                label = value;
                OnPropertyChanged(nameof(Label));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
