using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF_MvvMTest.EntityVo;
using WPF_MvvMTest.Model;
using WPF_MvvMTest.View.Windows.W_UC;

namespace WPF_MvvMTest.View.Windows
{
    
    /// <summary>
    /// W_RoomTableReservation.xaml 的交互逻辑
    /// </summary>
    public partial class W_RoomTableReservation : Window
    {
        public event Refresh Resh;
        List<RoomStage> roomStages;
        List<RoomStage> RsRight;
        List<RoomStage> RsLeft;
        static TabControl TabControl;//左边
        static TabControl  tabControl; //右边
        Model.EasternStar_WPF_MVVMEntities m = new Model.EasternStar_WPF_MVVMEntities();
        int ID_RoomStage;//房台id
        int ID_Guest;//客户id
        string tbRoomTable;
        string tbRoomTableName;
        string zhanghao = "111111";//客户帐号
        int Yydid;//预约单id
        public W_RoomTableReservation(List<RoomStage> rStages)
        {
            tabControl = DataGrid_right;
            TabControl = Room_DataGrid;
            roomStages = rStages;
            InitializeComponent();
            tbRoomTable = roomStages[0].Number_RoomStage.Trim();
            tbRoomTableName = roomStages[0].MC_RoomStage.Trim() + roomStages[0].Number_RoomStage.Trim();
        }
       
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///  
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //预定单号
            // TbOddNumbers.Text = DateTime.Now.ToFileTimeUtc().ToString();
            int Tb = m.YW_Subscribe.Count()+1;
            string count;
           
            count = "YD00" + Tb;
            
            TbOddNumbers.Text = count;

            ////客人帐号 
            TbAccounts.Text = zhanghao;
            ////房台号
            TbRoomTable.Text = roomStages[0].Number_RoomStage.Trim()==null? tbRoomTable: roomStages[0].Number_RoomStage.Trim();
            ////房台名
            TbRoomTableName.Text = (roomStages[0].MC_RoomStage.Trim() + roomStages[0].Number_RoomStage.Trim())==null? 
            tbRoomTableName: roomStages[0].MC_RoomStage.Trim() + roomStages[0].Number_RoomStage.Trim();


            //根据帐号查询预定表
            int ID_Guest_VIP = m.VIP_Table.Where(p => p.Accounts.Trim() == zhanghao.Trim()).Single().ID_Guest;
            int ID_Guest_YW = m.YW_Subscribe.Where(p => p.ID_Guest == ID_Guest_VIP).Count();
            ////表格加载前添加预定表
            
            if (ID_Guest_YW <=0)
            {
                YW_Subscribe y = new YW_Subscribe();
                y.ID_Guest = m.VIP_Table.Where(p => p.Accounts.Trim() == zhanghao).Single().ID_Guest;//客人id
                y.ID_VIP = m.VIP_Table.Where(p => p.Accounts.Trim() == zhanghao).Single().ID_VIP;//vipid
                y.HouseStageID = roomStages[0].ID_RoomStage.ToString() + ",";//客房id
                y.Number_Subscribe = TbOddNumbers.Text.Trim();//预约单号
                y.Remark = "我是黑大帅";
                y.Type_CheckIn = "足浴";
                m.YW_Subscribe.Add(y);
                m.SaveChanges();
            }else
            {
                //查询已存在的预约单id 
                Yydid = m.YW_Subscribe.Where(p => p.ID_Guest == ID_Guest_VIP && p.Number_Subscribe == TbOddNumbers.Text.Trim()).FirstOrDefault().ID_Subscribe;

            }
            //查询新增的id
            Yydid = m.YW_Subscribe.Where(p => p.ID_Guest == ID_Guest_VIP && p.Number_Subscribe == TbOddNumbers.Text.Trim()).Single().ID_Subscribe;

            ////修改房台状态
            ///
            //客人id 
            int keid = m.YW_Subscribe.Where(p => p.ID_Subscribe == Yydid).SingleOrDefault().ID_Guest;
            ID_RoomStage = roomStages[0].ID_RoomStage;
            SYS_RoomStage r = m.SYS_RoomStage.Where(p => p.ID_RoomStage == ID_RoomStage).Single();

