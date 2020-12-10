using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF_MvvMTest.EntityVo;
using WPF_MvvMTest.Model;
using WPF_MvvMTest.Tools;
namespace WPF_MvvMTest.View.FoodAndBeverageManagement.Windows
{
    /// <summary>
    /// FABM_Cost_of_inputs.xaml 的交互逻辑
    /// </summary>
    public partial class FABM_Cost_of_inputs : Window
    {
        ///// <summary>
        ///// 临时集合左边表格
        ///// </summary>
        //public partial class Project
        //{
        //    public int ID_Project { get; set; }
        //    public int ID_ProjectClass { get; set; }
        //    /// <summary>
        //    /// 项目名称
        //    /// </summary>
        //    public string MC_Project { get; set; }
        //    /// <summary>
        //    /// 项目编号
        //    /// </summary>
        //    public string Jc_Project { get; set; }
        //    /// <summary>
        //    /// 单位
        //    /// </summary>
        //    public string Unit { get; set; }
        //    /// <summary>
        //    /// 单价
        //    /// </summary>
        //    public decimal Price { get; set; }
        //    /// <summary>
        //    /// 折扣
        //    /// </summary>
        //    public decimal discount { get; set; }
        //    /// <summary>
        //    /// 金额
        //    /// </summary>
        //    public decimal money { get; set; }
        //    /// <summary>
        //    /// 时间
        //    /// </summary>
        //    public DateTime time { get; set; }

        //}


        /// <summary>
        /// 接收房台信息的傻逼集合
        /// </summary>
         List<RoomStage> Rs;
        /// <summary>
        /// 用于填充右边动态集合的集合
        /// </summary>
        List<Consumer> projects = new List<Consumer>();
        /// <summary>
        /// 右边表格动态表
        /// </summary>
        ObservableCollection<Consumer> op = new ObservableCollection<Consumer>();

        /// <summary>
        /// 临时集合
        /// </summary>
        ObservableCollection<Consumer> temporary = new ObservableCollection<Consumer>();

        Model.EasternStar_WPF_MVVMEntities m = new EasternStar_WPF_MVVMEntities();
        public FABM_Cost_of_inputs(List<RoomStage> room)
        {
            Rs = room;
            InitializeComponent();
        }
        int ID_Room = -1;
        int? ID_Guest = -1;
        int Dg_intselect = -1;
        string Dg_left_name = string.Empty;
        int ID_consumption = -1;

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Lb_Consumer_housing_units.Content = Rs[0].MC_RoomStage + " && " + Rs[0].ID_RoomStage;  //消费房台
            ID_Guest = Rs[0].ID_Guest;
            ID_Room = Rs[0].ID_RoomStage;

            //右上角单号账号信息
            Tb_odd_numbers.Text = string.Empty;
            //左边表格数据获取
            Get_data();
            #region MyRegion
            //右边表格
            //List<Consumer> con = (from tC in m.CW_Consumption //消费表
            //                      join tR in m.SYS_RoomStage on tC.ID_RoomStage equals tR.ID_RoomStage
            //                      join tCD in m.CW_ConsumeDetail on tC.ID_Consumption equals tCD.ID_Consumption  //消费明细
            //                      join tP in m.PJ_Project on tCD.ID_Project equals tP.ID_Project  //项目表
            //                      join tPD in m.PJ_ProjectDetail on tP.ID_Project equals tPD.ID_Project //项目明细
            //                      where tC.ID_RoomStage == ID_Room
            //                      select new Consumer
            //                      {
            //                          ID_Fangtai = tC.ID_RoomStage,
            //                          ID_Project = tP.ID_Project,
            //                          ID_ConsumeDetail = tCD.ID_ComsumeDetail,
            //                          Number_RoomStage = tR.Number_RoomStage,//房台号
            //                          MC_Project = tP.MC_Project,//项目名称
            //                          Unit = tP.Unit,//单位
            //                          Price = tPD.Price,//单价 or 单价
            //                          Mun = "1",//数量 
            //                          Discount = tC.Discount,//折扣
            //                          Total_money = +tPD.Price,
            //                      }).ToList();
            // var rigth = m.CW_ConsumeDetail.Where(c=>c.ID_Consumption ==)
            // var right = from tbc in m.CW_ConsumeDetail
            #endregion
            //右边表格数据获取
            Get_data_right();

