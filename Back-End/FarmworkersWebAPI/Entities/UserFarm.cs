namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserFarm")]
    public partial class UserFarm
    {
        [Key]
        public int UserInFarmID { get; set; }

        public int UserID { get; set; }

        public int FarmID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UpdateDate { get; set; }

        public int? IsLatest { get; set; }

        public virtual Farm Farm { get; set; }

        public virtual User User { get; set; }
    }
}
