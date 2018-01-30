namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserPermission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserPermission()
        {
            UserSecurityRoles = new HashSet<UserSecurityRole>();
        }

        [Key]
        public int PermissionID { get; set; }

        [Required]
        public string ModuleName { get; set; }

        [Required]
        public string Permission { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSecurityRole> UserSecurityRoles { get; set; }
    }
}
