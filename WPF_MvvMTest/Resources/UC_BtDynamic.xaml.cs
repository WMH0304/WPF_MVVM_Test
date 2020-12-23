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

namespace WPF_MvvMTest.Resources
{
    /// <summary>
    /// UC_BtDynamic.xaml 的交互逻辑
    /// </summary>
    /// 

    public delegate void Pus();
    public delegate void GetButton(List<RoomStage> roomStages);
   
    public partial class UC_BtDynamic : Button
    {
        public event Pus pus;
        public event GetButton GetButtons;
        public UC_BtDynamic(List<RoomStage> btL)
        {
           
            InitializeComponent();
            this.btL = btL;
        }
        //获取控件数据
        string tbRoom_name = "";
        string cbStatus = "";
        string tbDescribe = "";
        string cbClass = "";
        int ID_RoomStage = 0;
        string Number_RoomStage = "";
        static Button button;
        Tuple<string, string, string, string, int,string> tuple;
        //实例化实体
        Model.EasternStar_WPF_MVVMEntities m= new Model.EasternStar_WPF_MVVMEntities();
        private List<RoomStage> btL;

        //点击事件
        private void BtDynamic_Click(object sender, RoutedEventArgs e)
        {
            button = sender as Button;
            for (int i = 0; i < btL.Count(); i++)
            {
                if (button.Name == "btn" + btL[i].ID_RoomStage)
                {
                     List<RoomStage> GB = btL.Where(p => p.ID_RoomStage == btL[i].ID_RoomStage).ToList();
                    //var d = (from tb in btL
                    //         where tb.ID_RoomStage == btL[i].ID_RoomStage
                    //         select new RoomStage
                    //         {
                    //             ID_RoomStage = tb.ID_RoomStage,
                    //             Number_RoomStage = tb.Number_RoomStage,
                    //             MC_RoomStage = tb.MC_RoomStage,
                    //             State_RoomStage = tb.State_RoomStage,
                    //             Describe = tb.Describe,
                    //             JionSign = tb.JionSign
                    //         }).ToList() ;
                     GetButtons(GB);
                    
                    break;
                }
            }   
        }

