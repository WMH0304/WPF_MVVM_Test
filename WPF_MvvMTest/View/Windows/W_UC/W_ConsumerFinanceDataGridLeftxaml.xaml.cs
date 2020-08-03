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

namespace WPF_MvvMTest.View.Windows.W_UC
{

    public delegate void TransmitConsumer(List<Consumer> Cm);

    /// <summary>
    /// W_ConsumerFinanceDataGridLeftxaml.xaml 的交互逻辑
    /// </summary>
    public partial class W_ConsumerFinanceDataGridLeftxaml : UserControl
    {
        public  event  TransmitConsumer TransmitConsumer;
        int cT;
        //筛选条件
        string Select_Message;
        public W_ConsumerFinanceDataGridLeftxaml(int  consumptionType,string Select_message)
        {
            Select_Message = Select_message;
            cT = consumptionType;
            InitializeComponent();
        }


        EasternStar_WPF_MVVMEntities m = new Model.EasternStar_WPF_MVVMEntities();


        /// <summary>
        /// 控件加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<Consumer> consumers = (from tP in m.PJ_Project
                                        join tPD in m.PJ_ProjectDetail on tP.ID_Project equals tPD.ID_Project
                           select new Consumer
                           {
        
                               ID_Project =tP.ID_Project,
                               ID_ProjectClass = tP.ID_ProjectClass,
                               MC_Project = tP.MC_Project,
                               Jc_Project =tP.Jc_Project,
                               Unit = tP.Unit,
                               Price = tPD.Price,

                           }).ToList();
            if (cT >0)
            {
                consumers = consumers.Where(p => p.ID_ProjectClass == cT).ToList();
            }
            if (Select_Message!= null )
            {
                consumers = consumers.Where(p =>
                p.MC_Project.Contains(Select_Message) ||
                p.ID_Project.ToString() ==Select_Message ||
                p.Jc_Project.Contains(Select_Message) 
              ).ToList();
            }
            dgdt.ItemsSource = consumers;
        }

        /// <summary>
        /// 表格点击
        /// </summary>
        /// <param name="sender"></param>
        private void Dgdt_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Consumer consumer = dgdt.CurrentItem as Consumer;

            List<Consumer> consumers = new List<Consumer>() ;

            consumers.Add(consumer);
          
        
            STATIC_cache.StaticConsumerLeft = consumers;
            
        }

       
    }
}
