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
    /// HM_Front_desk_cashier_processing.xaml 的交互逻辑
    /// </summary>
    public partial class HM_Front_desk_cashier_processing : Window
    {
        /// <summary>
        /// 注册刷新父窗口的委托事件
        /// </summary>
        public event Refresh Resh;

        public HM_Front_desk_cashier_processing(List<SYS_RoomStage> rs)
        {

            mg = rs;
            InitializeComponent();
        }



        Model.EasternStar_WPF_MVVMEntities m = new EasternStar_WPF_MVVMEntities();

        List<SYS_RoomStage> mg;
        Stack<Consumer> sk_left = new Stack<Consumer>();
        Stack<Consumer> sk_right = new Stack<Consumer>();

        /// <summary>
        /// 右边集合
        /// </summary>
        ObservableCollection<Consumer> right = new ObservableCollection<Consumer>();

        /// <summary>
        /// 左边集合
        /// </summary>
        ObservableCollection<Consumer> left = new ObservableCollection<Consumer>();



        int ID_RoomStag = -1;



        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Lb_Consumer_housing_units.Content = mg[0].Number_RoomStage.Trim();
            ID_RoomStag = mg[0].ID_RoomStage;

            string nrs = mg[0].Number_RoomStage.Trim();
            //单号
            var ocn = from tc in m.CW_Consumption
                      join tb in m.CW_Bill on tc.ID_Bill equals tb.ID_Bill
                      join tr in m.SYS_RoomStage on tc.ID_RoomStage equals tr.ID_RoomStage
                      join tg in m.SYS_Guest on tr.ID_Guest equals tg.ID_Guest
                      join tv in m.VIP_Table on tg.ID_Guest equals tv.ID_Guest
                      where tc.Effective == true && tr.Number_RoomStage.Trim() == nrs
                      select new
                      {
                          tg.MC_Guest,
                          tb.Number_Bill,
                          tv.Accounts,

                      };

            string tg_nm = ocn.Select(c => c.MC_Guest).SingleOrDefault();
            string tb_nb = ocn.Select(c => c.Number_Bill).SingleOrDefault();
            string tv_ac = ocn.Select(c => c.Accounts).SingleOrDefault();
            Tb_odd_numbers.Text = "【" + tg_nm.Trim() + "】的账号【" + tv_ac.Trim() + "】的帐单号为【" + tb_nb.Trim() + "】的消费清单";

            Get_date_left();
            Get_date_right();
        }

        /// <summary>
        /// 获取数据 左边
        /// </summary>
        private void Get_date_left()
        {

            //左边表格 数据填充
            List<Consumer> p = (from tP in m.PJ_Project
                                join tPD in m.PJ_ProjectDetail on tP.ID_Project equals tPD.ID_Project
                                select new Consumer
                                {
                                    ID_Project = tP.ID_Project,
                                    ID_ProjectClass = tP.ID_ProjectClass,
                                    MC_Project = tP.MC_Project,
                                    Jc_Project = tP.Jc_Project,
                                    Unit = tP.Unit,
                                    Price = tPD.Price,
                                }).ToList();

            if (Tb_logogram.Text.Equals(string.Empty))
            {
                DG_The_famous_tea.ItemsSource = p.Where(c => c.ID_ProjectClass == 7).ToList();//名茶

                Dg_drinks.ItemsSource = p.Where(c => c.ID_ProjectClass == 5).ToList();//酒水

                Dg_seafood.ItemsSource = p.Where(c => c.ID_ProjectClass == 8).ToList();//酒水

                Dg_service_class.ItemsSource = p.Where(c => c.ID_ProjectClass == 9).ToList();//服务类

                Dg_meal.ItemsSource = p.Where(c => c.ID_ProjectClass == 11).ToList();//饭类

                Dg_Snacks_class.ItemsSource = p.Where(c => c.ID_ProjectClass == 12).ToList();//点心类

                DG_all.ItemsSource = p;
            }
            else
            {
                string condition = Tb_logogram.Text.Trim();

                Ti_all.IsSelected = true;

                //假如输入的是字符就装换成大写模式
                if (Tools.Tools.Isletter(condition))
                {
                    condition = condition.ToUpper();//返回字符串的大写~
                }
                //如果输入的是正整数
                if (Tools.Tools.IsInteger(condition))
                {
                    condition = Convert.ToDecimal(condition).ToString();
                }


                DG_all.ItemsSource = p.Where(c => c.Jc_Project.Trim().Contains(condition)
                || c.MC_Project.Trim().Contains(condition)
                || c.Unit.Trim().Contains(condition)
                || c.Price.ToString().Contains(condition)).ToList();
            }


        }

        /// <summary>
        /// 获取数据 右边
        /// </summary>
        private void Get_date_right()
        {
            //右边表格
            var consumption = m.CW_Consumption.Where(c => c.ID_RoomStage == ID_RoomStag && c.Effective == true).SingleOrDefault();
            int ID_consumption = consumption.ID_Consumption;


            List<Consumer> consumers = (from tbcd in m.CW_ConsumeDetail
                                        join tbp in m.PJ_Project on tbcd.ID_Project equals tbp.ID_Project
                                        join tbpd in m.PJ_ProjectDetail on tbp.ID_Project equals tbpd.ID_Project

                                        where tbcd.ID_Consumption == ID_consumption
                                        select new Consumer
                                        {
                                            ID_ConsumeDetail = tbcd.ID_ComsumeDetail,
                                            ID_Project = tbp.ID_Project,
                                            ID_Fangtai = ID_RoomStag,
                                            MC_Project = tbp.MC_Project,
                                            Unit = tbp.Unit,
                                            Time = consumption.Time_Consumption,
                                            Price = (decimal)tbcd.money,
                                            presenter = (bool)tbcd.presenter.Equals(null) || tbcd.presenter == false ? "否" : "是",
                                        }).ToList();

            //转换为动态数组
            foreach (var item in consumers)
            {
                right.Add(item);
            }
            Dg_right_data.ItemsSource = right;
            //消费总金额
            decimal total_money = m.CW_Consumption.Where(c => c.ID_RoomStage == ID_RoomStag && c.Effective == true).Single().Prict;



            DGTC_accruing_amounts.Header = total_money > 0 ? total_money : right.Where(c => c.presenter == "否").Select(p => p.Price).ToList().Sum();
        }


        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_addition_Click(object sender, RoutedEventArgs e)
        {
            Button tb = sender as Button;
            string _nm = tb.Name.Trim();

            if (_nm == "Bt_addition")
            {
                //添加按钮
                Accretion();

            }

            if (_nm == "Bt_Consumer_single_back")
            {
                //消费转单
                MessageBox.Show("暂不支持消费转单", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_nm == "Bt_Presented")
            {
                //结账退房

            }

            if (_nm == "Bt_Single_item_at_a_discount")
            {
                //消费退单

                Consumer_single_back();

            }

            if (_nm == "Bt_close_the_window")
            {
                //关闭窗口


            }




        }

        /// <summary>
        /// 消费退单
        /// </summary>
        private void Consumer_single_back()
        {
            try
            {
                //假如为空
                if (sk_right is null)
                {
                    MessageBox.Show("请在右边表格中选中需要消费退单的数据","大海提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                    return;
                }

            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// 添加按钮
        /// </summary>
        private void Accretion()
        {
            int _inum = -1;
            //添加
            try
            {
                string _snum = Tb_Consumption_quantity.Text.ToString().Trim();

                _inum = int.Parse(_snum);
                if (_inum <= 0)
                {
                    MessageBox.Show("请输入有效数字", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("请输入有效数字", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (sk_left == null)
            {
                MessageBox.Show("请选择要录入的数据", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int _icon_id = m.CW_Consumption.Where(c => c.ID_RoomStage == ID_RoomStag && c.Effective == true).Single().ID_Consumption;

            DateTime dt = DateTime.Now;

            for (int i = 0; i < _inum; i++)
            {

                CW_ConsumeDetail cd = new CW_ConsumeDetail();
                cd.ID_Consumption = _icon_id;
                cd.ID_Project = sk_left.Peek().ID_Project;
                cd.State_ComsumeDetail = true;
                cd.presenter = false;
                cd.money = sk_left.Peek().Price;
                cd.time = dt;
                m.CW_ConsumeDetail.Add(cd);
            }

            if (m.SaveChanges() > 0)
            {
                MessageBoxResult mbr = MessageBox.Show("客人消费录入成功", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                Window_Loaded(null, null);
                Tb_Consumption_quantity.Text = string.Empty;
            }
        }



        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_logogram_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Get_date_left();
        }

        /// <summary>
        /// 左边表格数据获取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DG_The_famous_tea_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            string[] dg_nam = new string[] { "DG_all", "DG_The_famous_tea", "Dg_drinks", "Dg_seafood", "Dg_service_class", "Dg_meal", "Dg_Snacks_class" };

            string dgname = dg.Name.Trim();
            Consumer con = dg.CurrentItem as Consumer;

            //如果存在于该数组中 则是左边表格的操作
            if (dg_nam.Contains(dgname))
            {
                sk_left.Push(con);
                return;
            }
            //右边表格操作
            if (dgname == "Dg_right_data")
            {
                sk_right.Push(con);
            }
        }








    }
}
