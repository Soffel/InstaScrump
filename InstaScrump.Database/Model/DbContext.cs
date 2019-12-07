using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LinqToDB.Data;

namespace InstaScrump.Database.Model
{
    public class DbContext : IDisposable
    {
        private InstaScrumpDB _db;

        public InstaScrumpDB Create(bool test = false)
        {
            if (test)
            {
                DataConnection.TurnTraceSwitchOn();
                DataConnection.WriteTraceLine = (msg, context) => Debug.WriteLine(msg, context);
                DataConnection.DefaultSettings = new DatabaseSettings("TEST", @"DataSource = G:\Projekte\Database\Tests\InstaScrump.db;");
                _db = new InstaScrumpDB("TEST");
            }
            else
            {
                DataConnection.DefaultSettings = new DatabaseSettings();
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
