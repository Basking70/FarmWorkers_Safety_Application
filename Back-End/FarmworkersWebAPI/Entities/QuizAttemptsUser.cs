namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuizAttemptsUser")]
    public partial class QuizAttemptsUser
    {
        [Key]
        [Column(Order = 0)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuizID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuizVersion { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuizScore { get; set; }

        [Key]
        [Column(Order = 5)]
        public string UserAnswers { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IsFinished { get; set; }

        [Key]
        [Column(Order = 7)]
        public DateTime DateTaken { get; set; }

        public virtual Quiz Quiz { get; set; }

        public virtual User User { get; set; }
    }
}
