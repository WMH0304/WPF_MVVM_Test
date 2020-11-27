using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WPF_MvvMTest
{
    /// <summary>
    /// 测试窗口.xaml 的交互逻辑
    /// </summary>
    public partial class 测试窗口 : Window
    {
        ObservableCollection<people> peoplelist = new ObservableCollection<people>();
        int count1 = 0;
        int count2 = 0;
        public 测试窗口()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            peoplelist.Add(new people
            {
                Name = "小明"
            });
            peoplelist.Add(new people
            {
                Name = "小红"
            });
            dataGrid1.ItemsSource = peoplelist;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            count1 += 1;
            people data;
            try
            {
                data = peoplelist.ElementAt(0);
            }
            catch (System.Exception ex)
            {
                return;
            }

            data.Name = data.Name + count1.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            count2 += 1;
            peoplelist.Add(new people
            {
                Name = "小红_" + count2.ToString()
            });
            dataGrid1.ItemsSource = peoplelist;
        }
    }
    public class people : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
    }
}
