namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Quiz")]
    public partial class Quiz
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Quiz()
        {
            QuizAttemptsUsers = new HashSet<QuizAttemptsUser>();
            QuizQuestions = new HashSet<QuizQuestion>();
            EducationalContents = new HashSet<EducationalContent>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuizID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuizVersion { get; set; }

        [Required]
        public string QuizName { get; set; }

        [Required]
        public string QuizDescription { get; set; }

        [Required]
        public string Scale { get; set; }

        [StringLength(1)]
        public string IsActive { get; set; }

        public DateTime DateModified { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuizAttemptsUser> QuizAttemptsUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EducationalContent> EducationalContents { get; set; }
    }
}
