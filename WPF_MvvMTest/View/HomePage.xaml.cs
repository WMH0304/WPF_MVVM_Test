﻿using System;
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
using System.Windows.Threading;

namespace WPF_MvvMTest.View
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Window
    {
        private DispatcherTimer ShowTimer;
        public HomePage(string name)
        {
          
            InitializeComponent();
            user_name = name;
            ShowTimer = new System.Windows.Threading.DispatcherTimer();
            ShowTimer.Tick += new EventHandler(ShowCurTime);
            //设置时间间隔
            ShowTimer.Interval = new TimeSpan(0, 0,0,1,0);
            ShowTimer.Start();
        }
       

        private void ShowCurTime(object s,EventArgs e)
        {

           
            //获得时分秒 
            //this.TbNowDateTime.Text += DateTime.Now.ToString("HH:mm:ss:ms");

            //获得星期几
            this.TbNowDateTime.Text = DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-cn"));
            this.TbNowDateTime.Text += " ";
            //获得年月日
            this.TbNowDateTime.Text += DateTime.Now.ToString("yyyy年MM月dd日");   //yyyy年MM月dd日
            this.TbNowDateTime.Text += " ";
            //获得时分秒
            this.TbNowDateTime.Text += DateTime.Now.ToString("HH:mm:ss");
            this.TbNowDateTime.Text += " ";
        }

        string user_name;
        //计时器
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            TbLogInPerson.Text ="登陆人: "+ user_name;
           

            double h = SystemParameters.WorkArea.Height;
            double w = SystemParameters.WorkArea.Width;
            this.Width = w;
            this.Height =h;



        }

        /// <summary>
        /// 常用功能按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtCommonlyUsedGunctions_Click(object sender, RoutedEventArgs e)
        {

            double w = SpCommonlyUsedFunctions.Width;
            if (BtCommonlyUsedGunctions.Visibility ==Visibility.Visible)
            {
                BtCommonlyUsedGunctions.Visibility = Visibility.Collapsed;
                BtHidden.Width = SpCommonlyUsedFunctions.Width;
                
            }
            else
            {
                SpCommonlyUsedFunctions.Width = w;
                BtCommonlyUsedGunctions.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// 桑拿沐足
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtSaunaLiquid_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.ShowDialog();
            this.Close();
        }
    }
}
