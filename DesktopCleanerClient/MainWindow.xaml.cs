using AduSkin.Controls.Metro;
using DesktopCleaner.DllUse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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

namespace DesktopCleanerClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        string desktopPath, backupPath,inipath = Path.Combine(Environment.CurrentDirectory, "Options.ini");
        bool isStartUp, isDate, IsShortCut, IsBackup, Is7day, IsQuiet;
        public MainWindow()
        {
            InitializeComponent();
            ReadOption();
            if (IsQuiet)
            {
                Tackle_Quiet();
            }
            else
            {
                Thread thread = new Thread(new ThreadStart(Tackle));
                thread.Start();
            }
        }

        private void ReadOption()
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
            desktopPath = Functions.INIRead(section, keys[0], inipath);
            backupPath = Functions.INIRead(section, keys[1], inipath);
            isStartUp = bool.Parse(Functions.INIRead(section, keys[2], inipath));
            isDate = bool.Parse(Functions.INIRead(section, keys[3], inipath));
            IsShortCut = bool.Parse(Functions.INIRead(section, keys[4], inipath));
            IsBackup = bool.Parse(Functions.INIRead(section, keys[5], inipath));
            Is7day = bool.Parse(Functions.INIRead(section, keys[6], inipath));
            IsQuiet = bool.Parse(Functions.INIRead(section, keys[7], inipath));
        }
        private void Tackle()
        {
            //开始表演
            if (isDate) backupPath = Path.Combine(backupPath, DateTime.Now.ToString("yyyy\\\\MM\\\\dd"));
            OutPut(string.Format("即将清理的桌面路径:{0} ,备份路径:{1},是否按日期分文件夹:{2} ,是否进行备份{3}", desktopPath, backupPath, isDate.ToString(), IsBackup.ToString()));
            for (int i = 5; i > 0; i--)
            {
                labelOperation.Dispatcher.Invoke(new Action(
                    delegate
                    {
                        labelOperation.Content = i.ToString() + "秒后清理桌面";
                    })
                    );
                Thread.Sleep(1000);
            }//定时5秒
            OutPut("开始清理桌面! 请不要关闭程序");
            labelOperation.Dispatcher.Invoke(new Action(
                    delegate
                    {
                        labelOperation.Content = "正在清理桌面...";
                    })
                    );
            if (!Directory.Exists(backupPath)) FileHelper.DCC_quick("CreateFolder", backupPath);
            OutPut("创建文件夹成功！(" + backupPath + ")");
            if (IsBackup)
            {
                DirectoryInfo d = new DirectoryInfo(desktopPath);
                FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
                string targetpath,file;
                for(int i = 0; i < fsinfos.Length; i++)
                {
                    file=fsinfos[i].FullName;
                    if (fsinfos[i] is DirectoryInfo)
                    {
                        targetpath = Path.Combine(backupPath, fsinfos[i].Name);
                        if(!Directory.Exists(targetpath))FileHelper.DCC("CreateFolder",targetpath);
                        FileHelper.MoveFolder(file, targetpath);
                        FileHelper.Delete(file);
                        OutPut(string.Format("移动文件夹: {0} -> {1}", file, targetpath));
                    }
                    else
                    {
                        targetpath = Path.Combine(backupPath, fsinfos[i].Name);
                        FileHelper.DCC("Move", file, targetpath);
                        OutPut(string.Format("移动文件: {0} -> {1}", file, targetpath));
                    }
                    ProgressBar.Dispatcher.Invoke(new Action(
                        delegate
                        {
                            ProgressBar.Value = (i+1)*100/fsinfos.Length;
                        })
                        );
                }
                OutPut("成功将桌面文件夹移动到备份目录!");
            }
            else
            {
                DirectoryInfo d = new DirectoryInfo(desktopPath);
                FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
                string file;
                for(int i = 0; i < fsinfos.Length; i++)
                {
                    file= fsinfos[i].FullName;
                    if (fsinfos[i] is DirectoryInfo)
                    {
                        FileHelper.DCC("Delete",file);
                        OutPut(string.Format("删除文件夹: {0}", file));
                    }
                    else
                    {
                        FileHelper.DCC("Delete", file);
                        OutPut(string.Format("删除文件: {0}", file));
                    }
                    ProgressBar.Dispatcher.Invoke(new Action(
                        delegate
                        {
                            ProgressBar.Value = (i+1)*100 / fsinfos.Length;
                        })
                        );
                }
                OutPut("成功删除桌面文件");

            }
            OutPut("完成!");
            for (int i = 5; i > 0; i--)
            {
                labelOperation.Dispatcher.Invoke(new Action(
                    delegate
                    {
                        labelOperation.Content = "清理完成! "+i.ToString() + "秒后退出";
                    })
                    );
                Thread.Sleep(1000);
            }//定时5秒
            
            windowMain.Dispatcher.Invoke(new Action(
           delegate
           {
             windowMain.Close();
           })
           );
        }
        private void Tackle_Quiet()
        {
            //开始表演
            
            if (isDate) backupPath = Path.Combine(backupPath, DateTime.Now.ToString("yyyy\\\\MM\\\\dd"));
            if (!Directory.Exists(backupPath)) FileHelper.DCC_quick("CreateFolder", backupPath);
            if (IsBackup)
            {
                DirectoryInfo d = new DirectoryInfo(desktopPath);
                FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
                string targetpath, file;
                for (int i = 0; i < fsinfos.Length; i++)
                {
                    file = fsinfos[i].FullName;
                    if (fsinfos[i] is DirectoryInfo)
                    {
                        targetpath = Path.Combine(backupPath, fsinfos[i].Name);
                        if (!Directory.Exists(targetpath)) FileHelper.DCC("CreateFolder", targetpath);
                        FileHelper.MoveFolder(file, targetpath);
                        FileHelper.Delete(file);
                    }
                    else
                    {
                        targetpath = Path.Combine(backupPath, fsinfos[i].Name);
                        FileHelper.DCC("Move", file, targetpath);
                    }
                }
            }
            else
            {
                DirectoryInfo d = new DirectoryInfo(desktopPath);
                FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
                string file;
                for (int i = 0; i < fsinfos.Length; i++)
                {
                    file = fsinfos[i].FullName;
                    if (fsinfos[i] is DirectoryInfo)
                    {
                        FileHelper.DCC("Delete", file);
                    }
                    else
                    {
                        FileHelper.DCC("Delete", file);
                    }
                }
            }
            windowMain.Dispatcher.Invoke(new Action(
           delegate
           {
               windowMain.Close();
           })
           );
        }
        public void OutPut(string msg)
        {
            //LogOutput.Text+="\n"+msg;
            // 委托
            LogOutput.Dispatcher.Invoke(new Action(
                delegate
                {
                    LogOutput.Text += "\n" + msg;
                })
                );
        }
    }
}
