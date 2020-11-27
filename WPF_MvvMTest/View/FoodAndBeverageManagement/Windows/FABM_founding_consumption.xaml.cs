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
using WPF_MvvMTest.EntityVo;

namespace WPF_MvvMTest.View.FoodAndBeverageManagement.Windows
{
    /// <summary>
    /// FABM_founding_consumption.xaml 的交互逻辑
    /// </summary>
    public partial class FABM_founding_consumption : Window
    {
        Model.EasternStar_WPF_MVVMEntities m = new Model.EasternStar_WPF_MVVMEntities();

        List<RoomStage> Rs;
        public FABM_founding_consumption(List<RoomStage> rooms)
        {
            if (!rooms.Equals(null))
            {
                Rs = rooms;
            }
            else
            {
                throw new NullReferenceException("rooms is null");
            }
         
            InitializeComponent();
        }
        //表格里面的 空房数 放到 TabControl 里
        private void DgLeft_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Rs[0].State_RoomStage.Trim() =="预定")
            {
                Cb_Founding_account.Visibility = Visibility.Collapsed;
                Tb_Founding_account.Visibility = Visibility.Visible;
            }
            else if(Rs[0].State_RoomStage.Trim() == "未用")
            {
                Cb_Founding_account.Visibility = Visibility.Visible;
                Tb_Founding_account.Visibility = Visibility.Collapsed;

                var zhanghao = m.SYS_Guest.ToList();
                Cb_Founding_account.ItemsSource = zhanghao;
                Cb_Founding_account.DisplayMemberPath = "MC_Guest";
                Cb_Founding_account.SelectedValuePath = "ID_Guest";

              
            }
        }
    }
}
