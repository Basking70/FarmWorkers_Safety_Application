using FarmworkersWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;
using FarmworkersWebAPI.ViewModels;
using System.Data.Entity.Infrastructure;

namespace FarmworkersWebAPI.Controllers
{
    public class NotificationsController : ApiController
    {
        //***Entity Framework**//

        FarmworkersWebAPI.Entities.FarmWorkerAppContext _context;

        public NotificationsController()
        {
            _context = new FarmworkersWebAPI.Entities.FarmWorkerAppContext();
        }

        [HttpGet]
        public IHttpActionResult ScheduledEducationalContentNotifications()
        {
            _context.Configuration.ProxyCreationEnabled = false;

            List<string> _listOfMessages = new List<string>();

            try
            {

                var _userList = _context.Users.Where(u => u.UserType.Equals("Farm Worker")).ToList();
                var _educationalContentList = _context.EducationalContents.ToList();

                foreach (FarmworkersWebAPI.Entities.User _userRow in _userList)
                {
                    FarmworkersWebAPI.Entities.UserCommunicationPreference _userComPreferences = _context.UserCommunicationPreferences.Where(ucp => ucp.UserID.Equals(_userRow.UserID)).FirstOrDefault();
                    string _userFrequencyPreference = "";

                    if (_userComPreferences != null) /*&& (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)*/
                        _userFrequencyPreference = _userComPreferences.UserCommunicationPreferenceFrequency;

                    if (_userFrequencyPreference == "Daily" ||
                        _userFrequencyPreference == "Weekly"  && (DateTime.Now.DayOfWeek == DayOfWeek.Sunday) ||
                        _userFrequencyPreference == "Monthly" && (DateTime.Now.Day == 1) ||
                        _userFrequencyPreference == ""  && (DateTime.Now.DayOfWeek == DayOfWeek.Sunday))
                    {

                        List<FarmworkersWebAPI.Entities.EducationalContent> _educationalContentsMisssing = new List<FarmworkersWebAPI.Entities.EducationalContent>();

                        string _query =
                        "SELECT E.IDEducationalContent, E.LinkEducationalContent, E.TypeEducationalContent, E.DescriptionEducationalContent, E.DateEducationalContentCreated, E.IsActive " +
                        "FROM EducationalContent AS E " +
                        "LEFT JOIN(SELECT * FROM UserEducationalContent AS UE WHERE UE.UserID = " + _userRow.UserID + ") AS SUBUE " +
                        "ON E.IDEducationalContent = SUBUE.EducationalContentID " +
                        "WHERE EducationalContentID IS NULL ";

                        _educationalContentsMisssing = _context.EducationalContents.SqlQuery(_query).ToList();

                        int counter = 0;
                        string _message = "";

                        if (_educationalContentsMisssing.Count > 0)
                        {
                            if (counter == 0)
                                _message = "Please take a look of these content available in the Farm Workers Safety Application: " + Environment.NewLine;

                            foreach (FarmworkersWebAPI.Entities.EducationalContent _educationalContentRow in _educationalContentsMisssing)
                            {

                                _message = _message + _userRow.UserID + " " + _userRow.UserPhoneNumber + " " + _educationalContentRow.DescriptionEducationalContent + ": " + Environment.NewLine +
                                                      _educationalContentRow.LinkEducationalContent + Environment.NewLine + Environment.NewLine;


                                counter++;
                            }

                            _listOfMessages.Add(_message);
                            //sendTextMessage(_userRow.UserPhoneNumber, _message);

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(_listOfMessages);
        }

        //*********************//

        public IEnumerable<Farm> GetListOfFarmsController()
        {
            Farm _farmInstance = new Farm();
            DataTable _farmsDataFromDataBase = new DataTable();

            _farmsDataFromDataBase = _farmInstance.GetListOfFarmsModel();

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
                    IsActive = _farmRow["IsActive"].ToString()
                };

                _listOfFarms[farmCounter] = _farm;

                farmCounter++;
            }

            return _listOfFarms;
        }

        [HttpGet]
        public IHttpActionResult ScheduledWeatherForeCastNotifications()
        {
                     
            DataTable _listOfZipCodesDataFromDataBase = new DataTable();
            Notifications _notificationsInstance = new Notifications();

            Farm[] _listOfFarms = (Farm[]) GetListOfFarmsController();

            _listOfZipCodesDataFromDataBase = _notificationsInstance.ReadAllDifferentFarmsZipCodesModel();

            int _numberOfZipCodes = _listOfZipCodesDataFromDataBase.Rows.Count;

            int counter = 0;

            string[,] _weatherPerZipcode = new string[_numberOfZipCodes, 7]; //This will hold the Zip Code, with the Daily Temperatures

            foreach (DataRow _farm in _listOfZipCodesDataFromDataBase.Rows)
            {            
                string[] _weatherInformation = DailyFetchForeCastInformationByLocation(_farm["FarmZipCode"].ToString(), _farm["FarmCountry"].ToString(), "imperial", "en");

                string _currentTemperature = _weatherInformation[0];
                string _minTemperature = _weatherInformation[1];
                string _maxTemperature = _weatherInformation[2];
                string _morningTemperature = _weatherInformation[3];
                string _eveningTemperature = _weatherInformation[4];
                string _nightTemperature = _weatherInformation[5];

                string city = _weatherInformation[1];

                _weatherPerZipcode[counter, 0] = _farm["FarmZipCode"].ToString();
                _weatherPerZipcode[counter, 1] = _currentTemperature;
                _weatherPerZipcode[counter, 2] = _minTemperature;
                _weatherPerZipcode[counter, 3] = _maxTemperature;
                _weatherPerZipcode[counter, 4] = _morningTemperature;
                _weatherPerZipcode[counter, 5] = _eveningTemperature;
                _weatherPerZipcode[counter, 6] = _nightTemperature;

                counter++;
            }

            if ((_weatherPerZipcode.Length/2) > 0)
            {

                for (int i = 0; i < _numberOfZipCodes; i++)
                {
                    string _zipCode = _weatherPerZipcode[i, 0];
                    string _currentTemperature = _weatherPerZipcode[i, 1];
                    string _minTemperature = _weatherPerZipcode[i, 2];
                    string _maxTemperature = _weatherPerZipcode[i, 3];
                    string _morningTemperature = _weatherPerZipcode[i, 4];
                    string _eveningTemperature = _weatherPerZipcode[i, 5];
                    string _nightTemperature = _weatherPerZipcode[i, 6];

                    foreach (Farm _farm in _listOfFarms)
                    {
                        if (_farm.FarmZipCode.ToString() == _zipCode)
                        {
                            _farm.FarmWeather = new string[6];

                            _farm.FarmWeather[0] = _currentTemperature;
                            _farm.FarmWeather[1] = _minTemperature;
                            _farm.FarmWeather[2] = _maxTemperature;
                            _farm.FarmWeather[3] = _morningTemperature;
                            _farm.FarmWeather[4] = _eveningTemperature;
                            _farm.FarmWeather[5] = _nightTemperature;

                        }
                    }
                }

                DataTable _listOfUsers = new DataTable();
                User _userInstance = new Models.User();

                _listOfUsers = _userInstance.GetListOfAllUsers();

                foreach(DataRow _user in _listOfUsers.Rows)
                {
                    string _userFarmID = _user["FarmID"].ToString();
                    string _userPhoneNumber = _user["UserPhoneNumber"].ToString();

                    foreach (Farm _farm in _listOfFarms)
                    {
                        if (_farm.FarmID.ToString() == _userFarmID)
                        {
                            string _message = buildForeCastMessage(_farm, "english");
                            sendTextMessage(_userPhoneNumber, _message);

                            System.Threading.Thread.Sleep(1000);

                        }
                    }

                }
            }

            return Ok("Method Executed Succesfully");

        }

        [HttpGet]
        public IHttpActionResult ScheduledCurrentWeatherWarningNotifications()
        {

            DataTable _listOfZipCodesDataFromDataBase = new DataTable();
            Notifications _notificationsInstance = new Notifications();

            Farm[] _listOfFarms = (Farm[])GetListOfFarmsController();

            _listOfZipCodesDataFromDataBase = _notificationsInstance.ReadAllDifferentFarmsZipCodesModel();

            int _numberOfZipCodes = _listOfZipCodesDataFromDataBase.Rows.Count;

            int counter = 0;

            string[,] _weatherPerZipcode = new string[_numberOfZipCodes, 7]; //This will hold the Zip Code, with the Daily Temperatures

            foreach (DataRow _farm in _listOfZipCodesDataFromDataBase.Rows)
            {
                string[] _weatherInformation =  FetchWeatherInformationByLocation(_farm["FarmZipCode"].ToString(), _farm["FarmCountry"].ToString(), "imperial", new string[] {"temperature", "city" }, "en");

                string _currentTemperature = _weatherInformation[0];
                //string _city = _weatherInformation[1];

                string city = _weatherInformation[1];

                _weatherPerZipcode[counter, 0] = _farm["FarmZipCode"].ToString();
                _weatherPerZipcode[counter, 1] = _currentTemperature;
                counter++;
            }

            if ((_weatherPerZipcode.Length / 2) > 0)
            {

                for (int i = 0; i < _numberOfZipCodes; i++)
                {
                    string _zipCode = _weatherPerZipcode[i, 0];
                    string _currentTemperature = _weatherPerZipcode[i, 1];

                    foreach (Farm _farm in _listOfFarms)
                    {
                        if (_farm.FarmZipCode.ToString() == _zipCode)
                        {
                            _farm.FarmWeather = new string[6];

                            _farm.FarmWeather[0] = _currentTemperature;

                        }
                    }
                }

                DataTable _listOfUsers = new DataTable();
                User _userInstance = new Models.User();

                _listOfUsers = _userInstance.GetListOfAllUsers();

                foreach (DataRow _user in _listOfUsers.Rows)
                {
                    string _userFarmID = _user["FarmID"].ToString();
                    string _userPhoneNumber = _user["UserPhoneNumber"].ToString();

                    foreach (Farm _farm in _listOfFarms)
                    {
                        if (_farm.FarmID.ToString() == _userFarmID)
                        {
                            string _message = "";

                            if (_farm.FarmTemperatureMax != null && _farm.FarmTemperatureMax != "" && _farm.FarmWeather[0] != "")
                            {
                                if (float.Parse(_farm.FarmWeather[0]) >= float.Parse(_farm.FarmTemperatureMax))
                                {
                                    _message = _message = buildCurrentWeatherMessage(_farm, "heat", "bilingual");

                                    sendTextMessage(_userPhoneNumber, _message);
                                    System.Threading.Thread.Sleep(500);
                                    _message = "'<audio src='http://clinicajuridicaucab.com/Voice%20007_sd.mp3'/>'";

                                    //makeWarningCall(_userPhoneNumber, _message);
                                }

                            }

                            if (_farm.FarmTemperatureMin != null && _farm.FarmTemperatureMin != "" && _farm.FarmWeather[0] != "")
                            {
                                if (float.Parse(_farm.FarmWeather[0]) <= float.Parse(_farm.FarmTemperatureMin))
                                {
                                    _message = _message = buildCurrentWeatherMessage(_farm, "heat", "bilingual");

                                    sendTextMessage(_userPhoneNumber, _message);
                                    System.Threading.Thread.Sleep(500);
                                    _message = "'<audio src='http://clinicajuridicaucab.com/Voice%20007_sd.mp3'/>'";
                                    //makeWarningCall(_userPhoneNumber, _message);
                                }

                            }
                                



                        }
                    }

                }
            }

            return Ok("Method Executed Succesfully");

        }

        public string buildForeCastMessage(Farm _farm, string language)
        {
            string _temperatureForecastTag = "";
            string _locatedInTag = "";
            string _currentTemperatureTag = "";
            string _morningTemperatureTag = "";
            string _eveningTemperatureTag = "";
            string _nightTemperatureTag = "";
            string _maxTemperatureTag = "";
            string _minTemperatureTag = "";

            string _hotTemperatureMessageTag = "";
            string _coldTemperatureMessageTag = "";

            string _todaysDateTag = "";

            string _todaysDayOfWeekTag = "";
            string _todaysMonthTag = "";

            if (language == "english")
            {
                _temperatureForecastTag = "Temperature Forecast of Farm: ";
                _locatedInTag = "Located in: ";
                _currentTemperatureTag = "Current Temperature: ";
                _morningTemperatureTag = "Morning Temperature: ";
                _eveningTemperatureTag = "Evening Temperature: ";
                _nightTemperatureTag = "Night Temperature: ";
                _maxTemperatureTag = "MAX. Temperature: ";
                _minTemperatureTag = "MIN. Temperature: ";

                _hotTemperatureMessageTag = "**TODAY THE WEATHER IS GOING TO BE VERY HOT**" + Environment.NewLine +
                                          "**PLEASE TAKE APPROPIATE PRECAUTIONS**" +
                                          Environment.NewLine +
                                          Environment.NewLine;

                _coldTemperatureMessageTag = "**TODAY THE WEATHER IS GOING TO BE VERY COLD**" + Environment.NewLine +
                                          "**PLEASE TAKE APPROPIATE PRECAUTIONS**" +
                                          Environment.NewLine +
                                          Environment.NewLine;

                _todaysDateTag = "Today's Date: ";
                _todaysDayOfWeekTag = DateTime.Now.ToString("dddd", new CultureInfo("en"));
                _todaysMonthTag = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("en"));

            }

            if (language == "spanish")
            {
                _temperatureForecastTag = "Pronósticos de Temperatura de la Granja: ";
                _locatedInTag = "Localizada en: ";
                _currentTemperatureTag = "Temperatura Actual: ";
                _morningTemperatureTag = "Temperatura en la Mañana: ";
                _eveningTemperatureTag = "Temperatura en la Tarde: ";
                _nightTemperatureTag = "Temperatura en la Noche: ";
                _maxTemperatureTag = "Temperatura Máxima: ";
                _minTemperatureTag = "Temperatura Mínima: ";

                _hotTemperatureMessageTag = "**EL DÍA DE HOY EL CLIMA ESTARÁ MUY CALIENTE**" + Environment.NewLine +
                                          "**POR FAVOR TOMAR LAS PRECAUCIONES NECESARIAS**" +
                                          Environment.NewLine +
                                          Environment.NewLine;

                _coldTemperatureMessageTag = "**EL DÍA DE HOY EL CLIMA ESTARÁ MUY FRÍO**" + Environment.NewLine +
                                          "**POR FAVOR TOMAR LAS PRECAUCIONES NECESARIAS**" +
                                          Environment.NewLine +
                                          Environment.NewLine;

                _todaysDateTag = "Fecha de Hoy: ";
                _todaysDayOfWeekTag = DateTime.Now.ToString("dddd", new CultureInfo("es"));
                _todaysMonthTag = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));


            }

