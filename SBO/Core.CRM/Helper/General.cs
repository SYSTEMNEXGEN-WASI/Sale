using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace Core.CRM.Helper
{
    public class General
    {
        public static DateTime CurrentDate
        {
            get { return DateTime.Now; }
        }

        public static DateTime CurrentDateUTC
        {
            get { return DateTime.Now.AddHours(-5); }
        }

        
        public static string CurrentIP
        {
            get { return HttpContext.Current == null ? "" : HttpContext.Current.Request.UserHostAddress.ToString(); } /// testing usrid 1 
        }
        public static long CurrentTicks
        {
            get { return DateTime.Now.Ticks; }
        }

        public static string GetAppValue(string key)
        {
            string val = string.Empty;
            if (ConfigurationManager.AppSettings[key] != null)
            {
                val = Convert.ToString(ConfigurationManager.AppSettings[key]);
            }
            return val;
        }

        public static string GetConValue(string key)
        {
            string val = string.Empty;
            if (ConfigurationManager.ConnectionStrings[key] != null)
            {
                val = Convert.ToString(ConfigurationManager.ConnectionStrings[key]);
            }
            return val;
        }

        public string CheckEmptyOrNull(string data)
        {
            string empty = string.Empty;
            return string.IsNullOrEmpty(data) ? "" : data;
        }

        public int CheckNullOrZero(string data)
        {
            int result = 0;
            int.TryParse(data, out result);
            return result;
        }

        private static string GetRandomNumber()
        {
            Random r = new Random();
            int number = r.Next(10000, 99999);
            return Convert.ToString(number);
        }

        public static string OTPNumber { get { return GetRandomNumber(); } }


        public static string CRMemailAPIKey { get { return GetAppValue("CRMEmailAPIKEY"); } }
        public static void CreateFolder(string FolderPath)
        {
            //  string folderPath = HttpContext.Current.Server.MapPath(FolderPath);
            string folderPath = FolderPath;
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            if (!dir.Exists)
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public static string GetCurrentUsername()
        {
            return "Admin";
        }

        public static string GetConstring()
        {

            return General.GetConValue("CRMConnectionString");

        }

        public static string GetBMSConString()
        {

            return General.GetConValue("BMS0517ConnectionString");

        }
        public static string GetDMISConString()
        {

            return General.GetConValue("DMISConnectionString");

        }
      
        public static string GetFAMConString()
        {

            return General.GetConValue("FAMSConnectionString");

        }
        public static string GetSBOConString()
        {

            return General.GetConValue("BMS0517ConnectionString");

        }

        private static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public static bool URLExists(string url)
        {
            bool result = true;

            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Timeout = 1200; // miliseconds
            webRequest.Method = "HEAD";

            try
            {
                webRequest.GetResponse();
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static string RandomPassword { get { return CreateRandomPassword(8); } }

        public static DateTime ToDate()
        {
            DateTime ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59.998");

            return ToDate;
        }

        public static DateTime FromDate()
        {
            DateTime FromDate = Convert.ToDateTime(DateTime.Now.AddDays(-3).ToShortDateString() + " 00:00:00.001");

            return FromDate;

        }

        public static DateTime ToEndDate
        {
            get
            {
                DateTime ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59.998");
                return ToDate;
            }
        }

        public static DateTime FromBeginDate
        {
            get
            {
                DateTime FromDate = new DateTime(1990, 1, 1, 0, 0, 0);
                return FromDate;
            }
        }

        public static T FromXML<T>(string xml)
        {
            using (StringReader stringReader = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }

        public string ToXML<T>(T obj)
        {
            using (StringWriter stringWriter = new StringWriter(new StringBuilder()))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }



        public static bool CheckNullOrEmpty<T>(T value)
        {
            if (typeof(T) == typeof(string))
                return string.IsNullOrEmpty(value as string);

            return value == null || value.Equals(default(T));
        }
        public static int ValueBwParenthesis(string value)
        {
            int result = 0;
            if (value != null)
            {
                if (value != "--" && value != "NA")
                {
                    var item = value.Split('(', ')')[1];
                    result = Convert.ToInt32(item);
                }
            }
            return result;
        }


        public static string GetQueryString(string key)
        {
            string result = string.Empty;

            Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
            if (myUri != null)
            {

                if (HttpUtility.ParseQueryString(myUri.Query) != null)
                {
                    if (HttpUtility.ParseQueryString(myUri.Query).Get(key) != null)
                    {
                        result = HttpUtility.ParseQueryString(myUri.Query).Get(key);
                    }
                }
            }
            return result;
        }

        public static string getAppendQueryString(string fullPath)
        {
            string qs = string.Empty;
            if (fullPath.IndexOf("?") != -1)
            {
                qs = fullPath.Substring(fullPath.IndexOf("?") + 1, fullPath.Length - fullPath.IndexOf("?") - 1);
            }
            return qs;
        }

        public enum IPSource
        {
            MaxMind = 1,
            FreeGeoIp = 2
        }

        #region CustomerArea

        public static int GetClientIDWebConfig
        {
            get
            {
                return Convert.ToInt32(General.GetAppValue("ClientID"));
            }
        }

        public static string GetConnectionString()
        {

            return General.GetConValue("JTJapanConnectionString");

        }




        #endregion CustomerArea
    }
}
