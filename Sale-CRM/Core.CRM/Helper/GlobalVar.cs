using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;


    public class GlobalVar
    {
        static string _mDataConnection;
        public static string mDataConnection
        {
            get
            {
                return _mDataConnection;
            }
            set
            {
                _mDataConnection = value;
            }
        }

        static string _mDealerCode;
        public static string mDealerCode
        {
            get
            {
                return _mDealerCode;
            }
            set
            {
                _mDealerCode = value;
            }
        }

        static string _mUserIPAddress;
        public static string mUserIPAddress
        {
            get
            {
                return _mUserIPAddress;
            }
            set
            {
                _mUserIPAddress = value;
            }
        }

        static SqlConnection _Con;
        public static SqlConnection mCon
        {
            get
            {
                return _Con;
            }
            set
            {
                _Con = value;
            }

        }

        public static string mAccountCategoryName;

        public static string mAccountsFormat;


        static string _mErrorMessage;
        public static string mErrorMessage
        {
            get
            {
                return _mErrorMessage;
            }
            set
            {
                _mErrorMessage = value;
            }
        }
    }
