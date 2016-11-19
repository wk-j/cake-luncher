using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CakeLauncher
{
    [ComVisible(true)]
    [DisplayName("Cake Power Launcher")]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".cake")]
    public class CakePowerLauncher : SharpContextMenu
    {
        protected override bool CanShowMenu()
        {
            return GetSelectedFile() != null;
        }

        private string GetSelectedFile()
        {
            return SelectedItemPaths.Where(x => x.EndsWith(".cake")).FirstOrDefault();
        }


        protected override ContextMenuStrip CreateMenu()
        {
            var file = GetSelectedFile();
            var fileInfo = new FileInfo(file);
            var dir = fileInfo.Directory.FullName;
            var tasks = CakeParser.ParseFile(fileInfo);

            var menu = new ContextMenuStrip();

            var cake = new ToolStripMenuItem
            {
                Text = "Cake"
            };

            menu.Items.Add(cake);

            tasks.ToList().ForEach(task =>
            {
                var taskName = task.Name;
                var item = cake.DropDownItems.Add(taskName);
                item.Click += (e, s) =>
                {
                    CakeExecutor.ExecuteCmd(taskName, dir);
                };
            });

            return menu;
        }
    }
}
