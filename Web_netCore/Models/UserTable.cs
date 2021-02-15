using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class UserTable
    {
        public int IdUser { get; set; }
        public string NumberJob { get; set; }
        public string Password { get; set; }
        public string McUser { get; set; }
        public bool StateUser { get; set; }
        public string Jrisdiction { get; set; }
    }
}
