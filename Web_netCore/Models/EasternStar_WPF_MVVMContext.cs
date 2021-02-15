using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Web_netCore.Models
{
    public partial class EasternStar_WPF_MVVMContext : DbContext
    {
        public EasternStar_WPF_MVVMContext()
        {
        }

        public EasternStar_WPF_MVVMContext(DbContextOptions<EasternStar_WPF_MVVMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AgAgreementClass> AgAgreementClasses { get; set; }
        public virtual DbSet<AgAgreementStagePrice> AgAgreementStagePrices { get; set; }
        public virtual DbSet<AgAgreementUser> AgAgreementUsers { get; set; }
        public virtual DbSet<CwBill> CwBills { get; set; }
        public virtual DbSet<CwConsumeDetail> CwConsumeDetails { get; set; }
        public virtual DbSet<CwConsumption> CwConsumptions { get; set; }
        public virtual DbSet<CwFinance> CwFinances { get; set; }
        public virtual DbSet<CwPayRecord> CwPayRecords { get; set; }
        public virtual DbSet<LostAndFound> LostAndFounds { get; set; }
        public virtual DbSet<PjProject> PjProjects { get; set; }
        public virtual DbSet<PjProjectClass> PjProjectClasses { get; set; }
        public virtual DbSet<PjProjectDetail> PjProjectDetails { get; set; }
        public virtual DbSet<SysClass> SysClasses { get; set; }
        public virtual DbSet<SysGuest> SysGuests { get; set; }
        public virtual DbSet<SysRoomStage> SysRoomStages { get; set; }
        public virtual DbSet<SysRoomStatusType> SysRoomStatusTypes { get; set; }
        public virtual DbSet<SysSystem> SysSystems { get; set; }
        public virtual DbSet<UserJurisdiction> UserJurisdictions { get; set; }
        public virtual DbSet<UserOperationLog> UserOperationLogs { get; set; }
        public virtual DbSet<UserTable> UserTables { get; set; }
        public virtual DbSet<VipGrade> VipGrades { get; set; }
        public virtual DbSet<VipTable> VipTables { get; set; }
        public virtual DbSet<YwOpenStage> YwOpenStages { get; set; }
        public virtual DbSet<YwSubscribe> YwSubscribes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source =127.0.0.1;Initial Catalog=EasternStar_WPF_MVVM;User ID =sa;Password=sa123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_PRC_CI_AS");

            modelBuilder.Entity<AgAgreementClass>(entity =>
            {
                entity.HasKey(e => e.IdAgreementClass)
                    .HasName("PK_AG_AGREEMENTCLASS");

                entity.ToTable("AG_AgreementClass");

                entity.Property(e => e.IdAgreementClass).HasColumnName("ID_AgreementClass");

                entity.Property(e => e.CodeAgreementClass)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Code_AgreementClass")
                    .IsFixedLength(true);

                entity.Property(e => e.McAgreementClass)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("MC_AgreementClass")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<AgAgreementStagePrice>(entity =>
            {
                entity.HasKey(e => e.IdAgreementStagePrice)
                    .HasName("PK_AG_AGREEMENTSTAGEPRICE");

                entity.ToTable("AG_AgreementStagePrice");

                entity.Property(e => e.IdAgreementStagePrice).HasColumnName("ID_AgreementStagePrice");

                entity.Property(e => e.IdAgreementClass).HasColumnName("ID_AgreementClass");

                entity.Property(e => e.IdClass).HasColumnName("ID_Class");

                entity.Property(e => e.PriceAgreement)
                    .HasColumnType("money")
                    .HasColumnName("Price_Agreement");

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AgAgreementUser>(entity =>
            {
                entity.HasKey(e => e.IdAgreementUser)
                    .HasName("PK_AG_AGREEMENTUSER");

                entity.ToTable("AG_AgreementUser");

                entity.Property(e => e.IdAgreementUser).HasColumnName("ID_AgreementUser");

                entity.Property(e => e.DateCredit)
                    .HasColumnType("date")
                    .HasColumnName("Date_Credit");

                entity.Property(e => e.IdAgreementClass).HasColumnName("ID_AgreementClass");

                entity.Property(e => e.LinkmanAgreementUser)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("Linkman_AgreementUser")
                    .IsFixedLength(true);

                entity.Property(e => e.McAgreementUser)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("MC_AgreementUser")
                    .IsFixedLength(true);

                entity.Property(e => e.PhoneAgreementUser)
                    .IsRequired()
                    .HasMaxLength(13)
                    .HasColumnName("Phone_AgreementUser")
                    .IsFixedLength(true);

                entity.Property(e => e.PrcieOverdraft)
                    .HasColumnType("money")
                    .HasColumnName("Prcie_Overdraft");

                entity.Property(e => e.PriceBalance)
                    .HasColumnType("money")
                    .HasColumnName("Price_Balance");
            });

            modelBuilder.Entity<CwBill>(entity =>
            {
                entity.HasKey(e => e.IdBill);

                entity.ToTable("CW_Bill");

                entity.Property(e => e.IdBill).HasColumnName("ID_Bill");

                entity.Property(e => e.NumberBill)
                    .HasMaxLength(30)
                    .HasColumnName("Number_Bill")
                    .IsFixedLength(true);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.Remark).HasMaxLength(50);

                entity.Property(e => e.StateBill)
                    .HasMaxLength(30)
                    .HasColumnName("State_Bill")
                    .IsFixedLength(true);

                entity.Property(e => e.SuOpId).HasColumnName("SuOp_ID");

                entity.Property(e => e.TimePayBill)
                    .HasColumnType("datetime")
                    .HasColumnName("Time_PayBill");
            });

            modelBuilder.Entity<CwConsumeDetail>(entity =>
            {
                entity.HasKey(e => e.IdComsumeDetail);

                entity.ToTable("CW_ConsumeDetail");

                entity.Property(e => e.IdComsumeDetail).HasColumnName("ID_ComsumeDetail");

                entity.Property(e => e.IdConsumption).HasColumnName("ID_Consumption");

                entity.Property(e => e.IdPayRecord).HasColumnName("ID_PayRecord");

                entity.Property(e => e.IdProject).HasColumnName("ID_Project");

                entity.Property(e => e.Money)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("money");

                entity.Property(e => e.Presenter).HasColumnName("presenter");

                entity.Property(e => e.StateComsumeDetail).HasColumnName("State_ComsumeDetail");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");
            });

            modelBuilder.Entity<CwConsumption>(entity =>
            {
                entity.HasKey(e => e.IdConsumption)
                    .HasName("PK_CW_CONSUMPTION");

                entity.ToTable("CW_Consumption");

                entity.Property(e => e.IdConsumption).HasColumnName("ID_Consumption");

                entity.Property(e => e.Discount).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.IdBill).HasColumnName("ID_Bill");

                entity.Property(e => e.IdProjectDetail).HasColumnName("ID_ProjectDetail");

                entity.Property(e => e.IdRoomStage).HasColumnName("ID_RoomStage");

                entity.Property(e => e.Prict).HasColumnType("money");

                entity.Property(e => e.TimeConsumption).HasColumnName("Time_Consumption");
            });

            modelBuilder.Entity<CwFinance>(entity =>
            {
                entity.HasKey(e => e.IdFinance)
                    .HasName("PK_CW_FINANCE");

                entity.ToTable("CW_Finance");

                entity.Property(e => e.IdFinance).HasColumnName("ID_Finance");

                entity.Property(e => e.Alipay).HasColumnType("money");

                entity.Property(e => e.CashPledge).HasColumnType("money");

                entity.Property(e => e.Checks).HasColumnType("money");

                entity.Property(e => e.Connect).HasColumnType("money");

                entity.Property(e => e.CreditCard).HasColumnType("money");

                entity.Property(e => e.Curtain).HasColumnType("money");

                entity.Property(e => e.Handin).HasColumnType("money");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.Income).HasColumnType("money");

                entity.Property(e => e.Issue).HasColumnType("money");

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Rmb)
                    .HasColumnType("money")
                    .HasColumnName("RMB");

                entity.Property(e => e.Surplus).HasColumnType("money");

                entity.Property(e => e.UseIdUser).HasColumnName("Use_ID_User");

                entity.Property(e => e.WeChat).HasColumnType("money");
            });

            modelBuilder.Entity<CwPayRecord>(entity =>
            {
                entity.HasKey(e => e.IdPayRecord);

                entity.ToTable("CW_PayRecord");

                entity.Property(e => e.IdPayRecord).HasColumnName("ID_PayRecord");

                entity.Property(e => e.IdBill).HasColumnName("ID_Bill");

                entity.Property(e => e.IdGuest).HasColumnName("ID_Guest");

                entity.Property(e => e.PoP)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.PricePay)
                    .HasColumnType("money")
                    .HasColumnName("Price_Pay");

                entity.Property(e => e.TimePay).HasColumnName("Time_Pay");
            });

            modelBuilder.Entity<LostAndFound>(entity =>
            {
                entity.HasKey(e => e.IdLostAndFound)
                    .HasName("PK_LOSTANDFOUND");

                entity.ToTable("LostAndFound");

                entity.Property(e => e.IdLostAndFound).HasColumnName("ID_LostAndFound");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("[Content");

                entity.Property(e => e.IdRoomStage).HasColumnName("ID_RoomStage");

                entity.Property(e => e.PeopleUp)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("People_Up")
                    .IsFixedLength(true);

                entity.Property(e => e.PeppleClaim)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("Pepple_Claim")
                    .IsFixedLength(true);

                entity.Property(e => e.TimeClaim)
                    .HasColumnType("datetime")
                    .HasColumnName("Time_Claim");

                entity.Property(e => e.TimeUp)
                    .HasColumnType("datetime")
                    .HasColumnName("Time_Up");
            });

            modelBuilder.Entity<PjProject>(entity =>
            {
                entity.HasKey(e => e.IdProject)
                    .HasName("PK_PJ_PROJECT");

                entity.ToTable("PJ_Project");

                entity.Property(e => e.IdProject).HasColumnName("ID_Project");

                entity.Property(e => e.IdProjectClass).HasColumnName("ID_ProjectClass");

                entity.Property(e => e.JcProject)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Jc_Project")
                    .IsFixedLength(true);

                entity.Property(e => e.McProject)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("MC_Project")
                    .IsFixedLength(true);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<PjProjectClass>(entity =>
            {
                entity.HasKey(e => e.IdProjectClass)
                    .HasName("PK_PJ_PROJECTCLASS");

                entity.ToTable("PJ_ProjectClass");

                entity.Property(e => e.IdProjectClass).HasColumnName("ID_ProjectClass");

                entity.Property(e => e.McProjectClass)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("MC_ProjectClass")
                    .IsFixedLength(true);

                entity.Property(e => e.StateProjectClass)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("State_ProjectClass")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<PjProjectDetail>(entity =>
            {
                entity.HasKey(e => e.IdProjectDetail)
                    .HasName("PK_PJ_PROJECTDETAIL");

                entity.ToTable("PJ_ProjectDetail");

                entity.Property(e => e.IdProjectDetail)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_ProjectDetail");

                entity.Property(e => e.IdProject).HasColumnName("ID_Project");

                entity.Property(e => e.Price).HasColumnType("money");
            });

            modelBuilder.Entity<SysClass>(entity =>
            {
                entity.HasKey(e => e.IdClass)
                    .HasName("PK_SYS_CLASS");

                entity.ToTable("SYS_Class");

                entity.Property(e => e.IdClass).HasColumnName("ID_Class");

                entity.Property(e => e.CodeClass)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Code_Class")
                    .IsFixedLength(true);

                entity.Property(e => e.JcClass)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("Jc_Class")
                    .IsFixedLength(true);

                entity.Property(e => e.McClass)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("MC_Class")
                    .IsFixedLength(true);

                entity.Property(e => e.PriceActual)
                    .HasColumnType("money")
                    .HasColumnName("Price_Actual");

                entity.Property(e => e.PricePlate)
                    .HasColumnType("money")
                    .HasColumnName("Price_Plate");

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StateClass)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("State_Class")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SysGuest>(entity =>
            {
                entity.HasKey(e => e.IdGuest)
                    .HasName("PK_SYS_GUEST");

                entity.ToTable("SYS_Guest");

                entity.Property(e => e.IdGuest).HasColumnName("ID_Guest");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("gender")
                    .IsFixedLength(true);

                entity.Property(e => e.IdAgreementUser).HasColumnName("ID_AgreementUser");

                entity.Property(e => e.McGuest)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("MC_Guest")
                    .IsFixedLength(true);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsFixedLength(true);

                entity.Property(e => e.Sex).HasColumnName("sex");

                entity.Property(e => e.证件号码)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.证件类型)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SysRoomStage>(entity =>
            {
                entity.HasKey(e => e.IdRoomStage)
                    .HasName("PK_SYS_ROOMSTAGE");

                entity.ToTable("SYS_RoomStage");

                entity.Property(e => e.IdRoomStage).HasColumnName("ID_RoomStage");

                entity.Property(e => e.Describe).HasMaxLength(100);

                entity.Property(e => e.Floor).HasColumnName("floor");

                entity.Property(e => e.IdClass).HasColumnName("ID_Class");

                entity.Property(e => e.IdGuest).HasColumnName("ID_Guest");

                entity.Property(e => e.IdRoomType).HasColumnName("ID_room_type");

                entity.Property(e => e.McRoomStage)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("MC_RoomStage")
                    .IsFixedLength(true);

                entity.Property(e => e.NumberRoomStage)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Number_RoomStage")
                    .IsFixedLength(true);

                entity.Property(e => e.Practical)
                    .HasColumnType("money")
                    .HasColumnName("practical");

                entity.Property(e => e.Preinstall)
                    .HasColumnType("money")
                    .HasColumnName("preinstall");

                entity.Property(e => e.StateRoomStage)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("State_RoomStage")
                    .HasDefaultValueSql("(N'1  可用  2 已用 3停用'')')")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SysRoomStatusType>(entity =>
            {
                entity.HasKey(e => e.IdRoomStatusType);

                entity.ToTable("SYS_Room_status_type");

                entity.Property(e => e.IdRoomStatusType).HasColumnName("ID_room_status_type");

                entity.Property(e => e.NameRoomStatusType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("name_room_status_type")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SysSystem>(entity =>
            {
                entity.HasKey(e => e.SystemId)
                    .HasName("PK_SYSTEM");

                entity.ToTable("SYS_System");

                entity.Property(e => e.SystemId).HasColumnName("SystemID");

                entity.Property(e => e.SystemMc)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("SystemMC")
                    .IsFixedLength(true);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<UserJurisdiction>(entity =>
            {
                entity.HasKey(e => e.IdJurisdiction)
                    .HasName("PK_USER_JURISDICTION");

                entity.ToTable("User_Jurisdiction");

                entity.Property(e => e.IdJurisdiction).HasColumnName("ID_Jurisdiction");

                entity.Property(e => e.McJurisdiction)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("MC_Jurisdiction")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<UserOperationLog>(entity =>
            {
                entity.HasKey(e => e.IdOperationLog)
                    .HasName("PK_USER_OPERATIONLOG");

                entity.ToTable("User_OperationLog");

                entity.Property(e => e.IdOperationLog).HasColumnName("ID_OperationLog");

                entity.Property(e => e.ContentAbstract)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Content_Abstract")
                    .IsFixedLength(true);

                entity.Property(e => e.ContentOperation)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Content_Operation")
                    .IsFixedLength(true);

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.TimeOperation)
                    .HasColumnType("datetime")
                    .HasColumnName("Time_Operation");
            });

            modelBuilder.Entity<UserTable>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PK_USER_TABLE");

                entity.ToTable("User_Table");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.Jrisdiction)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.McUser)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("MC_User")
                    .IsFixedLength(true);

                entity.Property(e => e.NumberJob)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("Number_Job")
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.StateUser).HasColumnName("State_User");
            });

            modelBuilder.Entity<VipGrade>(entity =>
            {
                entity.HasKey(e => e.IdGrade)
                    .HasName("PK_VIP_GRADE");

                entity.ToTable("VIP_Grade");

                entity.Property(e => e.IdGrade).HasColumnName("ID_Grade");

                entity.Property(e => e.Discount).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.McGrade)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("MC_Grade")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<VipTable>(entity =>
            {
                entity.HasKey(e => e.IdVip)
                    .HasName("PK_VIP_TABLE");

                entity.ToTable("VIP_Table");

                entity.Property(e => e.IdVip).HasColumnName("ID_VIP");

                entity.Property(e => e.Accounts)
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.DateCredit)
                    .HasColumnType("date")
                    .HasColumnName("Date_Credit");

                entity.Property(e => e.IdGrade).HasColumnName("ID_Grade");

                entity.Property(e => e.IdGuest).HasColumnName("ID_Guest");

                entity.Property(e => e.IntegraVipl)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Integra_VIPl");

                entity.Property(e => e.PrcieOverdraft)
                    .HasColumnType("money")
                    .HasColumnName("Prcie_Overdraft");

                entity.Property(e => e.PriceBalance)
                    .HasColumnType("money")
                    .HasColumnName("Price_Balance");
            });

            modelBuilder.Entity<YwOpenStage>(entity =>
            {
                entity.HasKey(e => e.IdOpenStage)
                    .HasName("PK_YW_OPENSTAGE");

                entity.ToTable("YW_OpenStage");

                entity.Property(e => e.IdOpenStage).HasColumnName("ID_OpenStage");

                entity.Property(e => e.ContentMessage)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Content_Message");

                entity.Property(e => e.GuestId).HasColumnName("GuestID");

                entity.Property(e => e.HouseStageId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("HouseStageID")
                    .IsFixedLength(true);

                entity.Property(e => e.IdAgreementUser).HasColumnName("ID_AgreementUser");

                entity.Property(e => e.IdVip).HasColumnName("ID_VIP");

                entity.Property(e => e.MoneyPledge)
                    .HasColumnType("money")
                    .HasColumnName("Money_Pledge");

                entity.Property(e => e.NumberOpenStage)
                    .HasMaxLength(50)
                    .HasColumnName("Number_OpenStage")
                    .IsFixedLength(true);

                entity.Property(e => e.NumberPeople).HasColumnName("Number_People");

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StateMessage).HasColumnName("State_Message");

                entity.Property(e => e.StateSecrecy).HasColumnName("State_Secrecy");

                entity.Property(e => e.TimeLeave).HasColumnName("Time_Leave");

                entity.Property(e => e.TimePredict).HasColumnName("Time_Predict");

                entity.Property(e => e.TypeCheckIn)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Type_CheckIn")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<YwSubscribe>(entity =>
            {
                entity.HasKey(e => e.IdSubscribe)
                    .HasName("PK_YW_SUBSCRIBE");

                entity.ToTable("YW_Subscribe");

                entity.Property(e => e.IdSubscribe).HasColumnName("ID_Subscribe");

                entity.Property(e => e.HouseStageId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("HouseStageID")
                    .IsFixedLength(true);

                entity.Property(e => e.IdAgreementUser).HasColumnName("ID_AgreementUser");

                entity.Property(e => e.IdGuest).HasColumnName("ID_Guest");

                entity.Property(e => e.IdVip).HasColumnName("ID_VIP");

                entity.Property(e => e.MoneyPledge)
                    .HasColumnType("money")
                    .HasColumnName("Money_Pledge");

                entity.Property(e => e.NumberOpenStage)
                    .HasMaxLength(30)
                    .HasColumnName("Number_OpenStage")
                    .IsFixedLength(true);

                entity.Property(e => e.NumberPeople).HasColumnName("Number_People");

                entity.Property(e => e.NumberSubscribe)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Number_Subscribe")
                    .IsFixedLength(true);

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StateSecrecy).HasColumnName("State_Secrecy");

                entity.Property(e => e.TimeLeave)
                    .HasColumnType("datetime")
                    .HasColumnName("Time_Leave");

                entity.Property(e => e.TimePredict)
                    .HasColumnType("datetime")
                    .HasColumnName("Time_Predict");

                entity.Property(e => e.TypeCheckIn)
                    .HasMaxLength(10)
                    .HasColumnName("Type_CheckIn")
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