        //鼠标移入事件
        private void BtDynamic_MouseEnter(object sender, MouseEventArgs e)
        {
            button = sender as Button;
            string s = button.Name;

            // myBtDynamic.Name = "btn" + BtL[i].ID_RoomStage;
            for (int i = 0; i < btL.Count(); i++)
            {
                if (button.Name == "btn" + btL[i].ID_RoomStage)
                {
                    // MessageBox.Show(btL[i].Number_RoomStage + "\n" + btL[i].State_RoomStage, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    if (btL[i].State_RoomStage.Trim() == "已用")
                    {
                        // MessageBox.Show(btL[i].Number_RoomStage + "\n" + btL[i].State_RoomStage +"\n", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        //button.ToolTip = "sadfasdfasd";
                        //房号
                        int ID_RoomStage = btL[i].ID_RoomStage;
                       List<BtMessage> MB = (from tbR in m.SYS_RoomStage
                                           //消费
                                       join tbCWC in m.CW_Consumption on tbR.ID_RoomStage equals tbCWC.ID_RoomStage
                                       //账单
                                       join tbCWB in m.CW_Bill on tbCWC.ID_Bill equals tbCWB.ID_Bill
                                       //支付记录
                                       join tbCWP in m.CW_PayRecord on tbCWB.ID_Bill equals tbCWP.ID_Bill
                                       //客人信息88888888888887
                                       join tbSYSG in m.SYS_Guest on tbCWP.ID_Guest equals tbSYSG.ID_Guest
                                       //会员
                                       join tbVIPT in m.VIP_Table on tbSYSG.ID_Guest equals tbVIPT.ID_Guest
                                       //开台
                                       join tbYWOS in m.YW_OpenStage on tbVIPT.ID_VIP equals tbYWOS.ID_VIP
                                       where tbR.ID_RoomStage == ID_RoomStage && tbR.State_RoomStage.Trim() =="已用"
                                             select new  BtMessage{

                                           MC_Guest     =    tbSYSG.MC_Guest,//名称
                                           Number_Bill  =    tbCWB.Number_Bill,//单号
                                           Time_Predict =    tbYWOS.Time_Predict,//开台时间
                                           Time_Leave   =    tbYWOS.Time_Leave,//离开时间
                                           Price_Pay    =    tbCWP.Price_Pay,//支付金额
                                           Prict        =    tbCWC.Prict,//消费金额
                                           
                                       }).ToList();
                        foreach (var item in MB)
                        {
                            var a = item.Price_Pay - item.Prict;
                            button.ToolTip = 
                                "客人名称： " + item.MC_Guest +"\n"+
                                "账单单号： " + item.Number_Bill +"\n"+
                                "开台时间： " + item.Time_Predict +"\n" +
                                "离开时间： " + item.Time_Leave +"\n"+
                                "支付金额： " + item.Price_Pay +"\n"+
                                "消费金额： " + item.Prict +"\n"+
                                "剩余金额： " + a;
                        }
                    }
                    if (btL[i].State_RoomStage.Trim() == "预定")
                    {
                        int rid = btL[i].ID_RoomStage;
                        int? keid = m.SYS_RoomStage.Where(p => p.ID_RoomStage == rid).Single().ID_Guest;
                        var item = (from tb in m.YW_Subscribe
                                  
                                    join tG in m.SYS_Guest on tb.ID_Guest equals tG.ID_Guest
                                    join tV in m.VIP_Table on tG.ID_Guest equals tV.ID_Guest
                                    where tb.ID_Guest == keid
                                    select new
                                    {
                                        tG.MC_Guest, //顾客名称
                                        tG.gender,//顾客性别
                                        tb.HouseStageID,//预定房台
                                        tb.Number_People,//人数
                                        tb.Number_Subscribe,//预约单号
                                        tb.Time_Predict,//预定时间
                                        
                                        tV.Accounts,
                                    }).SingleOrDefault();
                        if (item ==null)
                        {
                            return;
                        }
                        string gDe = item.gender.Trim() == "1" ? "男" : "女";

                        EntityVo.STATIC_cache.Number_Subscribe = item.Number_Subscribe.Trim(); 
                        button.ToolTip =
                            "客人名称： " + item.MC_Guest + "\n" +
                            "\0\0\0账号:  " + item.Accounts + "\n"+
                            "顾客性别： " + gDe + "\n"+
                            "预定房台： " + item.HouseStageID + "\n" +
                            "\0\0\0人数: " + item.Number_People + "\n" +
                            "预约单号： " + item.Number_Subscribe + "\n" +
                            "预定时间： " + item.Time_Predict + "\n";
                    }
                    if (btL[i].State_RoomStage.Trim() == "停用" || btL[i].State_RoomStage.Trim() == "未用")
                    {


                        button.ToolTip = 
                            "  房台号： " + btL[i].Number_RoomStage +"\n"+
                            "房台名称： " + btL[i].MC_RoomStage +"\n"+
                            "房台状态： " + btL[i].State_RoomStage +"\n"+
                            "连房标志： " + btL[i].JionSign
                            ;
                    }
                }
            }
        }
        //鼠标移开
        private void BtDynamic_MouseLeave(object sender, MouseEventArgs e)
        {

        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiUpdate_Click(object sender, RoutedEventArgs e)
        {
            /*
             *  TbRoom_Name.Text = "";
                  CbStatus.Text = "";
                  TbDescribe.Text = "";
                  CbClass.Text = "";
            */


            for (int i = 0; i < btL.Count(); i++)
            {
              

                if (button.Name == "btn" + btL[i].ID_RoomStage) {
                    ///string s = btL[i].Jc_Class;
                    ///
                    tbRoom_name = btL[i].MC_RoomStage.ToString().Trim();
                    cbStatus = btL[i].State_RoomStage.ToString().Trim();
                    tbDescribe = btL[i].Describe.ToString().Trim();
                    cbClass = btL[i].ID_Class.ToString().Trim();
                   // cbClass = btL[i].Jc_Class.ToString().Trim();
                    ID_RoomStage = btL[i].ID_RoomStage;
                    Number_RoomStage = btL[i].Number_RoomStage;
                }
            }

             tuple = new Tuple<string, string, string, string,int,string >(tbRoom_name, cbStatus, tbDescribe, cbClass , ID_RoomStage, Number_RoomStage);
            View.Windows.W_ButtonAdd w_ButtonAdd = new View.Windows.W_ButtonAdd(tuple,0);
            w_ButtonAdd.ShowDialog();

        }

       
        /// <summary>
        /// 换台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiChangechannel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            for (int i = 0; i < btL.Count(); i++)
            {


                if (button.Name == "btn" + btL[i].ID_RoomStage)
                {
                    ///string s = btL[i].Jc_Class;
                    ///
                    tbRoom_name = btL[i].MC_RoomStage.ToString().Trim();
                    cbStatus = btL[i].State_RoomStage.ToString().Trim();
                    tbDescribe = btL[i].Describe.ToString().Trim();
                    //cbClass = btL[i].Jc_Class.ToString().Trim();
                    cbClass = btL[i].ID_Class.ToString().Trim();
                    ID_RoomStage = btL[i].ID_RoomStage;
                    Number_RoomStage = btL[i].Number_RoomStage;
                }
            }

            tuple = new Tuple<string, string, string, string, int, string>(tbRoom_name, cbStatus, tbDescribe, cbClass, ID_RoomStage, Number_RoomStage);



            View.Windows.W_huantai w_ = new View.Windows.W_huantai(tuple);
            w_.btq += new View.Windows.BtQ(Bridge);
            w_.ShowDialog();
        }
        public void Bridge()
        {
            pus();
        }
    }
}
