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
using WPF_MvvMTest.EntityVo;
using WPF_MvvMTest.Model;

namespace WPF_MvvMTest.View.Windows
{
    /// <summary>
    /// W_ConsumerFinance.xaml 的交互逻辑
    /// </summary>
    public partial class W_ConsumerFinance : Window
    {
        public event Refresh Resh;
        public W_ConsumerFinance(List<RoomStage> DataM)
        {
            DataMessage = DataM;
            InitializeComponent();
        }
        Model.EasternStar_WPF_MVVMEntities m = new Model.EasternStar_WPF_MVVMEntities();
        List<RoomStage> DataMessage;
        List<Consumer> DataLeft;
        List<Consumer> DataRight;

        static TabItem tI;
   
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TbConsumeNum.Text = 1.00.ToString() ;
            //房台id
            int ftid = DataMessage[0].ID_RoomStage;
            var headerMessage = (from tB in m.CW_Bill
                                 join tC in m.CW_Consumption on tB.ID_Bill equals tC.ID_Bill
                                 join tR in m.SYS_RoomStage on tC.ID_RoomStage equals tR.ID_RoomStage
                                 join tV in m.VIP_Table on tR.ID_Guest equals tV.ID_Guest
                                 where tC.ID_RoomStage == ftid
                                 select new
                                 {
                                     tR.Number_RoomStage, 
                                     tV.Accounts
                                 }).ToList();

            TbFtNum.Text = DataMessage[0].Number_RoomStage.ToString();
            TbConsumptionMessage.Text = "单号：" + "  【" + headerMessage[0].Number_RoomStage + "】  "
                                       + "账号：" + headerMessage[0].Accounts.Trim() +"  的消费清单";
          
            //左边的表格
            View.Windows.W_UC.W_ConsumerFinanceDataGridLeftxaml w = new W_UC.W_ConsumerFinanceDataGridLeftxaml(0,null);
            w.TransmitConsumer += new W_UC.TransmitConsumer(DataLeftCount);
            Ti_All.Content = null;
            Ti_All.Content = w;

            //右边的表格
            View.Windows.W_UC.W_ConsumerFinanceDataGridRight r = new W_UC.W_ConsumerFinanceDataGridRight(ftid, "", false,0);
            r.TransmitConsumer += new W_UC.TransmitConsumer(DataRightCount);
            TiRight.Content = null;
            TiRight.Content = r;
        }
        #region 接收数据
        /// <summary>
        /// 接受左边数据
        /// </summary>
        /// <param name="consumers"></param>
        public void DataLeftCount(List<Consumer> consumers)
        {
            if (consumers != null)
            {
                if (DataLeft == null)
                {
                    DataLeft = consumers;
                }
                else
                {
                    DataLeft.Clear();
                    DataLeft = consumers;
                }

            }

        }
        /// <summary>
        /// 接收右边数据
        /// </summary>
        /// <param name="consumers"></param>
        public void DataRightCount(List<Consumer> consumers)
        {
            if (consumers != null)
            {
                if (DataRight == null)
                {
                    DataRight = consumers;
                }
                else
                {
                    DataRight.Clear();
                    DataRight = consumers;
                }
            }
        }
        #endregion


