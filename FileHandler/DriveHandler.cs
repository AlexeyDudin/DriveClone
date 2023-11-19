using Domain;
using System.Runtime.InteropServices;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using System.Collections.Generic;

namespace FileHandler
{
    public class DriveHandler
    {
        private const string closeDrive = "set cdaudio door closed";
        private const string openDrive = "set cdaudio door open";
        //private static string pathToAnalize = "";
        //private static Timer timer;
        public static Domain.DriveType GetDriveType(string path)
        {
            if (IsInstall(path))
                return Domain.DriveType.loader;
            if (IsType3(path))
                return Domain.DriveType.type_3;
            if (IsDistrib(path))
                return Domain.DriveType.distributive;
            return Domain.DriveType.type_4;
        }
        public static VidType GetVidType(string path)
        {
            if (IsType1(path))
                return VidType.type1;
            if (IsType2(path))
                return VidType.type2;
            return VidType.type3;
        }
        public static string GetDriveName(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            List<DirectoryInfo> directories = di.EnumerateDirectories("*_*.*", SearchOption.TopDirectoryOnly).ToList();
            if (directories.Count == 0)
                return "";
            string[] parcedStrings = directories.First().Name.Split('_');
            if (parcedStrings.Length < 2)
                return "";
            return parcedStrings[1];
        }

        public static Task AwaitFileAvailable(string path, Action action)
        {
            return Task.Run(() =>
            {
                bool isFound = false;
                while (!isFound)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(path))
                            return;
                        DirectoryInfo di = new DirectoryInfo(path);

                        var files = di.EnumerateFiles("*.*", SearchOption.AllDirectories);
                        if (files.Any())
                        {
                            isFound = true;
                        }
                    }
                    catch { }
                }
                action();
            });
        }

        public static void OpenDrive()
        {
            ExecuteCommand(openDrive);
        }
        public static void CloseDrive()
        {
            ExecuteCommand(closeDrive);
        }

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi)]
        protected static extern int mciSendString
        (string mciCommand,
            StringBuilder returnValue,
            int returnLenght,
            IntPtr callback);


        private static void ExecuteCommand(string command)
        {
            StringBuilder resultString = new StringBuilder();
            int result = mciSendString(command, resultString, 0, IntPtr.Zero);
            return;
        }

        private static bool IsInstall(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            var files = di.EnumerateFiles("*.bat", SearchOption.TopDirectoryOnly);
            if (!files.Any())
            {
                files = di.EnumerateFiles("install.exe", SearchOption.TopDirectoryOnly);
                return files.Any();
            }
            return true;
        }
        private static bool IsType3(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            var directories = di.EnumerateDirectories("TYPE2_*.*", SearchOption.TopDirectoryOnly);
            return directories.Any();
        }

        private static bool IsDistrib(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            var files = di.EnumerateFiles("install.bat", SearchOption.TopDirectoryOnly);
            return files.Any();
        }


        private static bool IsType2(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            var directories = directoryInfo.EnumerateDirectories("TYPE2_*.*", SearchOption.TopDirectoryOnly);
            return directories.Any();
        }
        private static bool IsType1(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            var directories = directoryInfo.EnumerateDirectories("TYPE1_*.*", SearchOption.TopDirectoryOnly);
            return directories.Any();
        }
    }
}
