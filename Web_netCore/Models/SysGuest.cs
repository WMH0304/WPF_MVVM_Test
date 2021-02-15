using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class SysGuest
    {
        public int IdGuest { get; set; }
        public int? IdAgreementUser { get; set; }
        public string McGuest { get; set; }
        public string Gender { get; set; }
        public string 证件类型 { get; set; }
        public string 证件号码 { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool? Sex { get; set; }
    }
}
