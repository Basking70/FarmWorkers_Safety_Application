using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FarmworkersWebAPI.Models
{
    public class Notifications
    {
        public DataTable GetListOfFarmsModel()
        {
            DataTable _dataBaseResults = new DataTable();
            try
            {
                SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
                string _dataSource = csBuilder.ToString();

                string _queryString = "Exec ReadAllFarms";


                SqlDataAdapter _sqlAdapter = new SqlDataAdapter(_queryString, _dataSource);
                _sqlAdapter.Fill(_dataBaseResults);
            }
            catch (Exception _exceptionData)
            {

            }

            return _dataBaseResults;
        }

        public DataTable ReadAllDifferentFarmsZipCodesModel()
        {
            DataTable _dataBaseResults = new DataTable();
            try
            {
                SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["FarmWorkerAppDB"].ConnectionString);
                string _dataSource = csBuilder.ToString();

                string _queryString = "Exec NotificationsReadAllDifferentZipCodes";


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