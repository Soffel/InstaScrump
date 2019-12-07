using InstaScrump.Business;
using InstaScrump.Common.Utils;

namespace InstaScrump.Command
{
    internal class CommandBase
    {
        public CommandBase()
        {
            InstaScrumpUnitOfWork = new InstaScrumpUnitOfWork();
            Config = new Config(@"InstaScrump.ini");
        }

        protected static InstaScrumpUnitOfWork InstaScrumpUnitOfWork { get; private set; }
        protected static Config Config { get; private set; }
    }
}