            r.State_RoomStage = "预定";
            r.ID_Guest = keid;
            m.SaveChanges();

            //获取id 右边获取数据用
            ID_Guest = m.VIP_Table.Where(o => o.Accounts.Trim() == zhanghao).Single().ID_Guest;//顾客id
            string YWid = m.YW_Subscribe.Where(p => p.ID_Guest == ID_Guest).Single().HouseStageID;//预订单客户id

            //嵌套右边表格
            View.Windows.W_UC.W_RoomTableDataGridRight w_RoomTableDataGridRight = new W_UC.W_RoomTableDataGridRight(Yydid,0);
            w_RoomTableDataGridRight.RetM +=new ReuntMessage(ReceiveRight);//定义委托
            heeh.Content = w_RoomTableDataGridRight;
            heeh.IsSelected = true;

            //嵌套左边的表格
            View.Windows.W_UC.W_RoomTableDastaGridLeft w_RoomTableDataGridLeft = new W_UC.W_RoomTableDastaGridLeft(roomStages);
            w_RoomTableDataGridLeft.ReuntLeft += new GetGridMessage(ReceiveLeft);//定义委托
            kexuanfangtai.Content = w_RoomTableDataGridLeft;
            kexuanfangtai.IsSelected = true;
            
          
          
          
         
           
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
        ///  添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtAdd_Click(object sender, RoutedEventArgs e)
        {
            //获取选中行
            if (RsLeft ==null ||RsLeft.Count ==0)
            {
                RsLeft = STATIC_cache.StaticRoomStages;
                //STATIC_cache.StaticRoomStages.Clear();
            }
            var id = (from tv in m.VIP_Table
                      join ty in m.YW_Subscribe on tv.ID_Guest equals ty.ID_Guest
                      where tv.Accounts.Trim() == zhanghao select new
                      {
                          ty.ID_Guest,
                          ty.ID_Subscribe
                      }).SingleOrDefault();
            //roomStage.ID_RoomStage;
            // dgdt.ItemsSource[roomStage.ID_RoomStage].
            //id.ID_Subscribe



            string strHouseStageID = m.YW_Subscribe.Where(p => p.ID_Subscribe == id.ID_Subscribe).SingleOrDefault().HouseStageID;

            YW_Subscribe y = m.YW_Subscribe.Where(p => p.ID_Subscribe == id.ID_Subscribe).SingleOrDefault();
            y.HouseStageID = strHouseStageID +  RsLeft[0].ID_RoomStage.ToString() + ",";
            m.Entry(y).State = System.Data.Entity.EntityState.Modified;
            m.SaveChanges();
            int getIDR = RsLeft[0].ID_RoomStage;
            int krid=   m.YW_Subscribe.Where(p => p.ID_Subscribe == id.ID_Subscribe).SingleOrDefault().ID_Guest;
            SYS_RoomStage o = m.SYS_RoomStage.Where(p => p.ID_RoomStage == getIDR).SingleOrDefault();
            o.ID_Guest = krid;
            o.State_RoomStage = "预定";



            //获取已修改的房台数据
            List<RoomStage> rooms = (from tb in m.SYS_RoomStage where tb.ID_RoomStage == getIDR
                                     select new RoomStage
                                     {
                                         ID_RoomStage = tb.ID_RoomStage,
                                         Number_RoomStage = tb.Number_RoomStage,//房号
                                         MC_RoomStage = tb.MC_RoomStage,
                                         //State_RoomStage = tb.State_RoomStage == "已用" ? "" : "否"
                                     }).ToList() ;

            if (m.SaveChanges()>0)
            {
              MessageBoxResult messageBoxResult =   MessageBox.Show("添加成功！","大海提示",MessageBoxButton.OK,MessageBoxImage.Asterisk);
                if (messageBoxResult ==MessageBoxResult.OK)
                {
                    //右边表格刷新
                    View.Windows.W_UC.W_RoomTableDataGridRight w_RoomTableDataGridRight = new W_UC.W_RoomTableDataGridRight(Yydid,0);
                   
                    heeh.Content = null;
                    heeh.Content = w_RoomTableDataGridRight;
                    w_RoomTableDataGridRight.UserControl_Loaded(null, null);
                    //左边表格刷新
                 
                    View.Windows.W_UC.W_RoomTableDastaGridLeft w_RoomTableDataGridLeft = new W_UC.W_RoomTableDastaGridLeft(rooms);
                    kexuanfangtai.Content = null;
                    kexuanfangtai.Content = w_RoomTableDataGridLeft;
                    w_RoomTableDataGridLeft.UserControl_Loaded(null,null);
                
                }

            }
            RsLeft.Clear();
        }
     
