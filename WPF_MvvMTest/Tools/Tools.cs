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
    }
}