            //DGTC_accruing_amounts.Header = op.Where(c => c.presenter == "否").Select(p => p.Price).ToList().Sum();

            //op.Select(c => c.Price).ToList().Sum();

        }

        /// <summary>
        /// 获取数据 左边
        /// </summary>
        public void Get_data()
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
        /// 获取数据右边
        /// </summary>
        public void Get_data_right()
        {
            //右边表格
            var consumption = m.CW_Consumption.Where(c => c.ID_RoomStage == ID_Room && c.Effective == true).SingleOrDefault();
            ID_consumption = consumption.ID_Consumption;


            List<Consumer> consumers = (from tbcd in m.CW_ConsumeDetail
                                        join tbp in m.PJ_Project on tbcd.ID_Project equals tbp.ID_Project
                                        join tbpd in m.PJ_ProjectDetail on tbp.ID_Project equals tbpd.ID_Project

                                        where tbcd.ID_Consumption == ID_consumption
                                        select new Consumer
                                        {
                                            ID_ConsumeDetail = tbcd.ID_ComsumeDetail,
                                            ID_Project = tbp.ID_Project,
                                            ID_Fangtai = ID_Room,
                                            MC_Project = tbp.MC_Project,
                                            Unit = tbp.Unit,
                                            Time = consumption.Time_Consumption,
                                            Price = (decimal)tbcd.money,
                                            presenter = (bool)tbcd.presenter.Equals(null) || tbcd.presenter == false ? "否" : "是",
                                        }).ToList();

            //转换为动态数组
            foreach (var item in consumers)
            {
                op.Add(item);
            }
            Dg_right_data.ItemsSource = op;
            //消费总金额
            decimal total_money = m.CW_Consumption.Where(c => c.ID_RoomStage == ID_Room && c.Effective == true).Single().Prict;



            DGTC_accruing_amounts.Header = total_money > 0 ? total_money : op.Where(c => c.presenter == "否").Select(p => p.Price).ToList().Sum();
        }

