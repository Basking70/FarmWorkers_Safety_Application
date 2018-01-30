using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FluentValidation;

namespace FarmworkersWebAPI.Models
{
    public class Farm
    {
        public int FarmID { get; set; }
        public string FarmName { get; set; }
        public string FarmHouseNumberStreetAddress { get; set; }
        public string FarmCity { get; set; }
        public string FarmState { get; set; }
        public string FarmCountry { get; set; }
        public int FarmZipCode { get; set; }
        public string FarmLatitude { get; set; }
        public string FarmLongitude { get; set; }
        public string FarmTemperatureMin { get; set; }
        public string FarmTemperatureMax { get; set; }
        public string IsActive { get; set; }

        public int NumberOfFarmWorkers { get; set; }
        public string FarmOwner { get; set; }


        public string[] FarmWeather { get; set; }

        public DataTable GetListOfFarmsModel()
        {
            DataTable _dataBaseResults = new DataTable();
            try
            {
                SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
                string _dataSource = csBuilder.ToString();

                string _queryString = "Exec  ReadAllFarms";


                SqlDataAdapter _sqlAdapter = new SqlDataAdapter(_queryString, _dataSource);
                _sqlAdapter.Fill(_dataBaseResults);
            }
            catch (Exception _exceptionData)
            {
                return _dataBaseResults;
            }

            return _dataBaseResults;
        }

        public bool InsertFarmModel(Farm _farm)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            DataTable _findFarm = ReadFarmByAnyParameterModel("FarmID", _farm.FarmID.ToString());

            if (_findFarm.Rows.Count != 0)
                return false;

            String query = "InsertFarm";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Tabla Datos Casos Cliente--
            command.Parameters.AddWithValue("@FarmName", _farm.FarmName);
            command.Parameters.AddWithValue("@FarmHouseNumberStreetAddress", _farm.FarmHouseNumberStreetAddress);
            command.Parameters.AddWithValue("@FarmCity", _farm.FarmCity);
            command.Parameters.AddWithValue("@FarmState", _farm.FarmState);
            command.Parameters.AddWithValue("@FarmCountry", _farm.FarmCountry);
            command.Parameters.AddWithValue("@FarmZipCode", _farm.FarmZipCode);
            command.Parameters.AddWithValue("@FarmLatitude", _farm.FarmLatitude);
            command.Parameters.AddWithValue("@FarmLongitude", _farm.FarmLongitude);
            command.Parameters.AddWithValue("@FarmTemperatureMin", _farm.FarmTemperatureMin);
            command.Parameters.AddWithValue("@FarmTemperatureMax", _farm.FarmTemperatureMax);
            command.Parameters.AddWithValue("@IsActive", _farm.IsActive);

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

        public bool DeleteFarmModel(string _parameterName, string _parameterData)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);


            String query = "DeleteFarmByAnyParameter";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            //--Data of Farm Table--
            command.Parameters.AddWithValue("@ParameterName", _parameterName);
            command.Parameters.AddWithValue("@ParamenterData", _parameterData);
            command.Parameters.AddWithValue("@ExactMatch", "Yes");

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

