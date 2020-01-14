using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TTIS.WebUi.Models;

namespace TTIS.WebUi.Data
{
    public partial class TtisDbContext : DbContext
    {
        public TtisDbContext()
        {
        }

        public TtisDbContext(DbContextOptions<TtisDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MyModule> MyModule { get; set; }
        public virtual DbSet<MasRoleAccess> MasRoleAccess { get; set; }
        public virtual DbSet<SysParam> SysParam { get; set; }
        public virtual DbSet<SysParamGroup> SysParamGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MasRoleAccess>(entity =>
            {
                entity.HasKey(e => e.RoleAccessId);

                entity.ToTable("MAS_ROLE_ACCESS");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

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

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
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
        }
    }
}
