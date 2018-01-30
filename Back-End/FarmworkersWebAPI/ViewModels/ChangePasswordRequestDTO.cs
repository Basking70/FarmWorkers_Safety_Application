using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmworkersWebAPI.ViewModels
{
    public class ChangePasswordRequestDTO
    {
        public string UserLoginID { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}