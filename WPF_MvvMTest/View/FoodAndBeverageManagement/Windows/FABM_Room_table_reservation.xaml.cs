using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
using WPF_MvvMTest.EntityVo;
using WPF_MvvMTest.Model;

namespace WPF_MvvMTest.View.FoodAndBeverageManagement.Windows
{
    /// <summary>
    /// FABM_Room_table_reservation.xaml 的交互逻辑
    /// </summary>
    public partial class FABM_Room_table_reservation : Window
    {
        EasternStar_WPF_MVVMEntities M = new EasternStar_WPF_MVVMEntities();
        Tuple<string, string> tuples;
        //临时表
        List<RoomStage> Rs;
        //左边表格
        ObservableCollection<RoomStage> LeftOc = new ObservableCollection<RoomStage>();
        //右边
        ObservableCollection<RoomStage> RightOc = new ObservableCollection<RoomStage>();
        //公共
        ObservableCollection<RoomStage> Common = new ObservableCollection<RoomStage>();

        int intdt =-1;
        /// <summary>
        /// 添加为true 移除为false
        /// </summary>
        bool operation = true;
        
        public FABM_Room_table_reservation(Tuple<string,string> tuple,List<RoomStage> rooms)
        {

            // Rs = rooms.Where(m=>m.State_RoomStage.Trim() =="未用").ToList();
            Common.Clear();
            Rs = rooms;
            tuples = tuple;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Cb_add.IsChecked = true;

            LbRoomNum.Text = tuples.Item1;
            LbRoomName.Text = tuples.Item2;

            //账号
            var zhanghao = M.SYS_Guest.ToList();
            CbZhanghao.ItemsSource = zhanghao;
            CbZhanghao.DisplayMemberPath = "MC_Guest";
            CbZhanghao.SelectedValuePath = "ID_Guest";
            //协议单位
            var xieyidanwei = M.AG_AgreementUser.ToList();
            CbBargainingUnit.ItemsSource = xieyidanwei;
            CbBargainingUnit.DisplayMemberPath = "MC_AgreementUser";
            CbBargainingUnit.SelectedValuePath = "ID_AgreementUser";

            GetData();
            DgLeft.ItemsSource = LeftOc;

            DgRight.ItemsSource = RightOc;
         
            Ti_room_choosable.Header = "可选房台 " +LeftOc.Count() + " 间";
            Ti_already_room.Header = "已选房台 " + RightOc.Count() + " 间";
            LbNumberOfRoomsAvailable.Content = "  "+LeftOc.Count() + " 间";
            Lb_set_number.Content = "  " + RightOc.Count() + " 间";
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public void  GetData()
        {
            foreach (var item in Rs)
            {
                if (item.State_RoomStage.Trim() == "未用")
                {
                    //LRS[0].Number_RoomStage, LRS[0].MC_RoomStage
                    if (item.Number_RoomStage.Trim() == tuples.Item1.Trim() && item.MC_RoomStage.Trim() == tuples.Item2.Trim())
                    {
                        //右边表格
                        RightOc.Add(item);
                    }
                    else
                    {
                        //左边表格
                        LeftOc.Add(item);
                    }
                }
                
            }
        }

        /// <summary>
        /// 左边表格点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgLeft_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            RoomStage room = DgLeft.CurrentItem as RoomStage;
            intdt = DgLeft.SelectedIndex;
        }

        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtAddition_Click(object sender, RoutedEventArgs e)
        {
            RoomStage room = DgLeft.Items[intdt] as RoomStage;

            if (intdt>=0)
            {

                if (!Common.Contains(room))
                {
                    Common.Add(room);
                };

                LeftOc.Remove(room);
                RightOc.Add(room);
                Ti_room_choosable.Header = "可选房台" + LeftOc.Count() + "间";
                Ti_already_room.Header = "" + RightOc.Count() + "间";
                LbNumberOfRoomsAvailable.Content = "可选房台 " + LeftOc.Count() + " 间";
                Lb_set_number.Content = "已选房台" + RightOc.Count() + " 间";
                intdt = -1;

                operation = true;
            }
            else
            {
                MessageBox.Show("请选中房台后再进行添加", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        /// <summary>
        /// 移除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtRemove_Click(object sender, RoutedEventArgs e)
        {
            RoomStage room = DgLeft.Items[intdt] as RoomStage;
            if (intdt >= 0)
            {
                if (!Common.Contains(room))
                {
                    Common.Add(room);
                }
               
                LeftOc.Add(room);
                RightOc.Remove(room); ;
                Ti_room_choosable.Header = "可选房台" + LeftOc.Count() + "间";
                Ti_already_room.Header = "已选房台" + RightOc.Count() + "间";
                LbNumberOfRoomsAvailable.Content = "  " + LeftOc.Count() + " 间";
                Lb_set_number.Content = "  " + RightOc.Count() + " 间";
                intdt = -1;
                operation = false;
            }
            else
            {
                MessageBox.Show("请选中房台后再进行移除", "大海提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }


        }

        /// <summary>
        /// 生成单号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void Tb_odd_MouseLeftButtonDown(object sender, TextChangedEventArgs e)
        {
            int Sub = M.YW_Subscribe.Where(m => m.Number_OpenStage == STATIC_cache.ID_Class.ToString()).Count();
            int subs = Sub + 1;
            DateTime dt = DateTime.Now;
            string str = dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString();
            string Sub_name = "CYYD" + str  + subs;
            Tb_odd.Text = Sub_name.Trim();
            return;
        }

        /// <summary>
        /// 下拉框回填
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbZhanghao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // CbZhanghao.Text.Trim();
            //用户id
            SYS_Guest CbZhanghao_id = CbZhanghao.SelectedItem as SYS_Guest;
            //var c =CbZhanghao.Items[CbZhanghao_id].ToString
            
            var t= M.VIP_Table.Where(m => m.ID_Guest == CbZhanghao_id.ID_Guest).Single().Accounts;

            //禁用按钮
            if (CbZhanghao_id.ID_AgreementUser.Equals(null))
            {
                CbBargainingUnit.IsReadOnly = false;
                CbBargainingUnit.IsEnabled = false;
                CbBargainingUnit.Opacity = 0.5;
            }

            Tb_vip_account.Text = t;
        }

        /// <summary>
        /// 加预定台按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_add_scheduled_station_Click(object sender, RoutedEventArgs e)
        {
            if (!CbZhanghao.Text.Equals(null) && !Tb_odd.Text.Equals(null) && !TbDevelopmentTime.Text.Equals(null) && !Tb_vip_account.Text.Equals(null) && !Tb_number_of_people.Text.Equals(null) &&!Tb_discount.Text.Equals(null) && !Tb_remark.Text.Equals(null))
            {
                if (operation == false)
                {
                    MessageBox.Show("加预定台前请在可选房台中选取目标房台添加到已选房台中", "大海提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                    Common.Clear();
                    GetData();
                }
                string vip_account = Tb_vip_account.Text.Trim();
                user_id = M.VIP_Table.Where(t => t.Accounts == vip_account).FirstOrDefault().ID_Guest;
                string discount = Tb_discount.Text.Trim();

                string house_id = String.Empty;//房台id
                //要加的
                foreach (var item in Common)
                {
                    if (Cb_add.IsChecked==true)
                    {
                        //添加消费
                        CW_Consumption c = new CW_Consumption();
                        c.ID_RoomStage = item.ID_RoomStage;
                        c.Discount = Convert.ToDecimal(discount);
                        c.Effective = true;
                        M.CW_Consumption.Add(c);
                        M.SaveChanges();
                    }
                   

                    //修改选择房台状态
                    SYS_RoomStage r = M.SYS_RoomStage.Where(s => s.ID_RoomStage == item.ID_RoomStage).Single();
                    r.State_RoomStage = "预定";
                    r.ID_Guest = user_id;
                    M.Entry(r).State = System.Data.Entity.EntityState.Modified;
                    M.SaveChanges();
                    house_id += item.ID_RoomStage + ",";
                }

             
                
                YW_OpenStage o = M.YW_OpenStage.Where(w => w.GuestID == user_id).Single();
                o.HouseStageID = house_id;
                M.Entry(o).State = System.Data.Entity.EntityState.Modified;



                if (M.SaveChanges()>0)
                {
                    MessageBox.Show("房台预加成功","大海提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("请将信息填写完整","大海提示",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 预定取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_due_to_cancellation_Click(object sender, RoutedEventArgs e)
        {
            if (operation == true)
            {
                MessageBox.Show("预定取消前请在已选房台中将目标房台移除", "大海提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                Common.Clear();
                GetData();
            }
            if (Common.Equals(null))
            {
                MessageBox.Show("请将房台移除后，再次预定取消","大海提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                return ;
            }
            foreach (var item in Common)
            {
                //删除消费
                CW_Consumption c = M.CW_Consumption.Where(m => m.ID_RoomStage == item.ID_RoomStage && m.Effective == true).Single();
                M.CW_Consumption.Remove(c);
                M.SaveChanges();

                //修改房间状态
                SYS_RoomStage r = M.SYS_RoomStage.Where(m => m.ID_RoomStage == item.ID_RoomStage).Single();
                r.State_RoomStage = "未用";
                r.ID_Guest = null;
                M.Entry(r).State = System.Data.Entity.EntityState.Modified;
                M.SaveChanges();
            }
            Common.Clear();
        }

        /// <summary>
        /// 房台id 获取
        /// </summary>
        /// <returns></returns>
        private string Reture(string str, int id)
        {
            string room =string.Empty; 
            foreach (var item in RightOc)
            {
               
               room += item.ID_RoomStage+",";

              
                if (id>=0)
                {
                    if (Cb_add.IsChecked == true)
                    {
                        //添加消费
                        CW_Consumption c = new CW_Consumption();
                        c.ID_RoomStage = item.ID_RoomStage;
                        c.Discount = Convert.ToDecimal(str);
                        c.Effective = true;
                        M.CW_Consumption.Add(c);
                        M.SaveChanges();
                    }
                   

                    //修改选择房台信息
                    SYS_RoomStage r = M.SYS_RoomStage.Where(s => s.ID_RoomStage == item.ID_RoomStage).Single();
                    r.State_RoomStage = "预定";
                    r.ID_Guest = id;
                    M.Entry(r).State = System.Data.Entity.EntityState.Modified;
                    M.SaveChanges();
                }
            }

            return room;
        }
        int user_id;
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_save_Click(object sender, RoutedEventArgs e)
        {
            if (!CbZhanghao.Text.Equals(null) && !Tb_odd.Text.Equals(null) && !TbDevelopmentTime.Text.Equals(null) && !Tb_vip_account.Text.Equals(null) && !Tb_number_of_people.Text.Equals(null) && !Tb_discount.Text.Equals(null) && !Tb_remark.Text.Equals(null))
            {
                string People = Tb_number_of_people.Text.Trim();

                string vip_account = Tb_vip_account.Text.Trim();

                int gruest_id = M.VIP_Table.Where(t => t.Accounts == vip_account).FirstOrDefault().ID_Guest;
                user_id = M.VIP_Table.Where(t => t.Accounts == vip_account).FirstOrDefault().ID_Guest;
                //新增预定
                YW_Subscribe m = new YW_Subscribe();
                m.ID_VIP = M.VIP_Table.Where(t => t.Accounts == vip_account).FirstOrDefault().ID_VIP;//会员id


                //m.ID_AgreementUser = (int)M.SYS_Guest.Where(g=>g.ID_Guest ==(M.VIP_Table.Where(t=>t.Accounts == vip_account).FirstOrDefault().ID_Guest)).Single().ID_AgreementUser;//协议用户id
                if (CbBargainingUnit.IsEnabled)
                {
                  string Cb_bargaining_name=  CbBargainingUnit.Text.Trim();

                    m.ID_AgreementUser = M.AG_AgreementUser.Where(a => a.MC_AgreementUser.Trim() == Cb_bargaining_name).Single().ID_AgreementUser;

                }
                else
                {
                    m.ID_AgreementUser = 0;
                }

                m.ID_Guest = gruest_id;//客人id
                m.Number_Subscribe = Tb_odd.Text.Trim();//预约单号
                
                m.Time_Predict =Convert.ToDateTime(TbDevelopmentTime.Text);//开台时间
                m.Number_People = Convert.ToInt32(People);//人数
                m.Remark = Tb_remark.Text.Trim();
                m.HouseStageID = Reture(Tb_discount.Text, user_id);//房台id
                m.State_Secrecy = true;//有效否
                m.Type_CheckIn = "餐饮";
                m.Number_OpenStage = STATIC_cache.ID_Class.ToString();
                M.YW_Subscribe.Add(m);
               // M.SaveChanges();
                if (M.SaveChanges() > 0)
                {
                    MessageBox.Show("保存成功","大海提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                    Refresh_table(gruest_id, STATIC_cache.ID_Class);
                    Common.Clear();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("请将信息填写完整", "大海提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 刷新表格
        /// </summary>
        private void Refresh_table(int gruest_id, int room_calss)
        {
            LeftOc.Clear();
            RightOc.Clear();
            // Rs = M.SYS_RoomStage.Where(d => d.ID_Class == room_calss && d.State_RoomStage.Trim() == "预定").ToList();

            Rs = (from tr in M.SYS_RoomStage
                  where tr.ID_Class == room_calss 
                  select new RoomStage
                  {
                      ID_RoomStage = tr.ID_RoomStage,
                      ID_Class = tr.ID_Class,
                      Number_RoomStage = tr.Number_RoomStage,
                      MC_RoomStage = tr.MC_RoomStage,
                      State_RoomStage = tr.State_RoomStage,
                      Describe = tr.Describe,
                      JionSign = (int)tr.JionSign,
                  }).AsParallel().ToList();

            foreach (var item in Rs)
            {
                if (item.State_RoomStage.Trim() =="预定")
                {
                    RightOc.Add(item);
                }
                else
                {
                    LeftOc.Add(item);
                }
            }
        }



        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult =   MessageBox.Show("关闭后当前窗口数据将不保留", "大海提示",MessageBoxButton.YesNo,MessageBoxImage.Warning);
            if (messageBoxResult ==MessageBoxResult.OK)
            {
                this.Close();
            }

           
        }
    }
}
