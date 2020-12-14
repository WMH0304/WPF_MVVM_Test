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
namespace WPF_MvvMTest.View.FoodAndBeverageManagement.Windows
{
    /// <summary>
    /// FABM_printer.xaml 的交互逻辑
    /// </summary>
    public partial class FABM_printer : Window
    {
        public FABM_printer(List<PrintfData> printfs,List<Consumer> consumers)
        {
            InitializeComponent();
        }
    }
}
