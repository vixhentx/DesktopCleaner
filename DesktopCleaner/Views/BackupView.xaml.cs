using DesktopCleaner.DllUse;
using DesktopCleaner.ViewBase;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace DesktopCleaner.Views
{
    /// <summary>
    /// BackupView.xaml 的交互逻辑
    /// </summary>
    public partial class BackupView : UserControl
    {
        private List<FileItem> FileList;
        private string backupDir;
        private static string inipath = Path.Combine(Environment.CurrentDirectory, "Options.ini"),
            backuppath = Functions.INIRead("用户设置", "BackupPath", inipath),
            desktopDir = Functions.INIRead("用户设置", "DesktopPath", inipath);

        public BackupView()
        {
            InitializeComponent();
            FileList = new List<FileItem>();
            datePicker.SelectedDate = DateTime.Now;

            SyncTime();
            GetFiles();
            datagridFileview.ItemsSource = FileList;
        }
        #region Events

        private void btnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            if (datagridFileview.SelectedIndex < 0) return;
            string path = FileList[datagridFileview.SelectedIndex].FilePath;
            FileHelper.Delete(path);
            FileList.Clear();
            GetFiles();
            Update();
        }

        private void btnDeleteChecked_Click(object sender, RoutedEventArgs e)
        {
            foreach (FileItem file in FileList)
            {
                if (file.IsChecked)
                {
                    FileHelper.Delete(file.FilePath);
                }
            }
            FileList.Clear();
            GetFiles();
            Update();
        }

        private void btnOpenPublicDesktop_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(backupDir);
        }

        private void btnReflash_Click(object sender, RoutedEventArgs e)
        {
            FileList.Clear();
            SyncTime();
            GetFiles();
            Update();
        }
        #endregion
        private void GetFiles()
        {
            if(!Directory.Exists(backupDir))return;
            DirectoryInfo d = new DirectoryInfo(backupDir);
            FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
            //先文件夹后文件
            foreach (FileSystemInfo x in fsinfos)
            {
                if (x is DirectoryInfo)
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
        private void Update()
        {
            datagridFileview.ItemsSource = null;
            datagridFileview.ItemsSource = FileList;
        }

        private void AduCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach(FileItem fileItem in FileList)
            {
                fileItem.IsChecked = true;
            }
            Update();
        }

        private void AduCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (FileItem fileItem in FileList)
            {
                fileItem.IsChecked = false;
            }
            Update();
        }

        private void btnRestoreSelected_Click(object sender, RoutedEventArgs e)
        {
            if (datagridFileview.SelectedIndex < 0) return;
            string path = FileList[datagridFileview.SelectedIndex].FilePath;
            if(FileHelper.IsDir(path))
            {
                FileHelper.MoveFolder(path, Path.Combine(desktopDir,Path.GetFileName(path)));
                FileHelper.Delete(path);
            }
            else FileHelper.Move(path, Path.Combine(desktopDir,Path.GetFileName(path)));
            FileList.Clear();
            GetFiles();
            Update();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            FileList.Clear();
            SyncTime();
            GetFiles();
            Update();
        }

        private void DataGridRow_GotFocus(object sender, RoutedEventArgs e)
        {
            //FileList[datagridFileview.SelectedIndex].IsChecked = !FileList[datagridFileview.SelectedIndex].IsChecked;
            var item = (DataGridRow)sender;
            FrameworkElement objElement = datagridFileview.Columns[0].GetCellContent(item);
            if (objElement != null)
            {
                FileItem o = (FileItem)objElement;
            }
        }

        private void btnRestoreChecked_Click(object sender, RoutedEventArgs e)
        {
            foreach (FileItem file in FileList)
            {
                if (file.IsChecked)
                {
                    if (FileHelper.IsDir(file.FilePath))
                    {
                        FileHelper.MoveFolder(file.FilePath, Path.Combine(desktopDir, file.FileName));
                        FileHelper.Delete(file.FilePath);
                    }
                    else FileHelper.Move(file.FilePath, Path.Combine(desktopDir, file.FileName));
                }
            }
            FileList.Clear();
            GetFiles();
            Update();
        }

        private void SyncTime()
        {
            DateTime dt = (DateTime)datePicker.SelectedDate;
            backupDir = Path.Combine(backuppath, dt.ToString("yyyy\\\\MM\\\\dd"));
        }
    }
}
