using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MvvMTest.EntityVo
{
  public  class Consumer :Model.PJ_Project
    {

        /// <summary>
        /// 房台id
        /// </summary>
        public int ID_Fangtai { get; set; }

        /// <summary>
        /// 房台号
        /// </summary>
        public string Number_RoomStage { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Mun { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 金额/单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 消费明细id 
        /// </summary>
        public int ID_ConsumeDetail { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Total_money { set; get; }

        /// <summary>
        /// 支付方式
        /// </summary>
         public string MethodOfPayment { set; get; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
        
    }

    /// <summary>
    /// 支付记录
    /// </summary>
    public class PaymentRecords :Model.CW_PayRecord 
    {
        /// <summary>
        /// 客人名称
        /// </summary>
        public string GuestName { set; get; }
         /// <summary>
         /// 帐单号
         /// </summary>
        public string Number_Bill { set; get; }


        /// <summary>
        /// 房台id
        /// </summary>
        public int ID_Fangtai { get; set; }

        /// <summary>
        /// 房台号
        /// </summary>
        public string Number_RoomStage { get; set; }
    }
}