            string   _message = _temperatureForecastTag + _farm.FarmName + ", " +
                      Environment.NewLine +
                      _locatedInTag + _farm.FarmCity + ", " + _farm.FarmCountry +
                      Environment.NewLine +
                      Environment.NewLine +
                      _currentTemperatureTag + _farm.FarmWeather[0] + " F." +
                      Environment.NewLine +
                      _morningTemperatureTag + _farm.FarmWeather[3] + " F." +
                      Environment.NewLine +
                      _eveningTemperatureTag + _farm.FarmWeather[4] + " F." +
                      Environment.NewLine +
                      _nightTemperatureTag + _farm.FarmWeather[5] + " F." +
                      Environment.NewLine +
                      Environment.NewLine +
                      _maxTemperatureTag + _farm.FarmWeather[2] + " F." +
                      Environment.NewLine +
                      _minTemperatureTag + _farm.FarmWeather[1] + " F." +
                      Environment.NewLine +
                      Environment.NewLine;

                if (_farm.FarmTemperatureMax != null && (float.Parse(_farm.FarmWeather[2]) >= float.Parse(_farm.FarmTemperatureMax)))
                {
                _message = _message + _hotTemperatureMessageTag;

                }


                if (_farm.FarmTemperatureMin != null && (float.Parse(_farm.FarmWeather[1]) <= float.Parse(_farm.FarmTemperatureMin)))
                {
                _message = _message + _coldTemperatureMessageTag;

                }