           /// <summary>
           /// 获取目标数据
           /// </summary>
           /// <param name="id"></param>m
           /// <returns></returns>
        public SYS_RoomStage GetRoomStageID(int id)
        {
            SYS_RoomStage sYS_Room = (from tb in m.SYS_RoomStage where tb.ID_RoomStage == id select tb).Single();

            ID_RoomStage = sYS_Room.ID_RoomStage;
            return sYS_Room;
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            //目标数据
            string ycft = (m.YW_Subscribe.Where(p => p.ID_Guest ==(m.VIP_Table.Where(q => q.Accounts.Trim().ToString()
            == zhanghao).FirstOrDefault().ID_Guest)).Single().HouseStageID);
            if (RsRight ==null || RsLeft.Count == 0)
            {
                RsRight = STATIC_cache.StaticRoomStages;
            }
            //预定房台.Except(RsRight[0].ID_RoomStage);
            //分割字符
            List<string> vs = ycft.Split(',').ToList();
            List<string> s =vs;

            //移出房台id
            string RRIDR = RsRight[0].ID_RoomStage.ToString();
            for (int i = 0; i < vs.Count; i++)
            {
                if (vs[i] =="")
                {
                    vs.RemoveAt(i);
                }
                if (vs[i] == RRIDR)
                {
                    vs.RemoveAt(i);
                }
            }
            //剩下的房台id 
                foreach (var item in vs)
                {
                    
                if (item =="")
                {
                    ycft = "";
                }
                else
                {
                    ycft += item + ",";
                }
                    
                }
            
                   
            
            //目标订单id 
            int yddid = m.YW_Subscribe.Where(p => p.ID_Guest == 
            (m.VIP_Table.Where(o => o.Accounts.Trim() == zhanghao.Trim()).FirstOrDefault().ID_Guest)).Single().ID_Subscribe;


            YW_Subscribe yW = m.YW_Subscribe.Where(p => p.ID_Subscribe == yddid).Single();
            yW.HouseStageID = ycft;
            m.Entry(yW).State = System.Data.Entity.EntityState.Modified;
            m.SaveChanges();
            int Id_RoomStage = RsRight[0].ID_RoomStage;
            SYS_RoomStage RS = m.SYS_RoomStage.Where(p => p.ID_RoomStage == Id_RoomStage).Single();
            RS.ID_Guest = null;
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
            if (m.SaveChanges()>0)
            {
                //右边表格刷新
                View.Windows.W_UC.W_RoomTableDataGridRight w_RoomTableDataGridRight = new W_UC.W_RoomTableDataGridRight(Yydid,0);

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
        /// 加预定台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtAddScheduledStation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("添加成功", "大海提示",MessageBoxButton.OKCancel,MessageBoxImage.Asterisk);
        }
        /// <summary>
        /// 预定取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtDueToCancellation_Click(object sender, RoutedEventArgs e)
        {
            //获取单号
            string odd = TbOddNumbers.Text.ToString();
            //房台id集合
            string ftid = m.YW_Subscribe.Where(p => p.Number_Subscribe == odd).Single().HouseStageID;
            List<string> vs = ftid.Split(',').ToList();
            foreach (var item in vs)
            {
                SYS_RoomStage SRS = m.SYS_RoomStage.Where(p => p.ID_RoomStage.ToString().Trim() == item.Trim()).SingleOrDefault();
                SRS.ID_Guest = null;
                SRS.State_RoomStage = "未用";
            }
            YW_Subscribe YWS = m.YW_Subscribe.Where(p => p.Number_Subscribe == odd).SingleOrDefault();
            m.YW_Subscribe.Remove(YWS);
            if (m.SaveChanges()>0)
            {
                MessageBox.Show("预定取消成功！","大海提示",MessageBoxButton.OKCancel,MessageBoxImage.Asterisk);
            }

        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtPreserve_Click(object sender, RoutedEventArgs e)
        {
          
            if (string.IsNullOrEmpty(TbAccounts.Text.ToString()) || 
                string.IsNullOrEmpty(TbRoomTable.Text.ToString())|| 
                string.IsNullOrEmpty(TbRoomTableName.Text.ToString()) ||
                string.IsNullOrEmpty(TbOddNumbers.Text.ToString())||
                string.IsNullOrEmpty(TbDevelopmentTime.Text.ToString()) ||
                string.IsNullOrEmpty(TbPopulation.Text.ToString())||
                string.IsNullOrEmpty(TbDiscount.Text.ToString())||
                string.IsNullOrEmpty(TbRemark.Text.ToString()))
            {
                MessageBox.Show("请将信息填写完整", "大海提示",MessageBoxButton.OKCancel,MessageBoxImage.Asterisk);
                return;
            }
                


            //预定单号
            string odd = TbOddNumbers.Text.ToString();
            YW_Subscribe YWS = m.YW_Subscribe.Where(p => p.Number_Subscribe == odd).SingleOrDefault();
            YWS.Time_Predict = Convert.ToDateTime(TbDevelopmentTime.Text.Trim());
            YWS.Number_People = Convert.ToInt32(TbPopulation.Text);
            YWS.Remark = TbRemark.Text.Trim();
            m.Entry(YWS).State = System.Data.Entity.EntityState.Modified;
            m.SaveChanges();
            //折扣
            
         

            //生成账单
                int yddid = m.YW_Subscribe.Where(p => p.Number_Subscribe == odd).SingleOrDefault().ID_Subscribe;//预订单id

                string  Mun = "ZD000" + m.CW_Bill.Count() + 1;//账单号
                CW_Bill Cb = new CW_Bill();
                Cb.SuOp_ID = yddid;//预订单id
                Cb.Number_Bill = Mun;


                Cb.Remark = "预定";//备注
                Cb.State_Bill = "预定";//状态
                m.CW_Bill.Add(Cb);
                m.SaveChanges();
            //保存数据
           // STATIC_cache.ScBill_id = Mun;

            //预订单房台id
            string ftid = m.YW_Subscribe.Where(p => p.Number_Subscribe == odd).SingleOrDefault().HouseStageID;
            List<string> vs = ftid.Split(',').ToList();
            //生成消费单方法
            int mSc=0;
            foreach (var item in vs)
            {
                if (item != "")
                {
                    CW_Consumption Cc = new CW_Consumption();
                    //房台id 
                    Cc.ID_RoomStage = Convert.ToInt32(item);
                    //账单id 
                    Cc.ID_Bill = m.CW_Bill.Where(p => p.SuOp_ID == yddid).SingleOrDefault().ID_Bill;
                    //折扣、TbDiscount
                    Cc.Discount = Convert.ToDecimal(TbDiscount.Text);
                    m.CW_Consumption.Add(Cc);
                    mSc= m.SaveChanges();
                }
                else
                {
                    break; 
                }
            }
            if (mSc >0)
            {
              MessageBoxResult messageBoxResult =  MessageBox.Show("保存成功", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
                if (messageBoxResult ==MessageBoxResult.OK)
                {
                    this.Close();

                  
                }
                Resh();
            }
          
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtShut_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
