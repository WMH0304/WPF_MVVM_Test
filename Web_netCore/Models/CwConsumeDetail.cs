using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class CwConsumeDetail
    {
        public int IdComsumeDetail { get; set; }
        public int? IdProject { get; set; }
        public int? IdConsumption { get; set; }
        public int? IdPayRecord { get; set; }
        public bool? StateComsumeDetail { get; set; }
        public bool? Presenter { get; set; }
        public decimal? Money { get; set; }
        public DateTime? Time { get; set; }
    }
}
