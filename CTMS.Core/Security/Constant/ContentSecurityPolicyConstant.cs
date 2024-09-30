namespace CTMS.Core.Security.Constant
{
    /// <summary>
    /// Content-Security-Policy-related constants.
    /// </summary>
    //public static class ContentSecurityPolicyConstant
    //{
    //    public static readonly string Header = "Content-Security-Policy";
    //    static string sq = "default-src 'self' ;script-src 'self' https://localhost:44380/js 'unsafe-eval' 'unsafe-inline' www.google-analytics.com www.googletagmanager.com https://platform.twitter.com; object-src 'none';style-src 'self' http://cdn.jsdelivr.net/ https://localhost:44380/css 'unsafe-inline' http://cdn.jsdelivr.net/ fonts.gstatic.com  fonts.googleapis.com stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.3.1/css/bootstrap.min.css cdnjs.cloudflare.com/ajax/libs/mdbootstrap/4.8.8/css/mdb.min.css ;img-src 'self' http://www.w3.org/2000/svg https://localhost:44380/ https://localhost:7132/ data:; media-src 'self' https://localhost:44310/;frame-src 'self' https://www.google.com/ ;font-src 'self' http://cdn.jsdelivr.net/ fonts.gstatic.com fonts.googleapis.com;connect-src 'self'; wss://localhost:44310 ws://localhost:44310 wss://localhost:7132 ws://localhost:7132 base-uri 'self';child-src 'none';frame-ancestors 'self';";
    //    public static readonly string defaultsrc = sq;
    //}
    public static class ContentSecurityPolicyConstant
    {
        public static readonly string Header = "Content-Security-Policy";


        static string sq = "default-src 'self'; script-src 'self'  https://localhost:44380/js 'unsafe-eval' 'unsafe-inline' www.google-analytics.com www.googletagmanager.com https://platform.twitter.com https://connect.facebook.net/en_US/; object-src 'none'; style-src 'self' http://cdn.jsdelivr.net/ https://localhost:44380/css https://cdn.datatables.net/1.10.15/css/ 'unsafe-inline' http://cdn.jsdelivr.net/ fonts.gstatic.com fonts.googleapis.com stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.3.1/css/bootstrap.min.css cdnjs.cloudflare.com/ajax/libs/mdbootstrap/4.8.8/css/mdb.min.css ; img-src 'self' http://www.w3.org/2000/svg https://localhost:44380/ https://localhost:7132/ https://syndication.twitter.com/ data: blob:; media-src 'self' blob: https://localhost:44310/; frame-src 'self' https://www.google.com/ https://platform.twitter.com/ https://www.facebook.com/ https://syndication.twitter.com/; font-src 'self' http://cdn.jsdelivr.net/ fonts.gstatic.com fonts.googleapis.com; connect-src 'self'; wss://localhost:44310 ws://localhost:44310 wss://localhost:7132 ws://localhost:7132 base-uri 'self'; child-src 'none'; frame-ancestors 'self';";

        public static readonly string defaultsrc = sq;
    }


}
