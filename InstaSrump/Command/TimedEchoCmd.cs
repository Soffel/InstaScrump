using System;
using System.Threading.Tasks;
using Basic.Utils;
using Extensions;
using InstaScrump.Common.Interfaces;

namespace InstaScrump.Command
{
    internal class TimedEchoCmd : CommandBase, ICommand
    {
        public bool CommandString(string cmd)
        {
            return cmd.Equals("timedecho", StringComparison.CurrentCultureIgnoreCase) || cmd.Equals("techo", StringComparison.CurrentCultureIgnoreCase);
        }


        public async Task Execute(string[] args)
        {
            args[0] = "";
            var result = string.Join(' ', args);
            result.WriteLine(ConsoleColor.DarkGreen);
            new Timer(() => result.WriteLine(ConsoleColor.DarkGreen), 1, Utils.Sleeper.SleepType.h) { Restart = false };
        }

        public string HelpText()
        {
            return "timedecho <text> \t\t => outputs the following text.";
        }
    
    }
}
