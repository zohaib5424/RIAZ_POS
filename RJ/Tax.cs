//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RJ
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tax
    {
        public string id { get; set; }
        public string Tax_Name { get; set; }
        public Nullable<decimal> Tax_Percent { get; set; }
        public string AddedBy_UserId { get; set; }
        public Nullable<System.DateTime> C_Date { get; set; }
        public Nullable<System.TimeSpan> C_Time { get; set; }
        public string Status { get; set; }
    }
}
