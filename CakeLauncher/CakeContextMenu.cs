using SharpShell.SharpContextMenu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CakeLauncher
{
    public class CakeContextMenu : SharpContextMenu
    {
        protected override bool CanShowMenu()
        {
            var ok = SelectedItemPaths.Where(x => x.ToString().EndsWith(".cake")).Any();
            return ok;
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
