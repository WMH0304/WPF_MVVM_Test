using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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


        /// <summary>
        /// 树形图临时集合
        /// </summary>
        class Temporary_collection
        {
            public int id { get; set; }

            public string name { get; set; }


        }

        #region MyRegion
        ///// <summary>
        ///// 超级集合
        ///// </summary>
        //class super_gather
        //{
        //    /// <summary>
        //    /// 消费id
        //    /// </summary>
        //    public int id { get; set; }

        //    /// <summary>
        //    /// 客人id
        //    /// </summary>
        //    public int id_guest { get; set; }

        //    /// <summary>
        //    /// 房台号
        //    /// </summary>
        //    public string room_table { get; set; }

        //    /// <summary>
        //    /// 客人名称
        //    /// </summary>
        //    public string the_guest_name { get; set; }

        //    /// <summary>
        //    /// 支付金额
        //    /// </summary>
        //    public string payment_amount { get; set; }

        //    /// <summary>
        //    /// 支付时间
        //    /// </summary>
        //    public string time_of_payment { get; set; }
        //    /// <summary>
        //    /// 支付状态
        //    /// </summary>
        //    public string state_of_payment { get; set; }

        //    /// <summary>
        //    /// 支付方式
        //    /// </summary>
        //    public string mode_of_payment { get; set; }

        //    /// <summary>
        //    /// 帐单号
        //    /// </summary>
        //    public string bill_number { get; set; }

        //    /// <summary>
        //    /// 房台名称
        //    /// </summary>
        //    public string room_table_name { get; set; }

        //    /// <summary>
        //    /// 项目名称
        //    /// </summary>
        //    public string project_name { get; set; }

        //    /// <summary>
        //    /// 单位
        //    /// </summary>
        //    public string unit { get; set; }

        //    /// <summary>
        //    /// 单价
        //    /// </summary>
        //    public string unit_price { get; set; }

        //    /// <summary>
        //    /// 数量
        //    /// </summary>
        //    public string quantity { get; set; }

        //    /// <summary>
        //    /// 折扣
        //    /// </summary>
        //    public string discount { get; set; }

        //    /// <summary>
        //    /// 金额
        //    /// </summary>
        //    public string money { get; set; }




        //}
        #endregion


        readonly List<RoomStage> rooms;

        int? ID_Guest = -1;
        int ID_RoomStage;
        int ID_Consumption ;
        int ID_Bill;
        List<Consumer> con;
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ID_Guest =(int) rooms[0].ID_Guest;
            ID_RoomStage = rooms[0].ID_RoomStage;
            ID_Guest = m.SYS_RoomStage.Where(c => c.ID_RoomStage == ID_RoomStage).SingleOrDefault().ID_Guest;
            ID_Bill = m.CW_Consumption.Where(w => w.ID_RoomStage == ID_RoomStage && w.Effective == true).Single().ID_Bill;
            ID_Consumption = m.CW_Consumption.Where(c => c.ID_RoomStage == ID_RoomStage && c.Effective ==true).SingleOrDefault().ID_Consumption;
            //下拉框数据绑定
            List<string> vs = new List<string>();
            vs.Add("现金");
            vs.Add("银行卡");
            vs.Add("微信");
            Cb_payment_method.ItemsSource = vs;


            //页面数据回填查询
            var listdata = (from t in m.SYS_RoomStage
                            join c in m.CW_Consumption on t.ID_RoomStage equals c.ID_RoomStage
                            join v in m.VIP_Table on t.ID_Guest equals v.ID_Guest
                            join b in m.CW_Bill on c.ID_Bill equals b.ID_Bill
                            join o in m.YW_OpenStage on b.Number_Bill equals o.Number_OpenStage
                            where t.ID_RoomStage == ID_RoomStage && t.ID_Guest == ID_Guest && c.Effective == true
                            select new
                            {
                                v.Accounts,//账号
                                t.Number_RoomStage,//主房台号
                                t.MC_RoomStage,//房台名称
                                t.State_RoomStage,//状态
                                b.Number_Bill,//房台帐单号
                                o.Time_Predict,//开台时间
                                o.Remark,//备注信息
                                c.Discount,//折扣
                                c.Prict,//累计消费
                            }).AsParallel().SingleOrDefault();
            if (listdata is null)
            {
                // Debug.WriteLine(listdata.ToString());
                throw new NullReferenceException("集合为空");

            }

            Tb_The_guest_account.Content = listdata.Accounts.ToString().Trim();
            Tb_The_main_room_number.Text = listdata.Number_RoomStage.Trim();
            Tb_Room_table_name.Text = listdata.MC_RoomStage.Trim();
            Tb_Room_with_a_single_number.Text = listdata.Number_Bill.Trim();
            Tb_Founding_time.Text = listdata.Time_Predict.ToString().Trim();
            Tb_condition.Text = listdata.State_RoomStage.Trim();
            Tb_postscript.Text = listdata.Remark.Trim();
            Tb_discount.Text = listdata.Discount.ToString();

            //string str = Tb_The_guest_account.Content.ToString(); ;
            //累计消费 
            Tb_accruing_amounts.Text = m.CW_Consumption.Where(c => c.ID_RoomStage == ID_RoomStage &&c.Effective ==true).SingleOrDefault().Prict.ToString();
            //已收定金
            Tb_received_the_deposit.Text = "0";
            //余额 
            Tb_balance.Text = "0";

            //买单时间
            Tb_The_check_time.Text = DateTime.Now.Date.ToString();
            string s = Tb_The_guest_account.Content.ToString();
            //时长
            DateTime dt1 = DateTime.Parse(Tb_Founding_time.Text.Trim().ToString());//开始时间
            DateTime dt2 = DateTime.Parse(Tb_The_check_time.Text.Trim().ToString());

            System.TimeSpan time1 = dt2.Subtract(dt1);//比较返回时间差
            Tb_duration.Text = time1.ToString();

            //左边树形
            List<Temporary_collection> tc = (from t in m.SYS_RoomStage
                                             where t.ID_RoomStage == ID_RoomStage && t.ID_Guest == ID_Guest && t.State_RoomStage.Trim() == "已用"
                                             select new Temporary_collection
                                             {
                                                 id = t.ID_RoomStage,
                                                 name = t.MC_RoomStage.Trim() + " ___ " + t.Number_RoomStage,
                                             }).AsParallel().ToList();

            treeView.ItemsSource = tc;
            //下面表数据
            Get_date();
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            string bt_con = button.Content.ToString().Trim();
            if (bt_con == "人民币结账")
            {
                RMB_settlement();
            }
            if (bt_con == "打印账单")
            {
                Print_the_bill();
            }
            if (bt_con == "多种方式结账")
            {
                RMB_settlement();
            }
            if (bt_con == "消费入帐")
            {
                Consumption_is_booked();
            }
        }

        /// <summary>
        /// 打印账单
        /// </summary>
        private void Print_the_bill()
        {
            List<Consumer> consumers = new List<Consumer>();
            consumers = con;

            PrintfData pd = new PrintfData();
            pd.krxm = m.SYS_Guest.Where(c => c.ID_Guest == ID_Guest).Single().MC_Guest.Trim();
            pd.zfth = Tb_The_main_room_number.Text.Trim().ToString();
            pd.ftmc = Tb_Room_table_name.Text.Trim().ToString();
            pd.ktsj = Tb_Founding_time.Text.Trim().ToString();
            pd.mdsj = Tb_The_check_time.Text.Trim().ToString();
            pd.sc = Tb_duration.Text.Trim().ToString();
            pd.zhekou = Tb_discount.Text.Trim().ToString();
            pd.beizhu = Tb_postscript.Text.Trim().ToString();
            pd.leijijine = Tb_accruing_amounts.Text.Trim().ToString();
            pd.yishoudingj = Tb_received_the_deposit.Text.Trim().ToString();
            pd.yve = Tb_balance.Text.Trim().ToString();
            pd.bkzf = Tb_guests_pay.Text.Trim().ToString();
            pd.dfje = Tb_to_pay_the_balance.Text.Trim().ToString();
            pd.jzfs = Cb_payment_method.Text.Trim().ToString();
            pd.jzbz = Tb_comment.Text.Trim().ToString();
            pd.ftdh = Tb_Room_with_a_single_number.Text.Trim().ToString();


            foreach (var item in consumers)
            {
                pd.LPDD.Add(new PrintfDataDetail
                {
                    fth = item.Number_RoomStage,
                    xmmc = item.MC_Project,
                    dw = item.Unit,
                    dj = item.Price.ToString(),
                    sl = item.Mun,
                    zk = item.Discount.ToString(),
                    je = item.Price.ToString(),
                    zffs = item.MethodOfPayment == null ? "*" : item.MethodOfPayment,
                });
            }

            List<PrintfData> prd = new List<PrintfData>();
            prd.Add(pd);





            //FABM_printer printer = new FABM_printer(prd,consumers);   printer.ShowDialog();


            WPF_MvvMTest.View.Windows.W_Print_DataDetail w = new View.Windows.W_Print_DataDetail("FlowDocument.xaml", pd, new OrderDocumentRenderer());

            w.Owner = this;
            w.Width = 1000;
            w.Height = 800;
            w.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            w.ShowInTaskbar = false;
            w.ShowDialog();

        }


        /// <summary>
        /// 消费入账
        /// </summary>
        private void Consumption_is_booked()
        {
            MessageBox.Show("消费已入张", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// 多种方式结账
        /// </summary>
        private void Multiple_checkout()
        {
            Cb_payment_method.IsEnabled = true;
        }
        /// <summary>
        /// 人民币结账
        /// </summary>
        private void RMB_settlement()
        {

            try
            {
                if (!Tools.Tools.IsInteger(Tb_balance.Text))
                {
                    MessageBox.Show("请输入正确的数据类型", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //钱够的情况
                if (Tb_balance.Text != string.Empty)
                {
                    //添加消费记录
                    CW_PayRecord c = new CW_PayRecord();
                    c.ID_Guest = ID_Guest;
                    c.ID_Bill = ID_Bill;
                    c.Price_Pay = decimal.Parse(Tb_guests_pay.Text.Trim());
                    c.Time_Pay = DateTime.Now.Date;
                   
                    c.PoP = Cb_payment_method.IsEnabled is false ?"人民币": Cb_payment_method.Text.Trim().ToString();

                    var co = m.CW_ConsumeDetail.Where(cc => cc.ID_Consumption == ID_Consumption && cc.State_ComsumeDetail == true).ToList();
                    foreach (var item in co)
                    {
                        //修改该消费记录状态
                        CW_ConsumeDetail cd = m.CW_ConsumeDetail.Where(cc => cc.ID_ComsumeDetail == item.ID_ComsumeDetail && cc.State_ComsumeDetail == true).SingleOrDefault();
                        cd.State_ComsumeDetail = false;
                        m.Entry(cd).State = System.Data.Entity.EntityState.Modified;
                    }

                    //注销本次消费
                    CW_Consumption cp = m.CW_Consumption.Where(cc => cc.ID_Consumption == ID_Consumption && cc.Effective == true).Single();
                    cp.Effective = false;
                    m.Entry(cp).State = System.Data.Entity.EntityState.Modified;

                    CW_Bill cb = m.CW_Bill.Where(cc => cc.ID_Bill == ID_Bill && cc.State_Bill == null).Single();
                    cb.Time_PayBill = DateTime.Parse(Tb_The_check_time.Text.Trim());
                    cb.Remark = Tb_comment.Text.Trim().ToString();
                    cb.Price = decimal.Parse(Tb_guests_pay.Text.Trim());
                    cb.State_Bill = "已结账";
                    m.Entry(cb).State = System.Data.Entity.EntityState.Modified;

                    SYS_RoomStage sr = m.SYS_RoomStage.Where(cc => cc.ID_RoomStage == ID_RoomStage && cc.ID_Guest == ID_Guest).Single();
                    sr.ID_Guest = null;
                    sr.State_RoomStage = "未用";
                    m.Entry(sr).State = System.Data.Entity.EntityState.Modified;

                    if (m.SaveChanges() > 0)
                    {
                        MessageBox.Show("结账成功", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
                else

                //当钱不够时
                if (Tb_to_pay_the_balance.Text != string.Empty)
                {
                    //添加消费记录
                    CW_PayRecord c = new CW_PayRecord();
                    c.ID_Guest = ID_Guest;
                    c.ID_Bill = ID_Bill;
                    c.Price_Pay = decimal.Parse(Tb_guests_pay.Text.Trim());
                    c.Time_Pay = DateTime.Now.Date;
                    c.PoP = Cb_payment_method.IsEnabled is false ? "人民币" : Cb_payment_method.Text.Trim().ToString();

                    m.SaveChanges();

                    //修改本次消费
                    CW_Consumption cp = m.CW_Consumption.Where(cc => cc.ID_Consumption == ID_Consumption && cc.Effective == true).Single();
                    cp.Prict = decimal.Parse(Tb_to_pay_the_balance.Text.Trim());
                    m.Entry(cp).State = System.Data.Entity.EntityState.Modified;

                    CW_Bill cb = m.CW_Bill.Where(cc => cc.ID_Bill == ID_Bill && cc.State_Bill == null).Single();
                    cb.Time_PayBill = DateTime.Parse(Tb_The_check_time.Text.Trim());
                    cb.Remark = Tb_comment.Text.Trim().ToString();
                    cb.Price = decimal.Parse(Tb_guests_pay.Text.Trim());
                    m.Entry(cb).State = System.Data.Entity.EntityState.Modified;

                    if (m.SaveChanges() > 0)
                    {
                        MessageBox.Show("部分结账成功", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception e)
            {

                WPF_MvvMTest.Tools.Common_means.error_log(e);
                  
            }
          

         





        }


        /// <summary>
        /// 获取表格数据
        /// </summary>
        private void Get_date()
        {
            // ID_RoomStage
            //全部记录
            // List<super_gather> super = 
            int id_consumer = m.CW_Consumption.Where(c => c.ID_RoomStage == ID_RoomStage && c.Effective == true).Single().ID_Consumption;

            //全部记录


            //支付记录 
            con = (from tC in m.CW_Consumption //消费表
                                  join tR in m.SYS_RoomStage on tC.ID_RoomStage equals tR.ID_RoomStage
                                  join tCD in m.CW_ConsumeDetail on tC.ID_Consumption equals tCD.ID_Consumption  //消费明细
                                  join tP in m.PJ_Project on tCD.ID_Project equals tP.ID_Project  //项目表
                                  join tPD in m.PJ_ProjectDetail on tP.ID_Project equals tPD.ID_Project //项目明细
                                  //join tPR in m.CW_PayRecord on tCD.ID_PayRecord equals tPR.ID_PayRecord //支付记录
                                  where tC.ID_RoomStage == ID_RoomStage
                                  select new Consumer
                                  {
                                      ID_Fangtai = tC.ID_RoomStage,
                                      ID_Project = tP.ID_Project,
                                      ID_ConsumeDetail = tCD.ID_ComsumeDetail,
                                      Number_RoomStage = tR.Number_RoomStage,//房台号
                                      MC_Project = tP.MC_Project,//项目名称
                                      Unit = tP.Unit,//单位
                                      Price = tPD.Price,//单价 or 单价
                                      Mun = "1",//数量 
                                      Discount = tC.Discount,//折扣
                                      Total_money = +tPD.Price,
                                  }).ToList();

            DgDateAll.ItemsSource = con;
            DgRecordsOfConsumption.ItemsSource = con;


            //支付记录
            List<PaymentRecords> records = (from tP in m.CW_PayRecord
                                            join tB in m.CW_Bill on tP.ID_Bill equals tB.ID_Bill
                                            join tG in m.SYS_Guest on tP.ID_Guest equals tG.ID_Guest
                                            join tR in m.SYS_RoomStage on tG.ID_Guest equals tR.ID_Guest
                                            where tP.ID_Guest == ID_Guest
                                            select new PaymentRecords
                                            {
                                                ID_Bill = tB.ID_Bill,
                                                ID_Guest = tG.ID_Guest,
                                                ID_PayRecord = tP.ID_PayRecord,
                                                ID_Fangtai = tR.ID_RoomStage,
                                                Price_Pay = tP.Price_Pay,//金额支付
                                                Time_Pay = tP.Time_Pay,//时间支付
                                                PoP = tP.PoP,//支付方式
                                                State = tP.State,//状态支付
                                                GuestName = tG.MC_Guest,
                                                Number_Bill = tB.Number_Bill,//站单号
                                                Number_RoomStage = tR.Number_RoomStage//房台号
                                            }).ToList();


            DgPaymentRecords.ItemsSource = records;

        }

        
      
        /// <summary>
        /// 树形子控点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Temporary_collection tc = treeView.SelectedItem as Temporary_collection;

            string c = tc.id.ToString();

            string cc = tc.name;

            ID_RoomStage = tc.id;

            Window_Loaded(null,null);
        }

        /// <summary>
        /// 计算待付余额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Tb_accruing_amounts.Text ==string.Empty || Tb_guests_pay.Text ==string.Empty)
            {
                MessageBox.Show("请输入宾客支付金额","大海提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
            //累计消费
            decimal amounts = decimal.Parse(Tb_accruing_amounts.Text.Trim());
            //宾客支付
            decimal guests_pay = decimal.Parse(Tb_guests_pay.Text.Trim());
            if (amounts> guests_pay)
            {
                //待付余额
                Tb_to_pay_the_balance.Text = (amounts - guests_pay).ToString();
                Tb_balance.Text = string.Empty;
                //Tb_balance.IsEnabled = false;
                //Tb_balance.Text = string.Empty;
            }
            else
            {
                Tb_balance.Text = (guests_pay - amounts).ToString();
                Tb_to_pay_the_balance.Text = string.Empty;
                //Tb_to_pay_the_balance.IsEnabled = false;
                //Tb_to_pay_the_balance.Text = string.Empty;
            }
           
        
        }


    }
}
