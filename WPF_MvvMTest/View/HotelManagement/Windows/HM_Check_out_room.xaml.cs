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

namespace WPF_MvvMTest.View.HotelManagement.Windows
{
    /// <summary>
    /// HM_Check_out_room.xaml 的交互逻辑
    /// </summary>
    public partial class HM_Check_out_room : Window
    {
        public HM_Check_out_room(int room_id)
        {
            this.room_id = room_id;
            InitializeComponent();
        }
        int room_id;
        Model.EasternStar_WPF_MVVMEntities m = new EasternStar_WPF_MVVMEntities();
        List<Consumer> con;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 树形图
            Get_tree();

            //房台信息
            var room_message = (from tbroom in m.SYS_RoomStage
                                join tbuser in m.SYS_Guest on tbroom.ID_Guest equals tbuser.ID_Guest
                                join tbvip in m.VIP_Table on tbuser.ID_Guest equals tbvip.ID_Guest
                                join tbgrade in m.VIP_Grade on tbvip.ID_Grade equals tbgrade.ID_Grade
                                join tbos in m.YW_OpenStage on tbvip.ID_VIP equals tbos.ID_VIP
                                where tbroom.ID_RoomStage == room_id
                                select new
                                {
                                    tbvip.Accounts
                                    ,tbroom.Number_RoomStage
                                    ,tbroom.MC_RoomStage
                                    ,tbos.Number_OpenStage
                                    ,tbos.Time_Predict
                                    ,tbroom.State_RoomStage
                                    ,tbos.Remark
                                    ,tbgrade.Discount
                                }).SingleOrDefault();
            if (room_message ==null)
            {

            }

            Tb_The_guest_account.Content = room_message.Accounts.Trim();
            Tb_The_main_room_number.Text = room_message.Number_RoomStage.Trim();
            Tb_Room_table_name.Text = room_message.MC_RoomStage.Trim();
            Tb_Room_with_a_single_number.Text = room_message.Number_OpenStage.Trim();
            DateTime leve = room_message.Time_Predict;
            DateTime now = DateTime.Now;
            Tb_Founding_time.Text = leve.ToShortDateString();
            Tb_The_check_time.Text = now.ToShortDateString();
            //DateTime.Compare
            //DateTime.Now.ToOADate() room_message.Time_Predict.ToOADate
            int datas =(int)(now - leve).TotalDays;
            Tb_duration.Text = datas.ToString().Trim();
            Tb_condition.Text = room_message.State_RoomStage.Trim();
            Tb_postscript.Text = room_message.Remark.Trim();
            Tb_discount.Text = room_message.Discount.ToString().Trim();

           
            //支付记录 
            con = (from tC in m.CW_Consumption //消费表
                   join tR in m.SYS_RoomStage on tC.ID_RoomStage equals tR.ID_RoomStage
                   join tCD in m.CW_ConsumeDetail on tC.ID_Consumption equals tCD.ID_Consumption  //消费明细
                   join tP in m.PJ_Project on tCD.ID_Project equals tP.ID_Project  //项目表
                   join tPD in m.PJ_ProjectDetail on tP.ID_Project equals tPD.ID_Project //项目明细
                                                                                         //join tPR in m.CW_PayRecord on tCD.ID_PayRecord equals tPR.ID_PayRecord //支付记录
                   where tC.ID_RoomStage == room_id
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





            




        }
        /// <summary>
        /// 树形图临时集合
        /// </summary>
        class Temporary_collection
        {
            public int id { get; set; }

            public string name { get; set; }
            

        }

        /// <summary>
        /// 树形图数据获取
        /// </summary>
        public void Get_tree()
        {

            int? ID_Guest = m.SYS_RoomStage.Where(c => c.ID_RoomStage == room_id).Single().ID_Guest;
            //左边树形
            List<Temporary_collection> tc = (from t in m.SYS_RoomStage
                                             where t.ID_RoomStage == room_id && t.ID_Guest == ID_Guest && t.State_RoomStage.Trim() == "已用"
                                             select new Temporary_collection
                                             {
                                                 id = t.ID_RoomStage,
                                                 name = t.MC_RoomStage.Trim() + " ___ " + t.Number_RoomStage,
                                             }).AsParallel().ToList();

            treeView.ItemsSource = tc;
            
            //treeView.Items = t;

        }


        /// <summary>
        /// 文本框输入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 树形图点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        
    }
}
