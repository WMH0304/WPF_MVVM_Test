using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class VipTable
    {
        public int IdVip { get; set; }
        public int IdGuest { get; set; }
        public int IdGrade { get; set; }
        public decimal IntegraVipl { get; set; }
        public decimal PriceBalance { get; set; }
        public decimal PrcieOverdraft { get; set; }
        public DateTime DateCredit { get; set; }
        public string Accounts { get; set; }
    }
}
