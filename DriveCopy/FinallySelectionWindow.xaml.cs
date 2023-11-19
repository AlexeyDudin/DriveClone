using Commands;
using Domain;
using FileHandler;
using ISOWorker;
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
    /// Interaction logic for FinallySelectionWindow.xaml
    /// </summary>
    public partial class FinallySelectionWindow : INotifyPropertyChanged
    {
        private Settings settings = null;
        private DriveInfo driveInfo = null;
        private ProgressDomain progressBar = null;
        private RelayCommand saveCommand;

        public FinallySelectionWindow()
        {
            InitializeComponent();
        }

        public FinallySelectionWindow(Settings settings, DriveInfo driveInfo): this()
        {
            Settings = settings;
            DriveInfo = driveInfo;
            DriveInfo.DriveType = DriveHandler.GetDriveType(Settings.SourcePath);
            DriveInfo.CkoType = DriveHandler.GetVidType(Settings.SourcePath);
            DriveInfo.CkoNumAndVer = DriveHandler.GetDriveName(Settings.SourcePath);
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
        public DriveInfo DriveInfo
        { 
            get => driveInfo;
            set
            {
                driveInfo = value;
                OnPropertyChanged(nameof(DriveInfo));
            }
        }

        public ProgressDomain ProgressBar 
        { 
            get => progressBar;
            set
            {
                progressBar = value;
                OnPropertyChanged(nameof(ProgressBar));
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                    (saveCommand = new RelayCommand(async obj =>
                    {
                        await Task.Run(async() =>
                        {
                            try
                            {
                                ProgressBar = new ProgressDomain();
                                var fileHandler = new FileHandler.FileHandler();
                                await fileHandler.CloneDirectory(Settings.SourcePath, DriveInfo.GetDestinationPath(Settings.DestPath), ProgressBar);
                                await fileHandler.AddPostInfo(DriveInfo.GetCkoPath(Settings.DestPath), DriveInfo);
                                var isoHandler = new ISOHandler();
                                ProgressBar = null;
                                ProgressBar = new ProgressDomain();
                                await isoHandler.CreateIsoAsync(DriveInfo.GetDestinationPath(Settings.DestPath), System.IO.Path.ChangeExtension(DriveInfo.GetDestinationPath(Settings.DestPath), "iso"), ProgressBar);
                                ProgressBar = null;
                            }
                            catch (Exception ex)
                            {
                                Dispatcher.Invoke(() => MessageBox.Show(this, ex.Message, "Ошибка при копировании диска", MessageBoxButton.OK, MessageBoxImage.Error)); ;
                            }
                            DriveHandler.OpenDrive();

                            Dispatcher.Invoke(() => this.Close());
                        });
                    }));
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
