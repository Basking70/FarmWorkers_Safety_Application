using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FarmworkersWebAPI.Models
{
    public class UserFarm
    {
        public int UserID { get; set; }
        public int FarmID { get; set; }
        public DateTime UpdateDate { get; set; }
        public int IsLatest { get; set; }


    }
}