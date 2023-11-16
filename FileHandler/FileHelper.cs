using System.IO;
using System.Linq;

namespace FileHandler
{
    public static class FileHelper
    {
        public static bool IsEmpty(this DirectoryInfo di)
        {
            return !di.EnumerateFiles("*.*", SearchOption.AllDirectories).Any();
        }

        public static string FileNameExeptRootPath(this string fullName, string rootPath)
        {
            if (rootPath.Length == 3)
                return fullName.Replace(rootPath, "");
            return fullName.Replace(rootPath + Path.DirectorySeparatorChar, "");
        }

        public static string ReplaceRoot(this string fullName, string sourcePath, string destPath)
        {
            if (Path.IsPathRooted(sourcePath))
            {
                var splitedString = sourcePath.Substring(0, 2);
                return fullName.Replace(splitedString, destPath);
            }
            return fullName.Replace(sourcePath, destPath);
        }
    }
}
