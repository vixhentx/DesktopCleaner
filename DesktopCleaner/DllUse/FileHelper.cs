using AduSkin.Controls.Metro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopCleaner.DllUse
{
    public class FileHelper
    {/// <summary>
    /// 通过DesktopCleanerConsole执行命令
    /// </summary>
    /// <param name="op">
    /// 操作命令
    /// Copy: 复制
    /// Delete：删除
    /// ...
    /// </param>
    /// <param name="otherArg">其他参数</param>
        public static void DCC(string op, params string[] otherArgs)
        {
            try
            {

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "DesktopCleanerConsole.exe", //启动的应用程序名称
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = op 
                };
                foreach(string arg in otherArgs)
                {
                    startInfo.Arguments += " \"" + arg + "\""; //[空格]+"+arg+"
                }
                Process p= Process.Start(startInfo);
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                AduMessageBox.Show(ex.Message);
                throw;
            }
            
        }
        /// <summary>
        /// 通过DesktopCleanerConsole执行命令 (不等待进程结束)
        /// </summary>
        /// <param name="op">
        /// 操作命令
        /// Copy: 复制
        /// Delete：删除
        /// ...
        /// </param>
        /// <param name="otherArg">其他参数</param>
        public static void DCC_quick(string op, params string[] otherArgs)
        {
            try
            {

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "DesktopCleanerConsole.exe", //启动的应用程序名称
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = op
                };
                foreach (string arg in otherArgs)
                {
                    startInfo.Arguments += " \"" + arg + "\""; //[空格]+"+arg+"
                }
                Process p = Process.Start(startInfo);
                //p.WaitForExit();
            }
            catch (Exception ex)
            {
                AduMessageBox.Show(ex.Message);
                throw;
            }

        }
        /// <summary>
        /// 判断是否为文件夹
        /// </summary>
        /// <param name="filepath">文件（夹）目录</param>
        /// <returns>文件：false ， 文件夹：true</returns>
        public static bool IsDir(string filepath)
        {
            FileInfo fi = new FileInfo(filepath);
            return (fi.Attributes & FileAttributes.Directory) != 0;
        }
        public static void Move(string desktopPath, string backupPath)
        {
            DCC("Move",desktopPath,backupPath);
        }

        /// <summary>
        /// 复制单个文件到目录
        /// </summary>
        /// <param name="filepaths">各个文件的path</param>
        /// <param name="targetdir">目标目录（末尾不带杠）</param>
        /// <summary>
        /// 复制多个文件到目录
        /// </summary>
        /// <param name="filepaths">各个文件的path</param>
        /// <param name="targetdir">目标目录（末尾不带杠）</param>
        public static void CopyFiles(string[] filepaths, string targetdir)
        {
            foreach (string filepath in filepaths)
            {
                DCC("Copy", filepath , Path.Combine(targetdir, Path.GetFileName(filepath)));
            }
        }


        /// <summary>
        /// 智能删除文件或文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void Delete(string path)
        {
            DCC("Delete", path);
        }
        public static void CopyFolder(string folderpath, string targetpath)
        {
            if (!Directory.Exists(targetpath))
            {
                DCC("CreateFolder",targetpath);
            }
            string[] files = Directory.GetFiles(folderpath);
            string[] directories = Directory.GetDirectories(folderpath);
            foreach (string s in files)
            {
                DCC("Copy",s, Path.Combine(targetpath, Path.GetFileName(s)));
            }
            foreach (string d in directories)
            {
                CopyFolder(Path.Combine(folderpath, Path.GetFileName(d)), Path.Combine(targetpath, Path.GetFileName(d)));
            }

        }
        public static void MoveFolder(string folderpath, string targetpath)
        {
            if (!Directory.Exists(targetpath))
            {
                DCC("CreateFolder", targetpath);
            }
            string[] files = Directory.GetFiles(folderpath);
            string[] directories = Directory.GetDirectories(folderpath);
            foreach (string s in files)
            {
                DCC("Move", s, Path.Combine(targetpath, Path.GetFileName(s)));
            }
            foreach (string d in directories)
            {
                MoveFolder(Path.Combine(folderpath, Path.GetFileName(d)), Path.Combine(targetpath, Path.GetFileName(d)));
                DCC("Delete", d);
            }
        }
    }
}
