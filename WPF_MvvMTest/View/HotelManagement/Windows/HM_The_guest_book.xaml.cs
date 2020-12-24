
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF_MvvMTest.EntityVo;
using WPF_MvvMTest.Model;


namespace WPF_MvvMTest.View.HotelManagement.Windows
{


    /// <summary>
    /// HM_The_guest_book.xaml 的交互逻辑
    /// </summary>
    public partial class HM_The_guest_book : Window
    {

        public event Refresh refresh;
        Model.EasternStar_WPF_MVVMEntities m = new EasternStar_WPF_MVVMEntities();




        public HM_The_guest_book(string base_name)
        {
            if (base_name == "Bt_the_guest_book")
            {
                //顾客预定
                whether = true;
            }

            if (base_name == "Bt_The_guest_registration")
            {
                //客人登记
                whether = false;
            }

            InitializeComponent();

        }

        int ID_RoomStage, ID_Guest, ID_VIP, ID_AgreementUser;
        string ck_tag;
        bool whether;

        /// <summary>
        /// 左边表格
        /// </summary>
        ObservableCollection<RoomStage> left = new ObservableCollection<RoomStage>();

        /// <summary>
        /// 右边表格
        /// </summary>
        ObservableCollection<RoomStage> right = new ObservableCollection<RoomStage>();

        /// <summary>
        /// 临时集合
        /// </summary>
        ObservableCollection<RoomStage> temporary = new ObservableCollection<RoomStage>();

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Tb_confidentiality.Visibility = Visibility.Hidden;
            Cb_Ifasecret.Visibility = Visibility.Hidden;


            #region 下拉框

            //主客姓名
            Cb_The_customers_name.ItemsSource = m.SYS_Guest.ToList();
            Cb_The_customers_name.DisplayMemberPath = "MC_Guest";
            Cb_The_customers_name.SelectedValuePath = "ID_Guest";

            //协议单位
            Cb_bargaining_unit.ItemsSource = m.AG_AgreementUser.ToList();
            Cb_bargaining_unit.DisplayMemberPath = "MC_AgreementUser";
            Cb_The_customers_name.SelectedValuePath = "ID_AgreementUser";

            ID_RoomStage = STATIC_cache.ID_RoomStage;

            SYS_RoomStage s = m.SYS_RoomStage.Where(c => c.ID_RoomStage == ID_RoomStage).SingleOrDefault();
            Tb_room_number.Text = s.Number_RoomStage;
            Tb_the_room.Text = s.MC_RoomStage;
            Tb_the_preset_prices.Text = s.preinstall.ToString();

            #endregion

            //客人登记回填
            if (!whether)
            {

            }



            Get_data();
            DgLeft.ItemsSource = left;
            DgRight.ItemsSource = right;
        }

