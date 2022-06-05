
namespace DesktopCleaner.ViewBase
{
    public class FileItem
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public System.Windows.Media.ImageSource FileIcon { get; set; }
        public string FileDate { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }//文件的完整路径
        public bool IsChecked { get; set; }
    }
}