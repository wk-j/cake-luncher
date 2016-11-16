using System.Linq;
using WixSharp;

namespace CakeLauncher.Installer
{

    class FileFilter
    {
        private readonly System.IO.DirectoryInfo TopDir;

        public string Root => TopDir.FullName;

        public FileFilter(string dir)
        {
            TopDir = new System.IO.DirectoryInfo(dir);
        }

        System.IO.FileInfo[] GetFiles(string pattern)
        {
            return TopDir.GetFiles(pattern);
        }

        System.IO.FileInfo[] GetFiles(string pattern, string subDir)
        {
            return new System.IO.DirectoryInfo(System.IO.Path.Combine(TopDir.FullName, subDir)).GetFiles(pattern);
        }

        File ToWixFile(System.IO.FileInfo file)
        {
            return new File(file.FullName)
            {
                Permissions = new FilePermission[] {
                    new FilePermission("Everyone", GenericPermission.All)
                }
            };
        }

        public File[] GetWixFiles(string pattern)
        {
            return GetFiles(pattern).Select(ToWixFile).ToArray();
        }

        public File[] GetWixFiles(string pattern, string subDir)
        {
            return GetFiles(pattern, subDir).Select(ToWixFile).ToArray();
        }
    }
}