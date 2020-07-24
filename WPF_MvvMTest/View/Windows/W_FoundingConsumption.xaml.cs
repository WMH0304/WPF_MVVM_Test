using System;
using System.Collections;
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
using WPF_MvvMTest.View.Windows.W_UC;

namespace WPF_MvvMTest.View.Windows
{
   public delegate void Refresh();
    /// <summary>
    /// W_FoundingConsumption.xaml 的交互逻辑
    /// </summary>
    public partial class W_FoundingConsumption : Window
    {
        public event Refresh Resh;

         List<RoomStage> RsRight;
        List<RoomStage> RsLeft;
        List<RoomStage> rS;
        List<RoomStage> rSts;
        string zhanghao = "111111";//客户帐号 
        string tbRoomTable;
        string tbRoomTableName;
        public W_FoundingConsumption(List<RoomStage> roomStages)
        {
            rSts = rS = roomStages;
            InitializeComponent();
            tbRoomTable = roomStages[0].Number_RoomStage.Trim();
            tbRoomTableName = roomStages[0].MC_RoomStage.Trim() + roomStages[0].Number_RoomStage.Trim();
        }
        Model.EasternStar_WPF_MVVMEntities m = new Model.EasternStar_WPF_MVVMEntities();
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*
             * 
             * 开台消费：
             * 
             * 主页选择房台号后，跳转到开台
             * 根据账号查询出顾客的信息，并向预约单询问顾客是否存在预订单
             * 如果存在，根据顾客预定的房台 开台消费
             *  如果不存在，直接开台消费
             *  
             */

            TbAccounts.Text = zhanghao;
            ////房台号
            TbRoomTable.Text = rS[0].Number_RoomStage.Trim() == null ? tbRoomTable : rS[0].Number_RoomStage.Trim();
            ////房台名
            TbRoomTableName.Text = (rS[0].MC_RoomStage.Trim() + rS[0].Number_RoomStage.Trim()) == null ?
            tbRoomTableName : rS[0].MC_RoomStage.Trim() + rS[0].Number_RoomStage.Trim();
            //开台单号 
            TbOddNumbers.Text = "KT000" + m.YW_OpenStage.Count() + 1;
            //开始时间
            TbDevelopmentTime.Text = DateTime.Now.ToLongDateString();
            //会员id 
            int vip_id = m.VIP_Table.Where(p => p.Accounts == zhanghao).SingleOrDefault().ID_VIP;

            // int yddid = 1;

            //查看预约表中是否存在对应会员
            int Sub = m.YW_Subscribe.Where(p => p.ID_VIP == vip_id).Count();
         
