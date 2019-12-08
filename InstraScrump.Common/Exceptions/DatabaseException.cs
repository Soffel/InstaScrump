using System;
using System.Collections.Generic;
using System.Text;

namespace InstaScrump.Common.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string msg) : base(msg) { }
        public DatabaseException(string msg, Exception e) : base(msg, e) { }
    }
}
