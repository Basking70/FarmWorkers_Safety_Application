using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmworkersWebAPI.ViewModels
{
    public class UserFarmDTO
    {
        public int UserInFarmID { get; set; }
        public int FarmID { get; set; }
        public int UserID { get; set; }
        public int? IsLatest { get; set; }
        public FarmInfoDTO Farm { get; set; }
    }
}