using System;
using System.Threading.Tasks;
using InstaScrump.Common.Extension;
using InstaScrump.Common.Interfaces;

namespace InstaScrump.Command
{
    internal  class LoginCmd : CommandBase, ICommand
    {
        public bool CommandString(string cmd)
        {
            return cmd.Equals("login", StringComparison.CurrentCultureIgnoreCase) || cmd.Equals("newlogin", StringComparison.CurrentCultureIgnoreCase);
        }

        public async Task Execute(string[] args)
        {
            if(args.Length >= 2  && args[0].Equals("login", StringComparison.CurrentCultureIgnoreCase) && !args[1].IsNullOrWhiteSpace())
            {
                await InstaScrumpUnitOfWork.AuthenticationRepository.Login(args[1]);         
            }
            else if(args.Length >= 3 && args[0].Equals("newlogin", StringComparison.CurrentCultureIgnoreCase) && !args[1].IsNullOrWhiteSpace() && !args[2].IsNullOrWhiteSpace())
            {
                await InstaScrumpUnitOfWork.AuthenticationRepository.SetNewLoginData(new Common.LoginModel { UserName = args[1], Pswd = args[2] });       
            }
            else
            {
                HelpText().WriteLine();
            }
        }

        public string HelpText()
        {
            return "login <username> \t\t =>  login as user <username> \r\n"+
                   ">> newlogin <username> <password> \t => create new login & login as user <username>";
        }
    }
}
