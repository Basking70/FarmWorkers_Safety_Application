﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmworkersWebAPI.ViewModels
{
    public class FarmDTO
    {
        public int FarmID { get; set; }
        public string FarmName { get; set; }
        public string FarmHouseNumberStreetAddress { get; set; }
        public string FarmCity { get; set; }
        public string FarmState { get; set; }
        public string FarmCountry { get; set; }
        public int FarmZipCode { get; set; }
        public string FarmLatitute { get; set; }
        public string FarmLongitude { get; set; }
        public string FarmTemperatureMin { get; set; }
        public string FarmTemperatureMax { get; set; }
        public string IsActive { get; set; }
        public int NumberOfFarmWorkers { get; set; }
        public IList<UserDTO> FarmOwners { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<UserFarmDTO> UserFarms { get; set; }

        ////[Computed]
        //public int NumberOfFarmworkers
        //{
        //    get
        //    {
        //        return UserFarms.Where(x => x.IsLatest == 1 && x.User.UserType == "Farm Worker").Count();
        //    }
        //}
    }
}