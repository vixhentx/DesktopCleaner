using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DesktopCleaner.ViewBase;
using DesktopCleaner.DllUse;
using Microsoft.Win32;
using AduSkin.Controls.Metro;

namespace DesktopCleaner.Views
{
    /// <summary>
    /// FileView.xaml 的交互逻辑
    /// </summary>
    public partial class FileView : UserControl
    {
        private List<FileItem> FileList;
        private string publicDesktopDir= Functions.GetAllUsersDesktopFolderPath();

        public FileView()
        {
            InitializeComponent();
            FileList = new List<FileItem>();
            #region datagrid
            //遍历共用桌面下的文件
            GetFiles();
            Update();
            #endregion
        }

       

        private void btnAddFile_Click(object sender, RoutedEventArgs e)
        {
            string []filepaths = Functions.SelectFiles();
            if (filepaths == null) return;
            FileHelper.CopyFiles(filepaths,publicDesktopDir);
            FileList.Clear();
            GetFiles();
            Update();
        }
        private void btnAddFolder_Click(object sender, RoutedEventArgs e)
        {
            string folderpath = Functions.SelectFolder();
            if (folderpath == null) return;
            FileHelper.CopyFolder(folderpath:folderpath, publicDesktopDir);
            FileList.Clear();
            GetFiles();
            Update();
        }
        /// <summary>
        /// 更新DataGrid中的文件
        /// </summary>
        private void Update()
        {
            datagridFileview.ItemsSource = null;
            datagridFileview.ItemsSource = FileList;
        } 
        private void GetFiles()
        {
            DirectoryInfo d = new DirectoryInfo(publicDesktopDir);
            FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
            //先文件夹后文件
            foreach (FileSystemInfo x in fsinfos)
            {
                if(x is DirectoryInfo)
                {
                    FileItem fileItem = new FileItem
                    {
                        FileName = x.Name,
                        FileType = "文件夹",
                        FileIcon = Functions.IconToImageSource(Functions.GetFolderIcon(x.Name, false, out _)),
                        FilePath = x.FullName
                    };
                    FileList.Add(fileItem);
                }
            }
            foreach (FileSystemInfo x in fsinfos)
            {
                FileInfo fileInfo = new FileInfo(x.FullName);
                if (!(x is DirectoryInfo))
                {
                    FileItem fileItem = new FileItem
                    {
                        FileName = x.Name,
                        FileType = x.Extension,
                        FileIcon = Functions.IconToImageSource(Functions.GetFileIcon(x.FullName, false)),
                        FileSize = Functions.SizeContent(fileInfo.Length),
                        FileDate = fileInfo.LastWriteTime.ToString(),
                        FilePath = fileInfo.FullName
                    };
                    FileList.Add(fileItem);
                }
            }

        }
        private void btnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            if (datagridFileview.SelectedIndex < 0) return;
            string path = FileList[datagridFileview.SelectedIndex].FilePath;
            FileInfo fi = new FileInfo(path);
            FileHelper.Delete(path);
            FileList.Clear();
            GetFiles();
            Update();
        }
        private void btnDeleteChecked_Click(object sender, RoutedEventArgs e)
        {
            foreach (FileItem file in FileList)
            {
                try
                {
                    if (file.IsChecked)
                    {
                        FileHelper.Delete(file.FilePath);
                    }
                }catch 
                {
                    AduMessageBox.Show("无操作权限！请以管理员权限运行重试！");
                    break;
                }
            }
            FileList.Clear();
            GetFiles();
            Update();
        }
        private void AduCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach(FileItem file in FileList)
            {
                file.IsChecked = true;
            }
            Update();
        }

        private void AduCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (FileItem file in FileList)
            {
                file.IsChecked = false;
            }
            Update();
        }

        private void btnReflash_Click(object sender, RoutedEventArgs e)
        {
            FileList.Clear();
            GetFiles();
            Update();
        }

        private void btnOpenPublicDesktop_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(publicDesktopDir);
        }

        private void gridDrop_Drop(object sender, DragEventArgs e)
        {
            string filepath ;
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                int count = ((System.Array)e.Data.GetData(System.Windows.DataFormats.FileDrop)).Length;
                for (int i = 0; i < count; i++)
                {
                    filepath = ((System.Array)e.Data.GetData(System.Windows.DataFormats.FileDrop)).GetValue(i).ToString(); ;
                    if(FileHelper.IsDir(filepath))
                    {
                        string[] files = Directory.GetFiles(filepath);
                        string targetpath = Path.Combine(publicDesktopDir+Path.GetDirectoryName(filepath));
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
                        catch { AduMessageBox.Show("无操作权限！请以管理员权限运行重试！"); return; }
                    }else//是文件
                    {
                        FileInfo x = new FileInfo(filepath);
                        try
                        {
                            x.CopyTo(Path.Combine(publicDesktopDir,x.Name), true);
                        }
                        catch { AduMessageBox.Show("无操作权限！请以管理员权限运行重试！"); return; }
                    }
                }
            }
            FileList.Clear();
            GetFiles();
            Update();
        }

        private void gridDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
    }
    
}
