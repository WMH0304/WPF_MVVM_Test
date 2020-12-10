using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_MvvMTest.Tools
{
    /// <summary>
    /// 公共方法
    /// </summary>
    class Common_means
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        static public void operate_successfully(string str)
        {
            MessageBox.Show(str + "成功", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="str"></param>
        static public void operation_failure(string str)
        {
            MessageBox.Show(str + "失败", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
        }



        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="err"></param>
        static public void error_log(Exception e)
        {
            System.Diagnostics.Debug.WriteLine("错误日志信息" + e.Message + "\n" + "对象名称" + e.Source + "\n" + "堆栈信息" + e.StackTrace);

            MessageBox.Show("错误日志信息" + e.Message + "\n" + "对象名称" + e.Source + "\n" + "堆栈信息" + e.StackTrace,"大海提示",MessageBoxButton.OK,MessageBoxImage.Error);
        }


    }
}
