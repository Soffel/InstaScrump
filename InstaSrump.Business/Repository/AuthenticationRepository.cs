using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstaScrump.Common;
using InstaScrump.Common.Exceptions;
using InstaScrump.Common.Extension;
using InstaScrump.Common.Extension.ModelExtension;
using InstaScrump.Common.Interfaces;
using InstaScrump.Common.Utils;
using InstaScrump.Database.Model;
using LinqToDB;

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

            if (ushort.TryParse(Config.Read("MaxLoginAttempt", "Config"), out var maxLoginAttempt))
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
                    var userInfo = await InstaApi.GetCurrentUserAsync();

                    if (userInfo.Succeeded)
                    {
                        var (pswd, salt) = Cryptography.Encrypt<AesManaged>(loginData.Pswd,
                            Config.Read("Pswd", "Security"), Config.Read("Vector", "Security"));

                        using (var db = DbContext.Create())
                        {
                            if (await db.InsertAsync(new LoginData()
                            {
                                Salt = salt,
                                UserPswd = pswd,
                                UserName = loginData.UserName
                            }) != 1)
                                throw new DatabaseException("LoginData insert failed!");

                            await db.CommitTransactionAsync();
                        }
                    }
                }
            }
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
                InstaApi = InstaApiBuilder.CreateBuilder()
                    .UseHttpClientHandler(new HttpClientHandler())
                    .SetUser(new UserSessionData {UserName = loginData.UserName, Password = loginData.Pswd})
                    .Build();

                "Wait for login...".WriteLine();

                var login = await InstaApi.LoginAsync();

                if (!login.Succeeded)
                {
                    count++;

                    switch (login.Value)
                    {
                        case InstaLoginResult.Success:
                        {
                            "login successful!".WriteLine(ConsoleColor.Green);
                            _activLogin = loginData;
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
                            "twofactor todo".WriteLine(ConsoleColor.Cyan);
                        }
                            break;
                        case InstaLoginResult.Exception:
                        {
                            "error!".WriteLine(ConsoleColor.Red);
                        }
                            break;
                        case InstaLoginResult.ChallengeRequired:
                        {
                            var challenge = await InstaApi.GetChallengeRequireVerifyMethodAsync();

                            if (!challenge.Succeeded)
                            {
                                challenge.Info.Message.WriteLine(ConsoleColor.Red);
                            }
                            else
                            {
                                Sleeper.Sleep();
                                var sendMail = await InstaApi.RequestVerifyCodeToEmailForChallengeRequireAsync();
                                if (sendMail.Succeeded)
                                {
                                    $"Instagram send a verify code to this email: \n{sendMail.Value.StepData.ContactPoint}"
                                        .WriteLine(ConsoleColor.Yellow);
                                    "Code: ".Write();

                                    var code = Console.ReadLine();

                                    $"send code {code} to Instagram".WriteLine(ConsoleColor.Yellow);

                                    var verify = await InstaApi.VerifyCodeForChallengeRequireAsync(code);
                                    if (verify.Succeeded)
                                    {
                                        if (InstaApi.IsUserAuthenticated)
                                        {
                                            "login successful!".WriteLine(ConsoleColor.Green);
                                            _activLogin = loginData;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                            break;
                        case InstaLoginResult.LimitError:
                            "idiot!".Write(ConsoleColor.Red);
                            break;
                        case InstaLoginResult.InactiveUser:
                            "inactiv user!".WriteLine(ConsoleColor.Red);
                            break;
                    }
                }
                else
                {
                    "login successful!".WriteLine(ConsoleColor.Green);
                    _activLogin = loginData;
                    return true;
                }

                Sleeper.Sleep();
            }

            "login unsuccessful".WriteLine(ConsoleColor.Red);
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