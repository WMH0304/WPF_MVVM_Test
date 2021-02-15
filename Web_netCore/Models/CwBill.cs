using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class CwBill
    {
        public int IdBill { get; set; }
        public int SuOpId { get; set; }
        public DateTime? TimePayBill { get; set; }
        public string Remark { get; set; }
        public decimal? Price { get; set; }
        public string StateBill { get; set; }
        public string NumberBill { get; set; }
    }
}
