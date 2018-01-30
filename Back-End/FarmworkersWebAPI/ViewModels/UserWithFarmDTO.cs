using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmworkersWebAPI.ViewModels
{
    public class UserWithFarmDTO
    {
        public int UserID { get; set; }
        public string UserType { get; set; }

        public string UserName { get; set; }

        public string UserLastName { get; set; }

        public string UserDateOfBirth { get; set; }

        public string UserHeight { get; set; }

        public string UserWeight { get; set; }

        public string UserGender { get; set; }

        public string UserEmail { get; set; }

        public string UserPhoneNumber { get; set; }

        public string UserWorkLocation { get; set; }

        public string UserMemberSince { get; set; }
        public string IsActive { get; set; }
        public FarmDTO CurrentFarm { get; set; }
        public List<UserFarmDTO> UserFarms { get; set; }
        public List<UserCommunicationPreferenceDTO> UserCommunicationPreferences { get; set; }
    }
}