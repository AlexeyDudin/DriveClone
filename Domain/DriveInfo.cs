using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Domain
{
    public class DriveInfo: INotifyPropertyChanged
    {
        private string firstName = "";
        private string secondName = "149/4/ПД/";
        private string ckoNumAndVer = "";
        private Post post = new Post();
        private DriveType driveType = DriveType.loader;
        private CkoType ckoType = CkoType.cko;

        public string FirstName 
        { 
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        public string SecondName 
        { 
            get => secondName;
            set
            {
                secondName = value;
                OnPropertyChanged(nameof(SecondName));
            }
        }
        public Post Post 
        { 
            get => post;
            set
            {
                post = value;
                OnPropertyChanged(nameof(Post));
            }
        }
        public DriveType DriveType 
        { 
            get => driveType;
            set
            {
                driveType = value;
                OnPropertyChanged(nameof(DriveType));
            }
        }
        public string CkoNumAndVer 
        { 
            get => ckoNumAndVer;
            set
            {
                ckoNumAndVer = value;
                OnPropertyChanged(nameof(CkoNumAndVer));
            }
        }
        public CkoType CkoType 
        { 
            get => ckoType;
            set
            {
                ckoType = value;
                OnPropertyChanged(nameof(CkoType));
            }
        }

        public string GetDestinationPath(string destPath)
        {
            var driveTypeString = $"{DriveType.ToConvertedString()} ({ReplaceIncorrectSymbols(FirstName)}, {ReplaceIncorrectSymbols(SecondName)})";
            return $"{System.IO.Path.Combine(GetCkoPath(destPath), driveTypeString)}";
        }

        public string GetCkoPath(string destPath)
        {
            var appendedString = $"@{CkoType.ToConvertedString()}_{ckoNumAndVer}";
            return System.IO.Path.Combine(destPath, appendedString);
        }

        private string ReplaceIncorrectSymbols(string path)
        {
            var result = path;
            foreach (var symbol in System.IO.Path.InvalidPathChars)
            {
                result = result.Replace(symbol, '_');
            }
            result = result.Replace(Path.DirectorySeparatorChar, '_');
            result = result.Replace(Path.AltDirectorySeparatorChar, '_');
            result = result.Replace(Path.VolumeSeparatorChar, '_');
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
