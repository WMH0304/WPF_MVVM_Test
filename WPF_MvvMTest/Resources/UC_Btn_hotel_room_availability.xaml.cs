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

namespace WPF_MvvMTest.Resources
{

   // public delegate void GetName(string _tag);

    /// <summary>
    /// UC_Btn_hotel_room_availability.xaml 的交互逻辑
    /// </summary>
    public partial class UC_Btn_hotel_room_availability : Button
    {
       // public event GetName Get_name;
        public UC_Btn_hotel_room_availability()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 临时集合
        /// </summary>
        class _room
        {
            public string _num { get; set; }

            public string __class { get; set; }
        }

        Model.EasternStar_WPF_MVVMEntities m = new Model.EasternStar_WPF_MVVMEntities();

        /// <summary>
        /// 鼠标移入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            string s = btn.Name;
            int rid = int.Parse(btn.Tag.ToString());

            bool exist = m.SYS_RoomStage.Where(c => c.ID_Class == 3 && c.ID_RoomStage == rid && c.State_RoomStage.Trim() == "已用").Count() > 0 ? true : false;
            //房台已用时
            if (exist ==true)
            {
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
                                      where tbR.ID_RoomStage == rid && tbR.State_RoomStage.Trim() == "已用"
                                      && tbR.ID_Class == 3
                                      select new BtMessage
                                      {
                                          MC_Guest = tbSYSG.MC_Guest,//名称
                                          Number_Bill = tbCWB.Number_Bill,//单号
                                          Time_Predict = tbYWOS.Time_Predict,//开台时间
                                          Time_Leave = tbYWOS.Time_Leave,//离开时间
                                          Price_Pay = tbCWP.Price_Pay,//支付金额
                                          Prict = tbCWC.Prict,//消费金额
                                      }).ToList();

                foreach (var item in MB)
                {
                    btn.ToolTip =
                                    "房台已启用 " + "\n" +
                                    "客人名称： " + item.MC_Guest.Trim() + "\n" +
                                    "账单单号： " + item.Number_Bill.Trim() + "\n" +
                                    "开台时间： " + item.Time_Predict + "\n" +
                                    "离开时间： " + item.Time_Leave+ "\n" +
                                    "支付金额： " + item.Price_Pay + "\n" +
                                    "消费金额： " + item.Prict + "\n";
                }
            }

            //房台未用时
            if (exist ==false)
            {
                List<SYS_RoomStage> sr = m.SYS_RoomStage.Where(c => c.ID_Class == 3 && c.ID_RoomStage == rid && c.State_RoomStage.Trim() == "未用").ToList();

                foreach (var item in sr)
                {
                    btn.ToolTip =
                        "房台未启用  " + "\n" +
                        "房台号:    " + item.Number_RoomStage.Trim() + "\n" +
                        "房台名称:  " + item.MC_RoomStage.Trim() + "\n" +
                        "房台状态:  " + item.State_RoomStage.Trim() + "\n" +
                        //"   描述:  " +item.Describe.Trim() +"\n"+
                        "   楼层:  " + item.floor + "楼" + "\n";                        
                }
            }

        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string str =  btn.Tag.ToString().Trim();
            //Get_name(str);
            STATIC_cache.ID_RoomStage = 0;
            STATIC_cache.ID_RoomStage = int.Parse(str);
        }
    }
}
