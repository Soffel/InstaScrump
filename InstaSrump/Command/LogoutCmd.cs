using InstaScrump.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InstaScrump.Command
{
    class LogoutCmd : CommandBase, ICommand
    {
        public bool CommandString(string cmd)
        {
            return cmd.Equals("logout", StringComparison.CurrentCultureIgnoreCase);
        }

        public async Task Execute(string[] args)
        {
            await InstaScrumpUnitOfWork.AuthenticationRepository.Logout();  
        }

        public string HelpText()
        {
            return "logout\t\t =>  logout"; 
        }
    }
}
