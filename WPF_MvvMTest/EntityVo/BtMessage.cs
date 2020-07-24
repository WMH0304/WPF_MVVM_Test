using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_MvvMTest.Model;

namespace WPF_MvvMTest.EntityVo
{
  public  class BtMessage
    {
        //名称
        public string MC_Guest { get; set; }
        //单号
        public string Number_Bill { get; set; }
        //开台时间
        public System.DateTime Time_Predict { get; set; }
        //离开时间
        public System.DateTime Time_Leave { get; set; }
        //支付金额
        public Nullable<decimal> Price_Pay { get; set; }
        //消费金额
        public decimal Prict { get; set; }
    }
}
