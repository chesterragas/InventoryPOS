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
    
    public partial class M_Item
    {
        public long ID { get; set; }
        public Nullable<long> ItemCategory { get; set; }
        public string ItemName { get; set; }
        public string Photo { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
        public Nullable<long> CurrentStock { get; set; }
        public bool IsDeleted { get; set; }
        public string CreateID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateID { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
