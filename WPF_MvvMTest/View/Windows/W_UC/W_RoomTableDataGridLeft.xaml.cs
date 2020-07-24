using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

namespace WPF_MvvMTest.View.Windows.W_UC
{
    public delegate void GetGridMessage(List<RoomStage> rooms);
   
    /// <summary>
    /// W_RoomTableDataGridLeft.xaml 的交互逻辑
    /// </summary>
    public partial class W_RoomTableDastaGridLeft : UserControl 
    {

        public event GetGridMessage ReuntLeft;
        List<RoomStage> r;
        public W_RoomTableDastaGridLeft(List<RoomStage> rooms)
        {
            //if (type !=null)
            //{

            //}else \
            if (r !=null)
            {
                //r.Clear();
                r.AddRange(rooms);
            }else if (r ==null)
            {
                 r = rooms;

            }
            InitializeComponent();
        }
        Model.EasternStar_WPF_MVVMEntities m = new Model.EasternStar_WPF_MVVMEntities();

        static List<RoomStage> stages;
        /// <summary>
        /// 控件加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            // stages.AddRange(r);
            int id_rs = r[0].ID_RoomStage;
            //左边的表格
            List <RoomStage> s = (from tb in m.SYS_RoomStage
                                  join tc in m.SYS_Class on tb.ID_Class equals tc.ID_Class
                                where tb.State_RoomStage == "未用" && tb.ID_RoomStage != id_rs
                                  select new RoomStage
                                {


                                      Code_Class = tc.Code_Class,
                                      MC_Class = tc.MC_Class,
                                      Jc_Class = tc.Jc_Class,
                                      Price_Plate = tc.Price_Plate,
                                      Price_Actual = tc.Price_Actual,
                                      Remark = tc.Remark,
                                      State_Class = tc.State_Class,


                                      ID_RoomStage = tb.ID_RoomStage,
                                     
                                      ID_Class = tb.ID_Class,
                                      Describe = tb.Describe,
                                      JionSign = tb.JionSign,
                                    Number_RoomStage = tb.Number_RoomStage,//房号
                                    MC_RoomStage = tb.MC_RoomStage,
                                    State_RoomStage = tb.State_RoomStage == "已用" ? "" : "否"
                                    
                                    //Duetono ="否"
                                }).ToList();
            #region MyRegion
            //RoomStage s = new List(rooms.ToList());
            //ObservableCollection<RoomStage> rooms = new ObservableCollection<RoomStage>();
            //rooms.Add(s);
            //if (r!=null)
            //{
            //    foreach (var item in s)
            //    {
            //        if (item.ID_RoomStage ==r[0].ID_RoomStage)
            //        {
            //            s.Remove(item);
            //            break;
            //        }
            //    }
            //}
            //dgdt.ItemsSource = s;
            //List<RoomStage> stages = new List<RoomStage>();
            //给动态数组赋值
            //if (s !=null)
            //{
            //    rooms.Clear();

            //    s.ForEach(p => rooms.Add(p));
            //}
            //dgdt.ItemsSource = null;
            //dgdt.ItemsSource = rooms;
            #endregion

            //求差集

            dgdt.ItemsSource = s.Except(r);

        }

        /// <summary>
        /// 控件点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dgdt_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            
        RoomStage room = dgdt.CurrentItem as RoomStage;

            List<RoomStage> roomStages = new List<RoomStage>();

            roomStages.Add(room);
            //ReuntLeft.
            //ReuntLeft.BeginInvoke(roomStages);
            //ReuntMessage reuntMessage;
            //W_RoomTableReservation w_RoomTableReservation = new W_RoomTableReservation(roomStages);
            //w_RoomTableDataGridLeft.ReuntLeft += new GetGridMessage(w_RoomTableReservation.ReceiveLeft);//定义委托
            View.Windows.W_UC.W_RoomTableDastaGridLeft w_RoomTableDataGridLeft = new W_UC.W_RoomTableDastaGridLeft(roomStages);
            if (ReuntLeft != null)
            {
                ReuntLeft(roomStages);
            }
            else
            {
                STATIC_cache.StaticRoomStages=roomStages;
            }
           


          
        }


    }
}
