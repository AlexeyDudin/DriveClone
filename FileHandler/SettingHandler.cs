using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHandler
{
    public static class SettingHandler
    {
        public static void SaveSettings(Settings settings)
        {
            Properties.Setup.Default.SourceDrive = settings.SourcePath;
            Properties.Setup.Default.DestDrive = settings.DestPath;
            Properties.Setup.Default.Save();
        }

        public static Settings ReadSettings()
        {
            Settings settings = new Settings();
            settings.SourcePath = Properties.Setup.Default.SourceDrive;
            settings.DestPath = Properties.Setup.Default.DestDrive;
            return settings;
        }
    }
}
