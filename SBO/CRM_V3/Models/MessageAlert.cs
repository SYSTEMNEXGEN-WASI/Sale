using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models.Classes
{
    public class MessageAlert
    {
        public static string MsgSuccess()
        {
            var msg = "Saved Successfully. . . !";
            return msg;
        }
        public static string MsgFaild()
        {
            var msg = "Faild. . . !";
            return msg;
        }
        public static string MsgUpdated()
        {
            var msg = "Updated Successfully. . . !";
            return msg;
        }
        public static string MsgAuthorized()
        {
            var msg = "You are Not Authorized";
            return msg;
        }
        public static string MsgDeleted()
        {
            var msg = "Deleted Successfully . . . !";
            return msg;
        }
        public static string MsgLoginError()
        {
            var msg = "User Name Or Password Is Incorrect...!";
            return msg;
        }
        public static string MsgServerError()
        {
            var msg = "Something went wrong with server!";
            return msg;
        }
        public static string MsgOutOfStock()
        {
            var msg = "Out of stock . . . !";
            return msg;
        }
        public static string MsgPost()
        {
            var msg = "Posted successfully! . . . !";
            return msg;
        }
    }
}