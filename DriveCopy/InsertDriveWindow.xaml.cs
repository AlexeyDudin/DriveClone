using Commands;
using Domain;
using FileHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DriveCopy
{
    /// <summary>
    /// Логика взаимодействия для InsertDriveWindow.xaml
    /// </summary>
    public partial class InsertDriveWindow : INotifyPropertyChanged
    {
        private DriveInfo drive;
        private Settings settings;
        private RelayCommand closeDriveCommand;
        private string viewText = "";
        public InsertDriveWindow()
        {
            InitializeComponent();
        }

        public InsertDriveWindow(Settings settings, DriveInfo drive) : this()
        {
            Settings = settings;
            Drive = drive;
            ViewText = $"Вставьте диск в дисковод {Settings.SourcePath}";
        }

        public DriveInfo Drive
        { 
            get => drive;
            set
            {
                drive = value;
                OnPropertyChanged(nameof(Drive));
            }
        }
        public Settings Settings 
        { 
            get => settings;
            set
            {
                settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        public RelayCommand CloseDriveCommand 
        { 
            get
            {
                return closeDriveCommand ??
                    (closeDriveCommand = new RelayCommand(obj =>
                    {
                        DriveHandler.CloseDrive();
                    }));
            } 
        }

        public string ViewText 
        {
            get => viewText;
            set
            {
                viewText = value;
                OnPropertyChanged(nameof(ViewText));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void InsertDriveForm_Loaded(object sender, RoutedEventArgs e)
        {
            Action act = () =>
            {
                Dispatcher.Invoke(() =>
                {
                    this.DialogResult = true;
                    this.Close();
                });
            };
            DriveHandler.AwaitFileAvailable(Settings.SourcePath, act);
        }
    }
}
