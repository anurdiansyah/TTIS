using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TTIS.API.Models
{
    public partial class TTISDbContext : DbContext
    {
        public TTISDbContext()
        {
        }

        public TTISDbContext(DbContextOptions<TTISDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MasCity> MasCity { get; set; }
        public virtual DbSet<MasCityGroup> MasCityGroup { get; set; }
        public virtual DbSet<MasCityGroupMember> MasCityGroupMember { get; set; }
        public virtual DbSet<MasClient> MasClient { get; set; }
        public virtual DbSet<MasCrewDetail> MasCrewDetail { get; set; }
        public virtual DbSet<MasCrewLicense> MasCrewLicense { get; set; }
        public virtual DbSet<MasCustomer> MasCustomer { get; set; }
        public virtual DbSet<MasCustomerBranch> MasCustomerBranch { get; set; }
        public virtual DbSet<MasCustomerContact> MasCustomerContact { get; set; }
        public virtual DbSet<MasDepartment> MasDepartment { get; set; }
        public virtual DbSet<MasDistrict> MasDistrict { get; set; }
        public virtual DbSet<MasDriver> MasDriver { get; set; }
        public virtual DbSet<MasDriverAssistant> MasDriverAssistant { get; set; }
        public virtual DbSet<MasEmployee> MasEmployee { get; set; }
        public virtual DbSet<MasEmployeeStatus> MasEmployeeStatus { get; set; }
        public virtual DbSet<MasFuel> MasFuel { get; set; }
        public virtual DbSet<MasProvince> MasProvince { get; set; }
        public virtual DbSet<MasRoleAccess> MasRoleAccess { get; set; }
        public virtual DbSet<MasRoleAccessLitem> MasRoleAccessLitem { get; set; }
        public virtual DbSet<MasSubDistrict> MasSubDistrict { get; set; }
        public virtual DbSet<MasTitle> MasTitle { get; set; }
        public virtual DbSet<MasUnit> MasUnit { get; set; }
        public virtual DbSet<MasUserDetail> MasUserDetail { get; set; }
        public virtual DbSet<MasUserDevice> MasUserDevice { get; set; }
        public virtual DbSet<MasUserRole> MasUserRole { get; set; }
        public virtual DbSet<MasVehicle> MasVehicle { get; set; }
        public virtual DbSet<MasVehicleModel> MasVehicleModel { get; set; }
        public virtual DbSet<MasVehicleStatus> MasVehicleStatus { get; set; }
        public virtual DbSet<MasVehicleType> MasVehicleType { get; set; }
        public virtual DbSet<MasVehicleUser> MasVehicleUser { get; set; }
        public virtual DbSet<SysApproval> SysApproval { get; set; }
        public virtual DbSet<SysApprovalStatus> SysApprovalStatus { get; set; }
        public virtual DbSet<SysCargoCategory> SysCargoCategory { get; set; }
        public virtual DbSet<SysCargoType> SysCargoType { get; set; }
        public virtual DbSet<SysDocumentPosition> SysDocumentPosition { get; set; }
        public virtual DbSet<SysExceptionLog> SysExceptionLog { get; set; }
        public virtual DbSet<SysLog> SysLog { get; set; }
        public virtual DbSet<SysModule> SysModule { get; set; }
        public virtual DbSet<SysModuleObject> SysModuleObject { get; set; }
        public virtual DbSet<SysModuleObjectMember> SysModuleObjectMember { get; set; }
        public virtual DbSet<SysParam> SysParam { get; set; }
        public virtual DbSet<SysParamGroup> SysParamGroup { get; set; }
        public virtual DbSet<SysProcess> SysProcess { get; set; }
        public virtual DbSet<SysProcessStatus> SysProcessStatus { get; set; }
        public virtual DbSet<SysUnit> SysUnit { get; set; }
        public virtual DbSet<SysUserLog> SysUserLog { get; set; }
        public virtual DbSet<SysVehicleUsageType> SysVehicleUsageType { get; set; }
        public virtual DbSet<SysWoStatus> SysWoStatus { get; set; }
        public virtual DbSet<TranCargo> TranCargo { get; set; }
        public virtual DbSet<TranDestination> TranDestination { get; set; }
        public virtual DbSet<TranOrigin> TranOrigin { get; set; }
        public virtual DbSet<TranUserLocation> TranUserLocation { get; set; }
        public virtual DbSet<TranWo> TranWo { get; set; }
        public virtual DbSet<TranWoHistory> TranWoHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MasCity>(entity =>
            {
                entity.HasKey(e => e.CityId);

                entity.ToTable("MAS_CITY");

                entity.Property(e => e.CityId).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ProvinceId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MasCityGroup>(entity =>
            {
                entity.HasKey(e => e.CityGroupId);

                entity.ToTable("MAS_CITY_GROUP");

                entity.Property(e => e.CityGroupId).ValueGeneratedNever();

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<MasCityGroupMember>(entity =>
            {
                entity.HasKey(e => e.CityGroupMemberId);

                entity.ToTable("MAS_CITY_GROUP_MEMBER");

                entity.Property(e => e.CityGroupMemberId).ValueGeneratedNever();
            });

            modelBuilder.Entity<MasCrewDetail>(entity =>
            {
                entity.HasKey(e => e.CrewDetailId);

                entity.ToTable("MAS_CREW_DETAIL");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.IdentityImage)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.TagNo)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MasCrewLicense>(entity =>
            {
                entity.HasKey(e => e.CrewLicenseId);

                entity.ToTable("MAS_CREW_LICENSE");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.LicenseImage)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.LicenseNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LicenseType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MasCustomer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.ToTable("MAS_CUSTOMER");

                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CityId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.DistrictId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ProvinceId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.SubDistrictId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<MasCustomerBranch>(entity =>
            {
                entity.HasKey(e => e.CustomerBranchId);

                entity.ToTable("MAS_CUSTOMER_BRANCH");

                entity.Property(e => e.CustomerBranchId).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BranchCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CityId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.DistrictId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ProvinceId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.SubDistrictId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.MasCustomerBranch)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MAS_CUSTOMER_BRANCH_MAS_CUSTOMER");
            });

            modelBuilder.Entity<MasCustomerContact>(entity =>
            {
                entity.HasKey(e => e.CustomerContactId);

                entity.ToTable("MAS_CUSTOMER_CONTACT");

                entity.Property(e => e.CustomerContactId).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.MasCustomerContact)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_MAS_CUSTOMER_CONTACT_MAS_CUSTOMER");
            });

            modelBuilder.Entity<MasDepartment>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);

                entity.ToTable("MAS_DEPARTMENT");

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<MasDistrict>(entity =>
            {
                entity.HasKey(e => e.DistrictId);

                entity.ToTable("MAS_DISTRICT");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MasDriver>(entity =>
            {
                entity.HasKey(e => e.DriverId);

                entity.ToTable("MAS_DRIVER");

                entity.Property(e => e.DriverId).ValueGeneratedNever();

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.LicenseExpiryDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.LicenseNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LicensePicture)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LicenseType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TagNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<MasDriverAssistant>(entity =>
            {
                entity.HasKey(e => e.DriverAssistantId);

                entity.ToTable("MAS_DRIVER_ASSISTANT");

                entity.Property(e => e.DriverAssistantId).ValueGeneratedNever();

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.TagNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<MasEmployee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.ToTable("MAS_EMPLOYEE");

                entity.Property(e => e.EmployeeId).ValueGeneratedNever();

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.DepartmentId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EmployeeStatusId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.IdentityAddress)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdentityCity)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.IdentityDistrict)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.IdentityNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdentityPicture)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdentityProvince)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.IdentitySubDistrict)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.JoinDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LivingAddress)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LivingCity)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.LivingDistrict)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.LivingProvince)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.LivingSubDistrict)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.MiddleName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NickName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PasPhoto)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.PlaceOfBirth)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ResignDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ResignReason)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TagNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<MasEmployeeStatus>(entity =>
            {
                entity.HasKey(e => e.EmployeeStatusId);

                entity.ToTable("MAS_EMPLOYEE_STATUS");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MasFuel>(entity =>
            {
                entity.HasKey(e => e.FuelId);

                entity.ToTable("MAS_FUEL");

                entity.Property(e => e.FuelId).ValueGeneratedNever();

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1028)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<MasProvince>(entity =>
            {
                entity.HasKey(e => e.ProvinceId);

                entity.ToTable("MAS_PROVINCE");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MasRoleAccess>(entity =>
            {
                entity.HasKey(e => e.RoleAccessId);

                entity.ToTable("MAS_ROLE_ACCESS");

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.RoleCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RoleDescription)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");
            });

            modelBuilder.Entity<MasRoleAccessLitem>(entity =>
            {
                entity.HasKey(e => e.RoleAccessLitemId);

                entity.ToTable("MAS_ROLE_ACCESS_LITEM");

                entity.HasOne(d => d.RoleAccess)
                    .WithMany(p => p.MasRoleAccessLitem)
                    .HasForeignKey(d => d.RoleAccessId)
                    .HasConstraintName("FK_MAS_ROLE_ACCESS_LITEM_MAS_ROLE_ACCESS");
            });

            modelBuilder.Entity<MasSubDistrict>(entity =>
            {
                entity.HasKey(e => e.SubDistrictId);

                entity.ToTable("MAS_SUB_DISTRICT");

                entity.Property(e => e.CityId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.DistrictId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProvinceId).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<MasTitle>(entity =>
            {
                entity.HasKey(e => e.TitleId);

                entity.ToTable("MAS_TITLE");

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.DepartmentId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OrganizationLevel).HasDefaultValueSql("((-99))");

                entity.Property(e => e.UnitId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<MasUnit>(entity =>
            {
                entity.HasKey(e => e.UnitId);

                entity.ToTable("MAS_UNIT");

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.DepartmentId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<MasUserDetail>(entity =>
            {
                entity.HasKey(e => e.UserDetailId);

                entity.ToTable("MAS_USER_DETAIL");

                entity.Property(e => e.AspNetUserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.TagNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MasUserDevice>(entity =>
            {
                entity.HasKey(e => e.UserDeviceId);

                entity.ToTable("MAS_USER_DEVICE");

                entity.Property(e => e.UserDeviceId).ValueGeneratedNever();

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Imei)
                    .IsRequired()
                    .HasColumnName("IMEI")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MasUserRole>(entity =>
            {
                entity.HasKey(e => e.UserRoleId);

                entity.ToTable("MAS_USER_ROLE");

                entity.Property(e => e.AspNetUserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MasVehicle>(entity =>
            {
                entity.HasKey(e => e.VehicleId);

                entity.ToTable("MAS_VEHICLE");

                entity.Property(e => e.VehicleId).ValueGeneratedNever();

                entity.Property(e => e.Bpkbimage)
                    .IsRequired()
                    .HasColumnName("BPKBImage")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BpkbpositionId)
                    .HasColumnName("BPKBPositionId")
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.BpkbpositionReffId)
                    .IsRequired()
                    .HasColumnName("BPKBPositionReffId")
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.FuelId).HasDefaultValueSql("('')");

                entity.Property(e => e.KirberlakuHingga)
                    .HasColumnName("KIRBerlakuHingga")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Kirimage)
                    .IsRequired()
                    .HasColumnName("KIRImage")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.KirpositionId)
                    .HasColumnName("KIRPositionId")
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.KirpositionReffId)
                    .IsRequired()
                    .HasColumnName("KIRPositionReffId")
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Merk)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NoMesin)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NoRangka)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NomorRegistrasi)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.Plate)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StnkberlakuHingga)
                    .HasColumnName("STNKBerlakuHingga")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Stnkimage)
                    .IsRequired()
                    .HasColumnName("STNKImage")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.StnkpositionId)
                    .HasColumnName("STNKPositionId")
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.StnkpositionReffId)
                    .IsRequired()
                    .HasColumnName("STNKPositionReffId")
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TahunPerakitan).HasDefaultValueSql("((-99))");

                entity.Property(e => e.TahunRegistrasi).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Tipe)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.VehicleCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.VehicleImage)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.VehicleStatusId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Warna)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MasVehicleModel>(entity =>
            {
                entity.HasKey(e => e.VehicleModelId);

                entity.ToTable("MAS_VEHICLE_MODEL");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MasVehicleStatus>(entity =>
            {
                entity.HasKey(e => e.VehicleStatusId);

                entity.ToTable("MAS_VEHICLE_STATUS");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MasVehicleType>(entity =>
            {
                entity.HasKey(e => e.VehicleTypeId);

                entity.ToTable("MAS_VEHICLE_TYPE");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MasVehicleUser>(entity =>
            {
                entity.HasKey(e => e.VehicleUserId);

                entity.ToTable("MAS_VEHICLE_USER");

                entity.Property(e => e.VehicleUserId).ValueGeneratedNever();

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.DateFrom).HasColumnType("datetime");

                entity.Property(e => e.DateTo).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((-99))");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.VehicleUsageTypeId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<SysApproval>(entity =>
            {
                entity.HasKey(e => e.ApprovalId);

                entity.ToTable("SYS_APPROVAL");

                entity.Property(e => e.ApprovalId).ValueGeneratedNever();

                entity.Property(e => e.ApprovalCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ApprovalStatusId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Detail)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ModuleObjectId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.ModuleObjectMemberId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.PreviousDetail)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ReffId)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ReffObj)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<SysApprovalStatus>(entity =>
            {
                entity.HasKey(e => e.ApprovalStatusId);

                entity.ToTable("SYS_APPROVAL_STATUS");

                entity.Property(e => e.StatusDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysCargoCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.ToTable("SYS_CARGO_CATEGORY");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysCargoType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.ToTable("SYS_CARGO_TYPE");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysDocumentPosition>(entity =>
            {
                entity.HasKey(e => e.DocumentPositionId);

                entity.ToTable("SYS_DOCUMENT_POSITION");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysExceptionLog>(entity =>
            {
                entity.HasKey(e => e.ExceptionLogId);

                entity.ToTable("SYS_EXCEPTION_LOG");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LogDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.ReferenceNumber)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<SysLog>(entity =>
            {
                entity.HasKey(e => e.SystemLogId);

                entity.ToTable("SYS_LOG");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Detail)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LogDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ProcessId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.ReferenceNumber)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<SysModule>(entity =>
            {
                entity.HasKey(e => e.ModuleId);

                entity.ToTable("SYS_MODULE");

                entity.Property(e => e.ModuleId).ValueGeneratedNever();

                entity.Property(e => e.AdditionalStyle)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icon)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ModuleCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModuleDescription)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModuleName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysModuleObject>(entity =>
            {
                entity.HasKey(e => e.ModuleObjectId);

                entity.ToTable("SYS_MODULE_OBJECT");

                entity.Property(e => e.ModuleObjectId).ValueGeneratedNever();

                entity.Property(e => e.AdditionalStyle)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DefaultUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Icon)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ObjectCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectDescription)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.SysModuleObject)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("FK_MAS_MODULE_OBJECT_MAS_MODULE");
            });

            modelBuilder.Entity<SysModuleObjectMember>(entity =>
            {
                entity.HasKey(e => e.ModuleObjectMemberId);

                entity.ToTable("SYS_MODULE_OBJECT_MEMBER");

                entity.Property(e => e.ModuleObjectMemberId).ValueGeneratedNever();

                entity.Property(e => e.MemberCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MemberDescription)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MemberName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ModuleId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.ModuleObjectId).HasDefaultValueSql("((-99))");

                entity.HasOne(d => d.ModuleObject)
                    .WithMany(p => p.SysModuleObjectMember)
                    .HasForeignKey(d => d.ModuleObjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MAS_MODULE_OBJECT_MEMBER_MAS_MODULE_OBJECT");
            });

            modelBuilder.Entity<SysParam>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("SYS_PARAM");

                entity.Property(e => e.Code)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IndexOrder).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SysParamGroupId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<SysParamGroup>(entity =>
            {
                entity.ToTable("SYS_PARAM_GROUP");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IndexOrder).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<SysProcess>(entity =>
            {
                entity.HasKey(e => e.ProcessId);

                entity.ToTable("SYS_PROCESS");

                entity.Property(e => e.ProcessDescription)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ProcessName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<SysProcessStatus>(entity =>
            {
                entity.HasKey(e => e.ProcessStatusId);

                entity.ToTable("SYS_PROCESS_STATUS");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ProcessId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.ProcessStatusCode)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ProcessStatusName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<SysUnit>(entity =>
            {
                entity.HasKey(e => e.UnitId);

                entity.ToTable("SYS_UNIT");

                entity.Property(e => e.UnitId).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysUserLog>(entity =>
            {
                entity.HasKey(e => e.UserLogId);

                entity.ToTable("SYS_USER_LOG");

                entity.Property(e => e.AspNetUserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Detail)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IpAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LogDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModuleObjectMemberId).HasDefaultValueSql("((-99))");

                entity.Property(e => e.PreviousDetail)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RefId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RefObject)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ReferenceNumber)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<SysVehicleUsageType>(entity =>
            {
                entity.HasKey(e => e.VehicleUsageTypeId);

                entity.ToTable("SYS_VEHICLE_USAGE_TYPE");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysWoStatus>(entity =>
            {
                entity.HasKey(e => e.WostatusId);

                entity.ToTable("SYS_WO_STATUS");

                entity.Property(e => e.WostatusId).HasColumnName("WOStatusId");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TranCargo>(entity =>
            {
                entity.HasKey(e => e.CargoId);

                entity.ToTable("TRAN_CARGO");

                entity.Property(e => e.CargoId).ValueGeneratedNever();

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Dimension)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<TranDestination>(entity =>
            {
                entity.HasKey(e => e.DestinationId);

                entity.ToTable("TRAN_DESTINATION");

                entity.Property(e => e.DestinationId).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Lat)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Lng)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Pic)
                    .IsRequired()
                    .HasColumnName("PIC")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Picnumber)
                    .IsRequired()
                    .HasColumnName("PICNumber")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<TranOrigin>(entity =>
            {
                entity.HasKey(e => e.OriginId);

                entity.ToTable("TRAN_ORIGIN");

                entity.Property(e => e.OriginId).ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Lat)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Lng)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Pic)
                    .IsRequired()
                    .HasColumnName("PIC")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Picnumber)
                    .IsRequired()
                    .HasColumnName("PICNumber")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<TranUserLocation>(entity =>
            {
                entity.HasKey(e => e.UserLocationId);

                entity.ToTable("TRAN_USER_LOCATION");

                entity.Property(e => e.UserLocationId).ValueGeneratedNever();

                entity.Property(e => e.AspNetUserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Bearing)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Latitude)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TranWo>(entity =>
            {
                entity.HasKey(e => e.WorkOrderId);

                entity.ToTable("TRAN_WO");

                entity.Property(e => e.WorkOrderId).ValueGeneratedNever();

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.UpdateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Version).HasDefaultValueSql("((-99))");

                entity.Property(e => e.Won)
                    .IsRequired()
                    .HasColumnName("WON")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.WostatusId)
                    .HasColumnName("WOStatusId")
                    .HasDefaultValueSql("((-99))");
            });

            modelBuilder.Entity<TranWoHistory>(entity =>
            {
                entity.HasKey(e => e.WohistoryId);

                entity.ToTable("TRAN_WO_HISTORY");

                entity.Property(e => e.WohistoryId)
                    .HasColumnName("WOHistoryID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateByUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.WoactionId).HasColumnName("WOActionId");
            });
        }
    }
}
