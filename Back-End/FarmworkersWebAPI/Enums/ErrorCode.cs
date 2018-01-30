using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmworkersWebAPI.Enums
{
    public class ErrorCode
    {

        public readonly static string USER_ALREADY_REGISTER = "10001";
        public readonly static string INVALID_CREDENTIALS = "10002";
        public readonly static string INVALID_PHONE_NUMBER_FOR_FORGET_PASSWORD = "10003";
        public readonly static string INVALID_EMAIL_FOR_FORGET_PASSWORD = "10004";
        public readonly static string INVALID_OLD_PASSWORD = "10011";
        public readonly static string OTHER = "10999";
    }
}