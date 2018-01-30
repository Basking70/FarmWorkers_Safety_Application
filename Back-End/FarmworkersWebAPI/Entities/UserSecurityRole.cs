namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserSecurityRole")]
    public partial class UserSecurityRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserSecurityRole()
        {
            UserPermissions = new HashSet<UserPermission>();
        }

        [Key]
        public int UserRoleID { get; set; }

        [StringLength(100)]
        public string UserSecurityRoleName { get; set; }

        [Required]
        public string UserSecurityRoleDescription { get; set; }

        [StringLength(1)]
        public string IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
    }
}
