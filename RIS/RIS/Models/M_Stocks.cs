//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class M_Stocks
    {
        public long ID { get; set; }
        public Nullable<long> ItemID { get; set; }
        public string StockType { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<long> StockQty { get; set; }
        public string Remarks { get; set; }
        public string UpdateID { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}