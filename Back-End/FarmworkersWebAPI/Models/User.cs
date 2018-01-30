using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FarmworkersWebAPI.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string UserDateOfBirth { get; set; }
        public string UserHeight { get; set; }
        public int UserWeight { get; set; }
        public string UserGender { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserWorkLocation { get; set; }
        public string UserMemberSince { get; set; }
        public string IsActive { get; set; }

        public string UserFarmID { get; set; }
        

        public string InsertUserModel(User _User)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            if (CheckActiveUserExistsModel("UserPhoneNumber", _User.UserPhoneNumber) ||
                CheckActiveUserExistsModel("UserEmail", _User.UserEmail) ||
                CheckInActiveUserExistsModel("UserPhoneNumber", _User.UserPhoneNumber) ||
                CheckInActiveUserExistsModel("UserEmail", _User.UserEmail))
            {
                String query = "InsertUser";
                SqlCommand command = new SqlCommand(query, myConnection);
                command.CommandType = CommandType.StoredProcedure;


                //--Data of User Table--
                command.Parameters.AddWithValue("@UserType", _User.UserType == null ? "Farm Worker" : _User.UserType);
                command.Parameters.AddWithValue("@UserName", _User.UserName == null ? "Undefined" : _User.UserName);
                command.Parameters.AddWithValue("@UserLastName", _User.UserLastName == null ? "Undefined" : _User.UserLastName);
                command.Parameters.AddWithValue("@UserDateOfBirth", _User.UserDateOfBirth == null ? "01/01/1900" : _User.UserDateOfBirth);
                command.Parameters.AddWithValue("@UserHeight", _User.UserHeight == null ? "Undefined" : _User.UserHeight);
                command.Parameters.AddWithValue("@UserWeight", _User.UserWeight == 0 ? "0" : _User.UserHeight);
                command.Parameters.AddWithValue("@UserGender", _User.UserGender == null ? "Undefined" : _User.UserGender);
                command.Parameters.AddWithValue("@UserEmail", _User.UserEmail == null ? "Undefined" : _User.UserEmail);
                command.Parameters.AddWithValue("@UserPhoneNumber", _User.UserPhoneNumber == null ? "Undefined" : _User.UserPhoneNumber);
                command.Parameters.AddWithValue("@UserWorkLocation", _User.UserWorkLocation == null ? "Undefined" : _User.UserWorkLocation);
                command.Parameters.AddWithValue("@UserMemberSince", _User.UserMemberSince == null ? DateTime.Now.ToShortDateString() : _User.UserMemberSince);
                command.Parameters.AddWithValue("@IsActive", _User.IsActive == null ? "1" : _User.IsActive);

                try
                {
                    myConnection.Open();

                    command.ExecuteNonQuery();

                    myConnection.Close();
                }
                catch (Exception _exception)
                {
                    return "User Wasn't Added - Server Error - " + _exception.Message;
                }

                return "User Added Successfully";
            }
            else
            {
                return "User Wasn't Added - User Already Exists";
            }
        }

        public string InsertUserInFarmModel(string _userID, string _farmID)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);


            String query = "InsertUserInFarm";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of UserFarm Table--
            command.Parameters.AddWithValue("@UserID", _userID);
            command.Parameters.AddWithValue("@FarmID", _farmID);
            command.Parameters.AddWithValue("@UpdateDate", DateTime.Today);
            command.Parameters.AddWithValue("@IsActive", "1");

            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();
            }
            catch (Exception _exception)
            {
                return "User Wasn't Inserted the Farm - Server Error - " + _exception.ToString();
            }

            return "User Added in Farm Successfully";

        }

        public DataTable SearchActiveUserByAnyParameter(string _parameterName, string _parameterData)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            DataTable _userData = new DataTable();

            String query = "ReadUsersActiveByAnyParameter";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@ParameterName", _parameterName);
            command.Parameters.AddWithValue("@ParameterData", _parameterData);
            command.Parameters.AddWithValue("@ExactMatch", "Yes");

            try
            {
                myConnection.Open();

                _userData.Load(command.ExecuteReader());

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return _userData;
            }

            return _userData;
        }

        public string DeleteUserFromFarm(string _parameterName, string _parameterData)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);


            String query = "DeleteUserFarmByAnyParameter";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@ParameterName", _parameterName);
            command.Parameters.AddWithValue("@ParamenterData", _parameterData);
            command.Parameters.AddWithValue("@ExactMatch", "Yes");

            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();
            }
            catch (Exception _exception)
            {
                return "User Wasn't in the Farm - Server Error - " + _exception.ToString();
            }

            return "User Deleted From Farm Successfully";
        }

        public bool CheckActiveUserExistsModel(string _parameterName, string _parameterData)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            String query = "ReadUsersActiveByAnyParameter";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@ParameterName", _parameterName);
            command.Parameters.AddWithValue("@ParameterData", _parameterData);
            command.Parameters.AddWithValue("@ExactMatch", "Yes");

            DataTable _userData = new DataTable();

            try
            {
                myConnection.Open();

                _userData.Load(command.ExecuteReader());

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return false;
            }

            if (_userData.Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckInActiveUserExistsModel(string _parameterName, string _parameterData)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            String query = "ReadUsersINActiveByAnyParameter";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@ParameterName", _parameterName);
            command.Parameters.AddWithValue("@ParameterData", _parameterData);

            DataTable _userData = new DataTable();

            try
            {
                myConnection.Open();

                _userData.Load(command.ExecuteReader());

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return false;
            }

            if (_userData.Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable GetListOfAllUsers()
        {
            DataTable _dataBaseResults = new DataTable();
            try
            {
                SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
                string _dataSource = csBuilder.ToString();

                string _queryString = "Exec  ReadAllUsers";


                SqlDataAdapter _sqlAdapter = new SqlDataAdapter(_queryString, _dataSource);
                _sqlAdapter.Fill(_dataBaseResults);
            }
            catch (Exception _exceptionData)
            {

            }

            return _dataBaseResults;
        }

        public string UpdateUserFromFarm(User _User)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            //Check Inactive Users Later On
            if (CheckActiveUserExistsModel("UserID", _User.UserID.ToString()))
            {

                return "User Wasn't Updated - User Doesn't Exist";
            }
            else
            {
                String query = "UpdateUser";
                SqlCommand command = new SqlCommand(query, myConnection);
                command.CommandType = CommandType.StoredProcedure;


                if (_User.UserID == 0)
                    return "User ID is zero";

                //--Data of User Table--
                command.Parameters.AddWithValue("@UserID", _User.UserID);
                command.Parameters.AddWithValue("@UserType", _User.UserType == null ? "Farm Worker" : _User.UserType);
                command.Parameters.AddWithValue("@UserName", _User.UserName == null ? "Undefined" : _User.UserName);
                command.Parameters.AddWithValue("@UserLastName", _User.UserLastName == null ? "Undefined" : _User.UserLastName);
                command.Parameters.AddWithValue("@UserDateOfBirth", _User.UserDateOfBirth == null ? "01/01/1900" : _User.UserDateOfBirth);
                command.Parameters.AddWithValue("@UserHeight", _User.UserHeight == null ? "Undefined" : _User.UserHeight);
                command.Parameters.AddWithValue("@UserWeight", _User.UserWeight == 0 ? "0" : _User.UserHeight);
                command.Parameters.AddWithValue("@UserGender", _User.UserGender == null ? "Undefined" : _User.UserGender);
                command.Parameters.AddWithValue("@UserEmail", _User.UserEmail == null ? "Undefined" : _User.UserEmail);
                command.Parameters.AddWithValue("@UserPhoneNumber", _User.UserPhoneNumber == null ? "Undefined" : _User.UserPhoneNumber);
                command.Parameters.AddWithValue("@UserWorkLocation", _User.UserWorkLocation == null ? "Undefined" : _User.UserWorkLocation);
                command.Parameters.AddWithValue("@UserMemberSince", _User.UserMemberSince == null ? DateTime.Now.ToShortDateString() : _User.UserMemberSince);
                command.Parameters.AddWithValue("@IsActive", _User.IsActive == null ? "1" : _User.IsActive);

                try
                {
                    myConnection.Open();

                    command.ExecuteNonQuery();

                    myConnection.Close();
                }
                catch (Exception _exception)
                {
                    return "User Wasn't Updated - Server Error - " + _exception.Data;
                }

                return "User Updated Successfully";
            }
        }

        public bool DeleteUserByAnyParameter(string _parameterName, string _parameterData, string _exactMatch)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            DataTable _listUsers = new DataTable();

            String query = "DeleteUserByAnyParameter";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@ParameterName", _parameterName);
            command.Parameters.AddWithValue("@ParamenterData", _parameterData);
            command.Parameters.AddWithValue("@ExactMatch", _exactMatch != "Yes" && _exactMatch != "No" ? "No" : _exactMatch);

            try
            {
                myConnection.Open();

                command.ExecuteReader();

                myConnection.Close();
            }
            catch (Exception _exception)
            {
                return false;
            }

            return true;
        }


        public string InsertUserEmergencyContactModel(UserEmergencyContacts _userEmergencyContact)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            if (!CheckActiveUserExistsModel("UserID", _userEmergencyContact.UserEmergencyContactsID.ToString()))
            {
                String query = "InsertUserEmergencyContact";
                SqlCommand command = new SqlCommand(query, myConnection);
                command.CommandType = CommandType.StoredProcedure;

                if (_userEmergencyContact.UserID == 0)
                    return "User Emergency Contact Could NOT be Inserted, User ID Missing";

                //--Data of User Emergency Contact Table--
                command.Parameters.AddWithValue("@UserID", _userEmergencyContact.UserEmergencyContactsID == 0);
                command.Parameters.AddWithValue("@UserEmergencyContactsName", _userEmergencyContact.UserEmergencyContactsName == null ? "Undefined" : _userEmergencyContact.UserEmergencyContactsName);
                command.Parameters.AddWithValue("@UserEmergencyContactsPhoneNumber", _userEmergencyContact.UserEmergencyContactsPhoneNumber == null ? "Undefined" : _userEmergencyContact.UserEmergencyContactsPhoneNumber);
                command.Parameters.AddWithValue("@UserEmergencyContactsAddress", _userEmergencyContact.UserEmergencyContactsAddress == null ? "Undefined" : _userEmergencyContact.UserEmergencyContactsAddress);
                
                try
                {
                    myConnection.Open();

                    command.ExecuteNonQuery();

                    myConnection.Close();
                }
                catch (Exception _exception)
                {
                    return "User Emergency Contact Wasn't Added - Server Error - " + _exception.Message;
                }

                return "User Emergency Contact Added Successfully";
            }
            else
            {
                return "User Wasn't Added - User Doesn't Exist or is NOT Active";
            }
        }

        public string UpdateUserEmergencyContactModel(UserEmergencyContacts _userEmergencyContact)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            if (!CheckActiveUserExistsModel("UserID", _userEmergencyContact.UserEmergencyContactsID.ToString()))
            {
                String query = "UpdateUserEmergencyContact";
                SqlCommand command = new SqlCommand(query, myConnection);
                command.CommandType = CommandType.StoredProcedure;

                if (_userEmergencyContact.UserID == 0 && _userEmergencyContact.UserEmergencyContactsID == 0)
                    return "User Emergency Contact Could NOT be Updated, User ID and Contact ID Missing";

                //--Data of User Emergency Contact Table--
                command.Parameters.AddWithValue("@UserID", _userEmergencyContact.UserEmergencyContactsID == 0);
                command.Parameters.AddWithValue("@UserEmergencyContactsName", _userEmergencyContact.UserEmergencyContactsName == null ? "Undefined" : _userEmergencyContact.UserEmergencyContactsName);
                command.Parameters.AddWithValue("@UserEmergencyContactsPhoneNumber", _userEmergencyContact.UserEmergencyContactsPhoneNumber == null ? "Undefined" : _userEmergencyContact.UserEmergencyContactsPhoneNumber);
                command.Parameters.AddWithValue("@UserEmergencyContactsAddress", _userEmergencyContact.UserEmergencyContactsAddress == null ? "Undefined" : _userEmergencyContact.UserEmergencyContactsAddress);

                try
                {
                    myConnection.Open();

                    command.ExecuteNonQuery();

                    myConnection.Close();
                }
                catch (Exception _exception)
                {
                    return "User Emergency Contact Wasn't Updated - Server Error - " + _exception.Message;
                }

                return "User Emergency Contact Updated Successfully";
            }
            else
            {
                return "User Wasn't Updated - User Doesn't Exist or is NOT Active";
            }
        }

        public bool DeleteUserEmergencyContact(string _userEmergencyContactID, string _userID)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            DataTable _listUsers = new DataTable();

            String query = "DeleteUserEmergencyContact";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@UserEmergencyContactsID", _userEmergencyContactID);
            command.Parameters.AddWithValue("@UserID", _userID);
            try
            {
                myConnection.Open();

                command.ExecuteReader();

                myConnection.Close();
            }
            catch (Exception _exception)
            {
                return false;
            }

            return true;
        }

        public DataTable GetListOfAllUserEmergencyContacts()
        {
            DataTable _dataBaseResults = new DataTable();
            try
            {
                SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
                string _dataSource = csBuilder.ToString();

                string _queryString = "Exec  ReadAllUsersEmergencyContacts";


                SqlDataAdapter _sqlAdapter = new SqlDataAdapter(_queryString, _dataSource);
                _sqlAdapter.Fill(_dataBaseResults);
            }
            catch (Exception _exceptionData)
            {

            }

            return _dataBaseResults;
        }

        public DataTable SearchEmergencyContactsByAnyParameter(string _parameterName, string _parameterData)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            DataTable _userData = new DataTable();

            String query = "ReadUserEmergencyContactsByAnyParameter";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@ParameterName", _parameterName);
            command.Parameters.AddWithValue("@ParameterData", _parameterData);
            command.Parameters.AddWithValue("@ExactMatch", "Yes");

            try
            {
                myConnection.Open();

                _userData.Load(command.ExecuteReader());

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return _userData;
            }

            return _userData;
        }

    }

    public class UserEmergencyContacts
    {
        public int UserEmergencyContactsID { get; set; }
        public int UserID { get; set; }
        public string UserEmergencyContactsName { get; set; }
        public string UserEmergencyContactsPhoneNumber { get; set; }
        public string UserEmergencyContactsAddress { get; set; }


    }


}