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
using WPF_MvvMTest.EntityVo;
using WPF_MvvMTest.Resources;
using System.Diagnostics;
using WPF_MvvMTest.View.Usercontrol;
using WPF_MvvMTest.View.Windows;
using WPF_MvvMTest.View.FoodAndBeverageManagement.Windows;

namespace WPF_MvvMTest.View.FoodAndBeverageManagement
{
    /// <summary>
    /// W_FoodAndBeverageManagement.xaml 的交互逻辑
    /// </summary>
    public partial class W_FoodAndBeverageManagement : Window
    {
        public event MessageSend FbMessageSend;
        int id_class;
        public W_FoodAndBeverageManagement(int idclass)
        {
            id_class = idclass;
            STATIC_cache.ID_Class = idclass;
            InitializeComponent();
        }
        
        Model.EasternStar_WPF_MVVMEntities m = new EasternStar_WPF_MVVMEntities();


        List<RoomStage> LRS;

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (LRS !=null)
            {
                LRS.Clear();
            }
            //查询房台数据
            SelectData();
            int int_room = LRS.Count();
            //控件加载
            ToGenerateTheControl(int_room);
            //左边 房台数量显示
            int int_all = LRS.Count(); //全部
            int int_available = LRS.Where(p => p.State_RoomStage.Trim() == "未用").Count();
            int int_disable = LRS.Where(p => p.State_RoomStage.Trim() == "停用").Count();
            int int_reservation = LRS.Where(p => p.State_RoomStage.Trim() == "预定").Count();
            int int_hasbeenused = int_all - int_available - int_disable - int_reservation;//已用
            //
            TbAll.Text = int_all.ToString() +"间";
            Tbavailable.Text = int_available.ToString() + "间";
            tBhasbeenused.Text = int_hasbeenused.ToString() + "间";
            tBdisable.Text = int_disable.ToString() + "间";
            tBreservation.Text = int_reservation.ToString() + "间";
        }
        /// <summary>
        /// 查询房台数据
        /// </summary>
        public void SelectData()
        {

            LRS = (from tr in m.SYS_RoomStage
                   where tr.ID_Class == id_class
                   select new RoomStage
                   {
                       ID_RoomStage = tr.ID_RoomStage,
                       ID_Class = tr.ID_Class,
                       Number_RoomStage = tr.Number_RoomStage,
                       MC_RoomStage = tr.MC_RoomStage,
                       State_RoomStage = tr.State_RoomStage,
                       Describe = tr.Describe,
                       JionSign = tr.JionSign,
                       
                   }).AsParallel().ToList();
        }

        /// <summary>
        /// 动态生成控件
        /// </summary>
        /// <param name="room"></param>
        public void ToGenerateTheControl(int int_room)
        {
            //清空控件
            if (WpButton.Children != null)
            {
                WpButton.Children.Clear();
            }

            for (int i = 0; i < int_room; i++)
            {
                UC_BtDynamic myBtDynamic = new UC_BtDynamic(LRS);
                myBtDynamic.Name = "btn" + LRS[i].ID_RoomStage;
                myBtDynamic.TbRoom_num.Text = LRS[i].Number_RoomStage.ToString();//房台号
                myBtDynamic.TbRoom_class.Text = LRS[i].MC_RoomStage.ToString();//房台名称
                //换台刷新
                myBtDynamic.pus += new Pus(Push);
                myBtDynamic.pus -= new Pus(Push);
                //获取控件内容id
                myBtDynamic.GetButtons += new GetButton(SetBtuuons);

                ////换台刷新
                //myBtDynamic.pus += new Pus(Push);
                //myBtDynamic.pus -= new Pus(Push);
                //获取控件内容id
                // myBtDynamic.GetButtons += new GetButton(SetBtuuons);
                if (LRS[i].State_RoomStage.Trim() == "预定")
                {
                    myBtDynamic.Background = System.Windows.Media.Brushes.LightSkyBlue;
                }
                else if (LRS[i].State_RoomStage.Trim() == "已用")
                {
                    myBtDynamic.Background = System.Windows.Media.Brushes.DodgerBlue;
                }
                else if (LRS[i].State_RoomStage.Trim() == "停用")
                {
                    myBtDynamic.Background = System.Windows.Media.Brushes.LightSlateGray;
                }


                WpButton.Children.Add(myBtDynamic);
            }
        }


        /// <summary>
        /// 接受button 信息
        /// </summary>
        /// <param name="roomStages"></param>
        public void SetBtuuons(List<RoomStage> Stages)
        {
            if (Stages != null)
            {
                if (LRS !=null)
                {
                    LRS.Clear();
                }
               LRS = Stages;
               TbCurrentTime.Text = LRS[0].MC_RoomStage.ToString().Trim();
            }
        }

        //调用刷新页面方法
        void Push()
        {
            Window_Loaded(null, null);
        }

