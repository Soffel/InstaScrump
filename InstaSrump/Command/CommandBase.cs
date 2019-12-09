using InstaScrump.Business;
using InstaScrump.Common.Utils;
using InstaScrump.Database.Model;

namespace InstaScrump.Command
{
    internal class CommandBase
    {
        public CommandBase()
        {
            Config = new Config(@"InstaScrump.ini");
            DbContext = new DbContext(Config);
            InstaScrumpUnitOfWork = new InstaScrumpUnitOfWork(DbContext, Config);
        }

        protected static InstaScrumpUnitOfWork InstaScrumpUnitOfWork { get; private set; }
        protected static Config Config { get; private set; }
        protected DbContext DbContext { get; private set; }
    }
}
