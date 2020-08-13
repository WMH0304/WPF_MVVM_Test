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
    /// W_StatementLeave.xaml 的交互逻辑
    /// </summary>
    public partial class W_StatementLeave : Window
    {
        public W_StatementLeave(List<RoomStage> stages)
        {
            PbRoomStages = stages;
            InitializeComponent();
        }
        EasternStar_WPF_MVVMEntities m = new EasternStar_WPF_MVVMEntities();
        List<RoomStage> PbRoomStages;
        List<Consumer> con;
        public List<Node> nodeList { set; get; }


        #region 绑定TreeView方法

        /// <summary>
        /// 节点数据
        /// </summary>
        /// <returns></returns>
        private List<Node> GetNodeList()
        {
            List<Node> nodes = new List<Node>();
            int idRoonStage = PbRoomStages[0].ID_RoomStage;
            int? idGuest = m.SYS_RoomStage.Where(p => p.ID_RoomStage == idRoonStage).SingleOrDefault().ID_Guest;
            //树形房台信息
            var treeRooms = (from tR in m.SYS_RoomStage
                             where tR.ID_Guest == idGuest
                             select new
                             {
                                 tR.ID_RoomStage,
                                 tR.Number_RoomStage,
                                 tR.MC_RoomStage,
                             }).ToList();
            for (int i = 0; i < treeRooms.Count(); i++)
            {
                Node node = new Node();
                node.NodeId = treeRooms[i].ID_RoomStage.ToString();
                node.NodeName = treeRooms[i].Number_RoomStage.Trim();
                node.NodeContent = treeRooms[i].MC_RoomStage.Trim();
                nodes.Add(node);
            }
            return new List<Node>()
            {
                new Node()
                {
                    NodeName = "关联房台",
                    NodeContent ="关联房台",
                    Nodes = nodes,
                }
            };
        }
       
        /// <summary>
        /// 加载节点
        /// </summary>
        private void ExpandTree()
        {
            if (this.TvTree.Items !=null && this.TvTree.Items.Count>0)
            {
                foreach (var item in this.TvTree.Items)
                {
                    //个 ItemContainerGenerator，负责为其宿主（如 ItemsControl）生成 用户界面 (UI)。 
                    // ContainerFromItem 返回对应于指定项的 UIElement。
                    DependencyObject dependencyObject = this.TvTree.ItemContainerGenerator.ContainerFromItem(item);
                    if (dependencyObject !=null)
                    {
                        //展开 TreeViewItem 控件及其所有子 TreeViewItem 元素。
                        ((TreeViewItem)dependencyObject).ExpandSubtree();
                    }
                }
            }
        }


        #endregion




        static int ftid;

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region 树形加载
            //房台信息
            nodeList = GetNodeList();
            //树形控件绑定
            this.TvTree.ItemsSource = nodeList;
            int idRoom;
           // 绑定数据树形点击
            if (ftid >0)
            {
                idRoom = ftid;
            }
            else
            {
                idRoom = PbRoomStages[0].ID_RoomStage;
            }
            
            //客人id
            int? igGuest = m.SYS_RoomStage.Where(p => p.ID_RoomStage == idRoom).SingleOrDefault().ID_Guest;
            #region 表格查询

            //全部记录
                            con = (from tC in m.CW_Consumption //消费表
                                  join tR in m.SYS_RoomStage on tC.ID_RoomStage equals tR.ID_RoomStage
                                  join tCD in m.CW_ConsumeDetail on tC.ID_Consumption equals tCD.ID_Consumption  //消费明细
                                  join tP in m.PJ_Project on tCD.ID_Project equals tP.ID_Project  //项目表
                                  join tPD in m.PJ_ProjectDetail on tP.ID_Project equals tPD.ID_Project //项目明细
                                  //join tPR in m.CW_PayRecord on tCD.ID_PayRecord equals tPR.ID_PayRecord //支付记录
                                  where tC.ID_RoomStage == idRoom
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

            if (ftid >0)
            {
                List<Consumer> consumers = new List<Consumer>();
                consumers.AddRange(con);
                DgDateAll.ItemsSource = consumers;
                DgRecordsOfConsumption.ItemsSource = consumers;
            }
            else
            {
                //全部记录
                DgDateAll.ItemsSource = con;
                //消费记录
                DgRecordsOfConsumption.ItemsSource = con;

            }



            //支付记录
            List<PaymentRecords> records = (from tP in m.CW_PayRecord
                                            join tB in m.CW_Bill on tP.ID_Bill equals tB.ID_Bill
                                            join tG in m.SYS_Guest on tP.ID_Guest equals tG.ID_Guest
                                            join tR in m.SYS_RoomStage on tG.ID_Guest equals tR.ID_Guest
                                            where tP.ID_Guest == igGuest
                                            select new PaymentRecords
                                            {
                                                ID_Bill = tB.ID_Bill,
                                                ID_Guest = tG.ID_Guest,
                                                ID_PayRecord = tP.ID_PayRecord,
                                                ID_Fangtai = tR.ID_RoomStage,
                                                Price_Pay = tP.Price_Pay,//金额支付
                                                Time_Pay = tP.Time_Pay,//时间支付
                                                PoP = tP.PoP,//支付方式
                                                State =tP.State,//状态支付
                                                GuestName = tG.MC_Guest,
                                                Number_Bill = tB.Number_Bill,//站单号
                                                Number_RoomStage = tR.Number_RoomStage//房台号
                                            }).ToList();
            #endregion

            #region 数据回填
            //客人账号
            TbGuestAccount.Text = m.VIP_Table.Where(o => o.ID_Guest == igGuest).SingleOrDefault().Accounts.Trim().ToString();
            //房台好
            TbGuestAcount.Text = PbRoomStages[0].Number_RoomStage.Trim().ToString();
            //房间单号
            TbRoomWithSingleNumber.Text = m.CW_Bill.Where(t => t.ID_Bill == (m.CW_Consumption.Where(c => c.ID_RoomStage == idRoom).FirstOrDefault().ID_Bill)).SingleOrDefault().Number_Bill;
            //开台时间
            DateTime dateTime = m.YW_OpenStage.Where(t => t.ID_VIP == (m.VIP_Table.Where(a => a.ID_Guest == igGuest).FirstOrDefault().ID_VIP)).SingleOrDefault().Time_Predict;
            TbFoundingTime.Text = dateTime.ToLongTimeString();
            //买单时间
            DateTime now = DateTime.Now;
            TbTheCheckTime.Text = now.ToLongTimeString();
            //时长
            string dateTme =(now - dateTime).ToString();
            TbDuration.Text = dateTme;
            //累计消费
            TbTheCumulative.Text = m.CW_Bill.Where(p => p.ID_Bill == (m.CW_Consumption.Where(o => o.ID_RoomStage == idRoom).FirstOrDefault().ID_Bill)).SingleOrDefault().Price.ToString() ;
            //已收定金       
           // TbGueAcount.Text = m.YW_Subscribe.Where(l => l.ID_Guest == igGuest).SingleOrDefault().Money_Pledge.ToString() ;
            //余额
           // TbRoTableName.Text = (Convert.ToDouble(TbTheCumulative.Text) - Convert.ToDouble(TbGueAcount.Text)).ToString();
            if (ftid >0)
            {
                TbFodingTime.Text = (Convert.ToDouble(TbFodingTime.Text) + Convert.ToDouble(TbTheCumulative.Text)).ToString();
            }
            else
            {
                
                //宾客支付
                TbFodingTime.Text = TbTheCumulative.Text;
            }
           
            //找零
            TbThCheckTime.Text = (Convert.ToDouble(TbFodingTime.Text) - Convert.ToDouble(TbTheCumulative.Text)).ToString();


            #endregion



            #endregion
        }



        /// <summary>
        ///  属性图控件点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        List<Node> nodes = new List<Node>();

        private void TvTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Node node = this.TvTree.SelectedItem as Node;
         
            if (node.Nodes.Count()==1)
            {
               //只有一条数据时
                nodes.Add(node);


            }
            if (node.Nodes.Count() > 1)
            {
                foreach (var item in node.Nodes)
                {
                    //当有多条数据时
                    nodes.Add(item);
                }
            }
           

        }

        /// <summary>
        /// 人民币结账
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtRMBSettlement_Click(object sender, RoutedEventArgs e)
        {
            //假如钱不够
            decimal ljxf = Convert.ToDecimal(TbTheCumulative.Text);
            decimal bkzf = Convert.ToDecimal(TbFodingTime.Text);
            if (bkzf < ljxf)
            {
                MessageBox.Show("你的钱不够哦，不要想着吃霸王餐哦", "大海提示",MessageBoxButton.OK,MessageBoxImage.Asterisk);
                return;
            }

            //客人账号信息
            string krzh = TbGuestAccount.Text.Trim().ToString();
            List<SYS_RoomStage> rS = m.SYS_RoomStage.Where(l => l.ID_Guest == m.VIP_Table.Where(k => k.Accounts == krzh).FirstOrDefault().ID_Grade).ToList();
            int rs_id = rS[0].ID_RoomStage;
            foreach (var item in rS)
            {
               

                //房台id 
                int ftid = item.ID_RoomStage;

                List<CW_Bill> CWb = m.CW_Bill.Where(p => p.ID_Bill == (m.CW_Consumption.Where(o => o.ID_RoomStage == ftid).FirstOrDefault().ID_Bill)).ToList();
                foreach (var cW in CWb)
                {
                 DateTime dtNow =   DateTime.Now;
                  
                    //添加支付记录
                    CW_PayRecord cWpr = new CW_PayRecord();
                    cWpr.ID_Bill = cW.ID_Bill;
                    cWpr.ID_Guest = rs_id;
                    cWpr.Price_Pay = Convert.ToDecimal(TbTheCumulative.Text);
                    cWpr.Time_Pay = dtNow;
                    cWpr.PoP = "现金支付";
                    cWpr.State = true;//已支付
                    m.CW_PayRecord.Add(cWpr);
                    m.SaveChanges();
                    int cwprid = m.CW_PayRecord.Where(p => p.ID_Bill == cW.ID_Bill && p.ID_Guest == rs_id &&p.State ==false).ToList()[0].ID_PayRecord;


                    //账单
                    CW_Bill cwB = m.CW_Bill.Where(p => p.ID_Bill == cW.ID_Bill).SingleOrDefault();
                    cwB.Price = Convert.ToDecimal(TbTheCumulative.Text);
                    cwB.State_Bill = "已结账";
                    cwB.Time_PayBill = dtNow;
                    m.Entry(cwB).State = System.Data.Entity.EntityState.Modified;
                    m.SaveChanges();



                    //消费记录明细
                    List<CW_ConsumeDetail> cwCD = m.CW_ConsumeDetail.Where(p => p.ID_Consumption == (m.CW_Consumption.Where(c => c.ID_RoomStage == ftid).FirstOrDefault().ID_Consumption)).ToList();
                    foreach (var cW_Cme in cwCD)
                    {
                        CW_ConsumeDetail cW_Consume = m.CW_ConsumeDetail.Where(p => p.ID_Consumption == cW_Cme.ID_Consumption).FirstOrDefault();
                        cW_Consume.State_ComsumeDetail = false;
                        cW_Consume.ID_PayRecord = cwprid;
                        m.Entry(cW_Consume).State = System.Data.Entity.EntityState.Modified;
                        m.SaveChanges();
                    }
                }

                //修改房台状态
                SYS_RoomStage sysRs = m.SYS_RoomStage.Where(p => p.ID_RoomStage == item.ID_RoomStage).SingleOrDefault();
                sysRs.ID_Guest = null;
                sysRs.State_RoomStage = "未用";
                m.Entry(sysRs).State = System.Data.Entity.EntityState.Modified;
                m.SaveChanges();


                if (CWb.Count >0)
                {
                MessageBoxResult mb=    MessageBox.Show("结账成功", "大海提示",MessageBoxButton.OK,MessageBoxImage.Asterisk);

                    if (mb==MessageBoxResult.OK)
                    {
                        this.Close();
                    }
                }
            }



        }

        /// <summary>
        /// 打印账单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtPrintTheBill_Click(object sender, RoutedEventArgs e)
        {
            #region MyRegion
            string kz = TbGuestAccount.Text.Trim();
            //客人姓名
            string km = m.SYS_Guest.Where(p => p.ID_Guest == (m.VIP_Table.Where(v => v.Accounts.Trim()
               == kz).FirstOrDefault().ID_Guest)).SingleOrDefault().MC_Guest;

            //       客名    房号    房名    房单    开时    买时    时长  总金额   
            Tuple<string, string, string, string, string, string, string,Tuple<string>> tuple =
           new Tuple<string, string, string, string, string, string, string, Tuple<string>>
           (km, TbGuestAcount.Text.Trim().ToString(),
           TbRoomTableName.Text.Trim().ToString(), TbRoomWithSingleNumber.Text.Trim().ToString(),
           TbFoundingTime.Text.Trim().ToString(), TbTheCheckTime.Text.ToString().Trim(),
           TbDuration.Text.Trim().ToString(), new Tuple<string>(TbTheCumulative.Text.Trim().ToString()));
            List<Consumer> consumers = new List<Consumer>();
            consumers = con;
            W_Print_Data w = new W_Print_Data(tuple, consumers);
            w.ShowDialog();
            #endregion

            //W_Print_Data w_Print_Data = new W_Print_Data("FlowDocument.xaml",null, new OrderDocumentRenderer());


        }





        /***************    ****   我也是有底线的   ****    ***********************/




    }
}
