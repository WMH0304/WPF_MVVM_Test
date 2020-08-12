using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public delegate void ReuntMessage(List<RoomStage> rooms);
    /// <summary>
    /// W_RoomTableDataGridRight.xaml 的交互逻辑
    /// </summary>
    public partial class W_RoomTableDataGridRight : UserControl
    {
        public event ReuntMessage RetM;
        int YYid;
        int rid;
        int kt_id;
        //string zhanghao = "111111";//客户帐号 
        List<RoomStage> stages = new List<RoomStage>();
        public W_RoomTableDataGridRight(int yydid,int rooid,int ktid)
        {
            if (yydid>0)
            {
                YYid = yydid;
            }
            if (rooid >0)
            {
                rid = rooid;
            }
            if (ktid >0)
            {
                kt_id = ktid;
            }

            InitializeComponent();
        }
        Model.EasternStar_WPF_MVVMEntities m = new Model.EasternStar_WPF_MVVMEntities();
       
        /// <summary>
        /// 控件加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //存在预订单的情况下是这个
            if (YYid >0)
            {
                string HouseStageID = m.YW_Subscribe.Where(p => p.ID_Subscribe == YYid).SingleOrDefault().HouseStageID;
                //加入有多个id 遍历数组获取
                string[] vs = HouseStageID.Split(',');
                stages.Clear();
                foreach (var item in vs)
                {
                    if (item.Trim() == "")
                    {
                        break;
                    }
                    int rsid = Convert.ToInt32(item);
                    stages.AddRange(from tb in m.SYS_RoomStage
                                    where tb.ID_RoomStage == rsid
                                    select new RoomStage
                                    {
                                        ID_RoomStage = tb.ID_RoomStage,
                                        Number_RoomStage = tb.Number_RoomStage,//房号
                                        MC_RoomStage = tb.MC_RoomStage,
                                    });
                }
            }
            else if(rid >0 )
            {
                //获取开台表中的客房id  rid 传过来的id
                string zhanghao = EntityVo.STATIC_cache.Zhanghao;



                string HouseStageID = m.YW_OpenStage.Where(p => p.ID_VIP ==
                (m.VIP_Table.Where(v => v.Accounts == zhanghao).FirstOrDefault().ID_VIP)).SingleOrDefault().HouseStageID;

                //加入有多个id 遍历数组获取
                string[] vs = HouseStageID.Split(',');
                stages.Clear();
                foreach (var item in vs)
                {
                    if (item.Trim() == "")
                    {
                        break;
                    }
                    int rsid = Convert.ToInt32(item);
                    stages.AddRange(from tb in m.SYS_RoomStage
                                    where tb.ID_RoomStage == rsid
                                    select new RoomStage
                                    {
                                        ID_RoomStage = tb.ID_RoomStage,
                                        Number_RoomStage = tb.Number_RoomStage,//房号
                                        MC_RoomStage = tb.MC_RoomStage,
                                    });
                }

            }

            else if(kt_id >0)
            {
                string HouseStageID = m.YW_OpenStage.Where(p => p.ID_OpenStage == kt_id).SingleOrDefault().HouseStageID;
                //加入有多个id 遍历数组获取
                string[] vs = HouseStageID.Split(',');
                stages.Clear();
                foreach (var item in vs)
                {
                    if (item.Trim() == "")
                    {
                        break;
                    }
                    int rsid = Convert.ToInt32(item);
                    stages.AddRange(from tb in m.SYS_RoomStage
                                    where tb.ID_RoomStage == rsid
                                    select new RoomStage
                                    {
                                        ID_RoomStage = tb.ID_RoomStage,
                                        Number_RoomStage = tb.Number_RoomStage,//房号
                                        MC_RoomStage = tb.MC_RoomStage,
                                    });
                }
            }

            else
            {
                stages = null;
                dgd.ItemsSource = stages;
                return;

            }
            stages.Distinct();
            //stages = (RoomStage)dgd.ItemsSource;
            //stages.AddRange();
            dgd.ItemsSource = stages;

        }
        /// <summary>
        /// 表格点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dgd_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            RoomStage roomStage = dgd.CurrentItem as RoomStage;
            List<RoomStage> rooms = new List<RoomStage>();
            rooms.Add(roomStage);
            //调用委托
            if (RetM !=null)
            {
                RetM(rooms);
            }
            else
            {
                STATIC_cache.StaticRoomStages = rooms;
            }
           
        }
    }
}