        /// <summary>
        /// 获取表格数据
        /// </summary>
        private void Get_data()
        {
            List<RoomStage> roomStage = (from tr in m.SYS_RoomStage
                                         where tr.ID_Class == 3 && tr.ID_room_type == 1
                                         select new RoomStage
                                         {
                                             ID_RoomStage = tr.ID_RoomStage,
                                             ID_Class = tr.ID_Class,
                                             ID_room_status_type = tr.ID_room_type,
                                             Number_RoomStage = tr.Number_RoomStage,
                                             MC_RoomStage = tr.MC_RoomStage,
                                             preinstall = tr.preinstall,
                                             practical = tr.practical
                                         }).ToList();

            if (roomStage == null)
            {
                MessageBox.Show("无房台", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            foreach (var item in roomStage)
            {
                //item.Number_RoomStage == Tb_room_number.Text.Trim() ? right.Add(item) : left.Add(item);


                if (item.Number_RoomStage.Trim() == Tb_room_number.Text.Trim())
                {
                    right.Add(item);
                }
                else
                {
                    left.Add(item);

                }
            }


        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtAddition_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;

            string name = bt.Name.Trim();
            string con = bt.Content.ToString().Trim();

            if (name == "Bt_The_page_to_narrow")
            {
                //页面最小化
                this.WindowState = WindowState.Minimized;
            }

            if (name == "Bt_Page_maximization")
            {
                //页面最大化
                this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                #region MyRegion
                //if (this.WindowState == WindowState.Maximized)
                //{
                //    this.WindowState = WindowState.Normal;
                //}
                //else
                //{
                //    this.WindowState = WindowState.Maximized;
                //}
                #endregion
            }

            if (name == "Bt_close")
            {
                //关闭
                this.Close();
            }

            if (name == "BtAddition")
            {
                //添加
                Append_or_remove(name);
            }

            if (name == "BtRemove")
            {
                //移除
                Append_or_remove(name);
            }

            if (name == "Bt_save")
            {
                //保存
                Save();
            }

        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            if (!Integrity())
            {
                return;
            }

            //修改房台状态
            SYS_RoomStage sr = m.SYS_RoomStage.Where(c => c.ID_RoomStage == ID_RoomStage).SingleOrDefault();
            sr.State_RoomStage = "预定";
            sr.ID_Guest = ID_Guest;
            sr.ID_room_type = int.Parse(ck_tag);
            m.Entry(sr).State = System.Data.Entity.EntityState.Modified;

            //添加预定信息
            YW_Subscribe sb = new YW_Subscribe();
            sb.ID_Guest = ID_Guest;
            sb.ID_VIP = ID_VIP;
            sb.ID_AgreementUser = ID_AgreementUser;
            sb.Number_Subscribe = Tb_Deposit_receipt_no.Text.Trim().ToString();
            sb.Time_Predict = DateTime.Parse(Dp_the_date_of_arrival.Text.Trim());
            sb.Time_Leave = DateTime.Parse(Dp_departure_time.Text.Trim());
            sb.Number_People = int.Parse(Tb_number_of_people.Text.Trim());
            sb.Money_Pledge = decimal.Parse(Tb_The_first_deposit.Text.Trim());
            sb.State_Secrecy = true;
            sb.Remark = Tb_note.Text.Trim();
            sb.Type_CheckIn = ck_tag;

            string _ID_houseID = string.Empty;
            foreach (var item in right)
            {
                _ID_houseID += item.ID_RoomStage + ",";
            }
            sb.HouseStageID = _ID_houseID;
            m.YW_Subscribe.Add(sb);

            m.SaveChanges();


            //int _IDsub = m.YW_Subscribe.Where(c => c.Number_Subscribe == sb.Number_Subscribe).Single().ID_Subscribe;\\
            // int _IDsub = m.YW_Subscribe.Select(c => sb).Single().ID_Subscribe;

            int _IDsub = m.YW_Subscribe.Where(c => c.Number_Subscribe == sb.Number_Subscribe && c.State_Secrecy == true).Single().ID_Subscribe;



            //   m.YW_Subscribe.Add().ID_Subscribe;

            //添加账单信息 预定押金的
            CW_Bill cb = new CW_Bill();
            cb.Price = decimal.Parse(Tb_The_first_deposit.Text.Trim());
            cb.SuOp_ID = _IDsub;
            cb.Number_Bill = Tb_Deposit_receipt_no.Text.Trim();
            cb.State_Bill = "未结账";
            m.CW_Bill.Add(cb);
            m.SaveChanges();

            int _IDbill = m.CW_Bill.Where(c => c.Number_Bill == cb.Number_Bill).Single().ID_Bill;

            //添加消费记录
            CW_Consumption cp = new CW_Consumption();
            cp.ID_Bill = _IDbill;
            cp.ID_RoomStage = ID_RoomStage;
            cp.ID_ProjectDetail = 0;
            cp.Prict = decimal.Parse(Tb_The_first_deposit.Text.Trim());
            cp.Discount = 1;
            cp.Effective = true;
            m.CW_Consumption.Add(cp);

            if (m.SaveChanges() > 0)
            {
                //Tools.Common_means.operate_successfully("房台预定");

                MessageBoxResult result = MessageBox.Show("房台预定成功", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    this.Close();
                    refresh();
                }
            }






        }

        /// <summary>
        /// 检测页面数据是否完整
        /// </summary>
        public bool Integrity()
        {
            /*
             FrameworkElement 是继承了 UIElement  的wpf 核心类
             */
            //遍历grid 内所有 控件 
            foreach (FrameworkElement item in Gr_con.Children)
            {
                /*
                 * 如果使文本框就判断是否为空，如果内容为空就全部返回
                 */
                if (item is TextBox)
                {
                    TextBox tb = item as TextBox;
                    if (tb.Text == string.Empty)
                    {
                        MessageBox.Show("请输入完整信息", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }

                if (item is ComboBox)
                {
                    ComboBox cb = item as ComboBox;
                    if (cb.Text == string.Empty)
                    {
                        MessageBox.Show("请输入完整下拉框信息", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }

                if (item is DatePicker)
                {
                    DatePicker dp = (DatePicker)item;
                    if (dp.Text == string.Empty)
                    {
                        MessageBox.Show("请输入完整日期", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// 添加 和移除 按钮
        /// </summary>
        public void Append_or_remove(string str)
        {

            if (str != string.Empty && str == "BtAddition")
            {
                //添加按钮
                left.Remove(temporary[0]);
                right.Add(temporary[0]);
            }

            if (str != string.Empty && str == "BtRemove")
            {
                // 移除按钮
                left.Add(temporary[0]);
                right.Remove(temporary[0]);
            }

            if (str == String.Empty)
            {
                MessageBox.Show("请选择要添加或者移除的按钮，然后进行操作", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            left.OrderBy(c => c.Number_RoomStage);
            right.OrderBy(c => c.Number_RoomStage);

        }

        /// <summary>
        /// 下拉框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cb_The_customers_name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ComboBox cb = sender as ComboBox;
            ComboBox cb = (ComboBox)sender;
            string cb_name = cb.Name.Trim().ToString();


            if (cb_name == "Cb_The_customers_name")
            {
                //主客姓名
                //var v =   cb.SelectedItem.ToString();
                SYS_Guest sg = Cb_The_customers_name.SelectedItem as SYS_Guest;

                ID_Guest = sg.ID_Guest;

                SYS_Guest guest = m.SYS_Guest.Where(c => c.ID_Guest == sg.ID_Guest).SingleOrDefault();

                Cb_gender.Text = guest.gender.Trim().ToString();
                Cb_certificate_type.Text = guest.证件类型.Trim().ToString();
                Tb_ID_Number.Text = guest.证件号码.Trim().ToString();
                Tb_site.Text = guest.Address.Trim().ToString();

                VIP_Table vt = m.VIP_Table.Where(c => c.ID_Guest == sg.ID_Guest).SingleOrDefault();
                Tb_account_number.Text = vt.Accounts.Trim().ToString();

                Tb_The_VIP_card_number.Text = vt.Accounts.Trim().ToString();

                ID_VIP = vt.ID_VIP;

                int num = m.YW_Subscribe.Count() + 1;
                Tb_Deposit_receipt_no.Text = "JDYD" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + num.ToString();
            }

            //协议单位
            if (cb_name == "Cb_bargaining_unit")
            {
                AG_AgreementUser ag = Cb_bargaining_unit.SelectedItem as AG_AgreementUser;
                ID_AgreementUser = ag.ID_AgreementUser;

            }



        }

        /// <summary>
        /// 计算天数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (Dp_the_date_of_arrival.Text == string.Empty)
            {
                MessageBox.Show("抵店日期不能为空", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // DateTime.Compare((DateTime)Dp_departure_time.SelectedDate, (DateTime)Dp_the_date_of_arrival.SelectedDate) > 0 ? Tb_number_of_days.Text = DateTime.Compare((DateTime)Dp_departure_time.SelectedDate, (DateTime)Dp_the_date_of_arrival.SelectedDate).ToString() : MessageBox.Show("抵店日期必须小于或等于离店日期","大海提示",MessageBoxButton.OK,MessageBoxImage.Warning);

            int days = Dp_departure_time.SelectedDate.Value.Day - Dp_the_date_of_arrival.SelectedDate.Value.Day;
            if (days > 0)
            {
                Tb_number_of_days.Text = days.ToString();
            }
            if (days == 0)
            {
                Tb_number_of_days.Text = "1";
            }
            if (days < 0)
            {
                MessageBox.Show("抵店日期必须小于或等于离店日期", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 选中表格数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgLeft_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid data = sender as DataGrid;

            string name = data.Name.Trim();
            if (name == "DgLeft" && data.CurrentItem != null)
            {
                RoomStage room = DgLeft.CurrentItem as RoomStage;
                temporary.Clear();
                temporary.Add(room);
            }
            if (name == "DgRight" && data.CurrentItem != null)
            {
                RoomStage room = DgRight.CurrentItem as RoomStage;
                temporary.Clear();
                temporary.Add(room);
            }



        }

        /// <summary>
        /// 单选框点击互斥事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cb_individual_traveler_Click(object sender, RoutedEventArgs e)
        {

            CheckBox box = sender as CheckBox;
            string box_name = box.Name.Trim();
            ck_tag = box.Tag.ToString().Trim();
            foreach (FrameworkElement item in Gr_con.Children)
            {
                if (item is CheckBox)
                {
                    if (item.Name != box_name)
                    {
                        CheckBox box_test = item as CheckBox;
                        box_test.IsChecked = false;
                    }
                }

            }

        }

        /// <summary>
        /// 是否留言 禁用和启用留言文本框-
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cb_whether_need_Click(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;

            Tb_message_contents.IsEnabled = box.IsChecked == true ? true : false;


        }
    }
}
