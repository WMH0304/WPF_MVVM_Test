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
using WPF_MvvMTest.Model;
namespace WPF_MvvMTest.View.HotelManagement.Windows
{

    public delegate void Bt();

    /// <summary>
    /// HM_Add_room.xaml 的交互逻辑
    /// </summary>
    public partial class HM_Add_room : Window
    {

        public event Bt btq;
        public HM_Add_room()
        {
            InitializeComponent();
        }
        Model.EasternStar_WPF_MVVMEntities m = new EasternStar_WPF_MVVMEntities();


        /// <summary>
        /// 页面刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<SYS_Room_status_type> SR = m.SYS_Room_status_type.ToList();

            Cb_real_estate.ItemsSource = SR;
            Cb_real_estate.DisplayMemberPath = "name_room_status_type";
            Cb_real_estate.SelectedValuePath = "ID_room_status_type";
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_confirm_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            string bt_name = bt.Name.Trim();
            if (bt_name == "Bt_confirm") 
            {
                confirm();
            }

            if (bt_name == "Bt_cancel")
            {
                if (Tb_Room_table_number.Text.Trim() != string.Empty || Tb_Room_table_name.Text.Trim() != string.Empty || Tb_floor.Text.Trim() != string.Empty || Cb_real_estate.Text.Trim() != string.Empty)
                {
                    MessageBoxResult mbr = MessageBox.Show("取消页面内容将会被清空,点击确定后关闭窗口", "大海提示",MessageBoxButton.OK,MessageBoxImage.Warning);

                    if (mbr ==MessageBoxResult.OK)
                    {
                        this.Close();
                        btq();
                    }
                }
            }
            

        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        private void confirm()
        {

            if (Tb_Room_table_number.Text.Trim() ==string.Empty || Tb_Room_table_name.Text.Trim() ==string.Empty || Tb_floor.Text.Trim() ==string.Empty || Cb_real_estate.Text.Trim() ==string.Empty)
            {
                Tools.Common_means.is_null();
                return;
            }

            SYS_RoomStage rs = new SYS_RoomStage();
            rs.MC_RoomStage = Tb_Room_table_name.Text.Trim();
            rs.Number_RoomStage = Tb_Room_table_number.Text.Trim();
            try
            {
                rs.floor = int.Parse(Tb_floor.Text.Trim());
            }
            catch (Exception)
            {

                MessageBox.Show("楼层内容只能是数字","大海提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
            string id = Cb_real_estate.Text.Trim();
            rs.ID_room_type = m.SYS_Room_status_type.Where(c=>c.name_room_status_type ==id).Single().ID_room_status_type;
            rs.State_RoomStage = "未用";
            rs.ID_Class = 3;
            m.SYS_RoomStage.Add(rs);
            if (m.SaveChanges() >0)
            {
                Tools.Common_means.operate_successfully("房台添加");
                btq();
            }




        }
        
    }
}
