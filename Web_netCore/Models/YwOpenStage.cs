using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class YwOpenStage
    {
        public int IdOpenStage { get; set; }
        public int IdAgreementUser { get; set; }
        public int IdVip { get; set; }
        public int NumberPeople { get; set; }
        public int GuestId { get; set; }
        public DateTime TimePredict { get; set; }
        public DateTime TimeLeave { get; set; }
        public decimal MoneyPledge { get; set; }
        public string TypeCheckIn { get; set; }
        public bool StateSecrecy { get; set; }
        public string Remark { get; set; }
        public bool StateMessage { get; set; }
        public string ContentMessage { get; set; }
        public string HouseStageId { get; set; }
        public string NumberOpenStage { get; set; }
    }
}
