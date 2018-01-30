using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarmworkersWebAPI.Entities;

namespace FarmworkersWebAPI.ViewModels
{
    public class LoginCredentialsRegisterForm
    {
        public LoginCredential LoginCredential { get; set; }
        public User User { get; set; }
        public string RegisterType { get; set; } // UserEmail, UserPhoneNumber
        public string UserType { get; set; }
    }

    public class LoginCredentialLoginForm
    {
        public string _loginType { get; set; }
        public string _username { get; set; }
        public string _password { get; set; }
    }
}