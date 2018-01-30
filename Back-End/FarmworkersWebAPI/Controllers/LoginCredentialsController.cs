using FarmworkersWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Transactions;
using FarmworkersWebAPI.Entities;
using FarmworkersWebAPI.ViewModels;
using System.Data.Entity.Infrastructure;
using FarmworkersWebAPI.Enums;

namespace FarmworkersWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginCredentialsController : ApiController
    {
        //***Entity Framework**//
        //NOTE: Watch out for conflicts, between: 
        //**FarmworkersWebAPI.Entities
        //**FarmworkersWebAPI.Models

        FarmWorkerAppContext _context;
        public LoginCredentialsController()
        {
            _context = new FarmWorkerAppContext();
            _context.Configuration.LazyLoadingEnabled = false;
            //_context.Configuration.ProxyCreationEnabled = false;
        }

        [HttpPost]
        public IHttpActionResult RegisterLoginCredentials(LoginCredentialsRegisterForm _loginFormData)
        {
            LoginCredential loginCredential = null;
            try
            {
                var loginCredentialData = _loginFormData.LoginCredential;
                var userData = _loginFormData.User;
                userData.UserType = _loginFormData.UserType;
                userData.IsActive = "1";

                if (_loginFormData.RegisterType == "UserPhoneNumber")
                {

                    if (_context.Users.Any(x => x.UserPhoneNumber == loginCredentialData.UserLoginID))
                    {
                        return Content(HttpStatusCode.Unauthorized, new
                        {
                            code = ErrorCode.USER_ALREADY_REGISTER
                        });
                    }
                    userData.UserPhoneNumber = loginCredentialData.UserLoginID;
                }
                if (_loginFormData.RegisterType == "UserEmail")
                    userData.UserEmail = loginCredentialData.UserLoginID;

                var createdUser = _context.Users.Add(userData);

                loginCredentialData.UserID = createdUser.UserID;
                loginCredentialData.UserSecurityRole = _context.UserSecurityRoles.Find(loginCredentialData.UserRoleID);
                loginCredential = _context.LoginCredentials.Add(loginCredentialData);

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    code = ErrorCode.OTHER,
                    exception = ex
                });
            }

            return Ok(AutoMapper.Mapper.Map<LoginCredentialDTO>(loginCredential));
        }

        [HttpPost]
        public IHttpActionResult LoginWithLoginCredentials([FromBody] LoginCredentialLoginForm request)
        {

            var _loginType = request._loginType;
            string _password = request._password;
            string _username = request._username;

            LoginCredential LoginCredential = null;
            var _contextIncluded = _context.LoginCredentials
                .Include("User")
                .Include("User.UserCommunicationPreferences")
                .Include("User.UserFarms")
                .Include("User.UserFarms.Farm");

            if (_loginType == "Email")
            {
                LoginCredential = _contextIncluded.
                                 Where(x => x.Password.Equals(_password) &&
                                       x.User.UserEmail == _username).FirstOrDefault();
            }
            else if (_loginType == "PhoneNumber")
            {
                LoginCredential = _contextIncluded.
                                  Where(x => x.Password.Equals(_password) &&
                                        x.User.UserPhoneNumber == _username).FirstOrDefault();
            }

            if (LoginCredential == null)
                return Content(HttpStatusCode.Unauthorized, new
                {
                    code = ErrorCode.INVALID_CREDENTIALS
                });
            return Ok(AutoMapper.Mapper.Map<LoginCredentialDTO>(LoginCredential));

        }

       
        [HttpPost]
        public IHttpActionResult ChangePassword([FromBody] ChangePasswordRequestDTO request)
        {
            var LoginCredential = _context.LoginCredentials.Where(x => x.UserLoginID == request.UserLoginID && x.Password == request.OldPassword).FirstOrDefault();
            if(LoginCredential == null)
            {
                return Content(HttpStatusCode.Unauthorized, new
                {
                    code = ErrorCode.INVALID_OLD_PASSWORD
                });
            }
            LoginCredential.Password = request.Password;
            _context.Entry(LoginCredential).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }
        [HttpPost]
        public IHttpActionResult ForgetPassword([FromBody] LoginCredentialLoginForm request)
        {
            var _loginType = request._loginType;
            string _username = request._username;

            LoginCredential LoginCredential = null;

            if (_loginType == "Email")
            {
                LoginCredential = _context.LoginCredentials.
                                 Where(x => x.User.UserEmail == _username).FirstOrDefault();
                if (LoginCredential == null)
                {
                    return Content(HttpStatusCode.Unauthorized, new
                    {
                        code = ErrorCode.INVALID_EMAIL_FOR_FORGET_PASSWORD
                    });
                }
            }
            else if (_loginType == "PhoneNumber")
            {
                LoginCredential = _context.LoginCredentials.
                                  Where(x => x.User.UserPhoneNumber == _username).FirstOrDefault();
                if (LoginCredential == null)
                {
                    return Content(HttpStatusCode.Unauthorized, new
                    {
                        code = ErrorCode.INVALID_PHONE_NUMBER_FOR_FORGET_PASSWORD
                    });
                }
                LoginCredential.Password = RandomString(8);
                _context.Entry(LoginCredential).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                NotificationsController NotiCtrl = new NotificationsController();
                NotiCtrl.sendTextMessage(LoginCredential.User.UserPhoneNumber, "Your new password is " + LoginCredential.Password);
                return Ok("SMS Sent to " + LoginCredential.User.UserPhoneNumber);
            }
            throw new NotSupportedException();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        //*********************//

        [HttpPost]
        public string InsertLoginCredentialsController(LoginCredentials _loginData)
        {
            LoginCredentials _loginCredInstance = new LoginCredentials();

            string _insertResult = _loginCredInstance.InsertLoginCredentialsModel(_loginData);

            return _insertResult;
        }

        [HttpPost]
        public string UpdateLoginCredentialsController(LoginCredentials _loginData)
        {
            LoginCredentials _loginCredInstance = new LoginCredentials();

            string _updateResult = _loginCredInstance.UpdateLoginCredentialsModel(_loginData);

            return _updateResult;
        }

        [HttpGet]
        public IEnumerable<LoginCredentials> SearchLoginCredentialByAnyParameterController(string _parameterName, string _parameterData)
        {
            LoginCredentials _loginInstance = new LoginCredentials();
            DataTable _loginDataFromDataBase = new DataTable();

            _loginDataFromDataBase = _loginInstance.SearchLoginCredentialByAnyParameterModel(_parameterName, _parameterData);

            LoginCredentials[] _listOfLoginCredentials = new LoginCredentials[_loginDataFromDataBase.Rows.Count];

            int _loginounter = 0;

            foreach (DataRow _loginRow in _loginDataFromDataBase.Rows)
            {


                LoginCredentials _loginCredential = new LoginCredentials
                {
                    UserLoginID = _loginRow["UserLoginID"].ToString(),
                    Password = _loginRow["Password"].ToString(),
                    UserID = int.Parse(_loginRow["UserID"].ToString()),
                    UserRoleID = int.Parse(_loginRow["UserRoleID"].ToString()),
                    IsActive = _loginRow["IsActive"].ToString(),

                };

                _listOfLoginCredentials[_loginounter] = _loginCredential;

                _loginounter++;
            }

            return _listOfLoginCredentials;
        }

        [HttpPost]
        public string DeleteLoginCredentialController(string _parameterName, string _parameterData)
        {
            LoginCredentials _loginInstance = new LoginCredentials();

            string _deleteResult = _loginInstance.DeleteLoginCredentialModel(_parameterName, _parameterData);

            return _deleteResult;

        }

        [HttpGet]
        public string VerifyLoginCredentialsController(string _parameterType, string _parameterData, string _password)
        {
            LoginCredentials _loginInstance = new LoginCredentials();

            string _loginResult = _loginInstance.VerifyLoginCredentialsModel(_parameterType, _parameterData, _password);

            return _loginResult;

        }

        [HttpGet]
        public IEnumerable<LoginCredentials> ReadAllLoginCredentialsController()
        {
            LoginCredentials _loginInstance = new LoginCredentials();
            DataTable _loginDataFromDataBase = new DataTable();

            _loginDataFromDataBase = _loginInstance.ReadAllLoginCredentials();

            LoginCredentials[] _listOfLoginCredentials = new LoginCredentials[_loginDataFromDataBase.Rows.Count];

            int _loginounter = 0;

            foreach (DataRow _loginRow in _loginDataFromDataBase.Rows)
            {


                LoginCredentials _loginCredential = new LoginCredentials
                {
                    UserLoginID = _loginRow["UserLoginID"].ToString(),
                    Password = _loginRow["Password"].ToString(),
                    UserID = int.Parse(_loginRow["UserID"].ToString()),
                    UserRoleID = int.Parse(_loginRow["UserRoleID"].ToString()),
                    IsActive = _loginRow["IsActive"].ToString(),

                };

                _listOfLoginCredentials[_loginounter] = _loginCredential;

                _loginounter++;
            }

            return _listOfLoginCredentials;
        }
    }
}
