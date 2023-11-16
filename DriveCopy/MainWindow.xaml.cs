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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DriveCopy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: INotifyPropertyChanged
    {
        private DriveInfo drive = new DriveInfo();
        private HandleDrive handleDriveCommand;
        private RelayCommand showSettingsCommand;
        private RelayCommand showSetDriveDialogCommand;
        private Settings settings;

        public MainWindow()
        {
            InitializeComponent();
            Settings = SettingHandler.ReadSettings();
        }

        public HandleDrive HandleDriveCommand 
        {
            get
            {
                return handleDriveCommand ??
                    (handleDriveCommand = new HandleDrive());
            }
        }
        public RelayCommand ShowSettingsCommand 
        {
            get
            {
                return showSettingsCommand ??
                    (showSettingsCommand = new RelayCommand(obj =>
                    {
                        var settingWindow = new SettingWindow(Settings.Clone());
                        this.Hide();
                        if (settingWindow.ShowDialog().Value)
                        {
                            Settings = settingWindow.Settings;
                            SettingHandler.SaveSettings(Settings);
                        }
                        this.Show();
                    }));
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
        public DriveInfo Drive 
        { 
            get => drive;
            set
            {
                drive = value;
                OnPropertyChanged(nameof(Drive));
            }
        }

        public RelayCommand ShowSetDriveDialogCommand
        {
            get
            {
                return showSetDriveDialogCommand ??
                    (showSetDriveDialogCommand = new RelayCommand(obj => 
                    {
                        DriveHandler.OpenDrive();
                        this.Hide();
                        InsertDriveWindow insertDriveWindow = new InsertDriveWindow(Settings, Drive);
                        if (insertDriveWindow.ShowDialog().Value)
                        {
                            FinallySelectionWindow finallySelectionWindow = new FinallySelectionWindow(Settings, Drive);
                            this.Hide();
                            finallySelectionWindow.ShowDialog();
                            if (MessageBox.Show(this, "Считать следующий диск?", "Повторить?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                Drive = new DriveInfo();
                                this.Show();
                                return;
                            }
                        }
                        this.Close();
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
