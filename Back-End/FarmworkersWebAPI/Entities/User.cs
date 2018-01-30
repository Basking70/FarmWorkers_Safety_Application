namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            UserCommunicationPreferences = new HashSet<UserCommunicationPreference>();
            UserFarms = new HashSet<UserFarm>();
            QuizAttemptsUsers = new HashSet<QuizAttemptsUser>();
            UserEducationalContents = new HashSet<UserEducationalContent>();
            UserEmergencyContacts = new HashSet<UserEmergencyContact>();
        }

        public int UserID { get; set; }

        [Required]
        public string UserType { get; set; }

        public string UserName { get; set; }

        public string UserLastName { get; set; }

        public string UserDateOfBirth { get; set; }

        public string UserHeight { get; set; }

        public string UserWeight { get; set; }

        public string UserGender { get; set; }

        public string UserEmail { get; set; }

        public string UserPhoneNumber { get; set; }

        public string UserWorkLocation { get; set; }

        public string UserMemberSince { get; set; }

        [StringLength(1)]
        public string IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserCommunicationPreference> UserCommunicationPreferences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFarm> UserFarms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuizAttemptsUser> QuizAttemptsUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserEducationalContent> UserEducationalContents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserEmergencyContact> UserEmergencyContacts { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Farm CurrentFarm => UserFarms.Where(x => x.IsLatest == 1).Select(x=>x.Farm).FirstOrDefault();
    }
}
