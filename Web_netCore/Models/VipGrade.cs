using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class VipGrade
    {
        public int IdGrade { get; set; }
        public string McGrade { get; set; }
        public decimal Discount { get; set; }
    }
}
