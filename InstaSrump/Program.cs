using System;
using System.Threading.Tasks;

namespace InstaScrump
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string input;
            while ((input = Console.ReadLine())?.Equals("quit", StringComparison.InvariantCultureIgnoreCase) != null)
            {
                await CmdFactory.ExecuteCmd(input.Split(' '));
            }
        }
    }
}
