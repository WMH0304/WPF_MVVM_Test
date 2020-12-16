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
using System.Windows.Shapes;
using WPF_MvvMTest.Model;

namespace WPF_MvvMTest.View.HotelManagement
{
    /// <summary>
    /// W_hotel_management.xaml 的交互逻辑
    /// </summary>
    public partial class W_hotel_management : Window
    {
        public W_hotel_management()
        {
            InitializeComponent();
        }

        Model.EasternStar_WPF_MVVMEntities m = new EasternStar_WPF_MVVMEntities();

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
