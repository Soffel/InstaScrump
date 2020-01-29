using System;
using System.Diagnostics;
using System.Threading.Tasks;
using InstaScrump.Common.Interfaces;
using InstaScrump.Common.Constants;
using LinqToDB.Data;

namespace InstaScrump.Database.Model
{
    public class DbContext : IDbContext<InstaScrumpDB>, IDisposable
    {
        private InstaScrumpDB _db;
        private readonly IConfig _config;

        public DbContext(IConfig config)
        {
            _config = config;
        }

        public InstaScrumpDB Create(bool test = false)
        {
            if (test)
            {
                DataConnection.TurnTraceSwitchOn();
                DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
                DataConnection.DefaultSettings = new DatabaseSettings("TEST", _config.Read(ConfigKey.Test_Db_Key,"Database"));
                _db = new InstaScrumpDB("TEST");
            }
            else
            {
                DataConnection.DefaultSettings = new DatabaseSettings("InstaScrump", _config.Read(ConfigKey.Prod_Db_Key,"Database"));
                _db = new InstaScrumpDB("InstaScrump");
            }

            _db.BeginTransaction();

            return _db;
        }

        public void Commit()
        {
            _db?.CommitTransaction();
        }

        public async Task CommitAsync()
        {
            if (_db != null)
                await _db.CommitTransactionAsync();
        }

        public void Rollback()
        {
            _db?.RollbackTransaction();
        }

        public async Task RollbackAsync()
        {
            if(_db != null)
                await _db.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            Rollback();
            _db?.Dispose();
        }
    }
}