        /// <summary>
        /// TabItem点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TabItem tI = sender as TabItem;
            int Num =Convert.ToInt32(tI.Tag);
            //左边的表格
            View.Windows.W_UC.W_ConsumerFinanceDataGridLeftxaml w = new W_UC.W_ConsumerFinanceDataGridLeftxaml(Num,null);
            tI.Content = null;
            tI.Content = w;
            tI.IsSelected = true;
            
        }
        
        /// <summary>
        ///搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>     
        private void Nan_Click(object sender, RoutedEventArgs e)
        {
            string  Select_Message= TbNum.Text.Trim();
        
            View.Windows.W_UC.W_ConsumerFinanceDataGridLeftxaml w = new W_UC.W_ConsumerFinanceDataGridLeftxaml(0, Select_Message);
            w.TransmitConsumer += new W_UC.TransmitConsumer(DataLeftCount);
            Ti_All.Content = null;
            Ti_All.IsSelected = true;
            Ti_All.Content = w;
        }
      
        /// <summary>
        /// 加号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtPuls_Click(object sender, RoutedEventArgs e)
        {
            if (TbConsumeNum !=null)
            {
                string cont = TbConsumeNum.Text;
                try
                {
                    int i = Convert.ToInt32(cont) + 1;
                    if (i<=0)
                    {
                        MessageBox.Show("请输入正整数", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        return;
                    }
                    TbConsumeNum.Text = i.ToString();
                }
                catch (Exception)
                {
                    MessageBox.Show("请输入正整数", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                }
               
            }
        }
      
        /// <summary>
        /// 减号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtSubtract_Click(object sender, RoutedEventArgs e)
        {

            string cont = TbConsumeNum.Text;
            if (cont == "1")
            {
                return;
            } 

            int i = Convert.ToInt32(cont) - 1;

            TbConsumeNum.Text = i.ToString();


        }
        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int j = 1;
        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            int count = Convert.ToInt32(TbConsumeNum.Text)==null?0: Convert.ToInt32(TbConsumeNum.Text);


            List<Consumer> cS = new List<Consumer>();
            cS = STATIC_cache.StaticConsumerLeft;
            if (cS ==null)
            {
                MessageBox.Show("请选择数据","大海提示",MessageBoxButton.OK,MessageBoxImage.Asterisk);
                return;
            }
            int ftid = DataMessage[0].ID_RoomStage;
            int cC = m.CW_Consumption.Where(p => p.ID_RoomStage == ftid).Count();
            //存在消费单
            if (cC >0)
            {
                int xfid = m.CW_Consumption.Where(p => p.ID_RoomStage == ftid).Single().ID_Consumption;

                for (int i = 0; i < count; i++)
                {
                    CW_ConsumeDetail iCD = new CW_ConsumeDetail();
                    iCD.ID_Consumption = xfid;
                    iCD.ID_Project = cS[0].ID_Project;
                    iCD.State_ComsumeDetail = true;
                    m.CW_ConsumeDetail.Add(iCD);
                }
             
                if (m.SaveChanges()>0)
                {
                   
  
                        MessageBoxResult m = MessageBox.Show("消费入单成功", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        if (m == MessageBoxResult.OK)
                        {
                            TbConsumeNum.Text ="1";
                        }
                        //右边的表格
                        View.Windows.W_UC.W_ConsumerFinanceDataGridRight r = new W_UC.W_ConsumerFinanceDataGridRight(ftid, "", false,0);
                        r.TransmitConsumer += new W_UC.TransmitConsumer(DataRightCount);
                        TiRight.Content = null;
                        TiRight.Content = r;
               }
                  
                    
                
               
            }
            else
            {
                MessageBox.Show("请先开台，再消费入单","大海提示",MessageBoxButton.OK,MessageBoxImage.Asterisk);
            }
        }

        /// <summary>
        /// 消费退单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtConsumeChargeback_Click(object sender, RoutedEventArgs e)
        {
            List<Consumer> consumers = new List<Consumer>();
            consumers = STATIC_cache.StaticConsumerRight;
            if (consumers ==null || consumers.Count() <1)
            {
                MessageBox.Show("请选择需要退单的项目","大海提示",MessageBoxButton.OKCancel,MessageBoxImage.Asterisk);
                return;
            }
            //房台id 
            int ftid = DataMessage[0].ID_RoomStage;
            //消费明细id
            int mxid = consumers[0].ID_ConsumeDetail;
            CW_ConsumeDetail cCd = m.CW_ConsumeDetail.Where(p => p.ID_ComsumeDetail == mxid).Single() ;
            m.CW_ConsumeDetail.Remove(cCd);
            if (m.SaveChanges() >0)
            {
             MessageBoxResult m=   MessageBox.Show("消费退单成功", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
                if (m==MessageBoxResult.OK)
                {

                }
                //右边的表格
                View.Windows.W_UC.W_ConsumerFinanceDataGridRight r = new W_UC.W_ConsumerFinanceDataGridRight(ftid,"", false,0);
                r.TransmitConsumer += new W_UC.TransmitConsumer(DataRightCount);
                TiRight.Content = null;
                TiRight.Content = r;
            }
        }

        /// <summary>
        /// 消费转单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtConsumerTurnSingle_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("傻逼，本店不支持转单","大爷提示",MessageBoxButton.OK,MessageBoxImage.Asterisk);
        }

        /// <summary>
        /// 打折
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtDiscount_Click(object sender, RoutedEventArgs e)
        {
            string Discount = TbDiscount.Text.Trim();
            if (string.IsNullOrEmpty(Discount))
            {
                MessageBox.Show("请输入", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            //限制输入整数
            if (Tools.Tools.IsInteger(Discount))
            {
                MessageBox.Show("请输入正确折扣", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            //折扣
            // int int_Discount = Convert.ToInt32(Discount);

            int int_Discount = int.Parse(Discount);
            decimal dec_Discount =(decimal) int_Discount / 10;

            string _Discount = dec_Discount.ToString();





            int ftid = DataMessage[0].ID_RoomStage;
            //右边的表格
            View.Windows.W_UC.W_ConsumerFinanceDataGridRight r = new W_UC.W_ConsumerFinanceDataGridRight(ftid , _Discount, false,0);
            r.TransmitConsumer += new W_UC.TransmitConsumer(DataRightCount);
            TiRight.Content = null;
            TiRight.Content = r;

            TbDiscount.Text = "";
        }
        /// <summary>
        /// 单项打折
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtDiscount_Click_1(object sender, RoutedEventArgs e)
        {
            //获取选中行
            List<Consumer> consumers = new List<Consumer>();
            consumers = STATIC_cache.StaticConsumerRight;
            if (consumers ==null || consumers.Count()==0)
            {
                MessageBox.Show("请选中数据", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            string Discount = TbDiscount.Text.Trim();
            if (string.IsNullOrEmpty(Discount))
            {
                MessageBox.Show("请输入", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            if (Tools.Tools.IsInteger(Discount))
            {
                MessageBox.Show("请输入正确折扣", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            //折扣
            int int_Discount = int.Parse(Discount);
            decimal dec_Discount = (decimal)int_Discount / 10;

            string _Discount = dec_Discount.ToString();

            int idP = consumers[0].ID_Project;
            int ftid = DataMessage[0].ID_RoomStage;
            //右边的表格
            View.Windows.W_UC.W_ConsumerFinanceDataGridRight r = new W_UC.W_ConsumerFinanceDataGridRight(ftid, _Discount, true, idP);
            r.TransmitConsumer += new W_UC.TransmitConsumer(DataRightCount);
            TiRight.Content = null;
            TiRight.Content = r;

            TbDiscount.Text = "";
        }

        /// <summary>
        /// 结账买单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtPayment_Click(object sender, RoutedEventArgs e)
        {

        }








        /********    这个类也是有底线的         ********/
    }
}
