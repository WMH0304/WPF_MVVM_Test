using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class SysClass
    {
        public int IdClass { get; set; }
        public string CodeClass { get; set; }
        public string McClass { get; set; }
        public string JcClass { get; set; }
        public decimal PricePlate { get; set; }
        public decimal PriceActual { get; set; }
        public string Remark { get; set; }
        public string StateClass { get; set; }
    }
}
