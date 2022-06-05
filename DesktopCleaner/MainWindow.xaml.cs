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
using System.Windows.Shapes;
using AduSkin.Controls.Metro;

namespace DesktopCleaner
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        static private Brush brushDisableSide,brushEnableSide;

        public MainWindow()
        {
            InitializeComponent();
            brushDisableSide = (SolidColorBrush)FindResource("DisableSide"); brushEnableSide = (SolidColorBrush)FindResource("EnableSide");//获取颜色Brush
        }
        #region Basic Arrays
        #endregion
        #region Functions Defination
        protected void CloseAllPanel()
        {
            FilePanel.Visibility = BackupPanel.Visibility = OptitionPanel.Visibility = AboutPanel.Visibility = Visibility.Collapsed;
            SideFileGrid.Background = SideBackupGrid.Background = SideOptitionGrid.Background = SideAboutGrid.Background =brushDisableSide;
            btnSideFile.Foreground = btnSideBackup.Foreground = btnSideOptition.Foreground = btnSideAbout.Foreground = brushEnableSide;
        }
        protected void TurnPanel(ref Grid gridPanel, ref Grid gridBackground, ref AduButtonSvg btn)
        {
            CloseAllPanel();
            gridPanel.Visibility = Visibility.Visible;
            gridBackground.Background = brushEnableSide;
            btn.Foreground = brushDisableSide;
        }
        #endregion
        #region SideBar Events

        private void btnSideFile_Click(object sender, RoutedEventArgs e)
        {
            TurnPanel(ref FilePanel,ref SideFileGrid,ref btnSideFile);
        }

        private void btnSideBackup_Click(object sender, RoutedEventArgs e)
        {
            TurnPanel(ref BackupPanel,ref SideBackupGrid,ref btnSideBackup);
        }

        private void btnSideOptition_Click(object sender, RoutedEventArgs e)
        {
            TurnPanel(ref OptitionPanel,ref SideOptitionGrid,ref btnSideOptition);
        }

        private void btnSideAbout_Click(object sender, RoutedEventArgs e)
        {
            TurnPanel(ref AboutPanel, ref SideAboutGrid, ref btnSideAbout);
        }
        #endregion

    }
}
