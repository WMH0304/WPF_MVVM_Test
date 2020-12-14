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

namespace WPF_MvvMTest.View
{
    /// <summary>
    /// W_Print_Data.xaml 的交互逻辑
    /// </summary>
    public partial class W_Print_Data : Window
    {                     //       客名    房号    房名    房单    开时    买时    时长    
        public W_Print_Data(Tuple<string, string, string, string, string, string, string, Tuple<string>> tuple,List<Consumer> consumers)
        {
            tuples = tuple;
            cos = consumers;
            InitializeComponent();
        }

        Tuple<string, string, string, string, string, string, string,Tuple<string>> tuples;
        List<Consumer> cos;
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // DgDateAll
            Tbname.Text = tuples.Item1;//客名
            TbThemainRoomNumber.Text = tuples.Item2;// 房号
            TbRoomTableName.Text = tuples.Item3;// 房名
            TbRoomWithSingleNumb.Text = tuples.Item4;// 房单
            TbStarTime.Text = tuples.Item5;//  开时 
            TbOutTime.Text = tuples.Item6;//   买时
            TbTimeLong.Text = tuples.Item7;//     时长  
            TbTheCumulative.Text = tuples.Rest.Item1;//总金额
            if (cos != null && cos.Count() != 0)
            {
                DgDateAll.ItemsSource = cos;
            }
            else
            {
                MessageBox.Show("接收数据异常","大海提示",MessageBoxButton.OK,MessageBoxImage.Asterisk);
            }
           // TbTheCumulative.Text =

           // cos.Where(p=>p.Price)+

        }

     
       
        //取消
        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtPrint_Click(object sender, RoutedEventArgs e)
        {
            // 客名    房号    房名    房单    开时    买时    时长    
            PrintfData pd = new PrintfData();
            pd.krxm = tuples.Item1;
            pd.zfth = tuples.Item2;
            pd.ftmc = tuples.Item3;
            pd.ftdh = tuples.Item4;
            pd.ktsj = tuples.Item5;
            pd.mdsj = tuples.Item6;
            pd.TotalPrice = tuples.Rest.Item1;
            foreach (var item in cos)
            {

                pd.LPDD.Add(new PrintfDataDetail
                {

                    fth = item.Number_RoomStage,
                    xmmc = item.MC_Project,
                    dw = item.Unit,
                    dj = item.Price.ToString(),
                    sl = item.Mun,
                    zk = item.Discount.ToString(),
                    je = item.Price.ToString(),
                    zffs = item.MethodOfPayment ==null?"*": item.MethodOfPayment,

                });
            }

            WPF_MvvMTest.View.Windows.W_Print_DataDetail w = 
                new Windows.W_Print_DataDetail("FlowDocument.xaml", pd, new OrderDocumentRenderer());
            w.Owner = this;
            w.Width = 1000;
            w.Height = 800;
            w.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            w.ShowInTaskbar = false;
            w.ShowDialog();
        }
    }
}
