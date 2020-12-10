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
using WPF_MvvMTest.Model;


namespace WPF_MvvMTest.View.FoodAndBeverageManagement.Windows
{
    /// <summary>
    /// FABM_Pay_the_bill.xaml 的交互逻辑
    /// </summary>
    public partial class FABM_Pay_the_bill : Window
    {

        Model.EasternStar_WPF_MVVMEntities m = new EasternStar_WPF_MVVMEntities();
        public FABM_Pay_the_bill(List<RoomStage> roomStages)
        {
            rooms = roomStages;
            InitializeComponent();
        }
         List<RoomStage> rooms;
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //客人账号
            Tb_The_guest_account.Text = m.VIP_Table.Where(c => c.ID_Guest == rooms[0].ID_Guest).Single().Accounts;
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (true)
            {

            }
        }
    }
}
