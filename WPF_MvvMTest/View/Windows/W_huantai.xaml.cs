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
using WPF_MvvMTest.Tools;

namespace WPF_MvvMTest.View.Windows
{
    /// <summary>
    /// W_huantai.xaml 的交互逻辑
    /// </summary>
    /// 
    //定义委托
    public delegate void BtQ();
    public partial class W_huantai : Window
    {
        Tuple<string, string, string, string, int, string> tuples;
        public event BtQ btq;
        public W_huantai(Tuple<string, string, string, string, int,string> tuple)
        {
            if (tuple !=null)
            {
                tuples = tuple;
            }
            else
            {
                System.Diagnostics.Debug.Write("换台值为空");
                throw new Exception();
                
            }
           
            InitializeComponent();
        }
        Model.EasternStar_WPF_MVVMEntities m = new EasternStar_WPF_MVVMEntities();

        /// <summary>
        /// 确定换房台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtQuit_Click(object sender, RoutedEventArgs e)
        {
            //   tuple = new Tuple<string, string, string, string, int>(tbRoom_name, cbStatus, tbDescribe, cbClass, ID_RoomStage,Num);
            string tbRoom_name = tuples.Item6;//房号
            int id = tuples.Item5;//id
           
            string nowNum = TbRoomNum.Text.Trim().ToString();
            if (!Tools.Tools.IsNum(nowNum))
            {
                MessageBox.Show("请输入数字","大海提示",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
            //赛选文本所对应的房台id
            List<RoomStage> RS = //m.SYS_RoomStage.Where(f => f.Number_RoomStage == nowNum).Single();
                (from tb in m.SYS_RoomStage
                 where tb.Number_RoomStage ==nowNum
                 select new RoomStage
                 {
                     ID_RoomStage = tb.ID_RoomStage,
                 }).ToList();
            int IDRoomStage =0;
            foreach (var item in RS)
            {
                IDRoomStage = item.ID_RoomStage;
            }

            //改变房台状态2
            int SYSRS2;
            Model.SYS_RoomStage rs = m.SYS_RoomStage.Where(l => l.ID_RoomStage == IDRoomStage).Single();
            rs.State_RoomStage = "已用";
            m.Entry(rs).State = System.Data.Entity.EntityState.Modified;
            SYSRS2 = m.SaveChanges();

            //改变消费表对应房台id
            int CWC;
            Model.CW_Consumption c = m.CW_Consumption.Where(m => m.ID_RoomStage == id).SingleOrDefault();
            c.ID_RoomStage = Convert.ToInt32(nowNum);
            m.Entry(c).State = System.Data.Entity.EntityState.Modified;
            CWC = m.SaveChanges();
            //改变房台状态1
            int SYSRS1;
            Model.SYS_RoomStage r = m.SYS_RoomStage.Where(l => l.ID_RoomStage == id).Single();
            r.State_RoomStage = "未用";
            SYSRS1 = m.SaveChanges();
            m.Entry(r).State = System.Data.Entity.EntityState.Modified;
           

            if (CWC > 0 && SYSRS1 >0 && SYSRS2 >0)
            {
               MessageBoxResult message =  MessageBox.Show("房台转换成功", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (message == MessageBoxResult.OK)
                {
                    btq();
                    this.Close();
                }

            }
            else
            {
                MessageBox.Show("房台转换失败", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }


        }
    }
}
