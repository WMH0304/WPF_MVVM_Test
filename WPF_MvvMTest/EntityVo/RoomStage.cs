using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MvvMTest.EntityVo
{
    public class RoomStage : Model.SYS_Class ,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int ID_RoomStage { get; set; }
      //  public int ID_Class { get; set; }
        public string Number_RoomStage { get; set; }
        public string MC_RoomStage { get; set; }
        public string State_RoomStage { get; set; }
        public string Describe { get; set; }
        public int? JionSign { get; set; }

        public int? floor { get; set; }
        public int ?ID_Guest { get; set; }

        public string room_status_type { get; set; }
        public int? ID_room_status_type { get; set; }
   public string Due_to_no { get; set; }


        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
