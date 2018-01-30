using FarmworkersWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FarmworkersWebAPI.Entities;
using System.Data.Entity.Infrastructure;
using FarmworkersWebAPI.ViewModels;

namespace FarmworkersWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {

        //***Entity Framework**//

        //NOTE: Watch out for conflicts, between: 
        //**FarmworkersWebAPI.Entities
        //**FarmworkersWebAPI.Models

        FarmWorkerAppContext _context;
        public UsersController()
        {
            _context = new FarmWorkerAppContext();
            //_context.Configuration.LazyLoadingEnabled = false;
        }

        //User Notifications Preferences
        [HttpGet]
        public IHttpActionResult ReadCommunicationPreferenceByAnyParameter(string _parameterName, string _parameterData, string _exactMatch)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            List<UserCommunicationPreference> _userData = new List<UserCommunicationPreference>();

            string _matchCriteria = _exactMatch == "Yes" ? " = " + _parameterData : " LIKE '%" + _parameterData + "%' ";

            string _selectClause = "SELECT * ";
            string _fromClause = "FROM UserCommunicationPreference ";
            string _whereClause = "WHERE " + _parameterName + _matchCriteria;
            string query = _selectClause + _fromClause + _whereClause;

            _userData = _context.UserCommunicationPreferences.SqlQuery(query).ToList();

