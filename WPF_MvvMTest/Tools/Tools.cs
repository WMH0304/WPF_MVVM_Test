using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MvvMTest.Tools
{
   static class Tools
    {

        /// <summary>
        /// 限制输入数字
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsNum(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]*$");
            return reg1.IsMatch(str);
        }
        /// <summary>
        /// 限制折扣正则
        /// </summary> 只能输入0-1之间的两个小数,比如 0.0.~ 0.99
        /// /// <param name="str"></param>
        /// <returns></returns>/^(0|[1-9]\d*)(\s|$|.\d{1,2}\b)/ 
        public static bool IsDiscount(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^(?:0(?:\.(?!00)[0-9]{2})?|1.00)$");
            return reg1.IsMatch(str);
          
        }
    }
}
