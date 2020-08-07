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
using WPF_MvvMTest.EntityVo;
using WPF_MvvMTest.Resources;

namespace WPF_MvvMTest.View.FoodAndBeverageManagement
{
    /// <summary>
    /// W_FoodAndBeverageManagement.xaml 的交互逻辑
    /// </summary>
    public partial class W_FoodAndBeverageManagement : Window
    {
        public W_FoodAndBeverageManagement()
        {
            InitializeComponent();
        }
        Model.EasternStar_WPF_MVVMEntities m = new EasternStar_WPF_MVVMEntities();


        List<RoomStage> LRS;
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LRS.Clear();
            LRS = (from tr in m.SYS_RoomStage
                   where tr.ID_Class == 2
                   select new RoomStage
                   {

                       ID_RoomStage = tr.ID_RoomStage,
                       ID_Class = tr.ID_Class,
                       Number_RoomStage = tr.Number_RoomStage,
                       MC_RoomStage = tr.MC_RoomStage,
                       State_RoomStage = tr.State_RoomStage,
                       Describe = tr.Describe,
                       JionSign = tr.JionSign,
                   }).ToList() ;
            int int_room = LRS.Count();
            //控件加载
            for (int i = 0; i < int_room; i++)
            {
                UC_BtDynamic myBtDynamic = new UC_BtDynamic(LRS);
                myBtDynamic.Name = "btn" + i;
                myBtDynamic.TbRoom_num.Text = LRS[i].Number_RoomStage.ToString();//房台号
                myBtDynamic.TbRoom_class.Text = LRS[i].MC_RoomStage.ToString();//房台名称

                //换台刷新
                myBtDynamic.pus += new Pus(Push);
                //获取控件内容id
               // myBtDynamic.GetButtons += new GetButton(SetBtuuons);
                if (LRS[i].State_RoomStage.Trim() == "预定")
                {
                    myBtDynamic.Background = System.Windows.Media.Brushes.Blue;
                }
                else if (LRS[i].State_RoomStage.Trim() == "已用")
                {
                    myBtDynamic.Background = System.Windows.Media.Brushes.DodgerBlue;
                }
                else if (LRS[i].State_RoomStage.Trim() == "停用")
                {
                    myBtDynamic.Background = System.Windows.Media.Brushes.LightSlateGray;
                }
                //  myBtDynamic.Click += new RoutedEventHandler(btnEvent_Click);

                WpButton.Children.Add(myBtDynamic);
            }
        }

        //调用刷新页面方法
        void Push()
        {

            Window_Loaded(null, null);
        }
       
    }
}
