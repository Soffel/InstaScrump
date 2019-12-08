using InstaScrump.Business;
using InstaScrump.Common.Utils;
using InstaScrump.Database.Model;

namespace InstaScrump.Command
{
    internal class CommandBase
    {
        public CommandBase()
        {
            DbContext = new DbContext();
            Config = new Config(@"InstaScrump.ini");
            InstaScrumpUnitOfWork = new InstaScrumpUnitOfWork(DbContext, Config);
        }

        protected static InstaScrumpUnitOfWork InstaScrumpUnitOfWork { get; private set; }
        protected static Config Config { get; private set; }
        protected DbContext DbContext { get; private set; }
    }
}
