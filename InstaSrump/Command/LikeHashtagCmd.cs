using System;
using System.Threading.Tasks;
using Basic.Utils;
using Extensions;
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
                switch (args[1].ToUpper())
                {
                    case "LP":
                        await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new LPRule(int.Parse(args[2])));
                        break;

                    case "CAT":
                        await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new CatRule(int.Parse(args[2])));
                        break;

                    case "FOTO":
                    case "PHOTO":
                        await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new PhotoRule(int.Parse(args[2])));
                        break;
                }
            }
            else if (args.Length == 4 && !args[1].IsNullOrWhiteSpace() && !args[2].IsNullOrWhiteSpace() && args[3].Equals("-t", StringComparison.CurrentCultureIgnoreCase))
            {
                switch (args[1].ToUpper())
                {
                    case "LP":
                        await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new LPRule(int.Parse(args[2])));
                        new Timer(async () => await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new LPRule(int.Parse(args[2]))), 1, Utils.Sleeper.SleepType.h) { Restart = true };
                        break;

                    case "CAT":
                        await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new CatRule(int.Parse(args[2])));
                        new Timer(async () => await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new CatRule(int.Parse(args[2]))), 1, Utils.Sleeper.SleepType.h) { Restart = true };
                        break;

                    case "FOTO":
                    case "PHOTO":
                        await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new PhotoRule(int.Parse(args[2])));
                        new Timer(async () => await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new PhotoRule(int.Parse(args[2]))), 1, Utils.Sleeper.SleepType.h) { Restart = true };
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
            return "like <name> <count> \t\t =>  like <count> pictures with Rule <name> \r\n"+
                   "like <name> <count> -t \t\t =>  like <count> pictures with Rule <name>, restart after 1h \r\n";
        }
    }
}