        #region 基本信息
        /// <summary>
        /// 点击过滤房台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void BtFiltration_Click(object sender, RoutedEventArgs e)
        {
            string tbCurrentTime = TbCurrentTime.Text;
            if (!string.IsNullOrEmpty(tbCurrentTime))
            {
                SelectData();
                LRS = LRS.Where(p => p.MC_RoomStage.Trim() == tbCurrentTime || p.Number_RoomStage.Trim() == tbCurrentTime).ToList();

                if (LRS.Count() == 0)
                {
                    MessageBox.Show("没有房台信息", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    TbCurrentTime.Text = "";
                    return;
                }
                ToGenerateTheControl(LRS.Count());
            }
            else
            {
                SelectData();
                ToGenerateTheControl(LRS.Count());
            }
        }

        /// <summary>
        /// 删除房台信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtDeleteRooms_Click(object sender, RoutedEventArgs e)
        {
            if (LRS.Count == 1)
            {
                int Room_id = LRS[0].ID_RoomStage;
                SYS_RoomStage sr = m.SYS_RoomStage.Where(p => p.ID_RoomStage == Room_id).SingleOrDefault();
                m.SYS_RoomStage.Remove(sr);
                // m.SaveChanges();
                m.Entry(sr).State = System.Data.Entity.EntityState.Modified;

                if (m.SaveChanges() > 0)
                {
                    MessageBox.Show("房台删除成功！", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Push();

                }
            }
        }

        /// <summary>
        /// 新建房台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void BtAddButton_Click(object sender, RoutedEventArgs e)
        {
            W_ButtonAdd WBA = new W_ButtonAdd(null, 2);

            WBA.ChangC += new ChangeClose(Push);
            WBA.ChangC -= new ChangeClose(Push);
            WBA.ShowDialog();
        }


        #endregion


        /// <summary>
        /// 房台预定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtRoomTableReservation_Click(object sender, RoutedEventArgs e)
        {
            if (LRS ==null ||LRS.Count ==0)
            {
                MessageBox.Show("请选择目标房台","大海提示",MessageBoxButton.OK,MessageBoxImage.Asterisk);
                return;
            }
            if (LRS[0].State_RoomStage.Trim() =="未用")
            {
              
                Tuple<string, string> tuple = new Tuple<string, string>(LRS[0].Number_RoomStage, LRS[0].MC_RoomStage);

                SelectData();
                if (tuple != null && LRS.Count != 0)
                {
                    FABM_Room_table_reservation w = new FABM_Room_table_reservation(tuple, LRS);
                   // this.Hide();
                    w.ShowDialog();  
                }
                else
                {
                    MessageBox.Show("请选中房台后再进行预定","大海提示",MessageBoxButton.OK,MessageBoxImage.Asterisk);
                    return;
                }
              
            }
        }
    
        /// <summary>
        /// 开台消费
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_Founding_consumption_Click(object sender, RoutedEventArgs e)
        {
            if (!LRS.Equals(null))
            {
                if (LRS[0].State_RoomStage.Trim() != "停用" && LRS[0].State_RoomStage.Trim() != "已用")
                {
                    FABM_founding_consumption f = new FABM_founding_consumption(LRS);
                    f.ShowDialog();

                }
                else
                {
                    MessageBox.Show("目标房台已开台或者已停用", "大海提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("请选中房台","大海提示",MessageBoxButton.YesNo,MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 消费入单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_Consumption_into_a_single_Click(object sender, RoutedEventArgs e)
        {
            if (!LRS.Equals(null))
            {
                if (LRS[0].State_RoomStage.Trim() == "停用")
                {
                    MessageBox.Show("房台已停用", "大海提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    return;
                }
                if (LRS[0].State_RoomStage.Trim() == "未用" || LRS[0].State_RoomStage.Trim() == "预定")
                {
                    MessageBox.Show("房台消费开台之后才能消费入单", "大海提示", MessageBoxButton.YesNo,MessageBoxImage.Warning);
                    return;
                }

                if (LRS[0].State_RoomStage.Trim() =="已用")
                {
                    FABM_Cost_of_inputs f = new FABM_Cost_of_inputs(LRS);
                    f.ShowDialog();
                }

            }
            else
            {
                MessageBox.Show("请选中房台", "大海提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 结账买单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_pay_the_bill_Click(object sender, RoutedEventArgs e)
        {
            if (!LRS.Equals(null))
            {
                if (LRS[0].State_RoomStage.Trim() == "停用")
                {
                    MessageBox.Show("房台已停用", "大海提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    return;
                }
                if (LRS[0].State_RoomStage.Trim() == "未用" || LRS[0].State_RoomStage.Trim() == "预定")
                {
                    MessageBox.Show("房台消费开台之后才能结账买单", "大海提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    return;
                }

                if (LRS[0].State_RoomStage.Trim() == "已用")
                {
                    FABM_Pay_the_bill f = new FABM_Pay_the_bill(LRS);
                    f.ShowDialog();


                }

            }
            else
            {
                MessageBox.Show("请选中房台", "大海提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            }
        }






        /************************ 我也是有底线的 ************************/
    }
}
