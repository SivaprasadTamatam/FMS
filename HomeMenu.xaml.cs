using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace FMS
{
    /// <summary>
    /// Interaction logic for HomeMenu.xaml
    /// </summary>
    public partial class HomeMenu : UserControl
    {
        public HomeMenu()
        {
            InitializeComponent();
        }

        private void FMSImageBtnAddAccount_Click(object sender, RoutedEventArgs e)
        {
            CAddAccount obj = new CAddAccount();
            obj.Owner = (MainWindow)System.Windows.Application.Current.MainWindow;
            obj.ShowDialog();
        }

        private void fMSPrintDetails_Click(object sender, RoutedEventArgs e)
        {
            MainWindow wnd = (MainWindow)System.Windows.Application.Current.MainWindow;
            PrintDialog dlg = new PrintDialog();
            PieChart.PieChartLayout ctrl = wnd.GetChartLayout();
            RenderTargetBitmap bmp = new RenderTargetBitmap(1200, 300, 96, 96, PixelFormats.Pbgra32);

            bmp.Render(ctrl);
            var encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using (Stream stm = File.Create(@"test.png"))
                encoder.Save(stm);

            var bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.UriSource = new Uri("F:\\FMS\\FMS\\FMS\\FMS\\bin\\Debug\\test.png");
            bi.EndInit();

            var vis = new DrawingVisual();
            var dc = vis.RenderOpen();
            dc.DrawImage(bi, new Rect { Width = bi.Width, Height = bi.Height });
            dc.Close();


            if (dlg.ShowDialog() == true)
            {
                dlg.PrintVisual(vis, "My Image");
            }
        }
       
    }
}
