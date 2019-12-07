using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaScrump.Command;
using InstaScrump.Common.Extension;
using InstaScrump.Interface;

namespace InstaScrump
{
    public static class CmdFactory
    {
        private static readonly List<ICommand> Commands;

        static CmdFactory()
        {
            Commands = new List<ICommand>
            {
               new EchoCmd(),
            };
        }

        public static async Task ExecuteCmd(string[] args)
        {
            if (!args[0].IsNullOrWhiteSpace())
            {
                foreach (var command in Commands.Where(command => string.Equals(command.CommandString(), args[0], StringComparison.CurrentCultureIgnoreCase)))
                {
                    await command.Execute(args);
                    return;
                }
                $"Wrong Command [{args[0]}]".Write(ConsoleColor.Red);
            }
            else
            {
                "the following commandos are available:".Write();
                "----".Write(ConsoleColor.DarkYellow);

                foreach (var command in Commands)
                {
                    command.HelpText().Write();
                }

                "----".Write(ConsoleColor.DarkYellow);
            }
        }
    }
}
