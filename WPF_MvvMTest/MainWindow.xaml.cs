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
using WPF_MvvMTest.View;

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
            uC_TealTimeExecutive.MessageSend += new MessageSend(Reception);
            AddItem(sender,"实时管理",uC_TealTimeExecutive);
            
            //创建监听
            //WPF_MvvMTest.View.Windows.W_RoomTableReservation room = new View.Windows.W_RoomTableReservation(null);
            //room.Resh += new Refresh(Refresh);

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
        
        /// <summary>
        /// 实施管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                w.Resh += new Refresh(Refresh);//委托页面刷新
                w.ShowDialog();
            }
            else
            {
                MessageBox.Show("该房台已被占用，请选择其他房台", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
            }
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
                MessageBox.Show("，请选择房台", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);  
            }
            if (rStages[0].State_RoomStage.Trim() == "未用" || rStages[0].State_RoomStage.Trim() == "预定")
            {
                View.Windows.W_FoundingConsumption w = new View.Windows.W_FoundingConsumption(rStages, rStages[0].State_RoomStage.Trim());
                w.Resh += new Refresh(Refresh);
                w.ShowDialog();
            }
            else
            {
                MessageBox.Show("该房台已被占用，请选择其他房台", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
            }
        }
       
        /// <summary>
        /// 页面刷新
        /// </summary>
        public void Refresh()
        {
            Window_Loaded(null, null);
        }

        /// <summary>
        /// 消费入单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtConsumptionIntoSingle_Click(object sender, RoutedEventArgs e)
        {
            if (roomStages==null || roomStages.Count() ==0)
            {
                MessageBox.Show("请选择消费房台", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
                return;
            }
            if (roomStages[0].State_RoomStage.Trim() == "已用")
            {
                View.Windows.W_ConsumerFinance w = new View.Windows.W_ConsumerFinance(roomStages);
                w.ShowDialog();
            }
            else
            {
                MessageBox.Show("该房台还未启用，请选择消费房台", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
                return;
            }
        }

        /// <summary>
        /// 结账买单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtCheckTheCheck_Click(object sender, RoutedEventArgs e)
        {
            if (roomStages ==null && roomStages.Count() ==0)
            {
                MessageBox.Show("请选中房台", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
                return;
            }
            if (roomStages[0].State_RoomStage.Trim() != "已用")
            {
                MessageBox.Show("该房台未开台，请先开台消费后再结账", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
                return;
            }
            List<RoomStage> rooms = roomStages;
            View.Windows.W_StatementLeave sL = new View.Windows.W_StatementLeave(rooms);
            sL.ShowDialog();





        }
    }
}
