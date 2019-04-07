using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProject.CommonConstant
{
    public class CommonConstant
    {
        // key session
        public const string SESSION_INFO_LOGIN = "SESSION_INFO_LOGIN";

        // type status of user when try login
        public const int STATUS_LOCK_ACCOUNT = -1;
        public const int STATUS_WRONG_PASS_ACCOUNT = 0;
        public const int STATUS_RIGHT_ACCOUNT = 1;
        public const int STATUS_NOT_EXIST_ACCOUNT = 2;

        //role of user
        public const int ROLE_MEMBER = 0;
        public const int ROLE_TEACHER = 1;
        public const int ROLE_ADMIN = 2;

    }
}
