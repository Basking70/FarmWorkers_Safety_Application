using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmworkersWebAPI.ViewModels
{
    public class UserCommunicationPreferenceDTO
    {
        public int IDUserCommunicationPreference { get; set; }
        public int UserID { get; set; }
        public string UserCommunicationPreferenceFrequency { get; set; }
        public string UserCommunicationPreferenceNotificationType { get; set; }
        public string IDUserCommunicationPreferenceLanguage { get; set; }
    }
}