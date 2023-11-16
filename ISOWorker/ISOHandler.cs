using Domain;
using System.IO;
using System.Linq;
using FileHandler;
using System.Threading.Tasks;

namespace ISOWorker
{
    public class ISOHandler
    {
        public void CreateIso(string directory, string pathToIsoFile, ProgressDomain progressBar = null)
        {
            DirectoryInfo di = new DirectoryInfo(directory);
            string fromPathP = Path.GetFullPath(directory) + Path.DirectorySeparatorChar;

            if (progressBar != null)
            {
                progressBar.Label = "Создание образа:";
                progressBar.MaxElements = (uint)di.EnumerateFiles("*.*", SearchOption.AllDirectories).Count();
                progressBar.PositionInCurrentElement = 0;
            }

            uint fileCounter = 0;

            DiscUtils.Iso9660.CDBuilder cdBuilder = new DiscUtils.Iso9660.CDBuilder();
            foreach (var fi in di.EnumerateFiles("*.*", SearchOption.AllDirectories))
            {
                fileCounter++;
                progressBar.Label = fi.Name;
                progressBar.PositionInElements = fileCounter;
                var name = fi.FullName.FileNameExeptRootPath(Path.GetFullPath(directory));
                cdBuilder.AddFile(name, fi.FullName);
            }

            foreach (var d in di.EnumerateDirectories("*.*", SearchOption.AllDirectories))
            {
                if (d.IsEmpty())
                {
                    var name = d.FullName.FileNameExeptRootPath(Path.GetFullPath(directory));
                    cdBuilder.AddDirectory(name);
                }
            }

            cdBuilder.Build(pathToIsoFile);
        }

        public Task CreateIsoAsync(string directory, string pathToIsoFile, ProgressDomain progressBar = null)
        {
            return Task.Run(() => CreateIso(directory, pathToIsoFile, progressBar));
        }
    }
}
