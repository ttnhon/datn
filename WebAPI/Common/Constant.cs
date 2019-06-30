using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Common
{
    public class Constant
    {
        // Constant value for compiler c# language 
        public static string COMPILER_VERSION = "CompilerVersion";
        public static string VERSION_VALUE = "4.6";
        public static string MSCOR_LIB = "mscorlib.dll";
        public static string CORE_LIB = "System.Core.dll";
        public static string HEADER_SUCCESS = "==========  Compiler successfull!  =========== ";
        public static string HEADER_FAIL =    "==========      Compiler fail!     =========== ";
        public static string FOOTER_DONE =    "==========      Compiler done!     =========== ";
        public static string BREAK_LINE = "/r/n";
        public static string OUTPUT = "Output: ";

        public static string STATUS_SUCCESS = "success";
        public static string STATUS_FAIL    = "fail";

        public static string JAVA_EXECUTE_LINK = "/App_Data/JavaSDK/jdk1.8.0_151/bin/";
        public static string CODE_DIR = "/App_Data/Dir_Code/";

        public static string TESTCASE_DIR = "App_Data/Dir_TestCase/";

        public const string SECRET_KEY_TOKEN = "phucphieu@secret.jwt.code";
    }
}