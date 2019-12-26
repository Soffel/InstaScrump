using System;
using System.Collections.Generic;
using System.Text;
using InstaScrump.Common.Interfaces;
using OtpNet;

namespace InstaScrump.Business.Utils
{
    public static class TimedOneTimePswdGenerator
    {
        public static string GetOneTimePswd(string securityKey)
        {
            return new Totp(Base32Encoding.ToBytes(securityKey)).ComputeTotp();
        }
    }
}
