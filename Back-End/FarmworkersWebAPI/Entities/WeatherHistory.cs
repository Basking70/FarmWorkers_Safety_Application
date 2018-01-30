namespace FarmworkersWebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WeatherHistory")]
    public partial class WeatherHistory
    {
        public int WeatherHistoryID { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string Temperature { get; set; }

        [Required]
        public string Humidity { get; set; }

        [Required]
        public string SkyConditions { get; set; }

        public int WeatherLocation { get; set; }

        public virtual Farm Farm { get; set; }
    }
}
