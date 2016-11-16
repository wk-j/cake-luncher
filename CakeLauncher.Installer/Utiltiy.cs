using WixSharp;

namespace CakeLauncher.Installer
{

    class Utiltiy
    {
        public static void CreateShortcut(File file)
        {
            file.Shortcuts = new FileShortcut[] {
                new FileShortcut(file.Id, "INSTALLDIR"),
                new FileShortcut(file.Id, "%Desktop%")
            };
        }
    }
}