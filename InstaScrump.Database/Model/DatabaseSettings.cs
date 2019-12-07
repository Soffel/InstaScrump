using System.Collections.Generic;
using System.Linq;
using LinqToDB.Configuration;

namespace InstaScrump.Database.Model
{
    internal class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }

    public class DatabaseSettings : ILinqToDBSettings
    {
        private readonly string _name;
        private readonly string _connectionString;
        public DatabaseSettings(string name, string connectionString)
        {
            _name = name;
            _connectionString = connectionString;
        }

        public DatabaseSettings()
        {
            _name = DefaultConfiguration;
            _connectionString = @"DataSource=G:\Projekte\Database\InstaScrump.db;";
        }

        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();
        public string DefaultConfiguration => "InstaScrump";
        public string DefaultDataProvider => "SQLite";
        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSettings
                    {
                        Name = _name,
                        ProviderName = DefaultDataProvider,
                        ConnectionString = _connectionString,
                    };
            }
        }
    }


}