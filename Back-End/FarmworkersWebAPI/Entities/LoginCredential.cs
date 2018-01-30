namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LoginCredential
    {
        [Key]
        [StringLength(255)]
        public string UserLoginID { get; set; }

        [Required]
        public string Password { get; set; }

        public int? UserID { get; set; }

        public int? UserRoleID { get; set; }

        [StringLength(1)]
        public string IsActive { get; set; }

        public virtual User User { get; set; }

        public virtual UserSecurityRole UserSecurityRole { get; set; }
    }
}
