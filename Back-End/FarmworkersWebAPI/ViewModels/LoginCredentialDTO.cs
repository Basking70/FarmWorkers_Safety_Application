namespace FarmworkersWebAPI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class LoginCredentialDTO
    {
        public string UserLoginID { get; set; }
        public string IsActive { get; set; }
        public virtual UserWithFarmDTO User { get; set; }
    }
}
