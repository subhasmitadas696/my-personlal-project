
namespace CTMS.Core
{
    /// <summary>
    /// Constant parameter set
    /// </summary>
    public class ConstParameters
    {
        public static string VerifyCodeKeyName = "netcore_session_verifycode";
        public static string VerifyCodeKeyNameCopy = "netcore_session_verifycode_copy";
        //CoreContextProvider.Configuration.GetSection() 
        public static string VerifyCodeKeyNameResend = "netcore_session_verifycode_resend";

        public static string SysLoginUserKey = "netcore_loginuserkey_manage";

        public static string MemLoginUserKey = "netcore_loginuserkey_mem";

        public static string SysLoginProvider = CoreContextProvider.Configuration.GetSection("SystemProvider")["LoginProvider"];
    }
}

