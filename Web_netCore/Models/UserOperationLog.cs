using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class UserOperationLog
    {
        public int IdOperationLog { get; set; }
        public int IdUser { get; set; }
        public DateTime TimeOperation { get; set; }
        public string ContentAbstract { get; set; }
        public string ContentOperation { get; set; }
    }
}
