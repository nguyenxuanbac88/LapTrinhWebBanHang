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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.AddressUsers = new HashSet<AddressUser>();
        }
    
        public int IdUser { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Nullable<int> IsAdmin { get; set; }
        public Nullable<bool> IsEmailVerified { get; set; }
        public string TokenPassword { get; set; }
        public Nullable<int> Stt { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public string Ip { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AddressUser> AddressUsers { get; set; }
    }
}
