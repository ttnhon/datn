using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProject
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

        //public const string URL_HOST_API = "http://aipcompiler.somee.com";
        public const string URL_HOST_API = "http://localhost:52508";
        public const string ROUTER_CSHARP_COMPILER_API = "/api/CSharpCompiler/Compiler";
        public const string ROUTER_CPLUS_COMPILER_API = "/api/CPlusCompiler/Compiler";
        public const string ROUTER_JAVA_COMPILER_API = "/api/JavaCompiler/Compiler";
        public const string ROUTE_TESTCASE_API = "/api/TestCaseFileManager/GetTestCase";
        public const string ROUTE_UPLOAD_TESTCASE_API = "/api/TestCaseFileManager/Upload";
        public const string ROUTE_UPDATE_TESTCASE_API = "/api/TestCaseFileManager/UpdateFile";
        public const string ROUTE_DELETE_TESTCASE_API = "/api/TestCaseFileManager/DeleteFile";

        public const string TYPE_CSHARP_COMPILER = "CSHARP";
        public const string TYPE_CPLUS_COMPILER = "CPP";
        public const string TYPE_JAVA_COMPILER = "JAVA";

        public const string DIR_CODE = "/Assets/File_Resource/Code_File/";
        public const string DIR_TESTCASE = "/Assets/File_Resource/Test_Case/";

        public const string ALERT_SUCCESS = "success";
        public const string ALERT_ERROR = "error";
        public const string ALERT_WARNING = "warning";

        public const string TYPE_UPLOAD_FILE_API = "UPLOAD";
        public const string TYPE_UPDATE_FILE_API = "UPDATE";
        public const string TYPE_DELETE_FILE_API = "DELETE";
    }
}
