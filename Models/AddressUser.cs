//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LapTrinhWebBanHang.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AddressUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AddressUser()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public int IdAddress { get; set; }
        public Nullable<int> IdUser { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public string Town { get; set; }
        public string Block { get; set; }
        public string SpecificAddress { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
