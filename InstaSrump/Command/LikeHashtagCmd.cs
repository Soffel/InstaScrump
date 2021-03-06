﻿using System;
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

                    case "COSPLAY":
                         await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new CosplayRule(int.Parse(args[2])));
                        break;
                }
            }
            else if (args.Length == 4 && !args[1].IsNullOrWhiteSpace() && !args[2].IsNullOrWhiteSpace() && !args[3].IsNullOrWhiteSpace() && int.Parse(args[3]) > 1)
            {
                var time = int.Parse(args[3]);

                switch (args[1].ToUpper())
                {
                    case "LP":
                        await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new LPRule(int.Parse(args[2])));
                        await new Timer().SetTimer(async () => await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new LPRule(int.Parse(args[2]))), Utils.Random.GetRandomIntBetween(time - 5,time + 5), Utils.Sleeper.SleepType.m);
                        break;

                    case "CAT":
                        await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new CatRule(int.Parse(args[2])));
                        await new Timer().SetTimer(async () => await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new CatRule(int.Parse(args[2]))), Utils.Random.GetRandomIntBetween(time - 5, time + 5), Utils.Sleeper.SleepType.m);
                        break;

                    case "FOTO":
                    case "PHOTO":
                        await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new PhotoRule(int.Parse(args[2])));
                        await new Timer().SetTimer(async () => await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new PhotoRule(int.Parse(args[2]))), Utils.Random.GetRandomIntBetween(time - 5, time + 5), Utils.Sleeper.SleepType.m);
                        break;

                    case "COSPLAY":
                        await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new CosplayRule(int.Parse(args[2])));
                        await new Timer().SetTimer(async () => await InstaScrumpUnitOfWork.LikeRepository.LikeHashTagbyRule(new CosplayRule(int.Parse(args[2]))), Utils.Random.GetRandomIntBetween(time - 5, time + 5), Utils.Sleeper.SleepType.m);
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
                   "like <name> <count> <time> \t\t =>  like <count> pictures with Rule <name>, restart after <time> minutes \r\n";
        }
    }
}
