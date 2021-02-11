﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RIS.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class RISDBEntities : DbContext
    {
        public RISDBEntities()
            : base("name=RISDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<M_Employee> M_Employee { get; set; }
        public DbSet<M_Item> M_Item { get; set; }
        public DbSet<M_ItemCategory> M_ItemCategory { get; set; }
        public DbSet<M_Service> M_Service { get; set; }
        public DbSet<M_ServiceCategory> M_ServiceCategory { get; set; }
        public DbSet<M_Stocks> M_Stocks { get; set; }
        public DbSet<M_Users> M_Users { get; set; }
        public DbSet<POS_Receipt> POS_Receipt { get; set; }
        public DbSet<T_EmployeeLogs> T_EmployeeLogs { get; set; }
    
        public virtual ObjectResult<GET_M_ServiceList_Result> GET_M_ServiceList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_M_ServiceList_Result>("GET_M_ServiceList");
        }
    
        public virtual ObjectResult<GET_T_InboundOutboundHistory_Result> GET_T_InboundOutboundHistory(Nullable<long> category, string type)
        {
            var categoryParameter = category.HasValue ?
                new ObjectParameter("Category", category) :
                new ObjectParameter("Category", typeof(long));
    
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_T_InboundOutboundHistory_Result>("GET_T_InboundOutboundHistory", categoryParameter, typeParameter);
        }
    
        public virtual ObjectResult<GET_M_ItemList_Result> GET_M_ItemList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_M_ItemList_Result>("GET_M_ItemList");
        }
    
        public virtual ObjectResult<GET_POS_ItemServiceList_Result> GET_POS_ItemServiceList(Nullable<int> pageCount, Nullable<int> rowCount)
        {
            var pageCountParameter = pageCount.HasValue ?
                new ObjectParameter("PageCount", pageCount) :
                new ObjectParameter("PageCount", typeof(int));
    
            var rowCountParameter = rowCount.HasValue ?
                new ObjectParameter("RowCount", rowCount) :
                new ObjectParameter("RowCount", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_POS_ItemServiceList_Result>("GET_POS_ItemServiceList", pageCountParameter, rowCountParameter);
        }
    
        public virtual ObjectResult<GET_POS_ReceiptList_Result> GET_POS_ReceiptList(Nullable<System.DateTime> dateFrom, Nullable<System.DateTime> dateTo)
        {
            var dateFromParameter = dateFrom.HasValue ?
                new ObjectParameter("DateFrom", dateFrom) :
                new ObjectParameter("DateFrom", typeof(System.DateTime));
    
            var dateToParameter = dateTo.HasValue ?
                new ObjectParameter("DateTo", dateTo) :
                new ObjectParameter("DateTo", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_POS_ReceiptList_Result>("GET_POS_ReceiptList", dateFromParameter, dateToParameter);
        }
    
        public virtual ObjectResult<GET_M_EmployeeList_Result> GET_M_EmployeeList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_M_EmployeeList_Result>("GET_M_EmployeeList");
        }
    
        public virtual ObjectResult<GET_R_DailyReports_Result> GET_R_DailyReports()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_R_DailyReports_Result>("GET_R_DailyReports");
        }
    
        public virtual ObjectResult<GET_POS_ItemServiceList_details_Result> GET_POS_ItemServiceList_details(string receiptNo, ObjectParameter total)
        {
            var receiptNoParameter = receiptNo != null ?
                new ObjectParameter("ReceiptNo", receiptNo) :
                new ObjectParameter("ReceiptNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_POS_ItemServiceList_details_Result>("GET_POS_ItemServiceList_details", receiptNoParameter, total);
        }
    
        public virtual ObjectResult<string> GETTIME()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GETTIME");
        }
    
        public virtual int Delete_EmployeeLogs(string date)
        {
            var dateParameter = date != null ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Delete_EmployeeLogs", dateParameter);
        }
    
        public virtual ObjectResult<GET_M_EmployeeAttendanceList_Result> GET_M_EmployeeAttendanceList(string date)
        {
            var dateParameter = date != null ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_M_EmployeeAttendanceList_Result>("GET_M_EmployeeAttendanceList", dateParameter);
        }
    }
}
