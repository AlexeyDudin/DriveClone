using Commands;
using Domain;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : INotifyPropertyChanged
    {
        private Settings settings;
        private ObservableCollection<string> drives = new ObservableCollection<string>();
        private RelayCommand selectPathCommand;
        private System.Windows.Forms.FolderBrowserDialog selectDirectoryDialog = new System.Windows.Forms.FolderBrowserDialog();

        public SettingWindow()
        {
            InitializeComponent();
            InitializeDrives();
        }

        private void InitializeDrives()
        {
            Drives.Clear();
            var driveList = System.IO.DriveInfo.GetDrives();
            foreach (var di in driveList)
            {
                if (di.DriveType == System.IO.DriveType.CDRom)
                {
                    Drives.Add(di.Name);
                }
            }
        }

        public SettingWindow(Settings settings):this()
        {
            Settings = settings;
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
        public ObservableCollection<string> Drives
        { 
            get => drives;
            set
            {
                drives = value;
                OnPropertyChanged(nameof(Drives));
            }
        }
        public RelayCommand SelectPathCommand
        {
            get
            {
                return selectPathCommand ??
                    (selectPathCommand = new RelayCommand(obj =>
                    {
                        if (selectDirectoryDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            Settings.DestPath = selectDirectoryDialog.SelectedPath;
                        }
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
