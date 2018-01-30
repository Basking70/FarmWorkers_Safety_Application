namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserCommunicationPreference")]
    public partial class UserCommunicationPreference
    {
        [Key]
        public int IDUserCommunicationPreference { get; set; }

        [Required]
        public string UserCommunicationPreferenceFrequency { get; set; }

        [Required]
        public string UserCommunicationPreferenceNotificationType { get; set; }

        [Required]
        public string IDUserCommunicationPreferenceLanguage { get; set; }

        public int UserID { get; set; }

        public virtual User User { get; set; }
    }
}
