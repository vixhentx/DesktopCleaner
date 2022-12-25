
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;

namespace DesktopCleaner.ViewBase
{
    public class FileItem : INotifyPropertyChanged
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public System.Windows.Media.ImageSource FileIcon { get; set; }
        public string FileDate { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }//文件的完整路径
        public bool isChecked;
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                NotifyPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}