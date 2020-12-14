using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF_MvvMTest.EntityVo;
using WPF_MvvMTest.Model;

namespace WPF_MvvMTest.View.FoodAndBeverageManagement.Windows
{
    /// <summary>
    /// FABM_founding_consumption.xaml 的交互逻辑
    /// </summary>
    public partial class FABM_founding_consumption : Window
    {
        Model.EasternStar_WPF_MVVMEntities m = new Model.EasternStar_WPF_MVVMEntities();

        /// <summary>
        /// 临时集合
        /// </summary>
        class Temporary
        {
            public string Number_Subscribe { get; set; }
            public int Number_People { get; set; }
            public decimal Discount { get; set; }
            public string Accounts { get; set; }
            public string Remark { get; set; }

            public int ID_RoomStage { get; set; }


        }

        List<RoomStage> Rs;

        int intdt = -1;//表格选择

        /// <summary>
        /// 左边集合
        /// </summary>
        ObservableCollection<RoomStage> LeftOc = new ObservableCollection<RoomStage>();
        /// <summary>
        /// 右边集合
        /// </summary>
        ObservableCollection<RoomStage> RightOc = new ObservableCollection<RoomStage>();
        /// <summary>
        /// 公共集合
        /// </summary>
        /// <param name="rooms"></param>
        ObservableCollection<RoomStage> common = new ObservableCollection<RoomStage>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rooms"></param>
        public FABM_founding_consumption(List<RoomStage> rooms)
        {
            if (!rooms.Equals(null))
            {
                Rs = rooms;
            }
            else
            {
                MessageBox.Show("请选择要操作的房台", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);

                // throw new NullReferenceException("rooms is null");
            }

            InitializeComponent();
        }

        /// <summary>
        /// 页面加载事件第三方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int ID_RoomStage = Rs[0].ID_RoomStage;

            int? ID_Guest = m.SYS_RoomStage.Where(c => c.ID_RoomStage == ID_RoomStage).SingleOrDefault().ID_Guest;

            int int_Effective_single = m.YW_OpenStage.Where(c => c.State_Message == true).ToList().Count + 1;

            string str_Effective_single = "KTD" + DateTime.Now.Year.ToString().Trim() + DateTime.Now.Month.ToString().Trim() + DateTime.Now.Day.ToString().Trim() + int_Effective_single;
            Tb_odd.Text = str_Effective_single;
            Tb_Founding_time.Text = DateTime.Now.ToString();//开台时间
            Tb_Room_table.Text = Rs[0].Number_RoomStage.Trim();//房台号
            Tb_Room_table_name.Text = Rs[0].MC_RoomStage.Trim();//房台名

            Cb_choice.IsChecked = true;//是否开台

            if (Rs[0].State_RoomStage.Trim() == "预定")
            {
                int ID_VIP = m.VIP_Table.Where(c => c.ID_Guest == ID_Guest).SingleOrDefault().ID_VIP;

                string ser = m.SYS_Guest.Where(c => c.ID_Guest == ID_Guest).FirstOrDefault().MC_Guest;

                //账号
                Tb_Founding_account.Text = ser.Trim();//账号

                string card_number = m.VIP_Table.Where(c => c.ID_Guest == ID_Guest).FirstOrDefault().Accounts;

                Tb_account.Text = card_number.Trim();//贵宾卡号

                Cb_Founding_account.Visibility = Visibility.Collapsed;
                Tb_Founding_account.Visibility = Visibility.Visible;

                var _str = from tbs in m.YW_Subscribe
                           join tbr in m.SYS_RoomStage on tbs.ID_Guest equals tbr.ID_Guest
                           join tbc in m.CW_Consumption on tbr.ID_RoomStage equals tbc.ID_RoomStage
                           join tbt in m.VIP_Table on tbs.ID_Guest equals tbt.ID_Guest
                           where tbr.ID_RoomStage == ID_RoomStage && tbs.State_Secrecy == true
                           select new Temporary
                           {

                               ID_RoomStage = tbr.ID_RoomStage,
                               Number_People = tbs.Number_People,//人数

                               Discount = tbc.Discount,//折扣

                               Accounts = tbt.Accounts,//账号

                               Remark = tbs.Remark,
                           };

                Tb_number_of_people.Text = _str.Select(c => c.Number_People).Single().ToString();//人数

                Tb_discount.Text = _str.Select(c => c.Discount).Single().ToString();//折扣

                //协议单位

                //card_number


                if (ID_Guest > 0)
                {
                    int? ID_AgreementUser = m.SYS_Guest.Where(g => g.ID_Guest == ID_Guest).FirstOrDefault().ID_AgreementUser;

                    if (!ID_AgreementUser.Equals(null))
                    {
                        string barit = m.AG_AgreementUser.Where(c => c.ID_AgreementUser == ID_AgreementUser).FirstOrDefault().MC_AgreementUser;

                        Tb_bargaining_unit.Text = barit.Trim();
                    }
                }
                else
                {
                    Tb_bargaining_unit.IsEnabled = true;
                    Tb_bargaining_unit.Opacity = 0.5;

                    tb_barit_text.IsEnabled = true;
                    tb_barit_text.Opacity = 0.5;
                }
            }

