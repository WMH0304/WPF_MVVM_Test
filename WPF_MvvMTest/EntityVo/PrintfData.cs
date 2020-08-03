using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MvvMTest.EntityVo
{
    /// <summary>
    /// 数据打印上半部分
    /// </summary>
  public  class PrintfData
    {
        /// <summary>
        /// 客人姓名
        /// </summary>
      public  string krxm { set; get; }
        /// <summary>
        /// 主房台号
        /// </summary>
        public string zfth { set; get; }

        /// <summary>
        /// 房台名称
        /// </summary>
        public string ftmc { set; get; }

        /// <summary>
        /// 房台单号
        /// </summary>
        public string ftdh { set; get; }

        /// <summary>
        /// 开台时间
        /// </summary>
        public string ktsj { set; get; }

        /// <summary>
        /// 买单时间
        /// </summary>
        public string mdsj { set; get; }

        /// <summary>
        /// 总金额
        /// </summary>
        public string TotalPrice { set; get; }

        /// <summary>
        /// 子数据
        /// </summary>
        public List<PrintfDataDetail> LPDD
        {
            get
            {
                return dataDetails;
            }
            set { LPDD = dataDetails; }
        }

        List<PrintfDataDetail> dataDetails = new List<PrintfDataDetail>();
    }

    /// <summary>
    /// 数据打印表格部分
    /// </summary>
    public class PrintfDataDetail
    {
        public string fth { set; get; }
        public string xmmc { set; get; }
        public string dw { set; get; }
        public string dj { set; get; }
        public string sl { set; get; }
        public string zk { set; get; }
        public string je { set; get; }
        public string zffs { set; get; }

    }
}
