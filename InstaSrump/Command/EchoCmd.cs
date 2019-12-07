using System;
using System.Threading.Tasks;
using InstaScrump.Common.Extension;
using InstaScrump.Interface;

namespace InstaScrump.Command
{
    internal class EchoCmd : CommandBase, ICommand
    {
        public string CommandString()
        {
            return "echo";
        }

#pragma warning disable 1998
        public async Task Execute(string[] args)
#pragma warning restore 1998
        {
            args[0] = "";
            var result = string.Join(' ', args);
            result.Write(ConsoleColor.DarkGreen);
        }

        public string HelpText()
        {
            return $"{CommandString()}\t\t => outputs the following text.";
        }
    }
}
