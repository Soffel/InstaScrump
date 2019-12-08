using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using InstaScrump.Business.Utils;
using InstaScrump.Common;
using InstaScrump.Common.Exceptions;
using InstaScrump.Common.Extension;
using InstaScrump.Common.Extension.ModelExtension;
using InstaScrump.Common.Utils;
using InstaScrump.Database.Model;
using LinqToDB;

namespace InstaScrump.Business.Repository
{
    public class AuthenticationRepository : BaseRepository
    {
        private readonly ushort _maxLoginAttempt;
        private LoginModel _activLogin;


        public AuthenticationRepository(DbContext dbContext, Config config) :base(dbContext, config)
        {
            _maxLoginAttempt = 5;
            _activLogin = null;

            if (ushort.TryParse(Config.Read("MaxLoginAttempt", "Config"), out var maxLoginAttempt))
                _maxLoginAttempt = maxLoginAttempt;
        }

        public async Task SetNewLoginData(LoginModel loginData)
        {
            if(!loginData.Validate())
                "loginData not valid!".Write(ConsoleColor.Red);
            
            if (_activLogin != null &&
                InstaApi != null &&
                InstaApi.IsUserAuthenticated)
                await Logout();

            if (await Login(loginData))
            {
                if (_activLogin != null &&
                    InstaApi != null &&
                    InstaApi.IsUserAuthenticated)
                {
                    var userInfo = await InstaApi.GetCurrentUserAsync();

                    if (userInfo.Succeeded)
                    {
                        var pswd = Cryptography.Encrypt<AesManaged>(loginData.Pswd,Config.Read("Pswd", "Security"), Config.Read("Vector", "Security"));

                        using (var db  = DbContext.Create())
                        {
                            if(await db.InsertAsync(new LoginData()
                            {
                                Salt = pswd.Item2,
                                UserPswd = pswd.Item1,
                                UserName = loginData.UserName
                            }) != 1)
                                throw new DatabaseException("LoginData insert failed!");
                        }
                    }
                }
            }
        }

        private async Task<bool> Login(LoginModel loginData)
        {
            throw new NotImplementedException();
        }


        public async Task<bool> Logout()
        {
            try
            {
                if (_activLogin != null && 
                    InstaApi != null    && 
                    InstaApi.IsUserAuthenticated)
                {
                    return (await InstaApi.LogoutAsync()).Succeeded;
                }
            }
            catch (Exception e)
            {
                throw new LogoutException("InstaApiException", e);
            }
            
            throw new LogoutException("Logout not possible. Missing Login?!");
        }
    }
}
