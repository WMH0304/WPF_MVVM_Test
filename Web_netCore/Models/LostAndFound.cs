using System;
using System.Collections.Generic;

#nullable disable

namespace Web_netCore.Models
{
    public partial class LostAndFound
    {
        public int IdLostAndFound { get; set; }
        public int IdRoomStage { get; set; }
        public string Content { get; set; }
        public string PeopleUp { get; set; }
        public DateTime TimeUp { get; set; }
        public string PeppleClaim { get; set; }
        public DateTime TimeClaim { get; set; }
        public bool State { get; set; }
    }
}
