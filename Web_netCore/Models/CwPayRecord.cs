using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class CwPayRecord
    {
        public int IdPayRecord { get; set; }
        public int? IdBill { get; set; }
        public int? IdGuest { get; set; }
        public decimal? PricePay { get; set; }
        public DateTime? TimePay { get; set; }
        public string PoP { get; set; }
        public bool? State { get; set; }
    }
}