            //如果存在预约单
            if (Sub > 0)
            {
                int Yydid = m.YW_Subscribe.Where(p => p.ID_VIP == vip_id).SingleOrDefault().ID_Subscribe;
               
                //如果存在直接从预约房id 开台
                string RoomID = m.YW_Subscribe.Where(p => p.ID_VIP == vip_id).SingleOrDefault().HouseStageID;
                //开台表
                YW_OpenStage oS = new YW_OpenStage();
                oS.ID_VIP = vip_id;
                //oS.Number_People = Convert.ToInt32(TbPopulation.Text);
                oS.Remark = TbRemark.Text.ToString();
                oS.HouseStageID = RoomID +",";
                oS.Time_Predict = DateTime.Now;
                m.YW_OpenStage.Add(oS);
                m.SaveChanges();
                List<RoomStage> rooms = rS;
                rooms.Clear();
                List<string> vs = RoomID.Split(',').ToList();
                for (int i = 0; i < vs.Count; i++)
                {

                    int vsid =0;
                    if (vs[i] == "")
                    {
                        vs.Remove(vs[i]);
                    }
                    else
                    {
                        try
                        {
                             vsid = Convert.ToInt32(vs[i]);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        //改变房台状态
                        SYS_RoomStage sRs = m.SYS_RoomStage.Where(p => p.ID_RoomStage == vsid).SingleOrDefault();
                        sRs.State_RoomStage = "已用";
                        m.Entry(sRs).State = System.Data.Entity.EntityState.Modified;
                        m.SaveChanges();

                        rooms.AddRange(from tb in m.SYS_RoomStage
                                       where tb.ID_RoomStage == vsid
                                       select new RoomStage
                                       {
                                           Number_RoomStage = tb.Number_RoomStage,
                                           MC_RoomStage = tb.MC_RoomStage,
                                           State_RoomStage = tb.State_RoomStage,
                                           Describe = tb.Describe,
                                           JionSign = tb.JionSign,
                                       });
                    }
                }
                //嵌套右边表格
                View.Windows.W_UC.W_RoomTableDataGridRight w_RoomTableDataGridRight = new W_UC.W_RoomTableDataGridRight(Yydid,0);
                w_RoomTableDataGridRight.RetM += new ReuntMessage(ReceiveRight);//定义委托
                heeh.Content = w_RoomTableDataGridRight;
                heeh.IsSelected = true;

                //嵌套左边的表格
                View.Windows.W_UC.W_RoomTableDastaGridLeft w_RoomTableDataGridLeft = new W_UC.W_RoomTableDastaGridLeft(rooms);
                w_RoomTableDataGridLeft.ReuntLeft += new GetGridMessage(ReceiveLeft);//定义委托
                kexuanfangtai.Content = w_RoomTableDataGridLeft;
                kexuanfangtai.IsSelected = true;

                //删除预定信息
                YW_Subscribe ySu = m.YW_Subscribe.Where(p => p.ID_VIP == vip_id).SingleOrDefault();
                m.YW_Subscribe.Remove(ySu);
                m.SaveChanges();
            }
            //当你没有预定时 
            else
            {

                //根据会员 id 找到客人id
                int Guest_id = m.SYS_Guest.Where(p => p.ID_Guest == 
                (m.VIP_Table.Where(v => v.ID_VIP == vip_id).FirstOrDefault().ID_Guest)).SingleOrDefault().ID_Guest;

                //房台状态
                int id_Rs = rS[0].ID_RoomStage;
                SYS_RoomStage Sr = m.SYS_RoomStage.Where(p => p.ID_RoomStage == id_Rs).Single();
                Sr.State_RoomStage = "已用";
                m.Entry(Sr).State = System.Data.Entity.EntityState.Modified;
                m.SaveChanges();
                
                //新增开台表
                YW_OpenStage oS = new YW_OpenStage();
                oS.ID_VIP = vip_id;
                //oS.Number_People = Convert.ToInt32(TbPopulation.Text);
                // oS.Remark = TbRemark.Text.ToString();
                oS.GuestID = Guest_id.ToString();
                oS.Type_CheckIn = "足浴";
                oS.Remark = "鬼知道你备注什么";
                oS.Content_Message = "谁设计的奇葩";
                oS.HouseStageID = rS[0].ID_RoomStage + ",";
                oS.Time_Predict = DateTime.Now;
                oS.Time_Leave = DateTime.Now;
                m.YW_OpenStage.Add(oS);
                m.SaveChanges();

                //
                int os_id = m.YW_OpenStage.Where(p => p.ID_VIP == vip_id).SingleOrDefault().ID_OpenStage;

                //嵌套右边表格
                View.Windows.W_UC.W_RoomTableDataGridRight w_RoomTableDataGridRight = new W_UC.W_RoomTableDataGridRight(0, os_id);
                w_RoomTableDataGridRight.RetM += new ReuntMessage(ReceiveRight);//定义委托
                heeh.Content = w_RoomTableDataGridRight;
                heeh.IsSelected = true;

                //嵌套左边的表格
                View.Windows.W_UC.W_RoomTableDastaGridLeft w_RoomTableDataGridLeft = new W_UC.W_RoomTableDastaGridLeft(rSts);
                w_RoomTableDataGridLeft.ReuntLeft += new GetGridMessage(ReceiveLeft);//定义委托
                kexuanfangtai.Content = w_RoomTableDataGridLeft;
                kexuanfangtai.IsSelected = true;


            }
            //刷新父页面
            Resh();
        }




        /// <summary>
        /// 接受 右边表格的数据
        /// </summary>
        /// <param name="rooms"></param>
        public void ReceiveRight(List<RoomStage> rooms)
        {
            RsRight = new List<RoomStage>();
            RsRight.Clear();
            // RsRight.Add(rooms);
            RsRight.AddRange(rooms);
        }

        /// <summary>
        /// 接收 左边表格数据
        /// </summary>
        /// <param name="rooms"></param>
        public void ReceiveLeft(List<RoomStage> rooms)
        {
            RsLeft = new List<RoomStage>();
            RsLeft.Clear();
            RsLeft.AddRange(rooms);
        }



        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            //获取选中行
            if (RsLeft == null || RsLeft.Count == 0)
            {
                RsLeft = STATIC_cache.StaticRoomStages;
                //STATIC_cache.StaticRoomStages.Clear();
            }
            if (RsLeft == null || RsLeft.Count == 0)
            {
                MessageBox.Show("请选择需要添加的房台", "大海提示",MessageBoxButton.OK,MessageBoxImage.Asterisk);
                return;
            }
            var id = (from tv in m.VIP_Table
                      join to in m.YW_OpenStage on tv.ID_VIP equals to.ID_VIP
                      where tv.Accounts.Trim() == zhanghao
                      select new
                      {
                          tv.ID_VIP,
                      }).SingleOrDefault();
          
