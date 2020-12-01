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

        }
        List<RoomStage> Rs;

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
        //表格里面的 空房数 放到 TabControl 里
        private void DgLeft_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
        /// <summary>
        /// 页面加载事件第三方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            DgLeft.ItemsSource = LeftOc;

            DgRight.ItemsSource = RightOc;


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

                /// string card_number = m.VIP_Table.Where(c => c.ID_Guest == (m.SYS_RoomStage.Where(r => r.ID_RoomStage == Rs[0].ID_RoomStage).FirstOrDefault().ID_Guest)).FirstOrDefault().Accounts;

                string card_number = m.VIP_Table.Where(c => c.ID_Guest == ID_Guest).FirstOrDefault().Accounts;

                Tb_account.Text = card_number.Trim();//贵宾卡号

                Cb_Founding_account.Visibility = Visibility.Collapsed;
                Tb_Founding_account.Visibility = Visibility.Visible;


                // Tb_Founding_account.Text = //账号


                //if (!Bargaining_unit.Equals(string.Empty))
                //{
                //    Tb_Founding_account.Text = Bargaining_unit.Trim();
                //}
                //else
                //{
                //    Tb_Founding_account.IsEnabled = true;
                //}


                var _str = from tbs in m.YW_Subscribe
                           join tbr in m.SYS_RoomStage on tbs.ID_Guest equals tbr.ID_Guest
                           join tbc in m.CW_Consumption on tbr.ID_RoomStage equals tbc.ID_RoomStage
                           join tbt in m.VIP_Table on tbs.ID_Guest equals tbt.ID_Guest
                           where tbr.ID_RoomStage == ID_RoomStage && tbs.State_Secrecy == true
                           select new Temporary
                           {


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

            Get_data(ID_Guest);
            Rs.Clear();

        }

        /// <summary>
        /// 过去数据
        /// </summary>
        /// <param name="id_guest"></param>
        private void Get_data(int? id_guest)
        {
            List<RoomStage> Date = (from tbr in m.SYS_RoomStage
                                    where tbr.ID_Class == STATIC_cache.ID_Class

                                    select new RoomStage
                                    {
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
            Tb_bargaining_unit.Text = m.AG_AgreementUser.Where(c => c.ID_AgreementUser == m.SYS_Guest.Where(global => global.ID_Guest == CbZhanghao_id.ID_Guest).FirstOrDefault().ID_AgreementUser).FirstOrDefault().MC_AgreementUser;

            //贵宾账号
            Tb_account.Text = m.VIP_Table.Where(c => c.ID_Guest == CbZhanghao_id.ID_Guest).FirstOrDefault().Accounts;



        }
    }
}
