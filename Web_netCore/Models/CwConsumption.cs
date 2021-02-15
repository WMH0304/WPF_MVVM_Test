using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class CwConsumption
    {
        public int IdConsumption { get; set; }
        public int IdProjectDetail { get; set; }
        public int IdRoomStage { get; set; }
        public int IdBill { get; set; }
        public int Quantity { get; set; }
        public decimal Prict { get; set; }
        public decimal Discount { get; set; }
        public DateTime TimeConsumption { get; set; }
        public bool? Effective { get; set; }
    }
}
