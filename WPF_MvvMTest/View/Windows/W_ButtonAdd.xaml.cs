using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_MvvMTest.Model;
//定义委托
public delegate void ChangeClose();
namespace WPF_MvvMTest.View.Windows
{
    /// <summary>
    /// W_ButtonAdd.xaml 的交互逻辑
    /// </summary>
    /// 

   
    public partial class W_ButtonAdd : Window
    {
        //定义事件
        public event ChangeClose ChangC;
        public W_ButtonAdd(Tuple<string,string,string,string,int,string> tuple,int roomtype)
        {
            tupl = tuple;
            type_room = roomtype;
            InitializeComponent();
        }
        int type_room;
        Tuple<string, string, string, string,int, string> tupl;
        static int Id_RoomStage;
        Model.EasternStar_WPF_MVVMEntities my = new Model.EasternStar_WPF_MVVMEntities();
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (tupl !=null)
            {
                string tbRoom_name = tupl.Item1;
                string cbStatus = tupl.Item2;
                string tbDescribe = tupl.Item3;
                string cbClass = tupl.Item4;
                Id_RoomStage = tupl.Item5;

                TbRoom_Name.Text = tbRoom_name;
                CbStatus.Text = cbStatus;
                TbDescribe.Text = tbDescribe;
                CbClass.Text = cbClass;
            }
            //绑定下拉框
            var c = my.SYS_Class.ToList();
            CbClass.ItemsSource = c;
            CbClass.DisplayMemberPath = "Jc_Class";
            CbClass.SelectedValuePath = "ID_Class";
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedEvent"></param>
        private void BtConfirm_Click(object sender, RoutedEventArgs routedEvent)
        {
            try
            {
                string strText = "";
                string tbRoom_name = TbRoom_Name.Text.Trim().ToString() == "" ? strText = "房台名称不能为空" : TbRoom_Name.Text.Trim().ToString();
                string cbStatus = CbStatus.Text.Trim().ToString() == "" ? strText = "请选择状态" : CbStatus.Text.Trim().ToString();
                string tbDescribe = TbDescribe.Text.Trim().ToString() == "" ? strText = "请输入房台描述" : TbDescribe.Text.Trim().ToString();
                //string cbClass = CbClass.SelectedIndex.ToString() == "0" ? strText = "请选中类型" : CbClass.SelectedIndex.ToString();
                Model.SYS_Class sYS_ = (CbClass.SelectedItem) as Model.SYS_Class;
                int cbClass = sYS_.ID_Class;
                int numberRoomStage = my.SYS_RoomStage.Count();

                int c = Convert.ToInt32(cbClass);
                int count = my.SYS_RoomStage.Where(p => p.MC_RoomStage.Trim().ToString() == tbRoom_name && p.State_RoomStage == cbStatus &&
                p.Describe == tbDescribe && p.ID_Class ==c ).Count();
                if (count > 0)
                {
                    MessageBox.Show("已存在房台信息", "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    return;
                }
              else  if (tbRoom_name!= "" && cbStatus != "" &&tbDescribe != "" && c>0)
                {
                    //修改
                    if (tupl != null)
                    {
                        strText = "房台修改成功";
                        Model.SYS_RoomStage r = my.SYS_RoomStage.Where(p => p.ID_RoomStage == Id_RoomStage).Single();
                        r.MC_RoomStage = tbRoom_name;
                        r.State_RoomStage = cbStatus;
                        r.Describe = tbDescribe;
                        r.ID_Class = type_room;
                        my.Entry(r).State = System.Data.Entity.EntityState.Modified;
                      
                        if (my.SaveChanges() > 0)
                        {
                            MessageBoxResult result = MessageBox.Show(strText, "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                            if (result == MessageBoxResult.OK)
                            {
                                View.Usercontrol.UC_TealTimeExecutive u = new Usercontrol.UC_TealTimeExecutive();
                                u.UserControl_Loaded(null, null);
                                this.Close();
                            }
                        }
                    
                    }
                    //添加
                    else if (tupl == null)
                    {
                        strText = "房台添加成功";
                        SYS_RoomStage m = new SYS_RoomStage();
                        m.MC_RoomStage = tbRoom_name;
                        m.State_RoomStage = cbStatus;
                        m.Number_RoomStage = (numberRoomStage +1).ToString();
                        m.Describe = tbDescribe;
                        m.ID_Class = type_room;
                        my.SYS_RoomStage.Add(m);
                        
                        if (my.SaveChanges() > 0)
                        {
                         MessageBoxResult s= MessageBox.Show(strText, "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                            // View.Usercontrol.UC_TealTimeExecutive u = new Usercontrol.UC_TealTimeExecutive();
                            if (s==MessageBoxResult.OK)
                            {
                                //调用委托
                                ChangC();
                                this.Close();
                            }
                              
                        }
                    }
                }
                else
                {
                    MessageBox.Show(strText, "大海提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                }
            }
            catch (Exception e)
            {

                Debug.Write(e);
            }
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="eventArgs"></param>
        public void UE_close(object o,EventArgs eventArgs)
        {
            this.Close();
        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtQuit_Click(object sender, RoutedEventArgs e)
        {
           
            if (TbRoom_Name.Text == "" && CbStatus.Text == "" && TbDescribe.Text == ""&& CbClass.Text == "")
            {
                this.Close();
            }
            else
            {
                TbRoom_Name.Text = "";
                CbStatus.Text = "";
                TbDescribe.Text = "";
                CbClass.Text = "";
            }

        }

    }
}
