using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FarmworkersWebAPI.Models
{
  
    public class LoginCredentials
    {
        public string UserLoginID { get; set; }
        public string Password { get; set; }
        public int UserID { get; set; }
        public int UserRoleID { get; set; }
        public string IsActive { get; set; }

        public string InsertLoginCredentialsModel(LoginCredentials _loginData)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            //ReadLoginCredentialsByAnyParameter

            if (CheckLoginCredentialsExistsModel("UserLoginID", _loginData.UserLoginID))
            {
                String query = "InsertLoginCredentials";
                SqlCommand command = new SqlCommand(query, myConnection);
                command.CommandType = CommandType.StoredProcedure;

                if(_loginData.UserID == 0)
                {
                    return "User ID is equal to zero";
                }

                //--Data of User Table--
                command.Parameters.AddWithValue("@UserLoginID", _loginData.UserLoginID == null ? "Farm Worker" : _loginData.UserLoginID);
                command.Parameters.AddWithValue("@Password", _loginData.Password == null ? "Undefined" : _loginData.Password);
                command.Parameters.AddWithValue("@UserID", _loginData.UserID);
                command.Parameters.AddWithValue("@UserRoleID", _loginData.UserRoleID == 0 ? 1: _loginData.UserRoleID);
                command.Parameters.AddWithValue("@IsActive", _loginData.IsActive == null ? "Undefined" : _loginData.IsActive);

                try
                {
                    myConnection.Open();

                    command.ExecuteNonQuery();

                    myConnection.Close();
                }
                catch (Exception _exception)
                {
                    return "User Wasn't Added - Server Error - " + _exception.ToString();
                }

                return "User Added Successfully";
            }
            else
            {
                return "User Wasn't Added - User Already Exists";
            }
        }

        public string UpdateLoginCredentialsModel(LoginCredentials _loginData)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            //ReadLoginCredentialsByAnyParameter

            if (CheckLoginCredentialsExistsModel("UserLoginID", _loginData.UserLoginID))
            {

                return "User Wasn't Updated - User Doesn't Exists";
            }
            else
            {

                String query = "InsertLoginCredentials";
                SqlCommand command = new SqlCommand(query, myConnection);
                command.CommandType = CommandType.StoredProcedure;

                if (_loginData.UserID == 0)
                {
                    return "User ID is equal to zero";
                }

                //--Data of User Table--
                command.Parameters.AddWithValue("@UserLoginID", _loginData.UserLoginID == null ? "Farm Worker" : _loginData.UserLoginID);
                command.Parameters.AddWithValue("@Password", _loginData.Password == null ? "Undefined" : _loginData.Password);
                command.Parameters.AddWithValue("@UserID", _loginData.UserID);
                command.Parameters.AddWithValue("@UserRoleID", _loginData.UserRoleID == 0 ? 1 : _loginData.UserRoleID);
                command.Parameters.AddWithValue("@IsActive", _loginData.IsActive == null ? "Undefined" : _loginData.IsActive);

                try
                {
                    myConnection.Open();

                    command.ExecuteNonQuery();

                    myConnection.Close();
                }
                catch (Exception _exception)
                {
                    return "User Wasn't Added - Server Error - " + _exception.ToString();
                }

                return "User Added Successfully";
            }
        }

        public DataTable SearchLoginCredentialByAnyParameterModel(string _parameterName, string _parameterData)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            DataTable _userData = new DataTable();

            String query = "ReadLoginCredentialsByAnyParameter";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@ParameterName", _parameterName);
            command.Parameters.AddWithValue("@ParamenterData", _parameterData);
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

        public string DeleteLoginCredentialModel(string _parameterName, string _parameterData)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);


            String query = "DeleteLoginCredentialsByAnyParameter";
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

        public bool CheckLoginCredentialsExistsModel(string _parameterName, string _parameterData)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            String query = "ReadLoginCredentialsByAnyParameter";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@ParameterName", _parameterName);
            command.Parameters.AddWithValue("@ParamenterData", _parameterData);
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

        public string VerifyLoginCredentialsModel(string _parameterType, string _parameterData, string _password)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);


            //Verifying by which paramater the Login is trying to be done.
            //By either Email, Phone Number or Login ID.
            String query = "";
            query = _parameterType == "Email" ? "VerifyLoginCredentialsWithEmail" : 
                    _parameterType == "PhoneNumber" ? "VerifyLoginCredentialsWithPhoneNumber" :
                    _parameterType == "LoginID"? "VerifyLoginCredentialsWithLoginID" : "NotValid";

            String _parameterName = "";
            _parameterName = _parameterType == "Email" ? "@UserEmail" :
                             _parameterType == "PhoneNumber" ? "@UserPhoneNumber" :
                             _parameterType == "LoginID" ? "@UserLoginID" : "NotValid";

            if (_parameterName == "NotValid" || _parameterType == "NotValid")
                return "Login Parameter Type Not Valid";


            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue(_parameterName, _parameterData);
            command.Parameters.AddWithValue("@UserPassword", _password);

            DataTable _userData = new DataTable();

            try
            {
                myConnection.Open();

                _userData.Load(command.ExecuteReader());

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return "Login Not Verified";
            }

            if (_userData.Rows.Count == 0)
            {
                return "Login Not Successful";
            }

            return "Login Successful";
            
        }

        public DataTable ReadAllLoginCredentials()
        {
            DataTable _dataBaseResults = new DataTable();
            try
            {
                SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
                string _dataSource = csBuilder.ToString();

                string _queryString = "Exec  ReadAllLoginCredentials";


                SqlDataAdapter _sqlAdapter = new SqlDataAdapter(_queryString, _dataSource);
                _sqlAdapter.Fill(_dataBaseResults);
            }
            catch (Exception _exceptionData)
            {

            }

            return _dataBaseResults;
        }


    }


}