using System;

namespace InstaScrump.Common.Exceptions
{
    public class LogoutException : Exception
    {
        public LogoutException(string msg) : base(msg) { }

        public LogoutException(string msg, Exception e) : base(msg, e) { }
    }
}
