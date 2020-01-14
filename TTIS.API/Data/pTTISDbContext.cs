using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TTIS.API.Models;

namespace TTIS.API.Models
{
    public partial class TTISDbContext : DbContext
    {
        public DbQuery<MasDriverWithDetail> MasDriverWithDetail { get; set; }

        public DbSet<MyModule> MyModule { get; set; }
        public DbSet<Crews> VCrewDetail { get; set; }
        public DbSet<VehicleDetail> VehicleDetail { get; set; }
        public DbSet<VehicleUserDetail> VehicleUserDetail { get; set; }
        public DbSet<VehicleUserHistory> VehicleUserHistory { get; set; }
        public DbSet<EmployeeDetail> EmployeeDetail { get; set; }
        public DbSet<MasUserDeviceDetail> MasUserDeviceDetail { get; set; }
        public DbSet<UserLatestGeoLoc> UserLatestGeoLoc { get; set; }
        public DbSet<SysUserLogDetail> VSysUserLogDetail { get; set; }
        public DbSet<SysApprovalDetail> SysApprovalDetail { get; set; }
    }
}
