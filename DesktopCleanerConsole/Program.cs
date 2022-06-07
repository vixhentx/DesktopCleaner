using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopCleanerConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                switch (args[0])
                {
                    case "Copy":
                        {
                            File.Copy(args[1], args[2], true);
                            break;
                        }
                    case "Delete":
                        {
                            if (IsDir(args[1]))//是文件夹
                            {
                                Directory.Delete(args[1], true);
                            }
                            else
                            {
                                File.Delete(args[1]);
                            }
                            break;
                        }
                    case "Move"://只能移动单个文件
                        {
                            File.Copy(args[1],args[2],true);
                            File.Delete(args[1]);
                            break;
                        }
                    case "CreateFolder":
                        {
                            Directory.CreateDirectory(args[1]);
                            break;
                        }
                    case "INIWrite":
                        {
                            INIWrite(args[1],args[2],args[3],args[4]);
                            break;
                        }
                    default:
                        Console.WriteLine("命令格式不正确");
                        break;
                }
            }catch
            {
                Console.WriteLine("命令格式不正确");
            }
        }
        public static bool IsDir(string filepath)
        {
            FileInfo fi = new FileInfo(filepath);
            return (fi.Attributes & FileAttributes.Directory) != 0;
        }
        #region INI
        // 声明INI文件的写操作函数 WritePrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        


        /// 写入INI的方法
        public static void INIWrite(string section, string key, string value, string path)
        {
            // section=配置节点名称，key=键名，value=返回键值，path=路径
            WritePrivateProfileString(section, key, value, path);
        }



        #endregion
    }
}
