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

namespace WPF_MvvMTest
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
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
            TbAccount.Text = "111111";

            PbPassword.Password = "111111";
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtLogin_Click(object sender, RoutedEventArgs e)
        {
              //账号
             string Account =  TbAccount.Text.ToString().Trim();
              //密码PbPassword
             string Password = PbPassword.Password.Trim().ToString();

            int int_Account = m.User_Table.Where(p => p.Number_Job == Account).Count();
            if (int_Account >0)
            {
                int int_User = m.User_Table.Where(p => p.Password.Trim() == Password).Count();
                if (int_User>0 )
                {
                    string name = m.User_Table.Where(o => o.Number_Job == Account).SingleOrDefault().MC_User;
                    //跳转到主页
                    WPF_MvvMTest.View.HomePage homePage= new View.HomePage(name);
                    this.Close();
                    homePage.ShowDialog();
                   
                }
                else
                {
                    MessageBox.Show("密码错误", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;

                }
            }
            else
            {
                MessageBox.Show("账号不存在", "大海提示",MessageBoxButton.OK,MessageBoxImage.Asterisk);
                return;
            }



        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            //账号
            string Account = TbAccount.Text.ToString().Trim();
            //密码PbPassword
            string Password = PbPassword.Password.Trim().ToString();
            if (Account!=null  || Password !=null)
            {
                TbAccount.Text = null;
                PbPassword.Password = null;
            }
            else
            {
                //his.Close();
                Application.Current.Shutdown();
            }
        }
        /// <summary>
        /// 鼠标移入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtEyes_MouseEnter(object sender, MouseEventArgs e)
        {
            TbPassword.Visibility = Visibility.Visible;
            PbPassword.Visibility = Visibility.Hidden;
            TbPassword.Text = PbPassword.Password.ToString();
        }

        /// <summary>
        /// 鼠标移入出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtEyes_MouseLeave(object sender, MouseEventArgs e)
        {
            TbPassword.Visibility = Visibility.Hidden;
            PbPassword.Visibility = Visibility.Visible;
            PbPassword.Password= TbPassword.Text;
        }
      
    }
}
