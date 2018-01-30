namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EducationalContent")]
    public partial class EducationalContent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EducationalContent()
        {
            UserEducationalContents = new HashSet<UserEducationalContent>();
            Quizs = new HashSet<Quiz>();
        }

        [Key]
        public int IDEducationalContent { get; set; }

        [Required]
        public string LinkEducationalContent { get; set; }

        [Required]
        public string TypeEducationalContent { get; set; }

        [Required]
        public string DescriptionEducationalContent { get; set; }

        public DateTime DateEducationalContentCreated { get; set; }

        [StringLength(1)]
        public string IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserEducationalContent> UserEducationalContents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quiz> Quizs { get; set; }
    }
}
