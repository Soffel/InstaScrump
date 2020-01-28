using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using InstaScrump.Common.Utils;
using InstaScrump.Database.Model;
using LinqToDB;
using Xunit;

namespace InstaScrump.Test
{
    public class CryptoTest
    {
        [Fact]
        public async Task TestCrypto()
        {
            var value = "test123!§$%&/";
            var pswd = "pswd123";
            var vector = "h1tslzt9dgeigelc";

            var (cryp, salt) = Cryptography.Encrypt<AesManaged>(value,pswd, vector);
            var p = Cryptography.Decrypt<AesManaged>(cryp, pswd, salt, vector);

            Assert.Equal(value, p);
        }
        
    }

}
