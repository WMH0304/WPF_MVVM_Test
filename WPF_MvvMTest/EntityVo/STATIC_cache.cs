using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MvvMTest.EntityVo
{
    static class STATIC_cache
    {
        static public int ID_RoomStage { get; set; }
        //  public int ID_Class { get; set; }
        static public string Number_RoomStage { get; set; }
        static public string MC_RoomStage { get; set; }
        static public string State_RoomStage { get; set; }
        static public string Describe { get; set; }
        static public int JionSign { get; set; }
        static public int ID_Class { get; set; }
        static public string Code_Class { get; set; }
        static public string MC_Class { get; set; }
        static public string Jc_Class { get; set; }
        static public decimal Price_Plate { get; set; }
        static public decimal Price_Actual { get; set; }
        static public string Remark { get; set; }
        static public string State_Class { get; set; }

        public static List<RoomStage> StaticRoomStages;

        public static List<Consumer> StaticConsumerLeft;
        public static List<Consumer> StaticConsumerRight;
        public static string ScBill_id { get; set; }
    }
}
