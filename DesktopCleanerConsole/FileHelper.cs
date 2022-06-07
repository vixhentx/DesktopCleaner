/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopCleanerConsole
{
    internal class FileHelper
    {
        public static bool IsDir(string filepath)
        {
            FileInfo fi = new FileInfo(filepath);
            return (fi.Attributes & FileAttributes.Directory) != 0;
        }
        /// <summary>
        /// 复制多个文件到目录
        /// </summary>
        /// <param name="filepaths">各个文件的path</param>
        /// <param name="targetdir">目标目录（末尾不带杠）</param>
        public static void CopyFiles(string[] filepaths, string targetdir)
        {
            foreach (string filepath in filepaths)
            {
                FileInfo x = new FileInfo(filepath);
                try
                {
                    x.CopyTo(Path.Combine(targetdir, x.Name), true);
                }
                catch {break; }
            }
        }
        /// <summary>
        /// 复制单个文件夹到目录
        /// </summary>
        /// <param name="folderpath">文件夹path</param>
        /// <param name="targetdir">目标目录</param>
        public static void CopyFolder(string folderpath, string targetdir)
        {
            string[] files = Directory.GetFiles(folderpath);
            string targetpath = Path.Combine(targetdir, new DirectoryInfo(folderpath).Name);
            if (!Directory.Exists(targetpath)) Directory.CreateDirectory(targetpath);
            try
            {             // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    string destFile = Path.Combine(targetpath, Path.GetFileName(s));
                    File.Copy(s, destFile, true);
                }
            }
            catch { ; }
        }
        /// <summary>
        /// 智能删除文件或文件夹
        /// </summary>
        /// <param name="path"></param>
        internal static void Delete(string path)
        {
            try
            {
                if (IsDir(path))//是文件夹
                {
                    Directory.Delete(path, true);
                }
                else
                {
                    File.Delete(path);
                }
            }
            catch { ; }
        }
    }
}
*/