using System;
using System.Collections.Generic;
using System.Drawing;
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
using WPF_MvvMTest.Resources;

namespace WPF_MvvMTest.View.HotelManagement
{
    /// <summary>
    /// W_hotel_management.xaml 的交互逻辑
    /// </summary>
    public partial class W_hotel_management : Window
    {
        public W_hotel_management()
        {
            InitializeComponent();
        }

        Model.EasternStar_WPF_MVVMEntities m = new EasternStar_WPF_MVVMEntities();

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<RoomStage> sr = (from tr in m.SYS_RoomStage
                                  join trst in m.SYS_Room_status_type on tr.ID_room_type equals trst.ID_room_status_type
                                  where tr.ID_Class == 3
                                  select new RoomStage
                                  {
                                      ID_RoomStage = tr.ID_RoomStage,
                                      ID_Guest = tr.ID_Guest,
                                      ID_room_status_type = trst.ID_room_status_type,
                                      room_status_type = trst.name_room_status_type,
                                      Number_RoomStage = tr.Number_RoomStage,
                                      State_RoomStage = tr.Number_RoomStage,
                                      Describe = tr.Describe,
                                      MC_RoomStage = tr.MC_RoomStage,
                                  }).ToList();

            //动态添加按钮
            Bt_fill(sr);

        }

        /// <summary>
        /// 房台按钮填充
        /// </summary>
        /// <param name="stages"></param>
        public void  Bt_fill(List<RoomStage> stages)
        {
            foreach (var item in stages)
            {
                UC_Btn_hotel_room_availability uBt = new UC_Btn_hotel_room_availability();
                
                uBt.Tb_room_id.Text = item.ID_RoomStage.ToString().Trim();
                uBt.TbRoom_class.Text = item.MC_RoomStage.Trim().ToString();
                uBt.TbRoom_num.Text = item.Number_RoomStage.Trim().ToString();
                switch (item.ID_room_status_type)
                {
                    case 1: 
                        //空净房
                        uBt.Background = System.Windows.Media.Brushes.Green;
                        break;
                    case 2:
                        //住房        
                        uBt.Background = System.Windows.Media.Brushes.SkyBlue;
                        break;
                    case 3:
                        //脏房
                        uBt.Background = System.Windows.Media.Brushes.Purple;
                        break;
                    case 4:
                        //维修房
                        uBt.Background = System.Windows.Media.Brushes.Blue;
                        break;
                    case 5:
                        //钟点房
                        uBt.Background = System.Windows.Media.Brushes.AliceBlue;
                        break;
                    case 6:
                        //自用房
                        uBt.Background = System.Windows.Media.Brushes.AntiqueWhite;
                        break;
                    case 7:
                        //团队
                        uBt.Background = System.Windows.Media.Brushes.Aqua;
                        break;
                    case 8:
                        //预定房
                        uBt.Background = System.Windows.Media.Brushes.Aquamarine;
                        break;
                    case 9:
                        //保密房
                        uBt.Background = System.Windows.Media.Brushes.Azure;
                        break;
                    case 10:
                        //午休房
                        uBt.Background = System.Windows.Media.Brushes.Beige;
                        break;
                    case 11:
                        //需打扫
                        uBt.Background = System.Windows.Media.Brushes.Bisque;
                        break;
                    case 12:
                        //预离房
                        uBt.Background = System.Windows.Media.Brushes.BlueViolet;
                        break;
                    default:
                        MessageBox.Show("客房类型不存在","大海提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                        break;
                }

                Wp_Room_table_diagram_display.Children.Add(uBt);
                //Wp_Room_table_diagram_display = VisualScrollableAreaClip.


            }
        }
    }
}
