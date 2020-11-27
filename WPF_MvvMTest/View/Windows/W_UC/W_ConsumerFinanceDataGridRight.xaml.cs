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
using WPF_MvvMTest.Model;

namespace WPF_MvvMTest.View.Windows.W_UC
{
    /// <summary>
    /// W_ConsumerFinanceDataGridRight.xaml 的交互逻辑
    /// </summary>
    public partial class W_ConsumerFinanceDataGridRight : UserControl
    {
        public event TransmitConsumer TransmitConsumer;
        public W_ConsumerFinanceDataGridRight(int ftid,string Discount,bool b,int idP)
        {
            id = ftid;
            thisDiscount = Discount;
            bools = b;
            xmid = idP;
            InitializeComponent();
        }
        //项目id
        int xmid;
        //是否单项打折
        bool bools;
        //折扣
        string thisDiscount;
        EasternStar_WPF_MVVMEntities m = new Model.EasternStar_WPF_MVVMEntities();
        int id;
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<Consumer> con = (from tC in m.CW_Consumption //消费表
                                  join tR in m.SYS_RoomStage on tC.ID_RoomStage equals tR.ID_RoomStage
                                  join tCD in m.CW_ConsumeDetail on tC.ID_Consumption equals tCD.ID_Consumption  //消费明细
                                  join tP in m.PJ_Project on tCD.ID_Project equals tP.ID_Project  //项目表
                                  join tPD in m.PJ_ProjectDetail on tP.ID_Project equals tPD.ID_Project //项目明细
                                  where tC.ID_RoomStage == id
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
                                      Total_money =+tPD.Price,
                                  }).ToList();

            dg.ItemsSource = con;
            //总金额
            decimal total_money =0;
            //折扣
            decimal total_moneys = thisDiscount.Trim() == "" ? 0 : Convert.ToDecimal(thisDiscount);
            // TbAsmount.Text = con[con.Count()-1].Total_money.ToString
            foreach (var item in con)
            {
                total_money = total_money + item.Price;
            }
            //单项打折
            if (!string.IsNullOrEmpty(thisDiscount) && bools == true && xmid > 0)
            {
                //  total_money 
                //单项金额
                List<Consumer>  Amoney = con.Where(p => p.ID_Project == xmid).ToList() ;
                decimal Amo = Amoney[0].Price;
                //剩余金额
                decimal Are = total_money - Amo * (1- Convert.ToDecimal(thisDiscount));
                total_money = Are;
                TbAsmount.Text = total_money.ToString();

            }
            else if (!string.IsNullOrEmpty(thisDiscount) && bools == false && xmid == 0)
            {
                //全部打折
                TbAsmount.Text = (total_money * total_moneys).ToString();
            }
            
            else if (thisDiscount == "" && bools == false && xmid == 0)
            {//啥都没有 
                TbAsmount.Text = total_money.ToString();
            }
            /**/
            //修改账单表
            int zdid = m.CW_Bill.Where(k => k.ID_Bill == (m.CW_Consumption.Where(p => p.ID_RoomStage == id && p.Effective ==true).FirstOrDefault().ID_Bill)).FirstOrDefault().ID_Bill;


           
            CW_Bill cB = m.CW_Bill.Where(p => p.ID_Bill == zdid).Single();
            cB.Price = total_money;//修改账单总金额
            m.Entry(cB).State = System.Data.Entity.EntityState.Modified;
            m.SaveChanges();


        }

        /// <summary>
        /// 表格点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Dg_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Consumer consumer = dg.CurrentItem as Consumer;

            List<Consumer> consumers = new List<Consumer>();

            consumers.Add(consumer);


            STATIC_cache.StaticConsumerRight = consumers;
        }

      

        /*************** 这个类也是有底线的  ****************/
    }
}
