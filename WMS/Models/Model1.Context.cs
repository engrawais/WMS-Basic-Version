﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WMS.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TAS2013Entities : DbContext
    {
        public TAS2013Entities()
            : base("name=TAS2013Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AttData> AttDatas { get; set; }
        public DbSet<AttDataManEdit> AttDataManEdits { get; set; }
        public DbSet<AttMnData> AttMnDatas { get; set; }
        public DbSet<AttMnDataPer> AttMnDataPers { get; set; }
        public DbSet<AttProcess> AttProcesses { get; set; }
        public DbSet<AttProcessorScheduler> AttProcessorSchedulers { get; set; }
        public DbSet<AuditForm> AuditForms { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AuditOperation> AuditOperations { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<DailySummary> DailySummaries { get; set; }
        public DbSet<DaysName> DaysNames { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<DownloadTime> DownloadTimes { get; set; }
        public DbSet<DutyCode> DutyCodes { get; set; }
        public DbSet<DutyTime> DutyTimes { get; set; }
        public DbSet<Emp> Emps { get; set; }
        public DbSet<EmpFace> EmpFaces { get; set; }
        public DbSet<EmpFp> EmpFps { get; set; }
        public DbSet<EmpPhoto> EmpPhotoes { get; set; }
        public DbSet<EmpType> EmpTypes { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<JobCard> JobCards { get; set; }
        public DbSet<JobCardApp> JobCardApps { get; set; }
        public DbSet<JobCardDetail> JobCardDetails { get; set; }
        public DbSet<LicenseDeviceInfo> LicenseDeviceInfoes { get; set; }
        public DbSet<LicenseInfo> LicenseInfoes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LvApplication> LvApplications { get; set; }
        public DbSet<LvConsumed> LvConsumeds { get; set; }
        public DbSet<LvData> LvDatas { get; set; }
        public DbSet<LvShort> LvShorts { get; set; }
        public DbSet<LvType> LvTypes { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<PollData> PollDatas { get; set; }
        public DbSet<PollDataError> PollDataErrors { get; set; }
        public DbSet<RdrDutyCode> RdrDutyCodes { get; set; }
        public DbSet<RdrEventLog> RdrEventLogs { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<ReaderType> ReaderTypes { get; set; }
        public DbSet<Reason> Reasons { get; set; }
        public DbSet<Remark> Remarks { get; set; }
        public DbSet<RosterApp> RosterApps { get; set; }
        public DbSet<RosterDetail> RosterDetails { get; set; }
        public DbSet<RosterType> RosterTypes { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<ServiceLog> ServiceLogs { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserModule> UserModules { get; set; }
        public DbSet<UserSection> UserSections { get; set; }
        public DbSet<EmpView> EmpViews { get; set; }
        public DbSet<ViewAttData> ViewAttDatas { get; set; }
        public DbSet<ViewDetailAttData> ViewDetailAttDatas { get; set; }
        public DbSet<ViewEmpPic> ViewEmpPics { get; set; }
        public DbSet<ViewJobCardApp> ViewJobCardApps { get; set; }
        public DbSet<ViewLvApplication> ViewLvApplications { get; set; }
        public DbSet<ViewMonthlyData> ViewMonthlyDatas { get; set; }
        public DbSet<ViewMultipleInOut> ViewMultipleInOuts { get; set; }
        public DbSet<ViewSection> ViewSections { get; set; }
    }
}