            string HouseStageID = m.YW_OpenStage.Where(p => p.ID_VIP == id.ID_VIP).SingleOrDefault().HouseStageID;

            YW_OpenStage Yos = m.YW_OpenStage.Where(p => p.ID_VIP == id.ID_VIP).SingleOrDefault();
            Yos.HouseStageID = HouseStageID + RsLeft[0].ID_RoomStage + ",";
            m.Entry(Yos).State = System.Data.Entity.EntityState.Modified;
            m.SaveChanges();
           
            int getIDR = RsLeft[0].ID_RoomStage;
            SYS_RoomStage o = m.SYS_RoomStage.Where(p => p.ID_RoomStage == getIDR).SingleOrDefault();
            o.State_RoomStage = "已用";
            m.Entry(o).State = System.Data.Entity.EntityState.Modified;
            

            //获取已修改的房台数据
            List<RoomStage> rooms = (from tb in m.SYS_RoomStage
                                     where tb.ID_RoomStage == getIDR
                                     select new RoomStage
                                     {
                                         ID_RoomStage = tb.ID_RoomStage,
                                         Number_RoomStage = tb.Number_RoomStage,//房号
                                         MC_RoomStage = tb.MC_RoomStage,
                                         //State_RoomStage = tb.State_RoomStage == "已用" ? "" : "否"
                                     }).ToList();

            if (m.SaveChanges() > 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("添加成功！", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    //开台表id 
                    int os_id = m.YW_OpenStage.Where(p => p.ID_VIP == id.ID_VIP).SingleOrDefault().ID_OpenStage;

                    //右边表格刷新
                    View.Windows.W_UC.W_RoomTableDataGridRight w_RoomTableDataGridRight = new W_UC.W_RoomTableDataGridRight(0,os_id);
                    heeh.Content = null;
                    heeh.Content = w_RoomTableDataGridRight;
                    w_RoomTableDataGridRight.UserControl_Loaded(null, null);
                    
                    
                    //左边表格刷新
                    View.Windows.W_UC.W_RoomTableDastaGridLeft w_RoomTableDataGridLeft = new W_UC.W_RoomTableDastaGridLeft(rooms);
                    kexuanfangtai.Content = null;
                    kexuanfangtai.Content = w_RoomTableDataGridLeft;
                    w_RoomTableDataGridLeft.UserControl_Loaded(null, null);

                }

            }
            RsLeft.Clear();
        }



        /// <summary>
        /// 移除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            //目标数据
            string ycft = (m.YW_OpenStage.Where(p => p.ID_VIP == (m.VIP_Table.Where(q => q.Accounts.Trim().ToString()
             == zhanghao).FirstOrDefault().ID_VIP)).Single().HouseStageID);
            if (RsRight == null || RsRight.Count == 0)
            {
                RsRight = STATIC_cache.StaticRoomStages;
            }
            //预定房台.Except(RsRight[0].ID_RoomStage);
            //分割字符
            List<string> vs = ycft.Split(',').ToList();
            List<string> s = vs;

            //移出房台id
            string RRIDR = RsRight[0].ID_RoomStage.ToString();
            for (int i = 0; i < vs.Count; i++)
            {
                if (vs[i] == "")
                {
                    vs.RemoveAt(i);
                    break;
                }
                if (vs[i] == RRIDR)
                {
                    vs.RemoveAt(i);
                    break;
                }
            }
            //剩下的房台id 
            string ycftid = "";
            foreach (var item in vs)
            {

                if (item == "")
                {
                    break;
                }
                else
                {
                    ycftid += item + ",";
                }
            }
            //目标订单id 
            int ktid = m.YW_OpenStage.Where(p => p.ID_VIP ==
            (m.VIP_Table.Where(o => o.Accounts.Trim() == zhanghao.Trim()).FirstOrDefault().ID_VIP)).Single().ID_OpenStage;

