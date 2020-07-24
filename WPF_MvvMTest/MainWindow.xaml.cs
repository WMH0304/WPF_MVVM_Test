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
using WPF_MvvMTest.EntityVo;
using WPF_MvvMTest.Resources;

namespace WPF_MvvMTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        static TabControl TC;//静态选项卡
        static List<RoomStage> roomStages;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TC = tab_Main;
            View.Usercontrol.UC_TealTimeExecutive uC_TealTimeExecutive = new View.Usercontrol.UC_TealTimeExecutive();
            uC_TealTimeExecutive.MessageSend += new View.Usercontrol.MessageSend(Reception);
            AddItem(sender,"实时管理",uC_TealTimeExecutive);
        }

        /// <summary>
        /// 添加选项卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="uc"></param>
        public static void AddItem(object sender, string trname, UserControl uc)//添加选项卡
        {
            bool bolWhetherBe = false;//是否存在当前选项
            //1、判断当前选项卡的个数是否大于0
            if (TC.Items.Count > 0)
            {
                for (int i = 0; i < TC.Items.Count; i++)
                {
                    if (((UC_TabItemWidthClose)TC.Items[i]).Name == trname)//判断是否存在
                    {
                        TC.SelectedItem = ((UC_TabItemWidthClose)TC.Items[i]);//选中子选项
                        bolWhetherBe = true;
                        break;
                    }
                }
                //2、是否存在当前选项
                if (bolWhetherBe == false)
                {
                    //3、创建子选项
                    UC_TabItemWidthClose item = new UC_TabItemWidthClose();//创建子选项
                    item.Name = trname;//名称
                    item.Header = string.Format(trname);//标头名称
                    item.Content = uc;//子选择里面的内容                  
                    item.Margin = new Thickness(0, 0, 1, 0);
                    item.Height = 28;
                    TC.Items.Add(item);//添加子选项
                    TC.SelectedItem = item;//选中子选项 
                }
            }
            else
            {
                UC_TabItemWidthClose item = new UC_TabItemWidthClose();//创建子选项
                item.Name = trname;//名称
                item.Header = string.Format(trname);//标头名称
                item.Content = uc;//子选择里面的内容           
                item.Margin = new Thickness(0, 0, 1, 0);
                item.Height = 28;
                TC.Items.Add(item);//添加子选项
                TC.SelectedItem = item;//选中子选项                
            }

        }
        private void BtRealTimeExecutive_Click(object sender, RoutedEventArgs e)
        {
            TC = tab_Main;
            View.Usercontrol.UC_TealTimeExecutive uC_TealTimeExecutive = new View.Usercontrol.UC_TealTimeExecutive();
           
            AddItem(sender, "实时管理", uC_TealTimeExecutive);
        }
        /// <summary>
        /// 数据接收
        /// </summary>
        void Reception(List<RoomStage> rStages)
        {
            roomStages = rStages;
        }




        /// <summary>
        /// 房台预定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbRoomTableReservation_Click(object sender, RoutedEventArgs e)
        {

            List<RoomStage> rStages = roomStages;
            if (rStages.Equals(null))
            {
                throw new NullReferenceException();
            }
            if (rStages[0].State_RoomStage.Trim() =="未用")
            {
                View.Windows.W_RoomTableReservation w = new View.Windows.W_RoomTableReservation(rStages);
                w.ShowDialog();

            }
            else
            {
                MessageBox.Show("该房台已被占用，请选择其他房台", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
            }
          
          
        }

        private void BtDownPaymentProcessing_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 开台消费
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtFoundingConsumption_Click(object sender, RoutedEventArgs e)
        {
            List<RoomStage> rStages = roomStages;
            if (rStages.Equals(null))
            {
                throw new NullReferenceException();
            }
            if (rStages[0].State_RoomStage.Trim() == "未用")
            {
                View.Windows.W_FoundingConsumption w = new View.Windows.W_FoundingConsumption(rStages);
                w.ShowDialog();
            }
            else
            {
                MessageBox.Show("该房台已被占用，请选择其他房台", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
            }
        }
    }
}
