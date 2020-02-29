using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstaScrump.Command;
using Extension;
using InstaScrump.Common.Interfaces;

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
               new ClearCmd(),
               new QuitCmd(),
               new LoginCmd(),
               new LogoutCmd(),
               new LikeHashtagCmd(),
            };
        }

        public static async Task ExecuteCmd(string[] args)
        {
            if (!args[0].IsNullOrWhiteSpace())
            {
                foreach (var command in Commands.Where(command => command.CommandString(args[0])))
                {
                    await command.Execute(args);
                    return;
                }

                $"Wrong Command [{args[0]}]".WriteLine(ConsoleColor.Red);
            }
            else
            {
                "the following commandos are available:".WriteLine();
                "----".WriteLine(ConsoleColor.DarkYellow);

                foreach (var command in Commands)
                {
                    command.HelpText().WriteLine();
                }

                "----".WriteLine(ConsoleColor.DarkYellow);
            }
        }
    }
}