            YW_OpenStage Yos = m.YW_OpenStage.Where(p => p.ID_OpenStage == ktid).Single();
            Yos.HouseStageID = ycftid;
            m.Entry(Yos).State = System.Data.Entity.EntityState.Modified;
            m.SaveChanges();
            int Id_RoomStage = RsRight[0].ID_RoomStage;
            SYS_RoomStage RS = m.SYS_RoomStage.Where(p => p.ID_RoomStage == Id_RoomStage).Single();
            RS.State_RoomStage = "未用";
            m.Entry(RS).State = System.Data.Entity.EntityState.Modified;

            //获取已修改的房台数据
            List<RoomStage> rooms = (from tb in m.SYS_RoomStage
                                     where tb.ID_RoomStage == Id_RoomStage
                                     select new RoomStage
                                     {
                                         ID_RoomStage = tb.ID_RoomStage,
                                         Number_RoomStage = tb.Number_RoomStage,//房号
                                         MC_RoomStage = tb.MC_RoomStage,
                                         //State_RoomStage = tb.State_RoomStage == "已用" ? "" : "否"
                                     }).ToList();
            if (m.SaveChanges() > 0)
            {
                //右边表格刷新
                View.Windows.W_UC.W_RoomTableDataGridRight w_RoomTableDataGridRight = new W_UC.W_RoomTableDataGridRight(0, RsRight[0].ID_RoomStage);

                heeh.Content = null;
                heeh.Content = w_RoomTableDataGridRight;
                w_RoomTableDataGridRight.UserControl_Loaded(null, null);
                //左边表格刷新

                View.Windows.W_UC.W_RoomTableDastaGridLeft w_RoomTableDataGridLeft = new W_UC.W_RoomTableDastaGridLeft(rooms);
                kexuanfangtai.Content = null;
                kexuanfangtai.Content = w_RoomTableDataGridLeft;
                w_RoomTableDataGridLeft.UserControl_Loaded(null, null);
            }
            RsRight.Clear();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //刷新页面
            Resh();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            
            if (TbOddNumbers.Text =="" || TbOddNumbers.Text == null &&
                TbDevelopmentTime.Text == "" || TbDevelopmentTime.Text == null &&
                TbPopulation.Text == "" || TbPopulation.Text == null &&
                TbDiscount.Text == "" || TbDiscount.Text == null &&
                TbRemark.Text == "" || TbRemark.Text == null )
            {
                MessageBox.Show("你大爷的，漏数据了","大海提示",MessageBoxButton.OK,MessageBoxImage.Asterisk);
                return;
            }




            //客人id 
            int krid = m.VIP_Table.Where(p => p.Accounts == zhanghao).SingleOrDefault().ID_Guest;
            //账单表
            CW_Bill cB = new CW_Bill();
            cB.Time_PayBill = null;
            cB.Remark = "该怎么形容你最贴切";
            cB.Price = 0;
            cB.State_Bill = "正在消费";
            cB.Number_Bill = zhanghao;
            m.CW_Bill.Add(cB);
            m.SaveChanges();

            int cbid = m.CW_Bill.Where(p => p.Number_Bill == zhanghao).SingleOrDefault().ID_Bill;
            //支付记录表录入
            CW_PayRecord cP = new CW_PayRecord();
            cP.ID_Guest = krid;
            cP.Price_Pay = 0;
            cP.Time_Pay = null;
            cP.PoP = "";
            cP.State = false;
            cP.ID_Bill = cbid;
            m.CW_PayRecord.Add(cP);
            m.SaveChanges();

            //开台id
            int ktid = m.YW_OpenStage.Where(p => p.ID_VIP ==
            (m.VIP_Table.Where(v => v.ID_Guest == krid).FirstOrDefault().ID_VIP)).SingleOrDefault().ID_OpenStage;

            //开台房台id
            string ktftid = m.YW_OpenStage.Where(p => p.ID_OpenStage == ktid).Single().HouseStageID;

            //分割数组
            List<string> vs = ktftid.Split(',').ToList();
            for (int i = 0; i < vs.Count(); i++)
            {
                if (vs[i] =="")
                {
                    vs.Remove(vs[i]);
                    break;
                }
                int ftid = Convert.ToInt32(vs[i]);
                CW_Consumption cC = new CW_Consumption();
                cC.ID_Bill = cbid;
                cC.ID_RoomStage = ftid;
                cC.Discount = Convert.ToDecimal(TbDiscount.Text) == 0 ? throw new NullReferenceException() : Convert.ToDecimal(TbDiscount.Text);
                cC.Time_Consumption =DateTime.Now.ToLocalTime();
                m.CW_Consumption.Add(cC);
                m.SaveChanges();
               
            }

        }
       




        /**************        这个 class 也是有底线的        *************/
    }
}
