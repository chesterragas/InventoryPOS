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
    
    public partial class GET_POS_ItemServiceList_Result
    {
        public Nullable<long> Rownum { get; set; }
        public long ID { get; set; }
        public string Photo { get; set; }
        public Nullable<long> ItemCategory { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
        public long CurrentStock { get; set; }
        public string Type { get; set; }
    }
}
