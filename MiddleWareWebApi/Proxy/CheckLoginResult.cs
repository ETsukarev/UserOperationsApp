using System;

namespace MiddleWareWebApi.Proxy
{
    public class CheckLoginResult
    {
        /// Result of checking login
        public bool ResultCheckLogin { get; set; }

        /// Error of checking login
        public Exception Error { get; set; }
    }
}