        public string UpdateFarmModel(Farm _farm)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);


            String query = "UpdateFarm";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.CommandType = CommandType.StoredProcedure;

            DataTable _findFarm = ReadFarmByAnyParameterModel("FarmID", _farm.FarmID.ToString());

            if (_findFarm.Rows.Count == 0)
                return "Farm Not Updated - A Farm with that ID Doesn't Exist";

            //--Tabla Datos Casos Cliente--
            command.Parameters.AddWithValue("@FarmID", _farm.FarmID);
            command.Parameters.AddWithValue("@FarmName", _farm.FarmName);
            command.Parameters.AddWithValue("@FarmHouseNumberStreetAddress", _farm.FarmHouseNumberStreetAddress);
            command.Parameters.AddWithValue("@FarmCity", _farm.FarmCity);
            command.Parameters.AddWithValue("@FarmState", _farm.FarmState);
            command.Parameters.AddWithValue("@FarmCountry", _farm.FarmCountry);
            command.Parameters.AddWithValue("@FarmZipCode", _farm.FarmZipCode);
            command.Parameters.AddWithValue("@FarmLatitude", _farm.FarmLatitude);
            command.Parameters.AddWithValue("@FarmLongitude", _farm.FarmLongitude);
            command.Parameters.AddWithValue("@FarmTemperatureMin", _farm.FarmTemperatureMin);
            command.Parameters.AddWithValue("@FarmTemperatureMax", _farm.FarmTemperatureMax);
            command.Parameters.AddWithValue("@IsActive", _farm.IsActive);

            try
            {
                myConnection.Open();

                command.ExecuteNonQuery();

                myConnection.Close();
            }
            catch (Exception exception)
            {
                return "Farm Not Updated - " + exception.Message;
            }

            return "Farm Updated";
        }

        public DataTable ReadFarmByAnyParameterModel(string _parameterName, string _parameterData)
        {
            SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
            string _dataSource = csBuilder.ToString();

            SqlConnection myConnection = new SqlConnection(_dataSource);

            DataTable _userData = new DataTable();

            String query = "ReadFarmsByAnyParameter";
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
    }


    public class FarmValidator : AbstractValidator<Farm>
    {
        public FarmValidator(string _actionType)
        {

            if (_actionType == "Update")
            {
                RuleFor(farm => farm.FarmID.ToString()).NotEmpty().WithMessage("The Farm ID Must NOT be Empty or Null").
                                           Matches("([0-9]){1,9}").
                                           WithMessage("The Farm ID should only contain numbers").
                                           Length(1,9).
                                           WithMessage("The Farm ID  should contain a maximum of 9 Digits");
            }

            RuleFor(farm => farm.FarmName).NotEmpty().
                                           WithMessage("The Farm Name Must NOT be Empty or Null").
                                           Matches(@"([a-zA-Z_À-ÿ]|[ ]|[´]|[.]|[,]|[0-9]){1,200}").
                                           WithMessage("Farm Name should only contain letters and numbers").
                                           Length(1, 200).
                                           WithMessage("Farm Name should contain at least 1 character and maximum 200");

            RuleFor(farm => farm.FarmHouseNumberStreetAddress).NotEmpty().
                                           WithMessage("The Farm Address Must NOT be Empty or Null").
                                           Matches(@"([a-zA-Z_À-ÿ]|[ ]|[´]|[.]|[,]|[0-9]){1,250}").
                                           WithMessage("Farm Address should only contain letters and numbers").
                                           Length(1, 250).
                                           WithMessage("Farm Name should contain at least 1 character and maximum 250");

            RuleFor(farm => farm.FarmCity).NotEmpty().
                                           WithMessage("The Farm City Must NOT be Empty or Null").
                                           Matches(@"([a-zA-Z_À-ÿ]|[ ]){1,50}").
                                           WithMessage("The Farm City should only contain letters").
                                           Length(1, 50).
                                           WithMessage("Farm City should contain at least 1 character and maximum 50");

            RuleFor(farm => farm.FarmState).NotEmpty().
                                           WithMessage("The Farm State Must NOT be Empty or Null").
                                           Matches(@"([a-zA-Z_À-ÿ]|[ ]){1,50}").
                                           WithMessage("The Farm State should only contain letters").
                                           Length(1, 50).
                                           WithMessage("The Farm State should contain at least 1 character and maximum 50");

            RuleFor(farm => farm.FarmCountry).NotEmpty().
                                           WithMessage("The Farm Country Must NOT be Empty or Null").
                                           Matches(@"([a-zA-Z_À-ÿ]|[ ]){1,50}").
                                           WithMessage("The Farm Country should only contain letters").
                                           Length(1, 50).
                                           WithMessage("The Farm Country should contain at least 1 character and maximum 50");
            
            RuleFor(farm => farm.FarmZipCode.ToString()).NotEmpty().
                                           WithMessage("The Farm Zip Code Must NOT be Empty or Null").
                                           Matches("([0-9]){1,5}").
                                           WithMessage("The Farm Zip Code should only contain numbers").
                                           Length(5).
                                           WithMessage("The Farm Zip Code should contain EXACTLY 5 Digits");
            
            //RuleFor(farm => farm.FarmLatitude).NotEmpty().
            //                               WithMessage("The Farm Latitude Must NOT be Empty or Null").
            //                               Matches(@"^(\+|-)?(?:90(?:(?:\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\.[0-9]{1,6})?))$").
            //                               WithMessage("The Farm Latitude Code should only contain numbers. And make sure it is valid").
            //                               Length(1, 20).
            //                               WithMessage("The Farm Latitude should contain at least 1 character and maximum 20");

            //RuleFor(farm => farm.FarmLongitude).NotEmpty().
            //                   WithMessage("The Farm Longitude Must NOT be Empty or Null").
            //                   Matches(@"^(\+|-)?(?:180(?:(?:\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\.[0-9]{1,6})?))$").
            //                   WithMessage("The Farm Longitude Code should only contain numbers. And make sure it is valid").
            //                   Length(1, 20).
            //                   WithMessage("The Farm Longitude should contain at least 1 character and maximum 20");

            RuleFor(farm => farm.FarmTemperatureMin).NotEmpty().
                                           WithMessage("The Farm Minimum Temperature Must NOT be Empty or Null").
                                           Matches("([0-9]){1,3}").
                                           WithMessage("The Farm Minimum Temperature should only numbers and must be an integer").
                                           Length(1,3).
                                           WithMessage("The Farm Farm Minimum Temperature should contain at least 1 character and maximum 3");

            RuleFor(farm => farm.FarmTemperatureMax).NotEmpty().
                                           WithMessage("The Farm Maximum Temperature Must NOT be Empty or Null").
                                           Matches("([0-9]){1,3}").
                                           WithMessage("The Farm Maximum Temperature should only numbers and must be an integer").
                                           Length(1, 3).
                                           WithMessage("The Farm Farm Maximum Temperature should contain at least 1 character and maximum 3");
            
            RuleFor(farm => farm.IsActive).NotEmpty().
                                           WithMessage("The Farm Is Active Attribute Must NOT be Empty or Null").
                                           Matches("[01]").
                                           WithMessage("The Farm Is Active Attribute should only be 0 (Zero) or 1 (One)").
                                           Length(1).
                                           WithMessage("The Farm Is Active Attribute should contain exactly ONE character");

            //RuleFor(farm => farm.FarmOwner).NotEmpty().
            //                               WithMessage("The Farm Owner Must NOT be Empty or Null").
            //                               Matches(@"([a-zA-Z_À-ÿ]|[ ]){1,50}").
            //                               WithMessage("The Farm Owner should only contain letters").
            //                               Length(1, 100).
            //                               WithMessage("The Farm Owner should contain at least 1 character and maximum 100");
        }

        private bool BeAValidPostcode(string postcode)
        {
            // custom postcode validating logic goes here

            return true;
        }
    }


}
