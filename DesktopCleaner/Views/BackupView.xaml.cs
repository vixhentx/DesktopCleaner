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
using System.Windows.Shapes;

namespace DesktopCleaner.Views
{
    /// <summary>
    /// BackupView.xaml 的交互逻辑
    /// </summary>
    public partial class BackupView : UserControl
    {
        private List<FileItem> FileList;
        private string backupDir;

        public BackupView()
        {
            InitializeComponent();
            //GetFiles();
        }
        #region Events

        private void btnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteChecked_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOpenPublicDesktop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReflash_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
        private void GetFiles()
        {
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
    }
}
