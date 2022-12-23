using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopCleanerConsole
{
    internal class ShortcutHelper
    {
        /// <summary>
        /// 创建快捷方式的类
        /// </summary>
        /// <remarks></remarks>
        //需要引入IWshRuntimeLibrary，搜索Windows Script Host Object Model

        
        public static void CreateShortcut(string referFilePath,string lnkFilePath)
        {
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(lnkFilePath);
            shortcut.TargetPath = referFilePath;
            shortcut.WindowStyle = 1;
            shortcut.WorkingDirectory = Path.GetDirectoryName(referFilePath);
            shortcut.Save();
        }
        

    }
}
