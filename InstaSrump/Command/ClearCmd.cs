using System;
using System.Threading.Tasks;
using InstaScrump.Common.Extension;
using InstaScrump.Interface;

namespace InstaScrump.Command
{
    internal class ClearCmd : CommandBase, ICommand
    {
        public bool CommandString(string cmd)
        {
            return cmd.Equals("clear", StringComparison.CurrentCultureIgnoreCase);
        }

#pragma warning disable 1998
        public async Task Execute(string[] args)
#pragma warning restore 1998
        {
            Console.Clear();
        }

        public string HelpText()
        {
            return $"clear\t\t => clears the console";
        }
    }
}
