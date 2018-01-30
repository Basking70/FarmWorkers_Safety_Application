using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FarmworkersWebAPI.Models
{
    public class EducationalContent
    {
        public int IDEducationalContent { get; set; }
        public string LinkEducationalContent { get; set; }
        public string TypeEducationalContent { get; set; }
        public string DescriptionEducationalContent { get; set; }
        public string DateEducationalContentCreated { get; set; }
        public string IsActive { get; set; }

        public DataTable GetListOfEducationalContentModel()
        {
            DataTable _dataBaseResults = new DataTable();
            try
            {
                SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
                string _dataSource = csBuilder.ToString();

                string _queryString = "Exec  ReadAllEducationalContent";


                SqlDataAdapter _sqlAdapter = new SqlDataAdapter(_queryString, _dataSource);
                _sqlAdapter.Fill(_dataBaseResults);
            }
            catch (Exception _exceptionData)
            {
                return _dataBaseResults;
            }

            return _dataBaseResults;
        }

        public bool InsertEducationalContentModel(EducationalContent _educationalContent)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);


            String query = "InsertEducationalContent";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            if (_educationalContent.LinkEducationalContent == null || _educationalContent.TypeEducationalContent == null)
                return false;

            //--Tabla Datos Casos Cliente--
            command.Parameters.AddWithValue("@LinkEducationalContent", _educationalContent.LinkEducationalContent);
            command.Parameters.AddWithValue("@TypeEducationalContent", _educationalContent.TypeEducationalContent);
            command.Parameters.AddWithValue("@DescriptionEducationalContent", _educationalContent.DescriptionEducationalContent == null ? "Undefined" : _educationalContent.DescriptionEducationalContent);
            command.Parameters.AddWithValue("@DateEducationalContentCreated", DateTime.Now.ToShortDateString());
            command.Parameters.AddWithValue("@IsActive", _educationalContent.IsActive == null ? "1" : _educationalContent.IsActive);

            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;
        }

        public bool DeleteEducationalContentModel(string _educationalContentID)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);


            String query = "DeleteEducationalContent";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of Farm Table--
            command.Parameters.AddWithValue("@IDEducationalContent", _educationalContentID);
            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;
        }

        public bool UpdateEducationalContentModel(EducationalContent _educationalContent)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);


            String query = "UpdateEducationalContent";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            if (_educationalContent.LinkEducationalContent == null || _educationalContent.TypeEducationalContent == null)
                return false;

            //--Table Educational Content Data--
            command.Parameters.AddWithValue("@IDEducationalContent", _educationalContent.IDEducationalContent);
            command.Parameters.AddWithValue("@LinkEducationalContent", _educationalContent.LinkEducationalContent);
            command.Parameters.AddWithValue("@TypeEducationalContent", _educationalContent.TypeEducationalContent);
            command.Parameters.AddWithValue("@DescriptionEducationalContent", _educationalContent.DescriptionEducationalContent == null ? "Undefined" : _educationalContent.DescriptionEducationalContent);
            command.Parameters.AddWithValue("@DateEducationalContentCreated", DateTime.Now.ToShortDateString());
            command.Parameters.AddWithValue("@IsActive", _educationalContent.IsActive == null ? "1" : _educationalContent.IsActive);

            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;
        }

        public DataTable ReadEducationalContentByAnyParameterModel(string _parameterName, string _parameterData, string _exactMatch)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            DataTable _educationalContentData = new DataTable();

            String query = "ReadEducationalContentByAnyParameter";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of User Table--
            command.Parameters.AddWithValue("@ParameterName", _parameterName);
            command.Parameters.AddWithValue("@ParamenterData", _parameterData);
            command.Parameters.AddWithValue("@ExactMatch", _exactMatch == null ? "No" : _exactMatch);

            try
            {
                myConnection.Open();

                _educationalContentData.Load(command.ExecuteReader());

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return _educationalContentData;
            }

            return _educationalContentData;


        }

        public bool InsertEducationalContentToUserModel(UserEducationalContent _userEducationalContent)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);


            String query = "InsertEducationalContentToUser";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            

            if (_userEducationalContent.EducationalContentID == 0 || _userEducationalContent.UserID == 0)
                return false;

            //--Table Educational Content--
            command.Parameters.AddWithValue("@EducationalContentID", _userEducationalContent.EducationalContentID);
            command.Parameters.AddWithValue("@UserID", _userEducationalContent.UserID);
            command.Parameters.AddWithValue("@UserEducationalContentClicked", "1");
            command.Parameters.AddWithValue("@UserEducationalContentCreated", DateTime.Now.ToShortDateString());

            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;
        }


    }

    public class UserEducationalContent
    {
        public int EducationalContentID { get; set; }
        public int UserID { get; set; }
        public string UserEducationalContentClicked { get; set; }
        public string UserEducationalContentCreated { get; set; }

    }
}