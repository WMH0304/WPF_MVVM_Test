using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class CwFinance
    {
        public int IdFinance { get; set; }
        public int IdUser { get; set; }
        public int UseIdUser { get; set; }
        public decimal Alipay { get; set; }
        public decimal WeChat { get; set; }
        public decimal Rmb { get; set; }
        public decimal Curtain { get; set; }
        public decimal CreditCard { get; set; }
        public decimal Checks { get; set; }
        public decimal Surplus { get; set; }
        public decimal Income { get; set; }
        public decimal CashPledge { get; set; }
        public decimal Handin { get; set; }
        public decimal Issue { get; set; }
        public decimal Connect { get; set; }
        public string Remark { get; set; }
    }
}
