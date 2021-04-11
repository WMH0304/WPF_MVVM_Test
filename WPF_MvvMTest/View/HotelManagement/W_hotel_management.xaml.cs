using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF_MvvMTest.EntityVo;
using WPF_MvvMTest.Model;
using WPF_MvvMTest.Resources;
using WPF_MvvMTest.View.HotelManagement.Windows;


namespace WPF_MvvMTest.View.HotelManagement
{


    /// <summary>
    /// W_hotel_management.xaml 的交互逻辑
    /// </summary>
    ///  
    public partial class W_hotel_management : Window
    {
        public W_hotel_management()
        {
            InitializeComponent();
        }

        Model.EasternStar_WPF_MVVMEntities m = new EasternStar_WPF_MVVMEntities();



        List<RoomStage> sr;

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //楼层下拉框
            var vis = (from tr in m.SYS_RoomStage
                       where tr.ID_Class == 3
                       select new
                       {
                           vare = tr.floor,
                       }).Distinct().ToList();

            List<string> vs = new List<string>();
            foreach (var item in vis)
            {
                // int it = item.ToString().Trim().Length;
                //vs.Add(item.ToString().Trim().Substring(10,2) +"楼");
                vs.Add(item.vare + "楼");
            }

            Cb_floor.ItemsSource = vs;

            /*******************/

            List<SYS_Room_status_type> rst = m.SYS_Room_status_type.ToList();

            Cb_Room_table_state.ItemsSource = rst;
            Cb_Room_table_state.DisplayMemberPath = "name_room_status_type";
            Cb_Room_table_state.SelectedValuePath = "ID_room_status_type";

            Get_date();//获取数据

            //  右边的傻逼状态回填显示
            Tb_complete.Text = sr.Count + "间";
            Tb_Empty_net_room.Text = sr.Where(c => c.room_status_type == "空净房").ToList().Count() + "间";
            Tb_housing.Text = sr.Where(c => c.room_status_type == "住房").ToList().Count() + "间";
            Tb_Dirty_room.Text = sr.Where(c => c.room_status_type == "脏房").ToList().Count() + "间";
            Tb_Maintenance_room.Text = sr.Where(c => c.room_status_type == "维修房").ToList().Count() + "间";
            Tb_hour_room.Text = sr.Where(c => c.room_status_type == "钟点房").ToList().Count() + "间";
            Tb_Since_the_housing.Text = sr.Where(c => c.room_status_type == "自用房").ToList().Count() + "间";
            Tb_team.Text = sr.Where(c => c.room_status_type == "团队").ToList().Count().ToString() + "间";
            Tb_Booking_room.Text = sr.Where(c => c.room_status_type == "预定房").ToList().Count() + "间";
            Tb_secret_room.Text = sr.Where(c => c.room_status_type == "保密房").ToList().Count() + "间";
            Tb_Lunch_room.Text = sr.Where(c => c.room_status_type == "午休房").ToList().Count() + "间";
            Tb_Need_to_clean_the.Text = sr.Where(c => c.room_status_type == "需打扫").ToList().Count() + "间";
            Tb_From_the_roomn_advance.Text = sr.Where(c => c.room_status_type == "预离房").ToList().Count() + "间";
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public void Get_date()
        {
            if (sr != null)
            {
                sr.Clear();

            }

            sr = (from tr in m.SYS_RoomStage
                  join trst in m.SYS_Room_status_type on tr.ID_room_type equals trst.ID_room_status_type
                  where tr.ID_Class == 3
                  select new RoomStage
                  {
                      ID_RoomStage = tr.ID_RoomStage,
                      ID_Guest = tr.ID_Guest,
                      ID_room_status_type = trst.ID_room_status_type,
                      room_status_type = trst.name_room_status_type.Trim(),
                      Number_RoomStage = tr.Number_RoomStage.Trim(),
                      State_RoomStage = tr.State_RoomStage.Trim(),
                      Describe = tr.Describe.Trim(),
                      MC_RoomStage = tr.MC_RoomStage.Trim(),
                      floor = tr.floor,
                  }).ToList();

            string floor = Cb_floor.Text.Trim();//楼层下拉框内容
            string fuzzy_query = Tb_fuzzy_query.Text.Trim();//查询按钮
            string room_table_state = Cb_Room_table_state.Text.Trim();//房间状态

            //楼层筛选下拉框
            if (floor != string.Empty)
            {
                int? int_floor = int.Parse(floor.Substring(0, 1));

                sr = sr.Where(c => c.floor == int_floor).ToList();
                Cb_floor.Text = string.Empty;
            }

            //那个查询按钮
            if (fuzzy_query != string.Empty)
            {
                sr = sr.Where(c => c.room_status_type.Contains(fuzzy_query) || c.Number_RoomStage.Contains(fuzzy_query) || c.State_RoomStage.Contains(fuzzy_query) || c.MC_RoomStage.Contains(fuzzy_query)).ToList();
            }

            //房子状态赛选下拉框
            if (room_table_state != string.Empty)
            {
                sr = sr.Where(c => c.room_status_type.Trim() == room_table_state).ToList();
                Cb_Room_table_state.Text = string.Empty;
            }

            //动态添加按钮
            Bt_fill(sr);
        }