            if (Rs[0].State_RoomStage.Trim() == "未用")
            {
                Cb_Founding_account.Visibility = Visibility.Visible;
                Tb_Founding_account.Visibility = Visibility.Collapsed;

                var zhanghao = m.SYS_Guest.ToList();
                Cb_Founding_account.ItemsSource = zhanghao;
                Cb_Founding_account.DisplayMemberPath = "MC_Guest";
                Cb_Founding_account.SelectedValuePath = "ID_Guest";

            }

            Ti_optional.Header = "可选房台" + LeftOc.Count + "间";
            Ti_The_selected_room.Header = "已选房台" + RightOc.Count + "间";
            Get_data(ID_Guest);
            Rs.Clear();
            DgLeft.ItemsSource = LeftOc;

            DgRight.ItemsSource = RightOc;

            List<string> ts = new System.Collections.Generic.List<string>();
            ts.Add("早市");
            ts.Add("午市");
            ts.Add("晚市");
            ts.Add("夜市");
            Cb_to_city.ItemsSource = ts;



        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="id_guest"></param>
        private void Get_data(int? id_guest)
        {
            List<RoomStage> Date = (from tbr in m.SYS_RoomStage
                                    where tbr.ID_Class == STATIC_cache.ID_Class

                                    select new RoomStage
                                    {
                                        ID_Guest = tbr.ID_Guest,
                                        ID_RoomStage = tbr.ID_RoomStage,
                                        MC_RoomStage = tbr.MC_RoomStage,//名称

                                        Number_RoomStage = tbr.Number_RoomStage,//房台号


                                        State_RoomStage = tbr.State_RoomStage,

                                        Due_to_no = "否",



                                    }).ToList();


            //预约状态
            if (!id_guest.Equals(null))
            {
                string arr = m.YW_Subscribe.Where(c => c.ID_Guest == id_guest && c.State_Secrecy == true).FirstOrDefault().HouseStageID;

                string[] arrays = arr.Split(',');
                List<string> vs = new List<string>();
                foreach (var item in arrays)
                {
                    vs.Add(item.Trim());
                }

                foreach (var item in Date)
                {
                    if (vs.Contains(item.ID_RoomStage.ToString().Trim()))
                    {
                        RightOc.Add(item);
                    }
                    else
                    {
                        LeftOc.Add(item);
                    }
                }
            }

            //未定状态 
            if (id_guest.Equals(null))
            {
                Date = Date.Where(c => c.State_RoomStage.Trim() == "未用").ToList();
                foreach (var item in Date)
                {

                    if (item.ID_RoomStage == Rs[0].ID_RoomStage)
                    {
                        RightOc.Add(item);
                    }
                    else
                    {
                        LeftOc.Add(item);
                    }
                }
            }

        }


        /// <summary>
        /// 下拉框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cb_Founding_account_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_barit_text.IsEnabled = false;
            Tb_bargaining_unit.Opacity = 1;

            SYS_Guest CbZhanghao_id = Cb_Founding_account.SelectedItem as SYS_Guest;

