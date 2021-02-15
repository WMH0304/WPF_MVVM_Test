using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class AgAgreementStagePrice
    {
        public int IdAgreementStagePrice { get; set; }
        public int IdAgreementClass { get; set; }
        public int IdClass { get; set; }
        public decimal PriceAgreement { get; set; }
        public string Remark { get; set; }
    }
}