        /// <summary>
        /// 房台按钮填充
        /// </summary>
        /// <param name="stages"></param>
        public void Bt_fill(List<RoomStage> stages)
        {
            if (Wp_Room_table_diagram_display.Children != null)
            {
                Wp_Room_table_diagram_display.Children.Clear();
            }

            foreach (var item in stages)
            {
                UC_Btn_hotel_room_availability uBt = new UC_Btn_hotel_room_availability();
                uBt.Name = "Bt_" + item.ID_RoomStage;
                uBt.Tag = item.ID_RoomStage;
                uBt.Tb_room_id.Text = item.ID_RoomStage.ToString().Trim();
                uBt.TbRoom_class.Text = item.MC_RoomStage.Trim().ToString();
                uBt.TbRoom_num.Text = item.Number_RoomStage.Trim().ToString();
                if (item.State_RoomStage.Trim() == "已用")
                {
                    uBt.Tb_resere.Visibility = Visibility.Visible;
                }
                if (item.State_RoomStage.Trim() == "预定")
                {
                    uBt.Tb_reserve.Visibility = Visibility.Visible;
                }
                //List<UC_Btn_hotel_room_availability> ls = new List<UC_Btn_hotel_room_availability>();
                //ls.Add(uBt);

                #region MyRegion

                #endregion
                switch (item.ID_room_status_type)
                {
                    case 1:
                        //空净房
                        uBt.Background = System.Windows.Media.Brushes.Green;
                        break;
                    case 2:
                        //住房        
                        uBt.Background = System.Windows.Media.Brushes.SkyBlue;
                        break;
                    case 3:
                        //脏房
                        uBt.Background = System.Windows.Media.Brushes.Purple;
                        break;
                    case 4:
                        //维修房
                        uBt.Background = System.Windows.Media.Brushes.Blue;
                        break;
                    case 5:
                        //钟点房
                        uBt.Background = System.Windows.Media.Brushes.AliceBlue;
                        break;
                    case 6:
                        //自用房
                        uBt.Background = System.Windows.Media.Brushes.AntiqueWhite;
                        break;
                    case 7:
                        //团队
                        uBt.Background = System.Windows.Media.Brushes.Aqua;
                        break;
                    case 8:
                        //预定房
                        uBt.Background = System.Windows.Media.Brushes.Aquamarine;
                        break;
                    case 9:
                        //保密房
                        uBt.Background = System.Windows.Media.Brushes.Azure;
                        break;
                    case 10:
                        //午休房
                        uBt.Background = System.Windows.Media.Brushes.Beige;
                        break;
                    case 11:
                        //需打扫
                        uBt.Background = System.Windows.Media.Brushes.Bisque;
                        break;
                    case 12:
                        //预离房
                        uBt.Background = System.Windows.Media.Brushes.BlueViolet;
                        break;
                    default:
                        MessageBox.Show("客房类型不存在", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                }

                Wp_Room_table_diagram_display.Children.Add(uBt);
                //Wp_Room_table_diagram_display = VisualScrollableAreaClip.


            }
        }

        /// <summary>
        /// 下拉框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cb_floor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox s = (ComboBox)sender;

            var ss = s.SelectedItem;
            if (ss == null)
            {
                return;
            }
            //左边的
            if (s.Name == "Cb_floor")
            {

                Cb_floor.Text = ss.ToString();
                Get_date();
            }

            if (s.Name == "Cb_Room_table_state")
            {
                SYS_Room_status_type t = ss as SYS_Room_status_type;
                Cb_Room_table_state.Text = t.name_room_status_type;
                Get_date();

            }

        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_Real_time_room_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            string con = bt.Content.ToString().Trim();
            string nm = bt.Name.ToString().Trim();
            string _room_sr = m.SYS_RoomStage.Where(c => c.ID_RoomStage == STATIC_cache.ID_RoomStage).Single().State_RoomStage.Trim();

            if (con == "实时房态")
            {
                Window_Loaded(null, null);
            }

            if (con == "查询")
            {
                Get_date();
            }

            if (nm == "btAddButton")
            {
                //房台新增按钮
                Windows.HM_Add_room d = new Windows.HM_Add_room();
                d.btq += new Bt(push);
                d.ShowDialog();
            }

            if (nm == "Bt_the_guest_book")
            {
                //客人预定
                if (STATIC_cache.ID_RoomStage == 0)
                {
                    MessageBox.Show("请选中需要预定的房间", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_room_sr != "未用")
                {
                    MessageBox.Show("该房台正在使用，请选择其他房台", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Windows.HM_The_guest_book tb = new HM_The_guest_book(nm);
                tb.refresh += new Refresh(push);
                tb.ShowDialog();
            }

            if (nm == "Bt_The_guest_registration")
            {
                //客人登记
                if (STATIC_cache.ID_RoomStage == 0 || _room_sr == "已用")
                {
                    MessageBox.Show("请选中需要预定的房间", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Windows.HM_The_guest_book tb = new HM_The_guest_book(nm);
                tb.refresh += new Refresh(push);
                tb.ShowDialog();

            }

            if (nm == "Bt_guest_delay")
            {
                //客人续住

                if (STATIC_cache.ID_RoomStage == 0 || _room_sr.Trim() != "已用")
                {
                    MessageBox.Show("请选中需要续住的房间", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Windows.HM_The_guest_book tb = new HM_The_guest_book(nm);
                tb.refresh += new Refresh(push);
                tb.ShowDialog();
            }

            if (nm == "Bt_consume_entrance")
            {
                //消费入单
                if (STATIC_cache.ID_RoomStage == 0 || _room_sr.Trim() != "已用")
                {
                    MessageBox.Show("请选中需要消费入单的房间", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                List<SYS_RoomStage> rs = m.SYS_RoomStage.Where(c => c.ID_RoomStage == STATIC_cache.ID_RoomStage).ToList();
                if (rs == null)
                {
                    MessageBox.Show("房台信息丢失请找大海", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                HM_Front_desk_cashier_processing fdcp = new HM_Front_desk_cashier_processing(rs);
                fdcp.Resh += new Refresh(push);
                //fdcp.Resh -= new Refresh(push);
                fdcp.ShowDialog();
            }

            //财务处理
            if (nm== "Bt_financial_transaction")
            {
                if (STATIC_cache.ID_RoomStage ==0 || _room_sr.Trim()!="已用")
                {
                    MessageBox.Show("请选中需要结账消费的房间", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                List<SYS_RoomStage> rs = m.SYS_RoomStage.Where(c => c.ID_RoomStage == STATIC_cache.ID_RoomStage).ToList();
                if (rs ==null)
                {
                    MessageBox.Show("房台信息丢失请找大海", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                HM_Check_out_room cr = new HM_Check_out_room(STATIC_cache.ID_RoomStage);


               // Action<string> action; 
                cr.ShowDialog();


            }

        }

        /// <summary>
        /// 页面刷新
        /// </summary>
        public void push()
        {

            Window_Loaded(null, null);


        }

    }
}
