using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WPF_MvvMTest.Tools
{
    /// <summary>
    /// 我的工具类
    /// </summary>
   static class Tools
    {
       


        /// <summary>
        /// 1.限制输入数字
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsNum(string str)
        {
            string pattern = @"^[0-9]*[1-9][0-9]*$";
            return Regex.IsMatch(str, pattern);
            //System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]*$");
            //return reg1.IsMatch(str);
        }


        /// <summary>
        /// 2.限制折扣正则
        /// </summary> 只能输入0-1之间的两个小数,比如 0.0.~ 0.99
        /// /// <param name="str"></param>
        /// <returns></returns>/^(0|[1-9]\d*)(\s|$|.\d{1,2}\b)/ 
        public static bool IsDiscount(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^((0\.[1-9]{1})|(([1-9]{1})(\.\d{1})?))$");
            return reg1.IsMatch(str);
          
        }

        /// <summary>
        /// 3.限制输入正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInteger(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^([0-9]{1,}[.][0-9]*)$");
            return reg1.IsMatch(str);
        }

        /// <summary>
        /// 4.判断是否是字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
         public static bool Isletter(string str)
        {
            Regex regex = new Regex(@"^[A-Za-z]+$");

            return regex.IsMatch(str);
        }

        
    }
}
