using FarmworkersWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FluentValidation;
using FarmworkersWebAPI.ViewModels;

namespace FarmworkersWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FarmsController : ApiController
    {
        //***Entity Framework**//

        //NOTE: Watch out for conflicts, between: 
        //**FarmworkersWebAPI.Entities
        //**FarmworkersWebAPI.Models

        FarmworkersWebAPI.Entities.FarmWorkerAppContext _context;

        public FarmsController()
        {
            _context = new FarmworkersWebAPI.Entities.FarmWorkerAppContext();
            _context.Configuration.LazyLoadingEnabled = false;
        }

        [HttpGet]
        public IHttpActionResult EFGetListOfFarmsController()
        {
            //_context.Configuration.ProxyCreationEnabled = false;
            List<FarmworkersWebAPI.Entities.Farm> _farmsData = new List<FarmworkersWebAPI.Entities.Farm>();
            _context.Configuration.ProxyCreationEnabled = false;
            try
            {
                _farmsData = _context.Farms
                    .Include(x=>x.UserFarms.Select(c=>c.User))
                    .ToList();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            
            return Ok(AutoMapper.Mapper.Map<IList<FarmDTO>>(_farmsData));
        }
      
        [HttpPost]
        public IHttpActionResult EFInsertFarm(FarmworkersWebAPI.Entities.Farm _farmInformation)
        {
            //Farm Owner Name and Email was Removed from this Section
            //That information will be Inserted in the USER Table

            string _inputValidationOutCome = EFFarmInputValidation(_farmInformation, "Insert");

            if (_inputValidationOutCome != "Success")
                return Ok(_inputValidationOutCome);

            _context.Configuration.ProxyCreationEnabled = false;

            FarmworkersWebAPI.Entities.Farm _farmData = new FarmworkersWebAPI.Entities.Farm();

            try
            {
                _farmData = _context.Farms.Add(_farmInformation);
                _context.SaveChanges();

                return Ok(_farmData);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        public IHttpActionResult EFUpdateFarm(FarmworkersWebAPI.Entities.Farm _farmInformation)
        {
            //Farm Owner Name and Email was Removed from this Section
            //That information will be Updated in the USER Table

            string _inputValidationOutCome = EFFarmInputValidation(_farmInformation, "Insert");

            if (_inputValidationOutCome != "Success")
                return Ok(_inputValidationOutCome);

            _context.Configuration.ProxyCreationEnabled = false;

            try
            {

                FarmworkersWebAPI.Entities.Farm original = _context.Farms.
                                                        Where(f => f.FarmID.Equals(_farmInformation.FarmID)).FirstOrDefault();

                FarmworkersWebAPI.Entities.Farm _updated = new FarmworkersWebAPI.Entities.Farm();

                if (original != null)
                {
                    _context.Entry(original).CurrentValues.SetValues(_farmInformation);
                    _context.SaveChanges();

                    _updated = _context.Farms.Find(_farmInformation.FarmID);

                    return Ok(_updated);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok("Farm Not Found");
        }

        [HttpPost]
        public IHttpActionResult EFDeleteFarm(FarmworkersWebAPI.Entities.Farm _farmInformation)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            try
            {
                var dbFarm = _context.Farms.Find(_farmInformation.FarmID);
                _context.Farms.Remove(dbFarm);
                _context.SaveChanges();

                return Ok("Farm Deleted Successfully");

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        public IHttpActionResult EFGetFarmByAnyParamaterController(string _parameterName, string _parameterData, string _exactMatch)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            List<FarmworkersWebAPI.Entities.Farm> _farmData = new List<FarmworkersWebAPI.Entities.Farm>();

            string _matchCriteria = _exactMatch == "Yes" ? " = " + _parameterData : " LIKE '%" + _parameterData + "%' ";

            string _selectClause = "SELECT * ";
            string _fromClause = "FROM Farm ";
            string _whereClause = "WHERE " + _parameterName + _matchCriteria;
            string query = _selectClause + _fromClause + _whereClause;

            try
            {

                _farmData = _context.Farms.SqlQuery(query).ToList();

                return Ok(_farmData);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public string EFFarmInputValidation(FarmworkersWebAPI.Entities.Farm _farmInformation, string _actionType)
        {
            Farm _farm = new Farm();
            FarmworkersWebAPI.Entities.Farm.FarmValidator validator = new FarmworkersWebAPI.Entities.Farm.FarmValidator(_actionType);
            FluentValidation.Results.ValidationResult results = validator.Validate(_farmInformation);

            bool validationSucceeded = results.IsValid;
            IList<FluentValidation.Results.ValidationFailure> failures = results.Errors;

            string _errorMessage = "";

            if (!results.IsValid)
            {

                foreach (var failure in results.Errors)
                {

                    _errorMessage += "Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage + " " + Environment.NewLine;
                }

            }

            else
            {
                _errorMessage = "Success";
            }

            return _errorMessage;
        }


        //*********************//

        [HttpGet]
        public IEnumerable<Farm> GetListOfFarmsController()
        {
            Farm _farmInstance = new Farm();
            DataTable _farmsDataFromDataBase = new DataTable();

            _farmsDataFromDataBase = _farmInstance.GetListOfFarmsModel();

            Farm[] _listOfFarms = new Farm[_farmsDataFromDataBase.Rows.Count];

            int farmCounter = 0;

            foreach (DataRow _farmRow in _farmsDataFromDataBase.Rows)
            {
                

                Farm _farm = new Farm { FarmID = int.Parse(_farmRow["FarmID"].ToString()),
                                        FarmName = _farmRow["FarmName"].ToString(),
                                        FarmHouseNumberStreetAddress = _farmRow["FarmHouseNumberStreetAddress"].ToString(),
                                        FarmCity = _farmRow["FarmCity"].ToString(),
                                        FarmState = _farmRow["FarmState"].ToString(),
                                        FarmCountry = _farmRow["FarmCountry"].ToString(),
                                        FarmZipCode = int.Parse(_farmRow["FarmZipCode"].ToString()),
                                        FarmLatitude = _farmRow["FarmLatitute"].ToString(),
                                        FarmLongitude = _farmRow["FarmLongitude"].ToString(),
                                        FarmTemperatureMin = _farmRow["FarmTemperatureMin"].ToString(),
                                        FarmTemperatureMax = _farmRow["FarmTemperatureMax"].ToString(), 
                                        IsActive = _farmRow["IsActive"].ToString(),
                                        NumberOfFarmWorkers = int.Parse(_farmRow["NumberOfFarmWorkers"].ToString()),
                                        FarmOwner = _farmRow["FarmOwner"].ToString()
                };

                _listOfFarms[farmCounter] = _farm;

                farmCounter++;
            }

            return _listOfFarms;
        }

        [HttpPost]
        public IHttpActionResult InsertFarm(Farm _farmInformation)
        {

            string _inputValidationOutCome = FarmInputValidation(_farmInformation, "Insert");

            if (_inputValidationOutCome != "Success")
                return Ok(_inputValidationOutCome);

            Farm _farmReceived = new Farm();

            if(!_farmReceived.InsertFarmModel(_farmInformation))
            {
                return Ok("Farm Not Added");
            }

            return Ok("Farm Successfully Added");
        }

        [HttpPost]
        public IHttpActionResult UpdateFarm(Farm _farmInformation)
        {
            string _inputValidationOutCome = FarmInputValidation(_farmInformation, "Update");

            if (_inputValidationOutCome != "Success")
                return Ok(_inputValidationOutCome);

            Farm _farmReceived = new Farm();

            return Ok(_farmReceived.UpdateFarmModel(_farmInformation));
        }

        [HttpDelete]
        public string DeleteFarm(string _parameterName, string _parameterData)
        {
            Farm _farmInstance = new Farm();

            return _farmInstance.DeleteFarmModel(_parameterName, _parameterData).ToString();
        }

        [HttpGet]
        public IEnumerable<Farm> GetFarmByAnyParamaterController(string _parameterName, string _parameterData)
        {
            Farm _farmInstance = new Farm();
            DataTable _farmsDataFromDataBase = new DataTable();

            _farmsDataFromDataBase = _farmInstance.ReadFarmByAnyParameterModel(_parameterName, _parameterData);

            Farm[] _listOfFarms = new Farm[_farmsDataFromDataBase.Rows.Count];

            int farmCounter = 0;

            foreach (DataRow _farmRow in _farmsDataFromDataBase.Rows)
            {


                Farm _farm = new Farm
                {
                    FarmID = int.Parse(_farmRow["FarmID"].ToString()),
                    FarmName = _farmRow["FarmName"].ToString(),
                    FarmHouseNumberStreetAddress = _farmRow["FarmHouseNumberStreetAddress"].ToString(),
                    FarmCity = _farmRow["FarmCity"].ToString(),
                    FarmState = _farmRow["FarmState"].ToString(),
                    FarmCountry = _farmRow["FarmCountry"].ToString(),
                    FarmZipCode = int.Parse(_farmRow["FarmZipCode"].ToString()),
                    FarmLatitude = _farmRow["FarmLatitute"].ToString(),
                    FarmLongitude = _farmRow["FarmLongitude"].ToString(),
                    FarmTemperatureMin = _farmRow["FarmTemperatureMin"].ToString(),
                    FarmTemperatureMax = _farmRow["FarmTemperatureMax"].ToString(),
                    IsActive = _farmRow["IsActive"].ToString(),
                    NumberOfFarmWorkers = int.Parse(_farmRow["NumberOfFarmWorkers"].ToString()),
                    FarmOwner = _farmRow["FarmOwner"].ToString()
                };

                _listOfFarms[farmCounter] = _farm;

                farmCounter++;
            }

            return _listOfFarms;
        }

        public string FarmInputValidation(Farm _farmInformation, string _actionType)
        {
            Farm _farm = new Farm();
            FarmValidator validator = new FarmValidator(_actionType);
            FluentValidation.Results.ValidationResult results = validator.Validate(_farmInformation);

            bool validationSucceeded = results.IsValid;
            IList<FluentValidation.Results.ValidationFailure> failures = results.Errors;

            string _errorMessage = "";

            if (!results.IsValid)
            {
                
                foreach (var failure in results.Errors)
                {
                    
                    /*"Property " + failure.PropertyName + " failed validation. Error was: " */
                    _errorMessage +=  failure.ErrorMessage + " <br> " ;
                }
             
            }

            else
            {
                _errorMessage = "Success";
            }

            return _errorMessage;
        }

    }
}
