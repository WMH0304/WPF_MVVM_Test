using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class AgAgreementUser
    {
        public int IdAgreementUser { get; set; }
        public int IdAgreementClass { get; set; }
        public string McAgreementUser { get; set; }
        public string LinkmanAgreementUser { get; set; }
        public string PhoneAgreementUser { get; set; }
        public decimal PriceBalance { get; set; }
        public decimal PrcieOverdraft { get; set; }
        public DateTime? DateCredit { get; set; }
    }
}
