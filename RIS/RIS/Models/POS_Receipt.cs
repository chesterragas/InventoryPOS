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
    
    public partial class POS_Receipt
    {
        public long ID { get; set; }
        public string ReceiptRefNo { get; set; }
        public Nullable<long> ItemID { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<int> RetailPrice { get; set; }
        public string Type { get; set; }
        public string CreateID { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