                _message = _message +
                           _todaysDateTag + _todaysDayOfWeekTag  + ", " + _todaysMonthTag + " " + DateTime.Now.Day + ", " + DateTime.Now.Year;
            



            return _message;

        }

        public string buildCurrentWeatherMessage(Farm _farm, string _typeOfWarning, string language)
        {

            string _hotTemperatureMessageTag = "";
            string _coldTemperatureMessageTag = "";

            string _todaysDateTag = "";

            string _todaysDayOfWeekTag = "";
            string _todaysMonthTag = "";

            if (language == "bilingual")
            {

                _hotTemperatureMessageTag = "**WARNING: THE CURRENT TEMPERATURE IS VERY HIGH!!**" + Environment.NewLine +
                                            "**PLEASE TAKE SHELTER IN THE SHADE, HAVE APPROPIATE BREAKS AND DRINK ENOUGH WATER!!**" +
                                             Environment.NewLine +
                                             Environment.NewLine +
                                             "**ALERTA: LA TEMPERATURA ACTUAL ESTÁ MUY ALTA!!**" + Environment.NewLine +
                                            "**POR FAVOR TOME REFUGIO EN LA SOMBRA, TOME LOS DESCANSOS NECESARIOS Y BEBE SUFICIENTE AGUA!!**" +
                                            Environment.NewLine +
                                            Environment.NewLine;

                _coldTemperatureMessageTag = "**WARNING: THE CURRENT TEMPERATURE IS VERY LOW!!**" + Environment.NewLine +
                                            "**PLEASE TAKE THE APPROPIATE PRECAUTIONS**" +
                                             Environment.NewLine +
                                             Environment.NewLine +
                                             "**ALERTA: LA TEMPERATURA ACTUAL FRIA ESTÁ MUY FRIA!!**" + Environment.NewLine +
                                             "**POR FAVOR TOMAR LAS PRECAUCIONES NECESARIAS**" +
                                             Environment.NewLine +
                                             Environment.NewLine;


                _todaysDateTag = "Today's Date: ";
                _todaysDayOfWeekTag = DateTime.Now.ToString("dddd", new CultureInfo("en"));
                _todaysMonthTag = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("en"));

            }



