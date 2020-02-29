using System;
using System.Threading.Tasks;
using Extension;
using InstaScrump.Business.Rules;
using InstaScrump.Common;
using InstaScrump.Common.Interfaces;
using InstaScrump.Rules;

namespace InstaScrump.Command
{
    internal class LikeHashtagCmd : CommandBase, ICommand
    {
        public bool CommandString(string cmd)
        {
            return cmd.Equals("like", StringComparison.CurrentCultureIgnoreCase);
        }

      

        public async Task Execute(string[] args)
        {
            if (args.Length == 3 && !args[1].IsNullOrWhiteSpace() && !args[2].IsNullOrWhiteSpace())
            {
                switch(args[1].ToUpper())
                {
                    case "LP":
                        await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new LPRule(int.Parse(args[2])));
                        break;

                    case "CAT":
                        await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new CatRule(int.Parse(args[2])));
                        break;
                }
             
            }
            else
            {
                HelpText().WriteLine();
            }
        }

        public string HelpText()
        {
            return "like <name> <count> \t\t =>  like <count> pictures with Rule <name> \r\n";
        }
    }
}
