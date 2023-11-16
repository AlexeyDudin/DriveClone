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
            if (IsLoader(path))
                return Domain.DriveType.loader;
            if (IsNt(path))
                return Domain.DriveType.shluz;
            if (IsDistrib(path))
                return Domain.DriveType.distributive;
            return Domain.DriveType.shablon;
        }
        public static CkoType GetCkoType(string path)
        {
            if (IsCko(path))
                return CkoType.cko;
            if (IsUpo(path))
                return CkoType.upo;
            return CkoType.um;
        }
        public static string GetCkoName(string path)
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

        private static bool IsLoader(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            var files = di.EnumerateFiles("copy_*.bat", SearchOption.TopDirectoryOnly);
            if (!files.Any())
            {
                files = di.EnumerateFiles("install.exe", SearchOption.TopDirectoryOnly);
                return files.Any();
            }
            return true;
        }
        private static bool IsNt(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            var directories = di.EnumerateDirectories("NT_*.*", SearchOption.TopDirectoryOnly);
            return directories.Any();
        }
        private static bool IsDistrib(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            var files = di.EnumerateFiles("install.bat", SearchOption.TopDirectoryOnly);
            return files.Any();
        }


        private static bool IsUpo(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            var directories = directoryInfo.EnumerateDirectories("UPO_*.*", SearchOption.TopDirectoryOnly);
            return directories.Any();
        }
        private static bool IsCko(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            var directories = directoryInfo.EnumerateDirectories("CKO_*.*", SearchOption.TopDirectoryOnly);
            return directories.Any();
        }
    }
}
