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
        string inipath = Path.Combine(Environment.CurrentDirectory,"Options.ini"),defultBackupPath=@"D:\DesktopCleanerBackup";
        public OptionView()
        {
            InitializeComponent();
            if(!Directory.Exists(defultBackupPath))FileHelper.DCC("CreateFolder",defultBackupPath);
            if (!File.Exists(inipath)) WriteDefult();
            ReadOptions();
            if((bool)checkShortCut.IsChecked)//快捷方式
            { 

            }
            if((bool)checkStartup.IsChecked)//开机启动
            {

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

        private void btnViewBackupPath_Click(object sender, RoutedEventArgs e)
        {
            textBackupPath.Text = Functions.SelectFolder();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveOptions();
            
        }
    }
}
