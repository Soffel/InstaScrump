using System;
using System.Threading.Tasks;
using Extension;
using InstaScrump.Common.Interfaces;

namespace InstaScrump.Command
{
    internal class EchoCmd : CommandBase, ICommand
    {
        public bool CommandString(string cmd)
        {
            return cmd.Equals("echo", StringComparison.CurrentCultureIgnoreCase);
        }

#pragma warning disable 1998
        public async Task Execute(string[] args)
#pragma warning restore 1998
        {
            args[0] = "";
            var result = string.Join(' ', args);
            result.WriteLine(ConsoleColor.DarkGreen);
        }

        public string HelpText()
        {
            return "echo <text> \t\t => outputs the following text.";
        }
    }
}
