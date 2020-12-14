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
        /// 时长
        /// </summary>
        public string sc { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public string TotalPrice { set; get; }
        /// <summary>
        /// 折扣
        /// </summary>
        public string zhekou{ get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string beizhu { get; set; }
        /// <summary>
        /// 累计金额
        /// </summary>
        public string leijijine { get; set; }
        /// <summary>
        /// 已收定金
        /// </summary>
        public string yishoudingj { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public string yve { get; set; }

        /// <summary>
        /// 宾客支付
        /// </summary>
        public string bkzf { get; set; }

        /// <summary>
        /// 待付金额
        /// </summary>
        public string dfje { get; set; }

        /// <summary>
        /// 结账方式
        /// </summary>
        public string jzfs { get; set; }

        /// <summary>
        /// 结账备注
        /// </summary>
        public string jzbz { get; set; }
        /// <summary>
        /// 
        /// </summary>
      

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
        /// <summary>
        /// 房台号
        /// </summary>
        public string fth { set; get; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string xmmc { set; get; }
        /// <summary>
        /// 单位
        /// </summary>
        public string dw { set; get; }
        /// <summary>
        /// 单价
        /// </summary>
        public string dj { set; get; }
        /// <summary>
        /// 数量
        /// </summary>
        public string sl { set; get; }
        /// <summary>
        /// 折扣
        /// </summary>
        public string zk { set; get; }
        /// <summary>
        /// 金额
        /// </summary>
        public string je { set; get; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string zffs { set; get; }

    }
}