            return Ok(_userData);
        }

        [HttpPost]
        public IHttpActionResult InsertCommunicationPreferenceUser(UserCommunicationPreference _user)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            UserCommunicationPreference _userData = new UserCommunicationPreference();

            try
            {
                _userData = _context.UserCommunicationPreferences.Add(_user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(_userData);
        }

        [HttpPost]
        public IHttpActionResult UpdateCommunicationPreferenceUser(UserCommunicationPreference _user)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            UserCommunicationPreference _updated = null;

            try
            {

                UserCommunicationPreference original = _context.UserCommunicationPreferences.
                                                        Where(ucp => ucp.IDUserCommunicationPreference.Equals(_user.IDUserCommunicationPreference)).FirstOrDefault();

                if (original != null)
                {
                    _context.Entry(original).CurrentValues.SetValues(_user);
                    _context.SaveChanges();

                    _updated = _context.UserCommunicationPreferences.
                                                        Where(ucp => ucp.IDUserCommunicationPreference.Equals(_user.IDUserCommunicationPreference)).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(_updated);
        }

        [HttpPost]
        public IHttpActionResult DeleteCommunicationPreferenceUser(UserCommunicationPreference _user)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            UserCommunicationPreference _userData = new UserCommunicationPreference();

            try
            {
                _context.UserCommunicationPreferences.Attach(_user);
                _userData = _context.UserCommunicationPreferences.Remove(_user);
                _context.SaveChanges();

                return Ok("User Communication Preference Deleted Successfully");

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        public IHttpActionResult ReadAllCommunicationPreferenceUser()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            List<UserCommunicationPreference> _userData = new List<UserCommunicationPreference>();

            try
            {
                _userData = _context.UserCommunicationPreferences.ToList();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(_userData);
        }


        //User
        [HttpPost]
        public IHttpActionResult EFInsertUser(FarmworkersWebAPI.Entities.User _userInformation)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            FarmworkersWebAPI.Entities.User _userData = new FarmworkersWebAPI.Entities.User();

            try
            {
                if (EFCheckUserExistsController("UserPhoneNumber", _userInformation.UserPhoneNumber, "Yes").FirstOrDefault() == null ||
                    EFCheckUserExistsController("UserEmail", _userInformation.UserEmail, "Yes").FirstOrDefault() == null)
                {

                    _userData = _context.Users.Add(_userInformation);
                    _context.SaveChanges();


                    return Ok(_userData);
                }
                else
                {
                    return Ok("User Was NOT Added - User Already Exists");
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        public IHttpActionResult EFUpdateUser(FarmworkersWebAPI.Entities.User _userInformation)
        {
            FarmworkersWebAPI.Entities.User _updated = null;

            try
            {
                if (_userInformation.UserID == 0)
                    return Ok("User Wasn't Updated - User ID is Invalid");

                FarmworkersWebAPI.Entities.User original = _context.Users.
                                                    Where(u => u.UserID.Equals(_userInformation.UserID)).FirstOrDefault();

                if (original != null)
                {
                    _context.Entry(original).CurrentValues.SetValues(_userInformation);
                    _context.SaveChanges();

                    _updated = _context.Users.
                                Where(u => u.UserID.Equals(_userInformation.UserID)).FirstOrDefault();

                    return Ok(AutoMapper.Mapper.Map<UserWithFarmDTO>(_updated));
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok("User Wasn't Updated - User Doesn't Exist");
        }

        [HttpGet]
        public IHttpActionResult EFReadListOfUsers()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            List<FarmworkersWebAPI.Entities.User> _userData = new List<FarmworkersWebAPI.Entities.User>();

            try
            {
                _userData = _context.Users.ToList();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(_userData);
        }

        [HttpPost]
        public IHttpActionResult EFDeleteByUserAnyParameter(string _parameterName, string _parameterData, string _exactMatch)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            List<FarmworkersWebAPI.Entities.User> _userData = new List<FarmworkersWebAPI.Entities.User>();

            if (EFCheckUserExistsController(_parameterName, _parameterData, _exactMatch).FirstOrDefault() == null)
                return Ok("User Was NOT Deleted - No User Was Found by the Parameter Provided");

            string _matchCriteria = _exactMatch == "Yes" ? " = " + _parameterData : " LIKE '%" + _parameterData + "%' ";

            string _deleteClause = "DELETE FROM User ";
            string _whereClause = "WHERE " + _parameterName + _matchCriteria;
            string query = _deleteClause + _whereClause;

            try
            {
                _userData = _context.Users.SqlQuery(query).ToList();

                return Ok("User Deleted Successfully");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult EFReadUserAnyParameter(string _parameterName, string _parameterData, string _exactMatch)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            List<FarmworkersWebAPI.Entities.User> _userData = new List<FarmworkersWebAPI.Entities.User>();

            string _matchCriteria = _exactMatch == "Yes" ? " = " + _parameterData : " LIKE '%" + _parameterData + "%' ";

            string _selectClause = "SELECT * ";
            string _fromClause = "FROM User ";
            string _whereClause = "WHERE " + _parameterName + _matchCriteria;
            string query = _selectClause + _fromClause + _whereClause;

            _userData = _context.Users.SqlQuery(query).ToList();

            return Ok(_userData);
        }

        public List<FarmworkersWebAPI.Entities.User> EFCheckUserExistsController(string _parameterName, string _parameterData, string _exactMatch)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            List<FarmworkersWebAPI.Entities.User> _userData = null;

            string _matchCriteria = _exactMatch == "Yes" ? " = " + _parameterData : " LIKE '%" + _parameterData + "%' ";

            string _selectClause = "SELECT * ";
            string _fromClause = "FROM User ";
            string _whereClause = "WHERE " + _parameterName + _matchCriteria;
            string query = _selectClause + _fromClause + _whereClause;

            _userData = _context.Users.SqlQuery(query).ToList();

            return _userData;
        }


        //User Emergency Contact
        [HttpPost]
        public IHttpActionResult EFInsertUserEmergencyContacts(FarmworkersWebAPI.Entities.UserEmergencyContact _userEmergencyContactInformation)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            FarmworkersWebAPI.Entities.UserEmergencyContact _userEmergencyContactData = new FarmworkersWebAPI.Entities.UserEmergencyContact();

            try
            {
                if (_userEmergencyContactInformation.UserID == 0)
                    return Ok("User Emergency Contact Was Not Added - User ID Missing");

                if (EFCheckUserExistsController("UserID", _userEmergencyContactInformation.UserID.ToString(), "Yes").FirstOrDefault() == null)
                    return Ok("User Emergency Contact Was Not Added - No User Was Found by the ID Provided");

                _userEmergencyContactData = _context.UserEmergencyContacts.Add(_userEmergencyContactInformation);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(_userEmergencyContactData);
        }

        [HttpPost]
        public IHttpActionResult EFUpdateUserEmergencyContact(FarmworkersWebAPI.Entities.UserEmergencyContact _userEmergencyContactInformation)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            FarmworkersWebAPI.Entities.UserEmergencyContact _updated = null;

            try
            {
                if (_userEmergencyContactInformation.UserID == 0 && _userEmergencyContactInformation.UserEmergencyContactsID == 0)
                    return Ok("User Emergency Contact Was Not Updated - User ID and Contact ID are Missing");

                FarmworkersWebAPI.Entities.UserEmergencyContact original = _context.UserEmergencyContacts.
                                                        Where(u => u.UserEmergencyContactsID.Equals(_userEmergencyContactInformation.UserEmergencyContactsID)).FirstOrDefault();

                if (original != null)
                {
                    _context.Entry(original).CurrentValues.SetValues(_userEmergencyContactInformation);
                    _context.SaveChanges();

                    _updated = _context.UserEmergencyContacts.
                                                        Where(u => u.UserEmergencyContactsID.Equals(_userEmergencyContactInformation.UserEmergencyContactsID)).FirstOrDefault();

                    return Ok(_updated);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok("User Emergency Contact Was Not Updated - No User Was Found by the ID Provided");
        }

        [HttpPost]
        public IHttpActionResult EFDeleteUserEmergencyContact(FarmworkersWebAPI.Entities.UserEmergencyContact _userEmergencyContactInformation)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            FarmworkersWebAPI.Entities.UserEmergencyContact _userEmergencyContactData = new FarmworkersWebAPI.Entities.UserEmergencyContact();

            try
            {
                _context.UserEmergencyContacts.Attach(_userEmergencyContactInformation);
                _userEmergencyContactData = _context.UserEmergencyContacts.Remove(_userEmergencyContactInformation);
                _context.SaveChanges();

                return Ok("User Emergency Contact Deleted Successfully");

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        public IHttpActionResult EFReadListOfAllUserEmergencyContacts()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            List<FarmworkersWebAPI.Entities.UserEmergencyContact> _userEmergencyContactData = new List<FarmworkersWebAPI.Entities.UserEmergencyContact>();

            try
            {
                _userEmergencyContactData = _context.UserEmergencyContacts.ToList();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(_userEmergencyContactData);
        }

        [HttpGet]
        public IHttpActionResult EFReadUserEmergencyContactsByAnyParameter(string _parameterName, string _parameterData, string _exactMatch)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            List<FarmworkersWebAPI.Entities.UserEmergencyContact> _userEmergencyContactData = new List<FarmworkersWebAPI.Entities.UserEmergencyContact>();

            string _matchCriteria = _exactMatch == "Yes" ? " = " + _parameterData : " LIKE '%" + _parameterData + "%' ";

            string _selectClause = "SELECT * ";
            string _fromClause = "FROM UserEmergencyContacts ";
            string _whereClause = "WHERE " + _parameterName + _matchCriteria;
            string query = _selectClause + _fromClause + _whereClause;

            _userEmergencyContactData = _context.UserEmergencyContacts.SqlQuery(query).ToList();

            return Ok(_userEmergencyContactData);
        }



        [HttpGet]
        public IHttpActionResult ListUserByUserType(string UserType)
        {
            return Ok(AutoMapper.Mapper.Map<List<UserDTO>>(_context.Users.Where(x => x.UserType == UserType).ToList()));
        }

        [HttpGet]
        public IHttpActionResult SearchUser(string UserType, String email = null, String phone = null, String name = null)
        {
            var usersEnumerable = _context.Users
                .Include("UserFarms.Farm")
                .Where(x => x.UserType == UserType);
            if (email != null)
            {
                usersEnumerable = usersEnumerable.Where(x => x.UserEmail.Contains(email));
            }
            if (phone != null)
            {
                usersEnumerable = usersEnumerable.Where(x => x.UserPhoneNumber.Contains(phone));
            }
            if (name != null)
            {
                usersEnumerable = usersEnumerable.Where(x => x.UserName.Contains(name) || x.UserLastName.Contains(name));
            }
            return Ok(AutoMapper.Mapper.Map<IList<UserWithFarmDTO>>(usersEnumerable.ToList()));

        }
        //*********************//



        //User

        [HttpPost]
        public IHttpActionResult InsertUser(FarmworkersWebAPI.Models.User _userInformation)
        {
            FarmworkersWebAPI.Models.User _userReceived = new FarmworkersWebAPI.Models.User();

            return Ok(_userReceived.InsertUserModel(_userInformation));
        }

        [HttpPost]
        public IHttpActionResult UpdateUser(FarmworkersWebAPI.Models.User _userInformation)
        {
            FarmworkersWebAPI.Models.User _userReceived = new FarmworkersWebAPI.Models.User();

            _userReceived.UpdateUserFromFarm(_userInformation);

            return Ok(_userReceived.InsertUserModel(_userInformation));
        }

        [HttpGet]
        public FarmworkersWebAPI.Models.User[] ReadListOfUsers()
        {
            FarmworkersWebAPI.Models.User _userControllerInstance = new FarmworkersWebAPI.Models.User();

            DataTable _listOfUsersFromDatabase = _userControllerInstance.GetListOfAllUsers();

            FarmworkersWebAPI.Models.User[] _listOfUsers = new FarmworkersWebAPI.Models.User[_listOfUsersFromDatabase.Rows.Count];

            int _userCounter = 0;

            foreach (DataRow _userRow in _listOfUsersFromDatabase.Rows)
            {


                FarmworkersWebAPI.Models.User _user = new FarmworkersWebAPI.Models.User
                {
                    UserID = int.Parse(_userRow["UserID"].ToString()),
                    UserType = _userRow["UserType"].ToString(),
                    UserName = _userRow["UserName"].ToString(),
                    UserLastName = _userRow["UserLastName"].ToString(),
                    UserDateOfBirth = _userRow["UserDateOfBirth"].ToString(),
                    UserWeight = int.Parse(_userRow["UserWeight"].ToString()),
                    UserHeight = _userRow["UserHeight"].ToString(),
                    UserGender = _userRow["UserGender"].ToString(),
                    UserEmail = _userRow["UserEmail"].ToString(),
                    UserPhoneNumber = _userRow["UserPhoneNumber"].ToString(),
                    UserWorkLocation = _userRow["UserWorkLocation"].ToString(),
                    UserMemberSince = _userRow["UserMemberSince"].ToString(),
                    IsActive = _userRow["IsActive"].ToString(),
                    UserFarmID = _userRow["FarmID"].ToString(),
                };

                _listOfUsers[_userCounter] = _user;

                _userCounter++;
            }

            return _listOfUsers;

        }

        [HttpGet]
        public FarmworkersWebAPI.Models.User[] ReadUserAnyParameter(string _parameterName, string _parameterData)
        {
            FarmworkersWebAPI.Models.User _userControllerInstance = new FarmworkersWebAPI.Models.User();

            DataTable _listOfUsersFromDatabase = _userControllerInstance.SearchActiveUserByAnyParameter(_parameterName, _parameterData);

            FarmworkersWebAPI.Models.User[] _listOfUsers = new FarmworkersWebAPI.Models.User[_listOfUsersFromDatabase.Rows.Count];

            int _userCounter = 0;

            foreach (DataRow _userRow in _listOfUsersFromDatabase.Rows)
            {


                FarmworkersWebAPI.Models.User _user = new FarmworkersWebAPI.Models.User
                {
                    UserID = int.Parse(_userRow["UserID"].ToString()),
                    UserType = _userRow["UserType"].ToString(),
                    UserName = _userRow["UserName"].ToString(),
                    UserLastName = _userRow["UserLastName"].ToString(),
                    UserDateOfBirth = _userRow["UserDateOfBirth"].ToString(),
                    UserWeight = int.Parse(_userRow["UserWeight"].ToString()),
                    UserHeight = _userRow["UserHeight"].ToString(),
                    UserGender = _userRow["UserGender"].ToString(),
                    UserEmail = _userRow["UserEmail"].ToString(),
                    UserPhoneNumber = _userRow["UserPhoneNumber"].ToString(),
                    UserWorkLocation = _userRow["UserWorkLocation"].ToString(),
                    UserMemberSince = _userRow["UserMemberSince"].ToString(),
                    IsActive = _userRow["IsActive"].ToString(),
                    UserFarmID = _userRow["FarmID"].ToString(),
                };

                _listOfUsers[_userCounter] = _user;

                _userCounter++;
            }

            return _listOfUsers;

        }

        [HttpPost]
        public bool DeleteByUserAnyParameter(string _parameterName, string _parameterData, string _exactMatch)
        {
            FarmworkersWebAPI.Models.User _userControllerInstance = new FarmworkersWebAPI.Models.User();

            bool _deleteUserResult = _userControllerInstance.DeleteUserByAnyParameter(_parameterName, _parameterData, _exactMatch);

            return _deleteUserResult;

        }


        //User Emergency Contact
        [HttpPost]
        public IHttpActionResult InsertUserEmergencyContacts(UserEmergencyContacts _userEmergencyContactInformation)
        {
            FarmworkersWebAPI.Models.User _userReceived = new FarmworkersWebAPI.Models.User();

            return Ok(_userReceived.InsertUserEmergencyContactModel(_userEmergencyContactInformation));
        }

        [HttpPost]
        public IHttpActionResult UpdateUserEmergencyContact(UserEmergencyContacts _userEmergencyContactInformation)
        {
            FarmworkersWebAPI.Models.User _userReceived = new FarmworkersWebAPI.Models.User();

            _userReceived.UpdateUserEmergencyContactModel(_userEmergencyContactInformation);

            return Ok(_userReceived.UpdateUserEmergencyContactModel(_userEmergencyContactInformation));
        }

        [HttpPost]
        public bool DeleteUserEmergencyContact(string _userEmergencyContactID, string _userID)
        {
            FarmworkersWebAPI.Models.User _userControllerInstance = new FarmworkersWebAPI.Models.User();

            bool _deleteUserResult = _userControllerInstance.DeleteUserEmergencyContact(_userEmergencyContactID, _userID);

            return _deleteUserResult;

        }

        [HttpGet]
        public UserEmergencyContacts[] ReadListOfAllUserEmergencyContacts()
        {
            FarmworkersWebAPI.Models.User _userControllerInstance = new FarmworkersWebAPI.Models.User();

            DataTable _listOfUsersEmergencyContactsFromDatabase = _userControllerInstance.GetListOfAllUsers();

            UserEmergencyContacts[] _listOfUsersEmergencyContacts = new UserEmergencyContacts[_listOfUsersEmergencyContactsFromDatabase.Rows.Count];

            int _userEmergencyContactCounter = 0;

            foreach (DataRow _userRow in _listOfUsersEmergencyContactsFromDatabase.Rows)
            {


                UserEmergencyContacts _userEmergencyContact = new UserEmergencyContacts
                {
                    UserID = int.Parse(_userRow["UserID"].ToString()),
                    UserEmergencyContactsID = int.Parse(_userRow["UserEmergencyContactsID"].ToString()),
                    UserEmergencyContactsName = _userRow["UserEmergencyContactsName"].ToString(),
                    UserEmergencyContactsPhoneNumber = _userRow["UserEmergencyContactsPhoneNumber"].ToString(),
                    UserEmergencyContactsAddress = _userRow["UserEmergencyContactsAddress"].ToString()
                };

                _listOfUsersEmergencyContacts[_userEmergencyContactCounter] = _userEmergencyContact;

                _userEmergencyContactCounter++;
            }

            return _listOfUsersEmergencyContacts;

        }

        [HttpGet]
        public UserEmergencyContacts[] ReadUserEmergencyContactsByAnyParameter(string _parameterName, string _parameterData)
        {
            FarmworkersWebAPI.Models.User _userControllerInstance = new FarmworkersWebAPI.Models.User();

            DataTable _listOfUsersEmergencyContactsFromDatabase = _userControllerInstance.SearchEmergencyContactsByAnyParameter(_parameterName, _parameterData);

            UserEmergencyContacts[] _listOfUsersEmergencyContacts = new UserEmergencyContacts[_listOfUsersEmergencyContactsFromDatabase.Rows.Count];

            int _userEmergencyContactCounter = 0;

            foreach (DataRow _userRow in _listOfUsersEmergencyContactsFromDatabase.Rows)
            {


                UserEmergencyContacts _userEmergencyContact = new UserEmergencyContacts
                {
                    UserID = int.Parse(_userRow["UserID"].ToString()),
                    UserEmergencyContactsID = int.Parse(_userRow["UserEmergencyContactsID"].ToString()),
                    UserEmergencyContactsName = _userRow["UserEmergencyContactsName"].ToString(),
                    UserEmergencyContactsPhoneNumber = _userRow["UserEmergencyContactsPhoneNumber"].ToString(),
                    UserEmergencyContactsAddress = _userRow["UserEmergencyContactsAddress"].ToString()
                };

                _listOfUsersEmergencyContacts[_userEmergencyContactCounter] = _userEmergencyContact;

                _userEmergencyContactCounter++;
            }

            return _listOfUsersEmergencyContacts;
        }

        // User Farm
        [HttpPost]
        public IHttpActionResult InsertOrUpdateUserFarm(IList<UserFarmDTO> userFarms)
        {
            var userId = userFarms.First().UserID;
            var dbUserFarms = _context.UserFarms.Where(x => x.UserID == userId);

            foreach (var dbUserFarm in dbUserFarms.ToList())
            {
                var userFarm = userFarms.FirstOrDefault(x => x.FarmID == dbUserFarm.FarmID && x.UserID == dbUserFarm.UserID);
                if (userFarm != null)
                {
                    //userFarm.UserInFarmID = dbUserFarm.UserInFarmID;
                    dbUserFarm.Farm = _context.Farms.Find(userFarm.FarmID);
                    dbUserFarm.FarmID = userFarm.FarmID;
                    dbUserFarm.IsLatest = userFarm.IsLatest;
                    _context.Entry(dbUserFarm).State = System.Data.Entity.EntityState.Modified;
                    //_context.Entry(dbUserFarm).CurrentValues.SetValues(userFarm);
                }
                else
                {
                    _context.Entry(dbUserFarm).State = System.Data.Entity.EntityState.Deleted;
                }
                userFarms.Remove(userFarm);
            }

            foreach (var userFarm in userFarms)
            {
                _context.UserFarms.Add(new Entities.UserFarm
                {
                    Farm = _context.Farms.Find(userFarm.FarmID),
                    UserID = userFarm.UserID,
                    FarmID = userFarm.FarmID,
                    IsLatest = userFarm.IsLatest
                });
            }
            _context.SaveChanges();
            var resp = dbUserFarms.ToList();
            return Ok(AutoMapper.Mapper.Map<List<UserFarmDTO>>(dbUserFarms.ToList()));
        }
        // User Farm
        [HttpPost]
        public IHttpActionResult InsertOrUpdateUserCommunicationPreference(IList<UserCommunicationPreferenceDTO> userCommunicationPreferences)
        {
            var userId = userCommunicationPreferences.First().UserID;
            var dbUserCommunicationPreferences = _context.UserCommunicationPreferences.Where(x => x.UserID == userId);

            foreach (var dbUserCommunicationPreference in dbUserCommunicationPreferences.ToList())
            {
                var userCommunicationPreference = userCommunicationPreferences.FirstOrDefault(x => x.UserCommunicationPreferenceNotificationType == dbUserCommunicationPreference.UserCommunicationPreferenceNotificationType);
                if (userCommunicationPreference != null)
                {
                    dbUserCommunicationPreference.UserCommunicationPreferenceFrequency = userCommunicationPreference.UserCommunicationPreferenceFrequency;
                    dbUserCommunicationPreference.UserCommunicationPreferenceNotificationType = userCommunicationPreference.UserCommunicationPreferenceNotificationType;
                    dbUserCommunicationPreference.IDUserCommunicationPreferenceLanguage = userCommunicationPreference.IDUserCommunicationPreferenceLanguage;
                    _context.Entry(dbUserCommunicationPreference).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    _context.Entry(dbUserCommunicationPreference).State = System.Data.Entity.EntityState.Deleted;
                }
                userCommunicationPreferences.Remove(userCommunicationPreference);
            }

            foreach (var userCommunicationPreference in userCommunicationPreferences)
            {
                _context.UserCommunicationPreferences.Add(new UserCommunicationPreference
                {
                    UserID = userCommunicationPreference.UserID,
                    UserCommunicationPreferenceFrequency = userCommunicationPreference.UserCommunicationPreferenceFrequency,
                    UserCommunicationPreferenceNotificationType = userCommunicationPreference.UserCommunicationPreferenceNotificationType,
                    IDUserCommunicationPreferenceLanguage = userCommunicationPreference.IDUserCommunicationPreferenceLanguage
                });
            }
            _context.SaveChanges();
            return Ok(AutoMapper.Mapper.Map<List<UserCommunicationPreferenceDTO>>(dbUserCommunicationPreferences.ToList()));
        }
    }
}
    
