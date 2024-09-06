using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Core
{
    public static class Constant
    {

        public const int DEFAULT_COOKIE_EXPIRY = 10;
        public const string JWT_COOKIE_NAME = "AuthToken";

        public const string ADMIN_POLICY = "AdminPolicy";
        public const string USER_POLICY = "UserPolicy";
        public const string ADMIN_USER_POLICY = "AdminOrUserPolicy";
        public const string ADMIN = "Admin";
        public const string USER = "User";
        public const string ADMIN_OR_USER = "Admin,User";

        public const string EmailPattern = @"(([a-zA-Z0-9_&'\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)(\s*;\s*|\s*$))*";
    }
}
