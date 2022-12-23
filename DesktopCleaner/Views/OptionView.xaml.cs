using DesktopCleaner.DllUse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;

namespace DesktopCleaner.Views
{
    /// <summary>
    /// OptionView.xaml 的交互逻辑
    /// </summary>
    public partial class OptionView : UserControl
    {
        string inipath = Path.Combine(Environment.CurrentDirectory,"Options.ini"),defultBackupPath=Path.Combine(Functions.GetAvaliableDisk(),"DesktopCleanerBackup"),startuppath= Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        public OptionView()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            if (!Directory.Exists(defultBackupPath)) FileHelper.DCC("CreateFolder", defultBackupPath);
            if (!File.Exists(inipath)) WriteDefult();
            ReadOptions();
            if ((bool)checkShortCut.IsChecked)//快捷方式
            {
                string[] refers =
                {
                    Path.Combine(Environment.CurrentDirectory,"DesktopCleaner.exe"),
                    Path.Combine(Environment.CurrentDirectory,"DesktopCleanerClient.exe")
                }, shortcutnames =
                {
                    "DesktopCleaner设置.lnk",
                    "立即清理桌面.lnk"
                };
                for (int i = 0; i < refers.Length; i++)
                {
                    Functions.CreateShortcut(refers[i], Path.Combine(Functions.GetAllUsersDesktopFolderPath(), shortcutnames[i]));
                }
            }
            else
            {
                string[] refers =
                {
                    Path.Combine(Environment.CurrentDirectory,"DesktopCleaner.exe"),
                    Path.Combine(Environment.CurrentDirectory,"DesktopCleanerClient.exe")
                }, shortcutnames =
                {
                    "DesktopCleaner设置.lnk",
                    "立即清理桌面.lnk"
                };
                for (int i = 0; i < refers.Length; i++)
                {
                    if (File.Exists(Path.Combine(Functions.GetAllUsersDesktopFolderPath(), shortcutnames[i])))
                        FileHelper.Delete(Path.Combine(Functions.GetAllUsersDesktopFolderPath(), shortcutnames[i]));//删除快捷方式
                }
            }

            if ((bool)checkStartup.IsChecked)//开机启动
            {
                string refer = Path.Combine(Environment.CurrentDirectory, "DesktopCleanerClient.exe");
                string shortcut = Path.Combine(startuppath, "立即清理桌面.lnk");
                Functions.CreateShortcut(refer, shortcut);
            }
            else
            {
                string shortcut = Path.Combine(startuppath, "立即清理桌面.lnk");
                if (File.Exists(shortcut)) FileHelper.Delete(shortcut);
            }
        }

        private void ReadOptions()
        {
            string section = "用户设置";
            string[] keys =
            {
                "DesktopPath",
                "BackupPath",
                "IsStartUp",
                "IsDate",
                "IsShortCut",
                "IsBackup",
                "Is7day",
                "IsQuiet"
            };
            textDesktopPath.Text = Functions.INIRead(section,keys[0],inipath);
            textBackupPath.Text = Functions.INIRead(section,keys[1],inipath);
            checkStartup.IsChecked = bool.Parse(Functions.INIRead(section, keys[2], inipath));
            checkDate.IsChecked = bool.Parse(Functions.INIRead(section, keys[3], inipath));
            checkShortCut.IsChecked = bool.Parse(Functions.INIRead(section, keys[4], inipath));
            checkBackup.IsChecked = bool.Parse(Functions.INIRead(section, keys[5], inipath));
            check7day.IsChecked = bool.Parse(Functions.INIRead(section, keys[6], inipath));
            checkQuiet.IsChecked=bool.Parse(Functions.INIRead(section, keys[7], inipath));
        }

        private void WriteDefult()
        {
            string section = "用户设置";
            string[] keys =
            {
                "DesktopPath",
                "BackupPath",
                "IsStartUp",
                "IsDate",
                "IsShortCut",
                "IsBackup",
                "Is7day",
                "IsQuiet"
            },
            values =
            {
                Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                defultBackupPath,
                true.ToString(),
                true.ToString(),
                true.ToString(),
                true.ToString(),
                false.ToString(),
                false.ToString()
            };
            for(int i=0; i<keys.Length; i++)
            {
                Functions.INIWrite(section, keys[i], values[i], inipath);
            }
        }
        private void SaveOptions()
        {
            string section = "用户设置";
            string[] keys =
            {
                "DesktopPath",
                "BackupPath",
                "IsStartUp",
                "IsDate",
                "IsShortCut",
                "IsBackup",
                "Is7day",
                "IsQuiet"
            },
            values =
            {
                textDesktopPath.Text,
                textBackupPath.Text,
                checkStartup.IsChecked.ToString(),
                checkDate.IsChecked.ToString(),
                checkShortCut.IsChecked.ToString(),
                checkBackup.IsChecked.ToString(),
                check7day.IsChecked.ToString(),
                checkQuiet.IsChecked.ToString()
            };
            for (int i = 0; i < keys.Length; i++)
            {
                Functions.INIWrite(section, keys[i], values[i], inipath);
            }
        }

        private void btnViewDesktopPath_Click(object sender, RoutedEventArgs e)
        {
            textDesktopPath.Text = Functions.SelectFolder();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            WriteDefult();
            ReadOptions();
        }

        private void btnViewBackupPath_Click(object sender, RoutedEventArgs e)
        {
            textBackupPath.Text = Functions.SelectFolder();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveOptions();
            Init();
        }
    }
}
