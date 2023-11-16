using Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FileHandler
{
    public class FileHandler
    {
        private const int buffSize = 8192;
        private const string postFileName = "post.txt";
        public async Task CloneDirectory(string source, string dest, ProgressDomain progressBar = null)
        {
            DirectoryInfo directory = new DirectoryInfo(source);
            foreach (var dir in directory.EnumerateDirectories("*.*", SearchOption.AllDirectories))
            {
                string splitedDirName;
                if (!Path.IsPathRooted(source))
                    splitedDirName = dir.FullName.Replace(source + Path.DirectorySeparatorChar, "");
                else
                    splitedDirName = dir.FullName.Replace(source, "");
                var finallyDir = Path.Combine(dest, splitedDirName);
                if (!Directory.Exists(finallyDir))
                    Directory.CreateDirectory(finallyDir);
            }
            var files = directory.EnumerateFiles("*.*", SearchOption.AllDirectories);
            if (progressBar != null)
            {
                progressBar.PositionInElements = 0;
                progressBar.MaxElements = (ulong)files.LongCount();
            }
            foreach (var fi in files)
            {
                try
                {
                    await CloneFile(fi.FullName, fi.FullName.ReplaceRoot(source, dest), progressBar);
                }
                catch (Exception ex)
                {
                    
                }
                if (progressBar != null)
                {
                    progressBar.PositionInElements++;
                }
            }
        }

        public async Task CloneFile(string source, string dest, ProgressDomain progressBar = null)
        {
            FileInfo sourceFile = new FileInfo(source);
            ShortFileInfo sourceFileInfo = new ShortFileInfo();
            ShortFileInfo destFileInfo = new ShortFileInfo();
            
            using (var destFileStream = System.IO.File.Create(dest))
            {
                using (var sourceStream = sourceFile.OpenRead())
                {
                    sourceFileInfo.FileSize = sourceFile.Length;
                    if (progressBar != null)
                    {
                        progressBar.CurrentElementVolume = (ulong)sourceFile.Length;
                        progressBar.PositionInCurrentElement = 0;
                        progressBar.Label = sourceFile.FullName;
                    }
                    int readedBytes = 0;
                    byte[] buffer = new byte[buffSize];
                    do
                    {
                        readedBytes = sourceStream.Read(buffer, 0, buffSize);
                        destFileStream.Write(buffer, 0, readedBytes);

                        //View
                        if (progressBar != null)
                            progressBar.PositionInCurrentElement += (ulong)readedBytes;

                        //CalcCrcs
                        var crc16Task = Task.Run(() => CopyerCKO.src.CRCAll.CRC.CRC16(buffer, sourceFileInfo.Crc16, readedBytes));
                        var crc32Task = Task.Run(() => CopyerCKO.src.CRCAll.CRC.CRC32(buffer, sourceFileInfo.Crc32, readedBytes));
                        //var crc64iTask = Task.Run(() => CopyerCKO.src.CRCAll.Crc64Iso(buffer, sourceFileInfo.Crc16, readedBytes));

                        sourceFileInfo.Crc16 = (ushort)await crc16Task;
                        sourceFileInfo.Crc32 = await crc32Task;
                    }
                    while (readedBytes == buffSize);

                    sourceStream.Close();
                    destFileStream.Close();

                    var destFiles = System.IO.File.ReadAllBytes(dest);
                    destFileInfo.FileSize = destFiles.Length;
                    var crc16DestFileTask = Task.Run(() => CopyerCKO.src.CRCAll.CRC.CRC16(destFiles, destFileInfo.Crc16));
                    var crc32DestFileTask = Task.Run(() => CopyerCKO.src.CRCAll.CRC.CRC32(destFiles, destFileInfo.Crc32));

                    destFileInfo.Crc16 = (ushort)await crc16DestFileTask;
                    destFileInfo.Crc32 = await crc32DestFileTask;

                    if (sourceFileInfo != destFileInfo)
                        throw new Exception($"Ошибка копирования файла {source}");
                }
            }
        }

        public Task AddPostInfo(string pathToCko, Domain.DriveInfo drive)
        {
            return Task.Run(() =>
            {
                string pathToPostFile = Path.Combine(pathToCko, postFileName);
                System.IO.File.AppendAllText(pathToPostFile, 
                    $"Диск {drive.DriveType.ToConvertedString()} {drive.FirstName} ({drive.SecondName}) поступил с пимьмом № {drive.Post.Number} от {drive.Post.DateStamp.Date} (вх. № {drive.Post.NumberIncome} от {drive.Post.DateIncome.Date})");
            });
        }
    }
}
