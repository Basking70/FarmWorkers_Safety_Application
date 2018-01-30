namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserEducationalContent")]
    public partial class UserEducationalContent
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EducationalContentID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [StringLength(1)]
        public string UserEducationalContentClicked { get; set; }

        public string UserEducationalContentDateCreated { get; set; }

        public virtual EducationalContent EducationalContent { get; set; }

        public virtual User User { get; set; }
    }
}