            //if (language == "spanish")
            //{

            //    _hotTemperatureMessageTag = "**LA TEMPERATURA ACTUAL ESTÁ MUY ALTA!!**" + Environment.NewLine +
            //                                "**POR FAVOR TOME REFUGIO EN LA SOMBRA, TOME LOS DESCANSOS NECESARIOS Y BEBE SUFICIENTE AGUA!!**" +
            //                                Environment.NewLine +
            //                                Environment.NewLine;

            //    _coldTemperatureMessageTag = "**LA TEMPERATURA ACTUAL FRIA ESTÁ MUY FRIA!!**" + Environment.NewLine +
            //                                 "**POR FAVOR TOMAR LAS PRECAUCIONES NECESARIAS**" +
            //                                 Environment.NewLine +
            //                                 Environment.NewLine;

            //    _todaysDateTag = "Fecha de Hoy: ";
            //    _todaysDayOfWeekTag = DateTime.Now.ToString("dddd", new CultureInfo("es"));
            //    _todaysMonthTag = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));


            //}



            string _message = "";

            if(_typeOfWarning == "heat")
            {
                _message = _hotTemperatureMessageTag;
            }
            else if(_typeOfWarning == "cold")
            {
                _message = _coldTemperatureMessageTag;
            }

                return _message;

        }

        [HttpPost]
        public IHttpActionResult HandleIncomingSMS(NexmoInComingMessage _incomingSMS)
        {

            string _message = "";
            UsersController _userController = new UsersController();
            User _userInstance = new User();

            _incomingSMS.text = _incomingSMS.text.ToString().ToUpperInvariant();

            if (_incomingSMS.text.ToString() == "HI" || _incomingSMS.text.ToString() == "HELLO")
            {
                _message = "Hello! " + Environment.NewLine +
                           "Thanks for texting the Farmworker Safety System." + Environment.NewLine +
                           "Please text the code of the farm you wish to be registered in." + Environment.NewLine +
                           "For Example: F1.";

            }
            else if (_incomingSMS.text.ToString() == "HOLA")
            {
                _message = "Hola! " + Environment.NewLine +
                           "Gracias por usar el Farmworker Safety System." + Environment.NewLine +
                           "Por favor envie el código de la granja en la que desea ser registrado." + Environment.NewLine +
                           "Por Ejemplo: G1.";
            }
            else if (_incomingSMS.text.ToString() == "REMOVE")
            {
                User[] _user = _userController.ReadUserAnyParameter("UserPhoneNumber", _incomingSMS.msisdn);
                if (_user != null)
                {
                    if (_user.Length > 0)
                    {

                        _message = "Are you sure you wish to be removed from the notifications list of the Farmworker Safety System?" + Environment.NewLine + Environment.NewLine +
                                   "If yes, reply with the phrase: REMOVE NOW";
                    }
                    else
                    {
                        _message = "You are not currently registered in the Farmworker Safety System.";
                    }
                }
            }

            else if (_incomingSMS.text.ToString() == "REMOVER")
            {
                User[] _user = _userController.ReadUserAnyParameter("UserPhoneNumber", _incomingSMS.msisdn);
                if (_user != null)
                {
                    if (_user.Length > 0)
                    {
                        _message = "Está seguro que desea ser removido de la lista de notificaciones del Farmworker Safety System?" + Environment.NewLine + Environment.NewLine +
                                    "Si es así, responda con la frase: REMOVER YA";
                    }
                    else
                    {
                        _message = "Usted no se encuentra registrado en el Farmworker Safety System.";
                    }
                }
            }
            else if (_incomingSMS.text.ToString() == "REMOVE NOW")
            {
                User[] _user = _userController.ReadUserAnyParameter("UserPhoneNumber", _incomingSMS.msisdn);
                if (_user != null)
                {
                    if (_user.Length > 0)
                    {
                        string _dataBaseDeleteResult = _userInstance.DeleteUserFromFarm("UserID", _user[0].UserID.ToString());
                        _message = "You have been removed from the Notifications List of the Farmworkers Safety System";
                    }
                    else
                    {
                        _message = "You are not currently registered in the Farmworker Safety System.";
                    }
                }
            }

            else if (_incomingSMS.text.ToString() == "REMOVER YA")
            {
                User[] _user = _userController.ReadUserAnyParameter("UserPhoneNumber", _incomingSMS.msisdn);
                if (_user != null)
                {
                    if (_user.Length > 0)
                    {
                        string _dataBaseDeleteResult = _userInstance.DeleteUserFromFarm("UserID", _user[0].UserID.ToString());
                        _message = "Usted ha sido removido de la Lista de Notificaciones del Farmworkers Safety System";
                    }
                    else
                    {
                        _message = "Usted no se encuentra registrado en el Farmworker Safety System.";
                    }
                }
            }
            //English Version
            else if (_incomingSMS.text[0].ToString() == "F")
            {
                bool _found = false;

                Farm[] _listOfFarms = (Farm[])GetListOfFarmsController();

                string _trucatedFarmID = _incomingSMS.text.Replace("F", "");
                _trucatedFarmID = _trucatedFarmID.Replace("f", "");
                foreach (Farm _farm in _listOfFarms)
                {
                    if (_farm.FarmID.ToString() == _trucatedFarmID)
                    {
                        _message = "Are you sure you want to register to " + _farm.FarmName + " located in " +
                                   _farm.FarmCity + " , " + _farm.FarmCountry + " and get forecasts and temperature alerts?" + Environment.NewLine + Environment.NewLine +
                                   "If yes, reply the words: YES " + _incomingSMS.text + Environment.NewLine +
                                   "If not, reply with the another Farm's Code you wish to register";

                        _found = true;

                    }

                    if (_found == false)
                    {
                        _message = "The code you entered is incorrect, please try again by entering a valid one.";
                    }
                }
            }

            //Spanish Version
            else if (_incomingSMS.text[0].ToString() == "G")
            {
                bool _found = false;
                Farm[] _listOfFarms = (Farm[])GetListOfFarmsController();

                string _trucatedFarmID = _incomingSMS.text.Replace("G", "");
                _trucatedFarmID = _trucatedFarmID.Replace("g", "");

                foreach (Farm _farm in _listOfFarms)
                {
                    if (_farm.FarmID.ToString() == _trucatedFarmID)
                    {
                        _message = "Está seguro que desea registrarse en " + _farm.FarmName + " localizada en " +
                                   _farm.FarmCity + " , " + _farm.FarmCountry + " y obtener pronósticos diarios y alertas de temperatura?" + Environment.NewLine + Environment.NewLine +
                                    "Si es así, responda con la frase: SI " + _incomingSMS.text + Environment.NewLine + Environment.NewLine +
                                    "Si NO es así, responda con otro código, correspondiente a la granja a la que se desea registrar";

                        _found = true;

                    }
                }

                if (_found == false)
                {
                    _message = "El código ingresado es incorrecto, por favor intente nuevamente con un código válido.";
                }
            }
            else
            {
                //English Version
                bool _found = false;
                int _lenghtOfIncomingMessage = _incomingSMS.text.Length;
                Farm[] _listOfFarms = (Farm[])GetListOfFarmsController();

                if (_lenghtOfIncomingMessage >= 4)
                {
                    if (_incomingSMS.text.Substring(0, 3) == "YES")
                    {
                        string _farmID = "";
                        string _farmCodePrefix = "";
                        try
                        {
                            _farmID = _incomingSMS.text.Substring(5, _lenghtOfIncomingMessage - 5);
                            _farmCodePrefix = _incomingSMS.text.Substring(4, 1);
                        }
                        catch
                        {
                            _message = "The word you entered is not recognized." + Environment.NewLine +
                            "La palabra que ingresaste no es reconocida ";
                            sendTextMessage(_incomingSMS.msisdn, _message);

                            return Ok();
                        }

                        foreach (Farm _farm in _listOfFarms)
                        {
                            if (_farm.FarmID.ToString() == _farmID && (_farmCodePrefix == "F" || _farmCodePrefix == "G"))
                            {
                                _message = "You have been SUCCESSFULLY registered to " + _farm.FarmName + " in " +
                                           _farm.FarmCity + " , " + _farm.FarmCountry + "!" +
                                           Environment.NewLine +
                                           Environment.NewLine;

                                _found = true;

                                string[] _weatherInformation = DailyFetchForeCastInformationByLocation(_farm.FarmZipCode.ToString(), _farm.FarmCountry.ToString(), "imperial", "es");

                                _farm.FarmWeather = _weatherInformation;

                                _message = _message + buildForeCastMessage(_farm, "english");

                            }

                        }

                        if (_found == true)
                        {
                            User[] _user = _userController.ReadUserAnyParameter("UserPhoneNumber", _incomingSMS.msisdn);
                            string _userFarm = "";


                            if (_user != null)
                            {
                                if (_user.Length > 0)
                                {
                                    _userFarm = _user[0].UserFarmID;

                                    if (_userFarm == _farmID)
                                    {
                                        _message = "You are already registered to this Farm." + Environment.NewLine + Environment.NewLine +
                                                   "If you would like to register to a different Farm, please enter a different Farm Code" + Environment.NewLine + Environment.NewLine +
                                                   "Have in mind that you can only be registered in JUST ONE Farm at the same time";

                                    }
                                    else
                                    {

                                        string _dataBaseDeleteResult = _userInstance.DeleteUserFromFarm("UserID", _user[0].UserID.ToString());

                                        string _dataBaseInsertResult = _userInstance.InsertUserInFarmModel(_user[0].UserID.ToString(), _farmID);

                                                //public LoginCredential LoginCredential { get; set; }
                                                //public User User { get; set; }
                                                //public string RegisterType { get; set; } // UserEmail, UserPhoneNumber
                                                //public string UserType { get; set; }



                                        if (_dataBaseInsertResult != "User Added in Farm Successfully")
                                        {


                                            _message = _dataBaseInsertResult;
                                        }
                                        else
                                        {
                                            _message = _message + Environment.NewLine + Environment.NewLine +
                                                "Take into account, you have also been removed from the notifications list of the farm you were previously registered";
                                        }
                                    }
                                }
                                else
                                {
                                    User _newUser = new User();
                                    _newUser.UserPhoneNumber = _incomingSMS.msisdn;

                                    try
                                    {
                                        _newUser.InsertUserModel(_newUser);
                                    }
                                    catch(Exception ex)
                                    {
                                        InternalServerError(ex);
                                    }

                                    _user = _userController.ReadUserAnyParameter("UserPhoneNumber", _incomingSMS.msisdn);

                                    string _dataBaseDeleteResult = _userInstance.DeleteUserFromFarm("UserID", _user[0].UserID.ToString());

                                    string _dataBaseInsertResult = _userInstance.InsertUserInFarmModel(_user[0].UserID.ToString(), _farmID);


                                    if (_dataBaseInsertResult != "User Added in Farm Successfully")
                                    {
                                        _message = _dataBaseInsertResult;
                                    }
                                    else
                                    {

                                        //Entity Framework
                                        LoginCredentialsRegisterForm _newEntry = null;
                                        FarmworkersWebAPI.Entities.LoginCredential _newCredentials = null;

                                        FarmworkersWebAPI.Entities.User _createdUser = _context.Users.Find(_user[0].UserID.ToString());

                                        _newCredentials.UserLoginID = _incomingSMS.msisdn;
                                        _newCredentials.Password = RandomString(8);
                                        _newCredentials.UserID = _createdUser.UserID;
                                        _newCredentials.IsActive = "1";

                                        _newEntry.User = _createdUser;
                                        _newEntry.LoginCredential = _newCredentials;
                                        _newEntry.RegisterType = "UserPhoneNumber";
                                        _newEntry.UserType = "Farm Worker";

                                        try
                                        {
                                            LoginCredentialsController _loginControllerInstance = new LoginCredentialsController();
                                            _loginControllerInstance.RegisterLoginCredentials(_newEntry);
                                        }
                                        catch (Exception ex)
                                        {
                                            InternalServerError(ex);
                                        }
                                        
                                        //****************

                                        _message = _message + Environment.NewLine + Environment.NewLine;
                                    }



                                }

                            }
                        }

                        if (_found == false)
                        {
                            _message = "The code you entered is incorrect, please try again by entering a valid one." + Environment.NewLine +
                                       "Remember reply the words: YES followed by the Farm's code." + Environment.NewLine +
                                       "For Example: YES F1";
                        }

                    }

                    //Spanish Version

                    if (_incomingSMS.text.Substring(0, 2) == "SI")
                    {
                        string _farmID = "";
                        string _farmCodePrefix = "";

                        try
                        {
                            _farmID = _incomingSMS.text.Substring(4, _lenghtOfIncomingMessage - 4);
                            _farmCodePrefix = _incomingSMS.text.Substring(3, 1);
                        }
                        catch
                        {
                            _message = "The word you entered is not recognized." + Environment.NewLine +
                                        "La palabra que ingresaste no es reconocida ";
                            sendTextMessage(_incomingSMS.msisdn, _message);

                            return Ok();
                        }

                        foreach (Farm _farm in _listOfFarms)
                        {
                            if (_farm.FarmID.ToString() == _farmID && (_farmCodePrefix == "F" || _farmCodePrefix == "G"))
                            {
                                _message = "Ha sido registrado EXITOSAMENTE en la granja " + _farm.FarmName + " ubicada en " +
                                           _farm.FarmCity + " , " + _farm.FarmCountry + "!" +
                                           Environment.NewLine +
                                           Environment.NewLine;

                                string[] _weatherInformation = DailyFetchForeCastInformationByLocation(_farm.FarmZipCode.ToString(), _farm.FarmCountry.ToString(), "imperial", "es");

                                _farm.FarmWeather = _weatherInformation;

                                _message = _message + buildForeCastMessage(_farm, "spanish");

                                _found = true;

                            }


                        }

                        if (_found == true)
                        {
                            User[] _user = _userController.ReadUserAnyParameter("UserPhoneNumber", _incomingSMS.msisdn);
                            string _userFarm = "";


                            if (_user != null)
                            {
                                if (_user.Length > 0)
                                {
                                    _userFarm = _user[0].UserFarmID;

                                    if (_userFarm == _farmID)
                                    {
                                        _message = "Ya usted se encuentra registrado en esta Granja." + Environment.NewLine +
                                                   "Si usted desea registrarse en otra Granja, Por Favor ingrese un código diferente" + Environment.NewLine +
                                                   "Tenga en cuenta, que puede estar registrado en UNA SOLA Granja al mismo tiempo";

                                    }
                                    else
                                    {
                                        string _previousFarmID = _user[0].UserID.ToString();


                                        string _dataBaseDeleteResult = _userInstance.DeleteUserFromFarm("UserID", _user[0].UserID.ToString());

                                        string _dataBaseInsertResult = _userInstance.InsertUserInFarmModel(_user[0].UserID.ToString(), _farmID);


                                        if (_dataBaseInsertResult != "User Added in Farm Successfully")
                                        {
                                            _message = _dataBaseInsertResult;
                                        }
                                        else
                                        {
                                            _message = _message + Environment.NewLine + Environment.NewLine +
                                                       "Tenga en cuenta que usted también ha sido removido de la lista de notificationes de la granja en la que estaba registrado previamente";
                                        }
                                    }
                                }
                                else
                                {
                                    User _newUser = new User();
                                    _newUser.UserPhoneNumber = _incomingSMS.msisdn;
                                    _newUser.InsertUserModel(_newUser);

                                    _user = _userController.ReadUserAnyParameter("UserPhoneNumber", _incomingSMS.msisdn);

                                    string _dataBaseDeleteResult = _userInstance.DeleteUserFromFarm("UserID", _user[0].UserID.ToString());

                                    string _dataBaseInsertResult = _userInstance.InsertUserInFarmModel(_user[0].UserID.ToString(), _farmID);

                                    if (_dataBaseInsertResult != "User Added in Farm Successfully")
                                    {
                                        _message = _dataBaseInsertResult;
                                    }
                                    else
                                    {
                                        _message = _message + Environment.NewLine + Environment.NewLine +
                                                   "Tenga en cuenta que usted también ha sido removido de la lista de notificationes de la granja en la que estaba registrado previamente";
                                    }
                                }

                            }
                        }

                        if (_found == false)
                        {
                            _message = _message = "El código ingresado es incorrecto, por favor intente nuevamente con un código válido." + Environment.NewLine +
                                                   "Recuerda, envía la palabra: SI seguida por el código de la Granja en la que desea registrarse." + Environment.NewLine +
                                                   "Por Ejemplo: SI G1";

                            sendTextMessage(_incomingSMS.msisdn, _message);

                            return Ok(_message);

                        }
                    }

                    if (_found == false)
                    {
                        _message = "The word you entered is not recognized." + Environment.NewLine +
                                 "La palabra que ingresaste no es reconocida ";

                    }
                }
                else
                {
                    _message = "The word you entered is not recognized." + Environment.NewLine +
                                "La palabra que ingresaste no es reconocida ";
                }

            }

            sendTextMessage(_incomingSMS.msisdn, _message);

            return Ok(_message);
        }


        
        //Methods and Classess used to Send an SMS Message using Nexmo API

        public void sendTextMessage(string destinataryPhoneNumber, string messageToSend)
        {
            NexmoResponse SMSResponse = SendSMS(destinataryPhoneNumber, messageToSend, "f848e55b", "ac18918502228f7d");

            foreach (NexmoMessageStatus message in SMSResponse.Messages)
            {
                try
                {
                    if (message.Status != "0")
                    {
                        var error = message.ErrorText;
                        //successDiv.Visible = false;
                        //errorDiv.Visible = true;
                        //ErrorMessage.Text = error.ToString();
                    }
                    else
                   {
                        //errorDiv.Visible = false;
                        //successDiv.Visible = true;
                        //SuccessMessage.Text = successMessageTAG;
                    }
                }
                catch
                {
                    //successDiv.Visible = false;
                    //errorDiv.Visible = true;
                    //ErrorMessage.Text = failureMessageTAG;
                }
            }

        }

        [HttpPost]
        public void makeWarningCall(string destinataryPhoneNumber, String messageToSend)
        {

            NexmoResponse SMSResponse = makeCall(destinataryPhoneNumber, messageToSend, "f848e55b", "ac18918502228f7d");

        }

        //OUTGOING MESSAGES CLASSES
        public NexmoResponse SendSMS(string to, string text, string apikey, string secretkey)
        {
            //var wc = new WebClient() { BaseAddress = "http://rest.nexmo.com/sms/json" };
            //wc.QueryString.Add("api_key", HttpUtility.UrlEncode(apikey));
            //wc.QueryString.Add("api_secret", HttpUtility.UrlEncode(secretkey));
            //wc.QueryString.Add("from", HttpUtility.UrlEncode("5037555597"));
            //wc.QueryString.Add("to", HttpUtility.UrlEncode(to));
            //wc.QueryString.Add("text", HttpUtility.UrlEncode(text));

            string url = "https://rest.nexmo.com/sms/json?api_key=" + apikey + "&api_secret="+ secretkey + "&from=+12109618835" + "&to=" + to + "&text=" + text;

            NexmoResponse request = ParseSmsResponseJson(url);

            return request;
        }

        public NexmoResponse makeCall(string to, string text, string apikey, string secretkey)
        {

            
            string language = "es-es";
            string voice = "female";

            string url = "https://api.nexmo.com/tts/json?api_key=" + apikey + "&api_secret=" + secretkey + "&from=+12109618835" + "&to=" + to + "&lg=" + language + "&voice=" + voice + "&text=" + text;


            NexmoResponse request = ParseSmsResponseJson(url);

            return request;
        }

        public NexmoResponse ParseSmsResponseJson(string json)
        {
            WebClient client = new WebClient();

            string jsonResponse = client.DownloadString(json);

            jsonResponse = jsonResponse.Replace("-", "");  // hyphens are not allowed in in .NET var names
            return new JavaScriptSerializer().Deserialize<NexmoResponse>(jsonResponse);
        }

        public class NexmoResponse
        {
            public string Messagecount { get; set; }
            public List<NexmoMessageStatus> Messages { get; set; }
        }

        public class NexmoMessageStatus
        {
            public string MessageId { get; set; }
            public string To { get; set; }
            public string clientRef;
            public string Status { get; set; }
            public string ErrorText { get; set; }
            public string RemainingBalance { get; set; }
            public string MessagePrice { get; set; }
            public string Network;
        }

        //INCOMING MESSAGES CLASSES
        public NexmoInComingMessage ParseIncomingMessageJson(string json)
        {
            WebClient client = new WebClient();

            string jsonResponse = client.DownloadString(json);

            jsonResponse = jsonResponse.Replace("-", "");  // hyphens are not allowed in in .NET var names
            return new JavaScriptSerializer().Deserialize<NexmoInComingMessage>(jsonResponse);
        }

        public class NexmoInComingMessage
        {
            public string msisdn { get; set; }
            public string to { get; set; }
            public string messageId;
            public string text { get; set; }
            public string type { get; set; }
            public string keyword { get; set; }
            public string messagetimestamp { get; set; }
        }


        //Weather Methods
        // Units: imperial or metrics
        protected string[] FetchWeatherInformationByLocation(string zipCode, string country, string unitType, string[] informationToFetch, string language)
        {

            string API_KEY = "af3dda45b0f693c067a06f249896ba6c";

            // Query URLs. Replace @LOC@ with the location.
            string CurrentUrl =
                    "http://api.openweathermap.org/data/2.5/weather?" +
                    "q=@LOC@&mode=xml&units="+ unitType + "&APPID=" + API_KEY + "&lang=" + language;

            //weather call
            string url = CurrentUrl.Replace("@LOC@", zipCode + "," + country);

            XmlDocument doc = new XmlDocument();
            string formatedURL = GetFormattedXml(url);

            string[] listOfFetchedData = new string[informationToFetch.Length];

            if (formatedURL != "Location Not Valid")
            {
                doc.LoadXml(GetFormattedXml(url));

                int counter = 0;

                foreach (string _info in informationToFetch)
                {
                    XmlNodeList elemList = doc.GetElementsByTagName(_info);

                    for (int i = 0; i < elemList.Count; i++)
                    {
                        if (informationToFetch[counter] == "temperature")
                            listOfFetchedData[counter] = elemList[i].Attributes["value"].Value;

                         if (informationToFetch[counter] == "city")
                            listOfFetchedData[counter] = elemList[i].Attributes["name"].Value;
                    }

                    counter++;
                }
            }

            return listOfFetchedData;
        }
        //// Get a forecast.
        //private void btnForecast_Click(object sender, EventArgs e)
        //{
        //    // Compose the query URL.
        //    string url = ForecastUrl.Replace("@LOC@", "USA 90015");
        //    TXTlocation.Text = GetFormattedXml(url);
        //}

        // Return the XML result of the URL.
        [HttpGet]
        public string[] DailyFetchForeCastInformationByLocation(string zipCode, string country, string unitsType, string language)
        {

            string API_KEY = "af3dda45b0f693c067a06f249896ba6c";

            // Query URLs. Replace @LOC@ with the location.
            string url = "http://api.openweathermap.org/data/2.5/" + "forecast/daily?q="+ 
                                                                            zipCode + "," + country + 
                                                                            "&mode=xml&units="+ unitsType +"&cnt=1&APPID="+ API_KEY;
            

            XmlDocument doc = new XmlDocument();
            string formatedURL = GetFormattedXml(url);

            string[] listOfFetchedData = new string[6];


            if (formatedURL != "Location Not Valid")
            {
                doc.LoadXml(GetFormattedXml(url));

                XmlNodeList xnList = doc.SelectNodes("/weatherdata/forecast/time");

                foreach (XmlNode xn in xnList)
                {
                    string currentTemperature = xn["temperature"].Attributes["day"].Value;
                    string minTemperature = xn["temperature"].Attributes["min"].Value;
                    string maxTemperature = xn["temperature"].Attributes["max"].Value;

                    string morningTemperature = xn["temperature"].Attributes["morn"].Value;                  
                    string eveningTemperature = xn["temperature"].Attributes["eve"].Value;
                    string nightTemperature = xn["temperature"].Attributes["night"].Value;


                    listOfFetchedData[0] = currentTemperature;
                    listOfFetchedData[1] = minTemperature;
                    listOfFetchedData[2] = maxTemperature;

                    listOfFetchedData[3] = morningTemperature;
                    listOfFetchedData[4] = eveningTemperature;
                    listOfFetchedData[5] = nightTemperature;
                }

            }

            return listOfFetchedData;
        }

        private string GetFormattedXml(string url)
        {
            // Create a web client.
            using (WebClient client = new WebClient())
            {

                try
                {
					// Get the response string from the URL.
					string xml = client.DownloadString(url);

					XmlDocument xml_document = new XmlDocument();
					// Load the response into an XML document.
					xml_document.LoadXml(xml);
					// Format the XML.
					using (StringWriter string_writer = new StringWriter())
					{
						XmlTextWriter xml_text_writer =
							new XmlTextWriter(string_writer);
						xml_text_writer.Formatting = Formatting.Indented;
						xml_document.WriteTo(xml_text_writer);

						// Return the result.
						return string_writer.ToString();
					}
				}
                catch
                {
                    //errorDiv.Visible = true;
                    //successDiv.Visible = false;
                    //ErrorMessage.Text = ErrorMessage.Text + " The location entered is not valid";
                    return "Location Not Valid";
                }
                
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
