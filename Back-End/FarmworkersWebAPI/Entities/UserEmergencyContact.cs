namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserEmergencyContact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserEmergencyContactsID { get; set; }

        public int UserID { get; set; }

        public string UserEmergencyContactsName { get; set; }

        [Required]
        public string UserEmergencyContactsPhoneNumber { get; set; }

        public string UserEmergencyContactsAddress { get; set; }

        public virtual User User { get; set; }
    }
}