            //协议单位
            if (CbZhanghao_id.ID_AgreementUser > 0)
            {
                Tb_bargaining_unit.IsEnabled = true;
                Tb_bargaining_unit.Text = m.AG_AgreementUser.Where(c => c.ID_AgreementUser == m.SYS_Guest.Where(global => global.ID_Guest == CbZhanghao_id.ID_Guest).FirstOrDefault().ID_AgreementUser).FirstOrDefault().MC_AgreementUser;
            }
            else
            {
                Tb_bargaining_unit.Text = string.Empty;
                Tb_bargaining_unit.IsEnabled = false;
            }
            //贵宾账号
            Tb_account.Text = m.VIP_Table.Where(c => c.ID_Guest == CbZhanghao_id.ID_Guest).FirstOrDefault().Accounts;

        }

        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_addition_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                RoomStage room = DgLeft.Items[intdt] as RoomStage;
                LeftOc.Remove(room);
                RightOc.Add(room);

            }
            catch (Exception)
            {

                MessageBox.Show("请选择要操作的房台", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


            Ti_optional.Header = "可选房台" + LeftOc.Count + "间";
            Ti_The_selected_room.Header = "已选房台" + RightOc.Count + "间";
        }

        /// <summary>
        /// 移除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RoomStage room = DgRight.Items[intdt] as RoomStage;
                LeftOc.Add(room);
                RightOc.Remove(room);

            }
            catch (Exception)
            {

                MessageBox.Show("请选择要操作的房台", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


            Ti_optional.Header = "可选房台" + LeftOc.Count;
            Ti_The_selected_room.Header = "已选房台" + RightOc.Count;
        }

        /// <summary>
        /// 左边表格点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgLeft_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            RoomStage room = DgLeft.CurrentItem as RoomStage;
            intdt = DgLeft.SelectedIndex;
        }

        /// <summary>
        /// 右边表格点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgRight_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            RoomStage room = DgRight.CurrentItem as RoomStage;
            intdt = DgRight.SelectedIndex;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_save_Click(object sender, RoutedEventArgs e)
        {

            //代码还有整合得空间，算了懒得去搞~
            if (Cb_Founding_account.Text.Equals(string.Empty) && Tb_Founding_account.Equals(string.Empty) || Tb_Room_table.Text.Equals(string.Empty) || Tb_Room_table_name.Text.Equals(string.Empty) || Tb_odd.Text.Equals(string.Empty) || Tb_Founding_time.Text.Equals(string.Empty) || Tb_number_of_people.Text.Equals(string.Empty) || Tb_discount.Text.Equals(string.Empty)  || Tb_account.Text.Equals(string.Empty) || Tb_postscript.Text.Equals(string.Empty))
            {
                MessageBox.Show("请将信息填写完整", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string hsid = string.Empty;

            //未定状态 
            if (Cb_Founding_account.IsVisible)
            {
                int? Guest = m.SYS_Guest.Where(c => c.MC_Guest.Trim() == Cb_Founding_account.Text).SingleOrDefault().ID_AgreementUser;

               
                foreach (var item in RightOc)
                {
                    hsid += item.ID_RoomStage + ",";


                    //账单表
                    CW_Bill b = new CW_Bill();
                    //b.ID_Bill = c.ID_Consumption;
                    b.Number_Bill = Tb_odd.Text.Trim();
                    m.CW_Bill.Add(b);
                    m.SaveChanges();
                  

                    //消费表
                    CW_Consumption c = new CW_Consumption();
                    c.ID_RoomStage = item.ID_RoomStage;
                    /// c.ID_Bill = b.ID_Bill;
                    //c.ID_Bill = m.CW_Bill.AsEnumerable().Last().ID_Bill;//查询表格中最后一条数据
                    c.ID_Bill = m.CW_Bill.Where(t => t.Number_Bill == Tb_odd.Text.Trim()
                    && t.State_Bill == null).Single().ID_Bill;
                    c.Discount = Convert.ToDecimal(Tb_discount.Text.Trim());
                    c.Effective = true;
                    m.CW_Consumption.Add(c);
                    m.SaveChanges();

                    
                    //房台
                    SYS_RoomStage r = m.SYS_RoomStage.Where(d => d.ID_RoomStage == item.ID_RoomStage).FirstOrDefault();
                    r.State_RoomStage = "已用";
                    r.ID_Guest = item.ID_Guest;
                    m.Entry(r).State = System.Data.Entity.EntityState.Modified;
                    m.SaveChanges();

                    


                };
                var ID_VIP = m.VIP_Table.Where(c => c.Accounts == Tb_account.Text.Trim()).FirstOrDefault().ID_VIP;
                //预约
                YW_OpenStage o = new YW_OpenStage();
                o.Number_People = int.Parse(Tb_number_of_people.Text.Trim());//人数
                o.Number_OpenStage = Tb_odd.Text.Trim();
                o.ID_VIP = ID_VIP;
                o.State_Secrecy = true;
                o.Remark = Tb_postscript.Text.Trim();
                o.Type_CheckIn = "餐饮";
                o.Content_Message = string.Empty;
                if (!Guest.Equals(null))
                {
                    o.ID_AgreementUser = (int)Guest;
                }
                o.HouseStageID = hsid;
                o.Time_Predict = Convert.ToDateTime(Tb_Founding_time.Text.Trim());
                m.YW_OpenStage.Add(o);

                if (m.SaveChanges() > 0)
                {
                    MessageBox.Show("开台成功", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Close();
                    return;
                }
            }

            //预定状态
            if (Tb_Founding_account.IsVisible)
            {           
                foreach (var item in RightOc)
                {
                    //查看该房台是否存在消费但
                    var date = m.CW_Consumption.Where(c => c.ID_RoomStage == item.ID_RoomStage && c.Effective == true).FirstOrDefault();
                    if (date.Equals(null))
                    {
                        CW_Bill b = new CW_Bill();
                      //  b.ID_Bill = c.ID_Consumption;
                        b.Number_Bill = Tb_odd.Text.Trim();
                        m.CW_Bill.Add(b);
                        m.SaveChanges();

                        CW_Consumption c = new CW_Consumption();
                        c.ID_RoomStage = item.ID_RoomStage;
                        c.Discount = Convert.ToDecimal(Tb_discount.Text.Trim());
                        c.ID_Bill = m.CW_Bill.Where(t => t.Number_Bill == Tb_odd.Text.Trim()
                   && t.State_Bill == null).Single().ID_Bill;
                        c.Effective = true;
                        m.CW_Consumption.Add(c);
                        m.SaveChanges();

                        
                    }
                    //修改房台信息
                    SYS_RoomStage r = m.SYS_RoomStage.Where(c => c.ID_RoomStage == item.ID_RoomStage && c.State_RoomStage.Trim() == "预定").SingleOrDefault();
                    r.ID_Guest = null;
                    r.State_RoomStage = "已用";
                    m.Entry(r).State = System.Data.Entity.EntityState.Modified;

                }
                var ID_VIP = m.VIP_Table.Where(c => c.Accounts == Tb_account.Text.Trim()).FirstOrDefault().ID_VIP;

                //修改预定信息
                YW_Subscribe w = m.YW_Subscribe.Where(c => c.ID_VIP == ID_VIP && c.State_Secrecy == true).SingleOrDefault();
                w.State_Secrecy = false;
                m.Entry(w).State = System.Data.Entity.EntityState.Modified;
                m.SaveChanges();

                YW_OpenStage o = new YW_OpenStage();
                o.Number_People = int.Parse(Tb_number_of_people.Text.Trim());//人数
                o.Number_OpenStage = Tb_odd.Text.Trim();
                o.ID_VIP = ID_VIP;
                o.State_Secrecy = true;
                o.Remark = Tb_postscript.Text.Trim();
                o.HouseStageID = hsid;
                o.Time_Predict = Convert.ToDateTime(Tb_Founding_time.Text.Trim());
                o.Type_CheckIn = "餐饮";
                o.Content_Message = string.Empty;
                m.YW_OpenStage.Add(o);

                if (m.SaveChanges() > 0)
                {
                    MessageBox.Show("开台成功", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Close();
                    return;
                }
            }
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
