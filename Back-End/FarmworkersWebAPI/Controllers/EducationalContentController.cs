using FarmworkersWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity.Infrastructure;
using FarmworkersWebAPI.ViewModels;


namespace FarmworkersWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EducationalContentController : ApiController
    {
        //***Entity Framework**//

        //NOTE: Watch out for conflicts, between: 
        //**FarmworkersWebAPI.Entities
        //**FarmworkersWebAPI.Models

        FarmworkersWebAPI.Entities.FarmWorkerAppContext _context;

        public EducationalContentController()
        {
            _context = new FarmworkersWebAPI.Entities.FarmWorkerAppContext();
            _context.Configuration.ProxyCreationEnabled = false;
        }

        [HttpGet]
        public IHttpActionResult EFGetListOfEducationalContentsController()
        {
            _context.Configuration.ProxyCreationEnabled = false;
            List<FarmworkersWebAPI.Entities.EducationalContent> _educationalContentData = new List<FarmworkersWebAPI.Entities.EducationalContent>();

            try
            {
                _educationalContentData = _context.EducationalContents.ToList();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(_educationalContentData);
        }

        [HttpPost]
        public IHttpActionResult EFInsertEducationalContent(FarmworkersWebAPI.Entities.EducationalContent _educationalContentInformation)
        {

            _context.Configuration.ProxyCreationEnabled = false;

            FarmworkersWebAPI.Entities.EducationalContent _educationalContentData = new FarmworkersWebAPI.Entities.EducationalContent();

            try
            {
                _educationalContentData = _context.EducationalContents.Add(_educationalContentInformation);
                _context.SaveChanges();

                return Ok(_educationalContentData);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            
        }

        [HttpPost]
        public IHttpActionResult EFUpdateEducationalContent(FarmworkersWebAPI.Entities.EducationalContent _educationalContentInformation)
        {

            _context.Configuration.ProxyCreationEnabled = false;
            
            try
            {
                FarmworkersWebAPI.Entities.EducationalContent original = _context.EducationalContents.
                                                        Where(ucp => ucp.IDEducationalContent.Equals(_educationalContentInformation.IDEducationalContent)).FirstOrDefault();

                FarmworkersWebAPI.Entities.EducationalContent _updated = new FarmworkersWebAPI.Entities.EducationalContent();

                if (original != null)
                {
                    _context.Entry(original).CurrentValues.SetValues(_educationalContentInformation);
                    _context.SaveChanges();

                    _updated = _context.EducationalContents.Find(_educationalContentInformation.IDEducationalContent);

                    return Ok(_updated);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok("Educational Content Not Found");
        }

        [HttpPost]
        public IHttpActionResult EFDeleteEducationalContent(FarmworkersWebAPI.Entities.EducationalContent _educationalContentInformation)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            try
            {
                _context.EducationalContents.Attach(_educationalContentInformation);
                _context.EducationalContents.Remove(_educationalContentInformation);
                _context.SaveChanges();

                return Ok("EducationalContent Deleted Successfully");

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        public IHttpActionResult EFGetListOfEducationalContentsbyAnyParameterController(string _parameterName, string _parameterData, string _exactMatch)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            List<FarmworkersWebAPI.Entities.EducationalContent> _educationalContentData = new List<FarmworkersWebAPI.Entities.EducationalContent>();

            string _matchCriteria = _exactMatch == "Yes" ? " = " + _parameterData : " LIKE '%" + _parameterData + "%' ";

            string _selectClause = "SELECT * ";
            string _fromClause = "FROM EducationalContent ";
            string _whereClause = "WHERE " + _parameterName + _matchCriteria;
            string query = _selectClause + _fromClause + _whereClause;

            try
            {

                _educationalContentData = _context.EducationalContents.SqlQuery(query).ToList();

                return Ok(_educationalContentData);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult EFInsertEducationalContentToUserController(FarmworkersWebAPI.Entities.UserEducationalContent _educationalContentInformation)
        {
            _context.Configuration.ProxyCreationEnabled = false;

            try
            {
                var existing = _context.UserEducationalContents.Where(
                    x => x.EducationalContentID == _educationalContentInformation.EducationalContentID
                    && x.UserID == _educationalContentInformation.UserID).FirstOrDefault();
                if(existing != null)
                {
                    //var currentClickedCount = Int32.Parse(existing.UserEducationalContentClicked);
                    //var newClickedCount = Int32.Parse(_educationalContentInformation.UserEducationalContentClicked);
                    if (existing.UserEducationalContentClicked == "0")
                    {
                        existing.UserEducationalContentClicked = "1";
                        _context.Entry(existing).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                else
                {
                    _educationalContentInformation = _context.UserEducationalContents.Add(_educationalContentInformation);
                }
                _context.SaveChanges();

                return Ok(_educationalContentInformation);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
		
        [HttpGet]
        public IEnumerable<Entities.EducationalContent> GetListOfVideoEducationContents(bool showAll = false)
        {
            var res = _context.EducationalContents
                .Include("UserEducationalContents")
                .Where(x => x.TypeEducationalContent == "Video" &&
                (showAll || x.IsActive == "1")).ToList();
            return res;
        }
        [HttpGet]
        public IEnumerable<Entities.EducationalContent> GetListOfDocumentEducationContents(bool showAll = false)
        {
            return _context.EducationalContents
                .Include("UserEducationalContents")
                .Where(x => x.TypeEducationalContent == "Document" &&
                (showAll || x.IsActive == "1")).ToList();
        }

        [HttpGet]
        public IEnumerable<Entities.UserEducationalContent> GetListOfUserVideoEducationContents(int UserID)
        {
            var res = _context.UserEducationalContents
                .Where(x => x.UserID == UserID && x.EducationalContent.TypeEducationalContent == "Video").ToList();
            return res;
        }
        [HttpGet]
        public IEnumerable<Entities.UserEducationalContent> GetListOfUserDocumentEducationContents(int UserID)
        {
            var res = _context.UserEducationalContents
                .Where(x => x.UserID == UserID && x.EducationalContent.TypeEducationalContent == "Document").ToList();
            return res;
        }


        //*********************//


        [HttpGet]
        public IEnumerable<EducationalContent> GetListOfEducationalContentsController()
        {
            EducationalContent _educationalContentInstance = new EducationalContent();
            DataTable _educationalContentDataFromDataBase = new DataTable();

            _educationalContentDataFromDataBase = _educationalContentInstance.GetListOfEducationalContentModel();

            EducationalContent[] _listOfEducationalContents = new EducationalContent[_educationalContentDataFromDataBase.Rows.Count];

            int _counter = 0;

            foreach (DataRow _educationalContentRow in _educationalContentDataFromDataBase.Rows)
            {


                EducationalContent _farm = new EducationalContent
                {
                    IDEducationalContent = int.Parse(_educationalContentRow["IDEducationalContent"].ToString()),
                    LinkEducationalContent = _educationalContentRow["LinkEducationalContent"].ToString(),
                    TypeEducationalContent = _educationalContentRow["TypeEducationalContent"].ToString(),
                    DescriptionEducationalContent = _educationalContentRow["DescriptionEducationalContent"].ToString(),
                    DateEducationalContentCreated = _educationalContentRow["DateEducationalContentCreated"].ToString(),
                    IsActive = _educationalContentRow["IsActive"].ToString()
                };

                _listOfEducationalContents[_counter] = _farm;

                _counter++;
            }

            return _listOfEducationalContents;
        }

        [HttpPost]
        public IHttpActionResult InsertEducationalContent(EducationalContent _educationalContentInformation)
        {
            EducationalContent _educationalContentReceived = new EducationalContent();

            if (!_educationalContentReceived.InsertEducationalContentModel(_educationalContentInformation))
            {
                return Ok("Educational Content Not Added");
            }

            return Ok("Educational Content  Successfully Added");
        }

        [HttpPost]
        public IHttpActionResult UpdateEducationalContent(EducationalContent _educationalContentInformation)
        {
            EducationalContent _educationalContentReceived = new EducationalContent();

            if (!_educationalContentReceived.UpdateEducationalContentModel(_educationalContentInformation))
            {
                return Ok("Educational Content Not Updated");
            }

            return Ok("Educational Content Successfully Updated");
        }

        [HttpGet]
        public string DeleteEducationalContent(string _IDEducationalContent)
        {
            EducationalContent _educationalContentInstance = new EducationalContent();

            return _educationalContentInstance.DeleteEducationalContentModel(_IDEducationalContent).ToString();
        }

        [HttpGet]
        public IEnumerable<EducationalContent> GetListOfEducationalContentsbyAnyParameterController(string _parameterName, string _parameterData, string _exactMatch)
        {
            
            EducationalContent _educationalContentInstance = new EducationalContent();
            DataTable _educationalContentDataFromDataBase = new DataTable();

            _educationalContentDataFromDataBase = _educationalContentInstance.ReadEducationalContentByAnyParameterModel(_parameterName, _parameterData, _exactMatch);

            EducationalContent[] _listOfEducationalContents = new EducationalContent[_educationalContentDataFromDataBase.Rows.Count];

            int _counter = 0;

            foreach (DataRow _educationalContentRow in _educationalContentDataFromDataBase.Rows)
            {


                EducationalContent _farm = new EducationalContent
                {
                    IDEducationalContent = int.Parse(_educationalContentRow["IDEducationalContent"].ToString()),
                    LinkEducationalContent = _educationalContentRow["LinkEducationalContent"].ToString(),
                    TypeEducationalContent = _educationalContentRow["TypeEducationalContent"].ToString(),
                    DescriptionEducationalContent = _educationalContentRow["DescriptionEducationalContent"].ToString(),
                    DateEducationalContentCreated = _educationalContentRow["DateEducationalContentCreated"].ToString(),
                    IsActive = _educationalContentRow["IsActive"].ToString()
                };

                _listOfEducationalContents[_counter] = _farm;

                _counter++;
            }

            return _listOfEducationalContents;
        }

        [HttpPost]
        public IHttpActionResult InsertEducationalContentToUserController(UserEducationalContent _userEducationalContent)
        {
            EducationalContent _educationalContentReceived = new EducationalContent();

            if (!_educationalContentReceived.InsertEducationalContentToUserModel(_userEducationalContent))
            {
                return Ok("The User was not linked to the Educational Content");
            }

            return Ok("Educational Content successfully linked to the User");
        }

    }
}