        /// <summary>
        /// 那些个按钮的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_addition_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            string button_name = b.Name.Trim();
            string btn_content = b.Content.ToString().Trim();
            //添加按钮
            if (button_name == "Bt_addition")
            {
                Addition("Bt_addition");
            }
            //消费退单
            if (button_name == "Bt_Consumer_single_back")
            {

                Consumer_single_back();
            }
            //赠送
            if (button_name == "Bt_Presented")
            {
                // Bt_Presented
                //string y = b.Content.ToString().Trim();
                Presented(b.Content.ToString().Trim());
            }
            //删除消费
            if (button_name == "Bt_Delete_the_consumption")
            {
                Delete_the_consumption(b.Content.ToString().Trim());
            }
            //结账买单
            if (button_name == "Bt_settle_accounts")
            {

                FABM_Pay_the_bill p = new FABM_Pay_the_bill(Rs);
                p.ShowDialog();

            }
            //消费转单
            if (button_name == "Bt_Consumer_turn_single")
            {

                MessageBox.Show("本店暂不支持消费转单，因为我不知道具体流程是什么~举个栗子，你是单项消费转单呢，还是消费转单呢？你是转到同客户的其它房台呢还是转到协议消费呢，计划书没说~就一个做消费转单的需求，你让我很难搞呀", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //打折
            if (button_name == "Bt_discount")
            {
                Discount(btn_content);
            }
            //单项打折
            if (button_name == "Bt_Single_item_at_a_discount")
            {
                Single_item_at_a_discount(btn_content);
            }
            //关闭窗口
            if (button_name == "Bt_close_the_window")

            {
                // Bt_close_the_window
                Close_the_window(btn_content);

            }
        }

        /// <summary>
        /// 获取选中行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DG_The_famous_tea_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //获取当控件 将 sender 逆变 为 datagrid
            DataGrid dg = (DataGrid)sender;//通过 sender 获取控件  sender 类似于指针，指向某个事件 obj类型，所有类的基类都是obj
            Dg_left_name = dg.Name;//获取值，这步没必要，因为sender 已经指向了当前控件
            Consumer p = dg.CurrentItem as Consumer;
            Dg_intselect = dg.SelectedIndex;
            // op.Add(p);
            projects.Clear();
            projects.Add(p);
            projects[0].Mun = "1";
            projects[0].Discount = 1.0M;
            projects[0].ID_Fangtai = ID_Room;
            projects[0].Time = DateTime.Now;


        }

        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_logogram_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!Tb_logogram.Text.Equals(null))
            {
                Get_data();
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void Close_the_window(string str)
        {
            MessageBoxResult r = MessageBox.Show("确定要关闭窗口吗？", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (MessageBoxResult.Cancel == r)
            {
                return;
            }

            if (MessageBoxResult.OK == r)
            {
                this.Close();
            }



        }

        /// <summary>
        /// 单项打折
        /// </summary>
        /// <param name="str"></param>
        private void Single_item_at_a_discount(string str)
        {
            if (!Tb_discount.IsVisible && str.Trim() == "单项打折")
            {
                Bt_Consumer_turn_single.Visibility = Visibility.Hidden;//隐藏目标按钮
                Tb_discount.Visibility = Visibility.Visible;//显示元素
                return;
            }
            //  temporary.Count == 0 ? MessageBox.Show("请选择需要打折的消费", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning) : temporary.Clear();
            if (temporary.Count == 0)
            {
                MessageBox.Show("请选择需要打折的消费", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //判断折扣
            if (!WPF_MvvMTest.Tools.Tools.IsDiscount(Tb_discount.Text.Trim()))
            {
                MessageBox.Show("请输入正确的折扣，只能在 1~10 之间", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            decimal dis = decimal.Parse(Tb_discount.Text.Trim()) / 10;
            decimal mon = 0;
            foreach (var item in temporary)
            {
                CW_ConsumeDetail cp = m.CW_ConsumeDetail.Where(c => c.ID_Project == item.ID_Project && c.ID_ComsumeDetail == item.ID_ConsumeDetail && c.State_ComsumeDetail == true).SingleOrDefault();
                cp.money = cp.money * dis;
                item.Price = item.Price * dis;
                mon = item.Price;

                m.Entry(cp).State = System.Data.Entity.EntityState.Modified;
            }
            CW_Consumption cpn = m.CW_Consumption.Where(c => c.ID_RoomStage == ID_Room && c.Effective == true).Single();

            //cpn.Discount = Convert.ToDecimal(Tb_discount.Text.Trim());
            cpn.Prict = cpn.Prict - mon + cpn.Prict;

            m.Entry(cpn).State = System.Data.Entity.EntityState.Modified;

            if (m.SaveChanges() > 0)
            {
                Show_message(str);
                Get_data_right();
            }

        }


        //Tb_discount
        /// <summary>
        /// 打折
        /// </summary>
        private void Discount(string str)
        {
            try
            {
                if (!Tb_discount.IsVisible && str.Trim() == "打折")
                {
                    Bt_Consumer_turn_single.Visibility = Visibility.Hidden;//隐藏目标按钮
                    Tb_discount.Visibility = Visibility.Visible;//显示元素
                    return;
                }

                //判断折扣
                if (!WPF_MvvMTest.Tools.Tools.IsDiscount(Tb_discount.Text.Trim()))
                {
                    MessageBox.Show("请输入正确的折扣，只能在 1~10 之间", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                decimal dis = decimal.Parse(Tb_discount.Text.Trim()) / 10;
                decimal gross_amount = 0;
                foreach (var item in op)
                {
                    CW_ConsumeDetail cp = m.CW_ConsumeDetail.Where(c => c.ID_Project == item.ID_Project && c.ID_ComsumeDetail == item.ID_ConsumeDetail && c.State_ComsumeDetail == true).SingleOrDefault();
                    cp.money = cp.money * dis;
                    item.Price = item.Price * dis;

                    m.Entry(cp).State = System.Data.Entity.EntityState.Modified;
                    gross_amount = gross_amount + item.Price;
                }

                CW_Consumption cpn = m.CW_Consumption.Where(c => c.ID_RoomStage == ID_Room && c.Effective == true).Single();
                cpn.Discount = Convert.ToDecimal(Tb_discount.Text.Trim());
                cpn.Prict = gross_amount;
                m.Entry(cpn).State = System.Data.Entity.EntityState.Modified;

                if (m.SaveChanges() > 0)
                {
                    Common_means.operate_successfully(str);
                    Bt_Consumer_turn_single.Visibility = Visibility.Visible;//隐藏目标按钮
                    Tb_discount.Visibility = Visibility.Hidden;//显示元素


                    Get_data_right();
                    Show_message(str);
                }
                else
                {
                    Common_means.operation_failure(str);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("错误日志信息" + e.Message + "\n" + "对象名称" + e.Source + "\n" + "堆栈信息" + e.StackTrace);
            }
        }

        /// <summary>
        /// 添加消费按钮
        /// </summary>
        private void Addition(string name)
        {
            string str = Tb_Consumption_quantity.Text.Trim();

            if (WPF_MvvMTest.Tools.Tools.IsInteger(str))
            {
                MessageBox.Show("请输入正整数", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int count = int.Parse(str);

            if (count > 0)
            {
                try
                {
                    //  int ID_cp = m.CW_Consumption.Where(c => c.ID_RoomStage == ID_Room && c.Effective == true).SingleOrDefault().ID_Consumption;
                    for (int i = 0; i < count; i++)
                    {
                        //添加消费明细
                        CW_ConsumeDetail c = new CW_ConsumeDetail();
                        c.ID_Project = projects[0].ID_Project;
                        c.ID_Consumption = ID_consumption;
                        c.ID_PayRecord = 0;
                        c.State_ComsumeDetail = true;
                        c.money = projects[0].Price;
                        m.CW_ConsumeDetail.Add(c);
                        op.Add(projects[0]);

                    }

                    CW_Consumption cc = m.CW_Consumption.Where(c => c.ID_Consumption == ID_consumption && c.Effective == true).SingleOrDefault();

                    cc.Prict = m.CW_Consumption.Where(c => c.ID_RoomStage == ID_Room && c.Effective == true).Single().Prict > 0 ? m.CW_Consumption.Where(c => c.ID_RoomStage == ID_Room && c.Effective == true).Single().Prict : op.Where(c => c.presenter == "否").Select(p => p.Price).ToList().Sum();
                    //消费总金额
                    // DGTC_accruing_amounts.Header = op.Where(c => c.presenter == "否").Select(p => p.Price).ToList().Sum();

                    m.Entry(cc).State = System.Data.Entity.EntityState.Modified;



                    if (m.SaveChanges() > 0)
                    {
                        MessageBox.Show("消费入单成功", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        Get_data_right();
                        Show_message(str);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("消费入单异常", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            else
            {
                MessageBox.Show("请输入消费数量", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 更改消费金额
        /// </summary>
        private void sum_of_consumption()
        {
            //CW_Consumption cc = m.CW_Consumption.Where(c => c.ID_Consumption == ID_consumption && c.Effective == true).SingleOrDefault();
            //decimal tot = m.PJ_ProjectDetail.Where(c => c.ID_Project ==).Single().Price;
            //cc.Prict = cc.Prict - tot > 0 ? tot : 0;
            //m.Entry(cc).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// 删除消费
        /// </summary>
        /// <param name="str"></param>
        private void Delete_the_consumption(string str)
        {
            if (!IS_temporary_null(str))
            {
                return;
            }


            CW_ConsumeDetail cd = m.CW_ConsumeDetail.Where(c => c.ID_ComsumeDetail == temporary[0].ID_ConsumeDetail).SingleOrDefault();
            int? ID_Project = cd.ID_Project;

            //sum_of_consumption();
            CW_Consumption cc = m.CW_Consumption.Where(c => c.ID_Consumption == ID_consumption && c.Effective == true).SingleOrDefault();
            decimal tot = m.PJ_ProjectDetail.Where(c => c.ID_Project == cd.ID_Project).Single().Price;
            cc.Prict = cc.Prict - tot > 0 ? tot : 0;

            m.CW_ConsumeDetail.Remove(cd);
            m.Entry(cc).State = System.Data.Entity.EntityState.Modified;

            if (m.SaveChanges() > 0)
            {
                Get_data_right();
                Show_message(str);
            }
        }
        /// <summary>
        /// 赠送
        /// </summary>
        private void Presented(string str)
        {
            if (!IS_temporary_null(str))
            {
                return;
            }
            //temporary[0].ID_ConsumeDetail

            CW_ConsumeDetail cd = m.CW_ConsumeDetail.Where(c => c.ID_ComsumeDetail == temporary[0].ID_ConsumeDetail).SingleOrDefault();
            cd.presenter = true;

            CW_Consumption cc = m.CW_Consumption.Where(c => c.ID_Consumption == ID_consumption && c.Effective == true).SingleOrDefault();
            decimal tot = m.PJ_ProjectDetail.Where(c => c.ID_Project == cd.ID_Project).Single().Price;
            cc.Prict = cc.Prict - tot > 0 ? tot : 0;

            m.Entry(cd).State = System.Data.Entity.EntityState.Modified;

            m.Entry(cc).State = System.Data.Entity.EntityState.Modified;


            if (m.SaveChanges() > 0)
            {
                Get_data_right();
                Show_message(str);
            }
        }

        /// <summary>
        /// 输出弹出
        /// </summary>
        public void Show_message(string str)
        {
            MessageBox.Show(str + "成功", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// 判断
        /// </summary>
        /// <returns></returns>
        public bool IS_temporary_null(string str)
        {
            if (temporary.Equals(null))
            {
                MessageBox.Show("请选择要进行" + str + "的数据", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }
            return false;

        }

        /// <summary>
        /// 消费退单
        /// </summary>
        private void Consumer_single_back()
        {
            if (!IS_temporary_null("消费退单"))
            {
                return;
            }

            MessageBoxResult mbr = MessageBox.Show("确定要对 " + temporary[0].MC_Project + " 这条数据进行消费退单吗？", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            if (mbr == MessageBoxResult.OK)
            {
                CW_ConsumeDetail cd = m.CW_ConsumeDetail.Where(c => c.ID_ComsumeDetail == temporary[0].ID_ConsumeDetail).SingleOrDefault();
                m.CW_ConsumeDetail.Remove(cd);

                CW_Consumption cc = m.CW_Consumption.Where(c => c.ID_Consumption == ID_consumption && c.Effective == true).SingleOrDefault();
                decimal tot = m.PJ_ProjectDetail.Where(c => c.ID_Project == cd.ID_Project).Single().Price;
                cc.Prict = cc.Prict - tot > 0 ? tot : 0;

                m.Entry(cc).State = System.Data.Entity.EntityState.Modified;


                if (m.SaveChanges() > 0)
                {
                    Get_data_right();
                    MessageBox.Show("消费退单成功", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// 右边表格点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dg_right_data_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            Consumer consumer = dg.CurrentItem as Consumer;
            temporary.Clear();
            temporary.Add(consumer);

        }




        /*************************** 底线**************************/
    }
}
