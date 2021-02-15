using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class YwSubscribe
    {
        public int IdSubscribe { get; set; }
        public int IdVip { get; set; }
        public int IdAgreementUser { get; set; }
        public int IdGuest { get; set; }
        public string NumberSubscribe { get; set; }
        public string HouseStageId { get; set; }
        public DateTime? TimePredict { get; set; }
        public DateTime? TimeLeave { get; set; }
        public int NumberPeople { get; set; }
        public decimal MoneyPledge { get; set; }
        public bool StateSecrecy { get; set; }
        public string Remark { get; set; }
        public string TypeCheckIn { get; set; }
        public string NumberOpenStage { get; set; }
    }
}
