using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class PjProject
    {
        public int IdProject { get; set; }
        public int IdProjectClass { get; set; }
        public string McProject { get; set; }
        public string JcProject { get; set; }
        public string Unit { get; set; }
    }
}
