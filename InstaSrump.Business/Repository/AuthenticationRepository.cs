using System;
using System.Net.Http;
using System.Threading.Tasks;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstaScrump.Common;
using InstaScrump.Common.Exceptions;
using InstaScrump.Common.Extension;
using InstaScrump.Common.Interfaces;
using InstaScrump.Database.Model;
using InstaScrump.Common.Constants;
using Extensions;
using LinqToDB;
using Utils;

namespace InstaScrump.Business.Repository
{
    public class AuthenticationRepository : BaseRepository
    {
        private readonly ushort _maxLoginAttempt;
        private LoginModel _activLogin;
        public AuthenticationRepository(IDbContext<InstaScrumpDB> dbContext, IConfig config) : base(dbContext, config)
        {
            _maxLoginAttempt = 5;
            _activLogin = null;

            if (ushort.TryParse(Config.Read(ConfigKey.Max_Login_Key, "Config"), out var maxLoginAttempt))
                _maxLoginAttempt = maxLoginAttempt;
        }

        public async Task SetNewLoginData(LoginModel loginData)
        {
            if (!loginData.Validate())
                "loginData not valid!".WriteLine(ConsoleColor.Red);

            if (_activLogin != null &&
                InstaApi != null &&
                InstaApi.IsUserAuthenticated)
            {
                await Logout();
            }

            if (await Login(loginData))
            {
                if (_activLogin != null &&
                    InstaApi != null &&
                    InstaApi.IsUserAuthenticated)
                {
                    CheckRequestLimit();
                    var userInfo = await InstaApi.GetCurrentUserAsync();

                    if (userInfo.Succeeded)
                    {
                        //var (pswd, salt) = Cryptography.Encrypt<AesManaged>(loginData.Pswd,
                        //    Config.Read("Pswd", "Security"), Config.Read("Vector", "Security"));

                        using (var db = DbContext.Create())
                        {
                            if (await db.InsertAsync(new LoginData()
                            {              
                                Salt = "abc",//salt,
                                UserPswd = loginData.Pswd,//pswd,
                                UserName = loginData.UserName
                            }) != 1)
                                throw new DatabaseException("LoginData insert failed!");

                            await db.CommitTransactionAsync();
                            return;
                        }
                    }
                   
                    userInfo.Info.Message.WriteLine(ConsoleColor.Yellow);   
                }
            }
        }

        public async Task<bool> Login(string username)
        {
            if(username.IsNullOrWhiteSpace())
            {
                "missing login name!".WriteLine(ConsoleColor.Red);
                return false;
            }
            var pswd = string.Empty;
            LoginData user;
            using (var db = DbContext.Create())
            {
                user = await db.LoginData.FindAsync(username);

                if (user == default)
                {
                    $"user {username} not stored in database".WriteLine(ConsoleColor.Red);
                    return false;
                }

                pswd = user.UserPswd;// Cryptography.Decrypt<AesManaged>(user.UserPswd, Config.Read(ConfigKey.Pswd_Key, "Security"), user.Salt, Config.Read("Vector", "Security"));  
            }

            if (!pswd.IsNullOrWhiteSpace())
                return await Login(new LoginModel { UserName = user.UserName, Pswd = pswd });

            return false;
        }

        private async Task<bool> Login(LoginModel loginData)
        {
            if (!loginData.Validate())
                return false;

            if (_activLogin != null &&
                InstaApi != null &&
                InstaApi.IsUserAuthenticated)
                return true;

            var count = 0;
            while (count < _maxLoginAttempt)
            {
                CheckRequestLimit();
                InstaApi = InstaApiBuilder.CreateBuilder()
                    .UseHttpClientHandler(new HttpClientHandler())
                    .SetUser(new UserSessionData {UserName = loginData.UserName, Password = loginData.Pswd})
                    .Build();

               "Wait for login...".WriteLine();

                CheckRequestLimit();
                var login = await InstaApi.LoginAsync();

                if(await ValidateLoginValue(login, loginData))
                {
                    "login successful!".WriteLine(ConsoleColor.Green);

                    _activLogin = loginData;

                    return true;
                }

                Sleeper.RandomSleep(400, 600);
            }

            "login unsuccessful".WriteLine(ConsoleColor.Red);
            return false;
        }

        private async Task<bool> ValidateLoginValue(IResult<InstaLoginResult> loginResult, LoginModel loginData)
        {
            if (loginResult.Succeeded)
                return true;

            switch (loginResult.Value)
            {
                case InstaLoginResult.Success:
                {
                    return true;
                }
                case InstaLoginResult.BadPassword:
                {
                    "bad password!".WriteLine(ConsoleColor.Red);
                    return false;
                }
                case InstaLoginResult.InvalidUser:
                {
                    "invalid user!".WriteLine(ConsoleColor.Red);
                    return false;
                }
                case InstaLoginResult.TwoFactorRequired:
                {
                    "TwoFactorAuthentifikation:".WriteLine(ConsoleColor.Yellow);

                   var code = Console.ReadLine();

                    $"use TwoFactorAuthentifikation: {code} ".WriteLine(ConsoleColor.Yellow);

                    CheckRequestLimit();
                    var twoFactorLogin = await InstaApi.TwoFactorLoginAsync(code);

                    if (!twoFactorLogin.Succeeded)
                    {
                        twoFactorLogin.Info.Message.WriteLine(ConsoleColor.Red);
                        return false;
                    }
                    return true;
                }
                case InstaLoginResult.Exception:
                {
                    "error!".WriteLine(ConsoleColor.Red);
                    loginResult.Info.Message.WriteLine(ConsoleColor.Red);
                    return false;
                }
                case InstaLoginResult.ChallengeRequired:
                {
                    CheckRequestLimit();
                    var challenge = await InstaApi.GetChallengeRequireVerifyMethodAsync();
                    if (!challenge.Succeeded)
                    {
                        challenge.Info.Message.WriteLine(ConsoleColor.Red);
                        return false;
                    }

                    Sleeper.RandomSleep(300, 500);

                    CheckRequestLimit();
                    var sendMail = await InstaApi.RequestVerifyCodeToEmailForChallengeRequireAsync();
                    if (sendMail.Succeeded)
                    {
                        try
                        {
                            $"Instagram send a verify code to this email: \n{sendMail.Value.StepData.ContactPoint}"
                                .WriteLine(ConsoleColor.Yellow);
                            "Code: ".Write();

                            var code = Console.ReadLine();

                            $"send code {code} to Instagram".WriteLine(ConsoleColor.Yellow);

                            CheckRequestLimit();
                            var verify = await InstaApi.VerifyCodeForChallengeRequireAsync(code);

                            return await ValidateLoginValue(verify, loginData);
                        }
                        catch (Exception e)
                        {
                           e.Message.WriteLine(ConsoleColor.Red);
                           return false;
                        }
                    }

                    sendMail.Info.Message.WriteLine(ConsoleColor.Red);

                    return false;
                }
                case InstaLoginResult.LimitError:
                {
                    "Login Request Limit!".Write(ConsoleColor.Red);
                    return false;
                }
                case InstaLoginResult.InactiveUser:
                {
                    "inactiv user!".WriteLine(ConsoleColor.Red);
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> Logout()
        {
            try
            {
                if (_activLogin != null &&
                    InstaApi != null &&
                    InstaApi.IsUserAuthenticated)
                {
                    CheckRequestLimit();
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