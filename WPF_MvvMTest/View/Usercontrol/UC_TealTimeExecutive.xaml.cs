using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_MvvMTest.EntityVo;
using WPF_MvvMTest.Model;
using WPF_MvvMTest.Resources;
using WPF_MvvMTest.View.Windows;

public delegate void MessageSend(List<RoomStage> roomStages);
namespace WPF_MvvMTest.View.Usercontrol
{
    /// <summary>
    /// UC_TealTimeExecutive.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class UC_TealTimeExecutive : UserControl
    {
        public event MessageSend MessageSend;
        public UC_TealTimeExecutive()
        {
            InitializeComponent();
         
        }
        //实体实例化
        EasternStar_WPF_MVVMEntities myModels = new EasternStar_WPF_MVVMEntities();
        List<RoomStage> BtL;

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ///房号赛选
            string RoomNum = tbTheCurrentHousingUnits.Text.Trim();
            if (BtL !=null)
            {
                BtL.Clear();
                 UC_WP.Children.Clear() ;
                //清除指定控件
                //List<UC_BtDynamic> uC_s = new List<UC_BtDynamic>();
                //foreach (var item in UC_WP.Children)
                //{
                //    if (item is UC_BtDynamic)
                //        continue;
                //    var button = item as UC_BtDynamic;
                //    string s = button.Name;
                //    if (!button.Name.Contains("btAddButton"))
                //        continue;

                //    uC_s.Add(button);
                //}
                //foreach (UC_BtDynamic item in uC_s)
                //{
                //    UC_WP.Children.Remove(item);
                //}

                //动态生成那个加号
                Button button = new Button()
                {
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(10),
                    // Background = "#39b54a";
                    IsEnabled = true,
                    Background = System.Windows.Media.Brushes.Transparent,
                   /// Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#39b54a")),
                    Name = "btAddButton",
                    
                };
                button.Click += new RoutedEventHandler(BtAddButton_Click);
                button.MouseEnter += new MouseEventHandler(BtDongTaisheng);
                Uri uri = new Uri(@"D:\练手的项目\WPFMvvM\WPF_MvvMTest\WPF_MvvMTest\Image\加号.png", UriKind.Relative);
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = new BitmapImage(uri);
                button.Background = ib;

                UC_WP.Children.Add(button);
                
            }
             BtL = (from tbR in myModels.SYS_RoomStage
                    join tbC in myModels.SYS_Class on tbR.ID_Class.ToString().Trim() equals tbC.Code_Class.ToString().Trim()
                    where tbR.ID_Class ==1
                    orderby tbR.Describe
                    select new 
                       RoomStage
            {  
                ID_RoomStage = tbR.ID_RoomStage,
                ID_Class = tbR.ID_Class,
                Number_RoomStage = tbR.Number_RoomStage,
                MC_RoomStage = tbR.MC_RoomStage,
                State_RoomStage = tbR.State_RoomStage,
                Describe = tbR.Describe,
                JionSign = (int)tbR.JionSign,
                MC_Class = tbC.MC_Class,  
                Jc_Class = tbC.Jc_Class,
                Code_Class = tbC.Code_Class,
                
            }).ToList();
            //房号赛选
            if (!string.IsNullOrEmpty(RoomNum))
            {
                BtL = BtL.Where(l => l.Number_RoomStage == RoomNum).ToList();
            }
            // BtL.Count();
            #region 动态生成控件
          
            for (int i = 0; i < BtL.Count(); i++)
            {

                //Button b = new Button() { Height = 100,
                //    Width = 100,

                //    //设置外边距
                //    Name = "btn" + 1,
                //    Margin = new Thickness(10),
                //    //按钮内容
                //    //Content =
                //    //  BtL[i].Number_RoomStage +
                //    //@"\n" + BtL[i].MC_RoomStage,

                //    Background = System.Windows.Media.Brushes.LightSeaGreen,

             

                    UC_BtDynamic myBtDynamic = new UC_BtDynamic(BtL);
                    myBtDynamic.TbRoom_num.Text = BtL[i].Number_RoomStage;
                    myBtDynamic.TbRoom_class.Text = BtL[i].MC_RoomStage.Trim() + BtL[i].Number_RoomStage;
                    myBtDynamic.Name = "btn" + BtL[i].ID_RoomStage;
                    //换台刷新
                    myBtDynamic.pus += new Pus(Push);
                    //获取控件内容id
                    myBtDynamic.GetButtons += new GetButton(SetBtuuons);
                    if (BtL[i].State_RoomStage.Trim() == "预定")
                    {
                        myBtDynamic.Background = System.Windows.Media.Brushes.Blue;
                    }
                    else if (BtL[i].State_RoomStage.Trim() == "已用")
                    {
                        myBtDynamic.Background = System.Windows.Media.Brushes.YellowGreen;
                    }
                    else if (BtL[i].State_RoomStage.Trim() == "停用")
                    {
                        myBtDynamic.Background = System.Windows.Media.Brushes.LightSlateGray;
                    }

                    //  myBtDynamic.Click += new RoutedEventHandler(btnEvent_Click);

                    UC_WP.Children.Add(myBtDynamic);

              
              
               
                
                };


           

            #endregion

            #region 动态生成控件


            ////开始离开时间布局
            //StackPanel stack = new StackPanel()
            //{
            //    Orientation = Orientation.Horizontal,
            //    Background = System.Windows.Media.Brushes.Black,
            //};
            ////开始时间
            //TextBlock t = new TextBlock()
            //{   
            //    FontSize = 7,
            //    Text = DateTime.Now.ToString() +"vvvvvvvv"+ DateTime.Now.ToString(),
            //};
            ////现在时间

            ////单击事件
            //b.Click += new RoutedEventHandler(btnEvent_Click);

            ////鼠标移入事件
            //b.MouseMove += new MouseEventHandler(tbnMouseMove);
            ////鼠标移开事件
            //b.MouseLeave += new MouseEventHandler(tbnMouseLeave);


            //UC_WP.Children.Add(b);
            //stack.Children.Add(t);
            #endregion

            //全部房数
            tbAllContet.Text = BtL.Count().ToString();
            //可用台
            TbAvailable.Text = (BtL.Count() - BtL.Where(p => p.State_RoomStage.Trim() == "已用").Count()).ToString() ;
            //已用台
            TbHadToUse.Text = BtL.Where(p=> p.State_RoomStage.Trim() == "已用").Count().ToString();
            

        }
     /// <summary>
     /// 接受button 信息
     /// </summary>
     /// <param name="roomStages"></param>
        public void SetBtuuons(List<RoomStage> Stages)
        {
            if (Stages!=null)
            {
                List<RoomStage> roomS =Stages;
                try
                {
                    MessageSend(Stages);
                }
                catch (Exception e)
                {

                    Debug.Write("委托传值，委托对象为空");
                    throw new Exception();
                }
              
            }
        }




        /// <summary>
        /// 动态控件之鼠标移入事件
        /// </summary>
        /// <param name="o"></param>
        /// <param name="mouse"></param>
        public void BtDongTaisheng(object o,MouseEventArgs mouse)
        {
            Button button = o as Button;
            Uri uri = new Uri(@"C:\Users\Administrator\source\repos\WPF_MvvMTest\WPF_MvvMTest\Image\加号.png", UriKind.Relative);
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(uri);
            button.Background = ib;

        }


        /// <summary>
        /// 那个绿色的加号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtAddButton_Click(object sender, RoutedEventArgs e)
        {
            W_ButtonAdd w_ButtonAdd = new W_ButtonAdd(null,1);
            // w_ButtonAdd.Closed += new EventHandler(w_ButtonAdd.UE_close);
            w_ButtonAdd.ChangC += new ChangeClose(Push);
           
            w_ButtonAdd.ShowDialog();
            w_ButtonAdd.ChangC -= new ChangeClose(Push);
        }
        //调用刷新页面方法
        void Push()
        {
           
            UserControl_Loaded(null,null);
        }
       
      
        //房台删除
        private void BtDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }


  

  
       


    


}

