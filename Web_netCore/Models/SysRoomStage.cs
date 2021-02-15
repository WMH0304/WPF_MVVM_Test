using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class SysRoomStage
    {
        public int IdRoomStage { get; set; }
        public int? IdGuest { get; set; }
        public int IdClass { get; set; }
        public string NumberRoomStage { get; set; }
        public string McRoomStage { get; set; }
        public string StateRoomStage { get; set; }
        public string Describe { get; set; }
        public int? JionSign { get; set; }
        public int? IdRoomType { get; set; }
        public int? Floor { get; set; }
        public decimal? Preinstall { get; set; }
        public decimal? Practical { get; set; }
    }
}
