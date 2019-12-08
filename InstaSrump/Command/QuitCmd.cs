using System;
using System.Threading.Tasks;
using InstaScrump.Common.Extension;
using InstaScrump.Interface;

namespace InstaScrump.Command
{
    internal class QuitCmd : CommandBase, ICommand
    {
        public bool CommandString(string cmd)
        {
            return cmd.Equals("quit", StringComparison.CurrentCultureIgnoreCase) ||
                   cmd.Equals("exit", StringComparison.CurrentCultureIgnoreCase);
        }

#pragma warning disable 1998
        public async Task Execute(string[] args)
#pragma warning restore 1998
        {
            Environment.Exit(0);
        }

        public string HelpText()
        {
            return $"quit | exit\t\t => stop InstaScrump :'(";
        }
    }
}
