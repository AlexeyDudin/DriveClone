using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Domain
{
    public class ShortFileInfo : INotifyPropertyChanged
    {
        private string fileName = "";
        private long fileSize = 0;
        private ushort crc16 = 0;
        private uint crc32 = 0;
        private ulong crc64i = 0;

        public string FileName
        {
            get => fileName;
            set
            {
                fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }
        public long FileSize 
        {
            get => fileSize;
            set
            {
                fileSize = value;
                OnPropertyChanged(nameof(FileSize));
            }
        }
        public ushort Crc16 
        { 
            get => crc16;
            set
            {
                crc16 = value;
                OnPropertyChanged(nameof(Crc16));
            }
        }
        public uint Crc32 
        { 
            get => crc32;
            set
            {
                crc32 = value;
                OnPropertyChanged(nameof(Crc32));
            }
        }
        public ulong Crc64i
        { 
            get => crc64i;
            set
            {
                crc64i = value;
                OnPropertyChanged(nameof(Crc64i));
            }
        }


        public static bool operator ==(ShortFileInfo left, ShortFileInfo right)
        {
            if (left.FileSize != right.FileSize)
                return false;
            if (left.Crc16 != right.Crc16)
                return false;
            if (left.Crc32 != right.Crc32)
                return false;
            if (left.Crc64i != right.Crc64i)
                return false;
            return true;
        }

        public static bool operator !=(ShortFileInfo left, ShortFileInfo right)
        {
            return !(left == right);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
